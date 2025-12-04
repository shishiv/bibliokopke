# Tasks: Add Fine Configuration System

**Change ID:** `add-fine-configuration`

## Overview

Implementa sistema configurável de multas permitindo admins ajustar valores, desabilitar completamente, ou configurar regras sem modificar código. Dividido em 3 sprints focados em: (1) infraestrutura, (2) integração com código existente, (3) UI e validação.

---

## Sprint 1: Core Infrastructure (Alta Prioridade)

**Objetivo**: Criar componentes base sem quebrar funcionalidade existente
**Duração**: 2-3 dias
**Impacto**: Fundação para todo o sistema

### Task 1.1: Create FinesConfig.cs

**Descrição**: Implementar classe de configuração seguindo padrão BackupConfig.cs com encriptação AES-256.

**Subtasks**:
1. Criar `06_bibliotecaJK/BLL/FinesConfig.cs`
2. Adicionar properties: DailyFine, GracePeriodDays, MaxFinePerBook, Enabled
3. Definir defaults: DailyFine=2.00m, GracePeriodDays=0, MaxFinePerBook=100.00m, Enabled=true
4. Implementar ConfigFilePath usando Constants.CONFIG_FILE_FINES
5. Copiar encryption/decryption methods de BackupConfig.cs (AES-256)
6. Implementar Salvar() - serialize to JSON, encrypt, save to file
7. Implementar static Carregar() - load file, decrypt, deserialize
8. Implementar static Existe() e Excluir()
9. Testar save/load/encrypt/decrypt isoladamente

**Arquivos**:
- `06_bibliotecaJK/BLL/FinesConfig.cs` (novo)

**Critérios de Aceitação**:
- [ ] FinesConfig serializa/desserializa corretamente
- [ ] Arquivo é salvo em `%LOCALAPPDATA%\BibliotecaJK\fines.config`
- [ ] Conteúdo do arquivo é encriptado (base64, não plaintext)
- [ ] Carregar() retorna config correta
- [ ] Carregar() retorna null se arquivo não existe
- [ ] Teste manual: criar config, salvar, recarregar valores

**Estimativa**: 3-4 horas

---

### Task 1.2: Create FinesService.cs

**Descrição**: Implementar serviço de cálculo de multas usando configurações.

**Subtasks**:
1. Criar `06_bibliotecaJK/BLL/FinesService.cs`
2. Constructor: carregar config via FinesConfig.Carregar() ?? new FinesConfig()
3. Implementar GetConfiguration() → retorna _config
4. Implementar EstaHabilitado() → _config.Enabled && _config.DailyFine > 0
5. Implementar CalcularMulta(int diasAtraso):
   - Return 0 se !Enabled ou DailyFine=0
   - Return 0 se diasAtraso ≤ GracePeriodDays
   - Calculate: (diasAtraso - GracePeriodDays) × DailyFine
   - Apply cap: Math.Min(multa, MaxFinePerBook)
6. Implementar ObterMensagemMulta() → string descritiva
7. Implementar SalvarConfiguracao(FinesConfig, idFuncionario) → ResultadoOperacao
   - Validar ranges (DailyFine 0-100, GracePeriodDays 0-30, MaxFinePerBook 1-1000)
   - Salvar via novaConfig.Salvar()
   - Log em Log_Acao
8. Implementar ResetarParaPadroes(idFuncionario) → ResultadoOperacao
9. Testar todos os scenarios do spec.md

**Arquivos**:
- `06_bibliotecaJK/BLL/FinesService.cs` (novo)

**Critérios de Aceitação**:
- [ ] CalcularMulta(5) com defaults retorna R$ 10.00
- [ ] CalcularMulta(3) com GracePeriodDays=2 retorna R$ 2.00
- [ ] CalcularMulta(10) com Enabled=false retorna R$ 0.00
- [ ] CalcularMulta(50) com MaxFinePerBook=20 retorna R$ 20.00 (capped)
- [ ] SalvarConfiguracao valida ranges corretamente
- [ ] Validação rejei ta DailyFine=150 (fora de 0-100)
- [ ] Log é criado ao salvar configuração

**Estimativa**: 4-5 horas

---

### Task 1.3: Update Constants.cs

**Descrição**: Adicionar constantes para arquivo de config e manter valor padrão.

**Subtasks**:
1. Adicionar `public const string CONFIG_FILE_FINES = "fines.config";`
2. Renomear `MULTA_POR_DIA` para `MULTA_POR_DIA_PADRAO` (manter 2.00m como default)
3. Manter backward compatibility

**Arquivos**:
- `06_bibliotecaJK/Constants.cs`

**Critérios de Aceitação**:
- [ ] CONFIG_FILE_FINES = "fines.config" definido
- [ ] MULTA_POR_DIA_PADRAO = 2.00m (default para FinesConfig)
- [ ] Compilação sem erros

**Estimativa**: 30 minutos

---

## Sprint 2: Integration with Existing Code (Média Prioridade)

**Objetivo**: Substituir hardcoded values por FinesService
**Duração**: 3-4 dias
**Impacto**: ~90% do cálculo de multas

### Task 2.1: Update EmprestimoService.cs

**Descrição**: Substituir cálculo manual por FinesService.

**Subtasks**:
1. Adicionar campo `private readonly FinesService _finesService;`
2. No constructor, instanciar: `_finesService = new FinesService();`
3. Em RegistrarDevolucao(), substituir:
   ```csharp
   // Old:
   decimal multa = 0;
   if (diasAtraso > 0) {
       multa = diasAtraso * MULTA_POR_DIA;
   }

   // New:
   var multa = _finesService.CalcularMulta(diasAtraso);
   ```
4. Remover `private const decimal MULTA_POR_DIA = Constants.MULTA_POR_DIA;`
5. Testar devolução com diferentes configs

**Arquivos**:
- `06_bibliotecaJK/BLL/EmprestimoService.cs`

**Critérios de Aceitação**:
- [ ] Devolução com 5 dias atraso e DailyFine=2.00 gera multa R$ 10.00
- [ ] Devolução com 5 dias e Enabled=false gera multa R$ 0.00
- [ ] Devolução com 3 dias e GracePeriodDays=2 gera multa R$ 2.00
- [ ] ResultadoOperacao.ValorMulta contém valor correto
- [ ] Funcionalidade existente não quebrada

**Estimativa**: 2 horas

---

### Task 2.2: Refactor FormDevolucao.cs

**Descrição**: Substituir 3 instâncias de `diasAtraso * 2.00m` por FinesService.

**Subtasks**:
1. Adicionar campo `private readonly FinesService _finesService = new FinesService();`
2. Substituir linha 255: `var multa = diasAtraso > 0 ? diasAtraso * 2.00m : 0;`
   → `var multa = _finesService.CalcularMulta(diasAtraso);`
3. Substituir linha 304 (similar)
4. Substituir linha 347 (similar)
5. Atualizar mensagem hardcoded linha 358:
   ```csharp
   // Old: "Multa por atraso: R$ 2,00/dia"
   // New:
   if (_finesService.EstaHabilitado()) {
       mensagem += _finesService.ObterMensagemMulta();
   } else {
       mensagem += "Multas desabilitadas";
   }
   ```
6. Adicionar visibilidade condicional:
   ```csharp
   if (!_finesService.EstaHabilitado()) {
       lblMulta.Visible = false;
       lblMultaInfo.Visible = false;
   }
   ```
7. Testar preview de devolução

**Arquivos**:
- `06_bibliotecaJK/Forms/FormDevolucao.cs`

**Critérios de Aceitação**:
- [ ] Preview mostra multa calculada conforme fines.config
- [ ] Se Enabled=false, labels de multa ficam invisíveis
- [ ] Mensagem exibe taxa correta ou "Multas desabilitadas"
- [ ] Confirmação de devolução mostra valor correto

**Estimativa**: 2-3 horas

---

### Task 2.3: Refactor FormConsultaEmprestimos.cs

**Descrição**: Substituir 1 hardcoded value.

**Subtasks**:
1. Adicionar `private readonly FinesService _finesService = new FinesService();`
2. Substituir linha 401: `diasAtraso * 2.00m` → `_finesService.CalcularMulta(diasAtraso)`
3. Atualizar estatísticas dashboard se necessário

**Arquivos**:
- `06_bibliotecaJK/Forms/FormConsultaEmprestimos.cs`

**Estimativa**: 1 hora

---

### Task 2.4: Refactor FormRelatorios.cs

**Descrição**: Substituir 4 hardcoded values.

**Subtasks**:
1. Adicionar `private readonly FinesService _finesService = new FinesService();`
2. Substituir linha 227: `diasAtraso * 2.00m`
3. Substituir linha 375: `diasAtraso * 2.00m`
4. Substituir linha 399 (similar)
5. Substituir linha 432 (similar)
6. Testar relatórios de multas

**Arquivos**:
- `06_bibliotecaJK/Forms/FormRelatorios.cs`

**Critérios de Aceitação**:
- [ ] Relatório de multas mostra valores conforme config
- [ ] Totalizações corretas

**Estimativa**: 1-2 horas

---

### Task 2.5: Update FormEmprestimo.cs

**Descrição**: Atualizar mensagem hardcoded.

**Subtasks**:
1. Adicionar `private readonly FinesService _finesService = new FinesService();`
2. Substituir linha 358 mensagem hardcoded por condicional

**Arquivos**:
- `06_bibliotecaJK/Forms/FormEmprestimo.cs`

**Estimativa**: 1 hora

---

### Task 2.6: Update Database Trigger (Optional but Recommended)

**Descrição**: Modificar trigger para não sobrescrever cálculo do C#.

**Subtasks**:
1. Abrir `06_bibliotecaJK/schema-postgresql.sql`
2. Localizar `trigger_emprestimo_devolucao()` (linhas 209-236)
3. **Option A (Recommended)**: Comentar cálculo de multa:
   ```sql
   -- Não calcular multa aqui, C# já calcula
   -- IF NEW.data_devolucao > NEW.data_prevista THEN
   --     NEW.multa = (NEW.data_devolucao - NEW.data_prevista) * 2.00;
   -- END IF;
   ```
4. **Option B**: Permitir C# sobrescrever:
   ```sql
   -- Apenas calcular se C# não definiu
   IF NEW.data_devolucao > NEW.data_prevista AND NEW.multa = 0 THEN
       NEW.multa = (NEW.data_devolucao - NEW.data_prevista) * 2.00;
   END IF;
   ```
5. Testar que C# calculation prevalece

**Arquivos**:
- `06_bibliotecaJK/schema-postgresql.sql`

**Critérios de Aceitação**:
- [ ] Trigger não sobrescreve multa calculada pelo C#
- [ ] Devolução com multa R$ 15.00 permanece R$ 15.00 após trigger

**Estimativa**: 1 hora

---

## Sprint 3: UI and Validation (Baixa Prioridade)

**Objetivo**: Criar interface administrativa
**Duração**: 3-4 dias
**Impacto**: UX para admins

### Task 3.1: Create FormConfiguracoes.cs

**Descrição**: Criar tela de configurações do sistema.

**Subtasks**:
1. Criar `06_bibliotecaJK/Forms/FormConfiguracoes.cs`
2. Layout base:
   - Title: "Configurações do Sistema"
   - Left sidebar com categorias (Conexão, Backup, Multas, ...)
   - Right panel com conteúdo da categoria selecionada
3. Panel "Multas":
   - NumericUpDown: Valor por dia (R$ 0.00-100.00, increment 0.50)
   - NumericUpDown: Período de carência (0-30 dias, increment 1)
   - NumericUpDown: Multa máxima por livro (R$ 1.00-1000.00, increment 5.00)
   - CheckBox: Habilitar sistema de multas
   - Label: Preview da mensagem (via FinesService.ObterMensagemMulta())
4. Botões:
   - "Salvar" → chama FinesService.SalvarConfiguracao()
   - "Resetar Padrões" → chama FinesService.ResetarParaPadroes()
   - "Cancelar" → fecha form sem salvar
5. Load config on form open:
   ```csharp
   var config = _finesService.GetConfiguration();
   numDailyFine.Value = config.DailyFine;
   // etc...
   ```
6. Validações:
   - Prevenir valores fora de range
   - Mostrar preview em tempo real
7. Tratamento de ResultadoOperacao com ToastNotification

**Arquivos**:
- `06_bibliotecaJK/Forms/FormConfiguracoes.cs` (novo)

**Critérios de Aceitação**:
- [ ] Form abre e carrega config atual
- [ ] Campos são editáveis
- [ ] Validações previnem valores inválidos
- [ ] Salvar persiste mudanças em fines.config
- [ ] Resetar restaura defaults
- [ ] Toast notification mostra sucesso/erro

**Estimativa**: 5-6 horas

---

### Task 3.2: Add Settings Button to FormPrincipal

**Descrição**: Adicionar botão "Configurações" na sidebar, apenas para ADMIN.

**Subtasks**:
1. No FormPrincipal sidebar, adicionar botão após "Backup":
   ```csharp
   var btnConfiguracoes = CriarBotaoMenu("⚙️ Configurações", btnY + btnSpacing * X);
   btnConfiguracoes.Click += BtnConfiguracoes_Click;
   ```
2. Verificar perfil:
   ```csharp
   if (_funcionarioLogado.Perfil != Constants.PerfilFuncionario.ADMIN)
   {
       btnConfiguracoes.Visible = false;
   }
   ```
3. Implementar event handler:
   ```csharp
   private void BtnConfiguracoes_Click(object sender, EventArgs e)
   {
       var formConfig = new FormConfiguracoes(_funcionarioLogado);
       formConfig.ShowDialog();
       // Refresh dashboard se multas foram alteradas
       AtualizarDashboard();
   }
   ```

**Arquivos**:
- `06_bibliotecaJK/Forms/FormPrincipal.cs`

**Critérios de Aceitação**:
- [ ] Botão visível apenas para ADMIN
- [ ] Clicar abre FormConfiguracoes
- [ ] Dashboard atualiza após mudanças de config

**Estimativa**: 1-2 horas

---

### Task 3.3: Comprehensive Testing

**Descrição**: Testar todos os scenarios do spec.md.

**Test Matrix**:

| Config | DailyFine | Grace | MaxFine | Enabled | Dias | Expected |
|--------|-----------|-------|---------|---------|------|----------|
| Default | 2.00 | 0 | 100 | true | 5 | R$ 10.00 |
| Disabled | 2.00 | 0 | 100 | false | 5 | R$ 0.00 |
| Zero rate | 0.00 | 0 | 100 | true | 5 | R$ 0.00 |
| With grace | 2.00 | 3 | 100 | true | 3 | R$ 0.00 |
| Grace passed | 2.00 | 3 | 100 | true | 5 | R$ 4.00 |
| Capped | 5.00 | 0 | 20 | true | 10 | R$ 20.00 |
| High rate | 10.00 | 0 | 100 | true | 5 | R$ 50.00 |

**Subtasks**:
1. Criar fines.config com cada configuração
2. Testar RegistrarDevolucao em EmprestimoService
3. Testar preview em FormDevolucao
4. Testar relatórios em FormRelatorios
5. Verificar UI visibility (labels escondidas quando Enabled=false)
6. Testar persistence (reiniciar app)
7. Testar backward compatibility (remover fines.config, usar defaults)
8. Testar encryption (arquivo não é plaintext JSON)

**Deliverable**: Test report em `openspec/changes/add-fine-configuration/test-report.md`

**Estimativa**: 3-4 horas

---

### Task 3.4: Update Documentation

**Descrição**: Documentar novo sistema de configuração.

**Subtasks**:
1. Atualizar `06_bibliotecaJK/README.md`:
   - Seção "Configurações de Multas"
   - Explicar arquivo fines.config
   - Como alterar valores via FormConfiguracoes
2. Atualizar `06_bibliotecaJK/BLL/README_BLL.md`:
   - Adicionar FinesConfig e FinesService
   - Documentar CalcularMulta() method
3. Atualizar `CLAUDE.md`:
   - Seção "Business Rules" - multas agora configuráveis
   - Pattern de configuração com encryption
4. Criar `06_bibliotecaJK/Forms/README_CONFIGURACOES.md` (novo):
   - Como usar FormConfiguracoes
   - Explicar cada campo
   - Exemplos de políticas de multa

**Estimativa**: 2 horas

---

## Dependency Graph

```
Sprint 1 (Sequential - must complete in order)
  ├─ Task 1.1 (FinesConfig) ── base para tudo
  ├─ Task 1.2 (FinesService) ── depende de 1.1
  └─ Task 1.3 (Constants) ── independente, pode ser paralelo

Sprint 2 (Mostly Parallel)
  ├─ Task 2.1 (EmprestimoService) ── depende de 1.2
  ├─ Task 2.2 (FormDevolucao) ── depende de 1.2, paralelo com 2.1
  ├─ Task 2.3 (FormConsulta) ── depende de 1.2, paralelo com 2.1-2.2
  ├─ Task 2.4 (FormRelatorios) ── depende de 1.2, paralelo com 2.1-2.3
  ├─ Task 2.5 (FormEmprestimo) ── depende de 1.2, paralelo com 2.1-2.4
  └─ Task 2.6 (DB Trigger) ── independente, pode ser qualquer momento

Sprint 3 (Depends on Sprint 2)
  ├─ Task 3.1 (FormConfiguracoes) ── depende de 1.2
  ├─ Task 3.2 (Add button) ── depende de 3.1
  ├─ Task 3.3 (Testing) ── depende de todos anteriores
  └─ Task 3.4 (Docs) ── depende de todos anteriores
```

---

## Summary Statistics

**Total Tasks**: 13 tasks
**Total Estimated Time**: 24-32 hours (~3-4 dias full-time)

**Breakdown by Sprint**:
- Sprint 1: 7-9 horas (3 tasks - infraestrutura)
- Sprint 2: 8-10 horas (6 tasks - integração)
- Sprint 3: 11-13 horas (4 tasks - UI e validação)

**Files Created**: 3 novos arquivos
**Files Modified**: 8 arquivos existentes
**Database Modified**: 1 trigger (opcional)

**Risk Assessment**:
- **Low Risk**: Sprint 1 (isolated infrastructure)
- **Medium Risk**: Sprint 2 (integration with existing code)
- **Low Risk**: Sprint 3 (UI only, no breaking changes)

---

## Success Criteria

- [x] Proposal aprovado
- [ ] Sprint 1: FinesConfig e FinesService funcionais
- [ ] Sprint 2: Todos hardcoded values substituídos
- [ ] Sprint 3: FormConfiguracoes funcional para ADMIN
- [ ] Testes em matriz de configurações passam
- [ ] Backward compatibility mantida (sem config = defaults)
- [ ] Encryption funciona (arquivo não é plaintext)
- [ ] UI esconde elementos quando Enabled=false
- [ ] Log_Acao registra alterações de config
- [ ] Documentation updated
- [ ] Change merged to main branch

---

## Rollback Plan

Se mudanças causarem regressões:

1. **Immediate**: Reverter commits individuais por task
2. **FinesConfig/Service**: Se quebrar, sistema volta para Constants.MULTA_POR_DIA (ainda existirá como MULTA_POR_DIA_PADRAO)
3. **Forms**: Cada form modificado pode ser revertido independentemente
4. **Last Resort**: Reverter entire change, usar hardcoded values temporariamente

**Git Strategy**: Commit por task individual para rollback granular.
