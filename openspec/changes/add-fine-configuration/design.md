# Design: Add Fine Configuration System

**Change ID:** `add-fine-configuration`

## Overview

Sistema de configuração de multas que permite administradores ajustar valores de multa, desabilitar o sistema completamente, e configurar regras de penalidades sem modificar código ou banco de dados. Segue padrão estabelecido por BackupConfig.cs para consistência arquitetural.

## Current State Analysis

### Problems Identified

1. **Hardcoded Fine Rate**: R$ 2.00/dia definido em 10+ localizações
   - `Constants.cs`: `MULTA_POR_DIA = 2.00m`
   - `schema-postgresql.sql` trigger: `* 2.00`
   - `FormDevolucao.cs`: 3x `diasAtraso * 2.00m`
   - `FormConsultaEmprestimos.cs`: 1x `diasAtraso * 2.00m`
   - `FormRelatorios.cs`: 4x `diasAtraso * 2.00m`

2. **No Disable Mechanism**: Impossível desativar multas sem quebrar lógica
   - Setar `MULTA_POR_DIA = 0` funciona no C# mas trigger SQL ainda usa 2.00
   - UI mostra mensagens hardcoded "R$ 2,00/dia"

3. **Inconsistent Calculation**: Multa calculada em 3 lugares diferentes
   - `EmprestimoService.RegistrarDevolucao()` (C#)
   - `trigger_emprestimo_devolucao()` (PostgreSQL)
   - Múltiplos forms para preview (UI)

4. **No Flexibility**: Bibliotecas têm políticas diferentes
   - Algumas querem período de carência (grace period)
   - Algumas querem limitar multa máxima
   - Algumas não cobram multas

## Proposed Architecture

### Component Diagram

```
┌─────────────────────────────────────────────────────────┐
│                   FormConfiguracoes                     │
│  (ADMIN only - UI para configurações do sistema)       │
│                                                         │
│  ┌─────────────────┐  ┌──────────────────────────┐    │
│  │ Panel Conexão   │  │ Panel Multas (NOVO)      │    │
│  │ (Link FormConf) │  │ - Valor por dia          │    │
│  └─────────────────┘  │ - Período carência       │    │
│                       │ - Limite máximo          │    │
│  ┌─────────────────┐  │ - Habilitar/Desabilitar │    │
│  │ Panel Backup    │  └──────────────────────────┘    │
│  │ (Link FormBkp)  │                                   │
│  └─────────────────┘  [Botão Salvar] [Botão Resetar]  │
└─────────────────────────────────────────────────────────┘
                              │
                              ↓ Salvar()
┌─────────────────────────────────────────────────────────┐
│                     FinesService                        │
│  (BLL - Lógica de negócio e gerenciamento de config)   │
│                                                         │
│  + GetConfiguration() → FinesConfig                     │
│  + SalvarConfiguracao(config) → ResultadoOperacao      │
│  + CalcularMulta(diasAtraso) → decimal                 │
│  + EstaHabilitado() → bool                             │
│  + ObterMensagemMulta() → string                       │
└─────────────────────────────────────────────────────────┘
                              │
                              ↓ usa
┌─────────────────────────────────────────────────────────┐
│                     FinesConfig                         │
│  (BLL - Persistência e encriptação de configurações)   │
│                                                         │
│  Properties:                                            │
│  + DailyFine: decimal = 2.00                           │
│  + GracePeriodDays: int = 0                            │
│  + MaxFinePerBook: decimal = 100.00                    │
│  + Enabled: bool = true                                │
│                                                         │
│  Methods:                                               │
│  + Salvar() → void                                      │
│  + static Carregar() → FinesConfig?                    │
│  + static Existe() → bool                              │
│  + static Excluir() → void                             │
│  - Encrypt(json) → string (AES-256)                    │
│  - Decrypt(encrypted) → string                         │
└─────────────────────────────────────────────────────────┘
                              │
                              ↓ persiste
┌─────────────────────────────────────────────────────────┐
│         %LOCALAPPDATA%\BibliotecaJK\fines.config        │
│                   (JSON Encriptado)                     │
│                                                         │
│  {                                                      │
│    "DailyFine": 2.00,                                  │
│    "GracePeriodDays": 0,                               │
│    "MaxFinePerBook": 100.00,                           │
│    "Enabled": true                                     │
│  }                                                      │
└─────────────────────────────────────────────────────────┘
```

### Integration Flow

```
EmprestimoService.RegistrarDevolucao()
  │
  ├─ var finesService = new FinesService();
  ├─ var diasAtraso = CalcularDiasAtraso();
  ├─ var multa = finesService.CalcularMulta(diasAtraso);
  │
  └─ FinesService.CalcularMulta(diasAtraso)
       │
       ├─ Carregar config: _config = FinesConfig.Carregar() ?? Defaults
       ├─ if (!_config.Enabled) return 0;
       ├─ if (diasAtraso <= _config.GracePeriodDays) return 0;
       ├─ multa = (diasAtraso - grace) * _config.DailyFine
       ├─ multa = Math.Min(multa, _config.MaxFinePerBook)
       └─ return multa
```

## Detailed Design

### 1. FinesConfig Class

**File**: `06_bibliotecaJK/BLL/FinesConfig.cs`

```csharp
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Configurações de multas por atraso
    /// Armazena configurações em arquivo JSON encriptado no AppData
    /// </summary>
    public class FinesConfig
    {
        // ==================== PROPERTIES ====================

        /// <summary>
        /// Valor da multa por dia de atraso (em reais)
        /// </summary>
        public decimal DailyFine { get; set; } = Constants.MULTA_POR_DIA_PADRAO;

        /// <summary>
        /// Número de dias de carência antes de aplicar multa
        /// </summary>
        public int GracePeriodDays { get; set; } = 0;

        /// <summary>
        /// Valor máximo de multa por livro (teto)
        /// </summary>
        public decimal MaxFinePerBook { get; set; } = 100.00m;

        /// <summary>
        /// Sistema de multas habilitado ou não
        /// </summary>
        public bool Enabled { get; set; } = true;

        // ==================== FILE PATH ====================

        private static readonly string ConfigFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Constants.CONFIG_FOLDER_NAME,
            Constants.CONFIG_FILE_FINES
        );

        // ==================== ENCRYPTION ====================

        private static readonly byte[] EncryptionKey =
            Encoding.UTF8.GetBytes("BiblioJK2025Key!");
        private static readonly byte[] EncryptionIV =
            Encoding.UTF8.GetBytes("BiblioJK2025IV!!");

        // ==================== PUBLIC METHODS ====================

        /// <summary>
        /// Salva as configurações em arquivo encriptado
        /// </summary>
        public void Salvar()
        {
            var directory = Path.GetDirectoryName(ConfigFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var encrypted = Encrypt(json);
            File.WriteAllText(ConfigFilePath, encrypted);
        }

        /// <summary>
        /// Carrega configurações do arquivo ou retorna null se não existir
        /// </summary>
        public static FinesConfig? Carregar()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                    return null;

                var encrypted = File.ReadAllText(ConfigFilePath);
                var json = Decrypt(encrypted);
                return JsonSerializer.Deserialize<FinesConfig>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Verifica se arquivo de configuração existe
        /// </summary>
        public static bool Existe() => File.Exists(ConfigFilePath);

        /// <summary>
        /// Exclui arquivo de configuração
        /// </summary>
        public static void Excluir()
        {
            if (File.Exists(ConfigFilePath))
                File.Delete(ConfigFilePath);
        }

        // ==================== ENCRYPTION (AES-256) ====================

        private static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = EncryptionKey;
            aes.IV = EncryptionIV;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        private static string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = EncryptionKey;
            aes.IV = EncryptionIV;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
```

### 2. FinesService Class

**File**: `06_bibliotecaJK/BLL/FinesService.cs`

```csharp
using BibliotecaJK.Model;

namespace BibliotecaJK.BLL
{
    /// <summary>
    /// Serviço para gerenciamento e cálculo de multas
    /// </summary>
    public class FinesService
    {
        private readonly FinesConfig _config;
        private readonly LogService _logService;

        public FinesService()
        {
            // Tenta carregar config, senão usa padrões
            _config = FinesConfig.Carregar() ?? new FinesConfig();
            _logService = new LogService();
        }

        /// <summary>
        /// Retorna configuração atual
        /// </summary>
        public FinesConfig GetConfiguration() => _config;

        /// <summary>
        /// Verifica se sistema de multas está habilitado
        /// </summary>
        public bool EstaHabilitado() => _config.Enabled && _config.DailyFine > 0;

        /// <summary>
        /// Calcula multa baseado em dias de atraso e configurações
        /// </summary>
        public decimal CalcularMulta(int diasAtraso)
        {
            // Sistema desabilitado
            if (!_config.Enabled || _config.DailyFine == 0)
                return 0;

            // Dentro do período de carência
            if (diasAtraso <= _config.GracePeriodDays)
                return 0;

            // Calcular dias passíveis de multa
            var diasMultaveis = diasAtraso - _config.GracePeriodDays;

            // Calcular multa
            var multa = diasMultaveis * _config.DailyFine;

            // Aplicar teto
            multa = Math.Min(multa, _config.MaxFinePerBook);

            return multa;
        }

        /// <summary>
        /// Retorna mensagem descritiva sobre multa
        /// </summary>
        public string ObterMensagemMulta()
        {
            if (!_config.Enabled || _config.DailyFine == 0)
                return "Multas desabilitadas";

            var mensagem = $"Multa: R$ {_config.DailyFine:F2}/dia";

            if (_config.GracePeriodDays > 0)
                mensagem += $" (após {_config.GracePeriodDays} dias de carência)";

            if (_config.MaxFinePerBook < 1000)
                mensagem += $" (máx R$ {_config.MaxFinePerBook:F2})";

            return mensagem;
        }

        /// <summary>
        /// Salva nova configuração
        /// </summary>
        public ResultadoOperacao SalvarConfiguracao(FinesConfig novaConfig, int? idFuncionario = null)
        {
            try
            {
                // Validações
                if (novaConfig.DailyFine < 0 || novaConfig.DailyFine > 100)
                    return ResultadoOperacao.Erro("Valor de multa deve estar entre R$ 0,00 e R$ 100,00");

                if (novaConfig.GracePeriodDays < 0 || novaConfig.GracePeriodDays > 30)
                    return ResultadoOperacao.Erro("Período de carência deve estar entre 0 e 30 dias");

                if (novaConfig.MaxFinePerBook < 1 || novaConfig.MaxFinePerBook > 1000)
                    return ResultadoOperacao.Erro("Multa máxima deve estar entre R$ 1,00 e R$ 1.000,00");

                // Salvar
                novaConfig.Salvar();

                // Log da alteração
                _logService.Registrar(
                    idFuncionario,
                    "CONFIGURACAO_MULTAS_ALTERADA",
                    $"DailyFine={novaConfig.DailyFine:F2}, Grace={novaConfig.GracePeriodDays}, " +
                    $"Max={novaConfig.MaxFinePerBook:F2}, Enabled={novaConfig.Enabled}"
                );

                return ResultadoOperacao.Ok("Configurações de multa salvas com sucesso!");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao salvar: {ex.Message}");
            }
        }

        /// <summary>
        /// Reseta configurações para padrões
        /// </summary>
        public ResultadoOperacao ResetarParaPadroes(int? idFuncionario = null)
        {
            try
            {
                FinesConfig.Excluir();

                _logService.Registrar(
                    idFuncionario,
                    "CONFIGURACAO_MULTAS_RESETADA",
                    "Configurações de multa resetadas para padrões"
                );

                return ResultadoOperacao.Ok("Configurações resetadas para padrões!");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao resetar: {ex.Message}");
            }
        }
    }
}
```

### 3. Integration Points

#### Update EmprestimoService.cs

**Before** (Hardcoded):
```csharp
private const decimal MULTA_POR_DIA = Constants.MULTA_POR_DIA;

// In RegistrarDevolucao():
decimal multa = 0;
if (diasAtraso > 0)
{
    multa = diasAtraso * MULTA_POR_DIA;  // Hardcoded
}
```

**After** (Configurable):
```csharp
private readonly FinesService _finesService;

public EmprestimoService()
{
    // ... existing code
    _finesService = new FinesService();
}

// In RegistrarDevolucao():
var multa = _finesService.CalcularMulta(diasAtraso);
```

#### Update FormDevolucao.cs

**Before** (3x Hardcoded):
```csharp
var multa = diasAtraso > 0 ? diasAtraso * 2.00m : 0;
```

**After** (Configurable):
```csharp
private readonly FinesService _finesService = new FinesService();

// Everywhere multa is calculated:
var multa = _finesService.CalcularMulta(diasAtraso);

// For messages:
lblMultaInfo.Text = _finesService.ObterMensagemMulta();

// Conditional visibility:
if (!_finesService.EstaHabilitado())
{
    lblMulta.Visible = false;
    lblMultaInfo.Visible = false;
}
```

## Testing Matrix

| Scenario | DailyFine | Grace | MaxFine | Enabled | Expected Result |
|----------|-----------|-------|---------|---------|-----------------|
| Default | 2.00 | 0 | 100 | true | R$ 2.00/dia |
| Disabled | 2.00 | 0 | 100 | false | R$ 0.00 (hidden UI) |
| Zero rate | 0.00 | 0 | 100 | true | R$ 0.00 (hidden UI) |
| With grace | 2.00 | 3 | 100 | true | R$ 0.00 if ≤3 days |
| Capped | 5.00 | 0 | 20 | true | Max R$ 20.00 |
| High rate | 10.00 | 0 | 100 | true | R$ 10.00/dia |

## Migration Strategy

1. **Phase 1**: Criar FinesConfig e FinesService (sem quebrar existente)
2. **Phase 2**: Atualizar EmprestimoService para usar FinesService
3. **Phase 3**: Refatorar Forms (substituir hardcoded values)
4. **Phase 4**: Atualizar database trigger ou remover
5. **Phase 5**: Criar FormConfiguracoes UI
6. **Phase 6**: Testar backward compatibility

## Backwards Compatibility

- Se `fines.config` não existe: usa `Constants.MULTA_POR_DIA_PADRAO = 2.00m`
- Existing loans mantêm multas já calculadas (não recalcular retroativo)
- Database trigger pode ser mantido inicialmente, removido depois

## Security Considerations

- Config file encriptado (AES-256) como BackupConfig
- Apenas perfil ADMIN pode acessar FormConfiguracoes
- Log todas as alterações em Log_Acao para auditoria
- Validação de ranges (multa 0-100, carência 0-30, etc.)
