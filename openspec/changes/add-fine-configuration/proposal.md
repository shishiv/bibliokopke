# Change: Add Fine Configuration System

## Why

O valor da multa está hardcoded em múltiplos lugares (Constants.cs = R$ 2.00, database trigger = 2.00, 8 localizações hardcoded em Forms). Bibliotecas precisam flexibilidade para configurar, desabilitar ou ajustar multas sem modificar código. Atualmente impossível desativar multas ou alterar valor por dia sem recompilar aplicação e atualizar triggers do banco.

## What Changes

- **FinesConfig.cs**: Nova classe de configuração seguindo padrão BackupConfig (JSON encriptado AES-256)
- **FinesService.cs**: Serviço de negócio para cálculo de multas usando configurações
- **FormConfiguracoes.cs**: Nova tela de configurações do sistema (inclui multas, pode expandir futuramente)
- **Refactor hardcoded values**: Substituir todos os `2.00m` hardcoded por `FinesService.CalcularMulta()`
- **Database trigger update**: Modificar trigger para usar valor configurável ou mover cálculo apenas para C#
- **UI conditional visibility**: Esconder elementos de multa quando `DailyFine = 0` (multas desabilitadas)
- **Arquivo**: `%LOCALAPPDATA%\BibliotecaJK\fines.config` (JSON encriptado)

## Impact

**Affected specs**:
- `fine-settings-management` (novo)

**Affected code**:
- `06_bibliotecaJK/BLL/FinesConfig.cs` (novo) - Persistência de configurações
- `06_bibliotecaJK/BLL/FinesService.cs` (novo) - Lógica de cálculo
- `06_bibliotecaJK/BLL/EmprestimoService.cs` - Usar FinesService ao invés de constante
- `06_bibliotecaJK/Forms/FormConfiguracoes.cs` (novo) - Tela de configurações
- `06_bibliotecaJK/Forms/FormDevolucao.cs` - Substituir 3x hardcoded `2.00m`
- `06_bibliotecaJK/Forms/FormConsultaEmprestimos.cs` - Substituir 1x hardcoded `2.00m`
- `06_bibliotecaJK/Forms/FormRelatorios.cs` - Substituir 4x hardcoded `2.00m`
- `06_bibliotecaJK/Forms/FormEmprestimo.cs` - Mensagem condicional
- `06_bibliotecaJK/Forms/FormPrincipal.cs` - Adicionar botão "Configurações" na sidebar
- `06_bibliotecaJK/schema-postgresql.sql` - Atualizar trigger ou remover cálculo do trigger
- `06_bibliotecaJK/Constants.cs` - Adicionar `CONFIG_FILE_FINES`, manter `MULTA_POR_DIA_PADRAO`

**Success Criteria**:
- ✅ Admin pode configurar valor de multa por dia (R$ 0.00 a R$ 100.00)
- ✅ Admin pode desabilitar multas completamente (valor = 0)
- ✅ Configurações persistem entre reinicializações
- ✅ UI se adapta: esconde elementos de multa quando desabilitado
- ✅ Zero hardcoded values: todos usam FinesService
- ✅ Backward compatible: se config não existe, usa Constants.MULTA_POR_DIA_PADRAO (R$ 2.00)

## Scope

### In Scope
- Configuração de valor de multa diária
- Habilitar/desabilitar sistema de multas
- Período de carência (dias antes de aplicar multa)
- Limite máximo de multa por livro
- Tela administrativa de configurações (perfil ADMIN apenas)
- Migração de todos hardcoded values para usar serviço
- Atualização de database trigger

### Out of Scope
- Pagamento online de multas (futuro)
- Diferentes valores de multa por tipo de livro (futuro)
- Perdão de multas (waiver) - pode ser adicionado manualmente por ADMIN
- Histórico de alterações de configuração (futuro - poderia usar Log_Acao)
- Múltiplas políticas de multa simultâneas

## Alternatives Considered

### 1. Database Configuration Table
- **Approach**: `CREATE TABLE ConfiguracaoSistema (chave, valor)`
- **Pro**: Centralized, accessible from triggers
- **Con**: More complex, requires migration, adds database bloat
- **Decision**: ❌ Rejected - File-based config is simpler and consistent with existing pattern

### 2. Keep Hardcoded in Constants.cs
- **Approach**: Don't make configurable, just update Constants.cs when needed
- **Pro**: Simplest, no new code
- **Con**: Requires recompilation, doesn't solve hardcoded values in Forms/SQL
- **Decision**: ❌ Rejected - Not flexible enough for libraries with different policies

### 3. Admin UI in Existing Forms
- **Approach**: Add fine config to FormConfiguracaoConexao or FormBackup
- **Pro**: No new form needed
- **Con**: Mixing unrelated concerns (connection ≠ fines)
- **Decision**: ❌ Rejected - Better to create comprehensive FormConfiguracoes for all settings

### 4. Move All Calculation to Database
- **Approach**: Calculate fines only in PostgreSQL trigger
- **Pro**: Single source of truth
- **Con**: Hard to make configurable, C# needs to read config from DB
- **Decision**: ❌ Rejected - Prefer C# calculation for easier configuration

## Dependencies

- None (independent change)

## Related Changes

- None currently
- Could be extended in future: `add-payment-gateway`, `add-fine-waiver-system`

## References

- BackupConfig.cs pattern (encryption, AppData storage)
- Conexao.cs pattern (config file management)
- Constants.cs current fine rate (R$ 2.00/day)
- Database trigger `trigger_emprestimo_devolucao()` in schema-postgresql.sql
