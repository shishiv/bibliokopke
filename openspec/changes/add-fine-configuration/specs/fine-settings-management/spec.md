# Spec: Fine Settings Management

**Capability**: fine-settings-management
**Change**: add-fine-configuration

## ADDED Requirements

### Requirement: FinesConfig SHALL persist fine settings

Sistema SHALL armazenar configurações de multa em arquivo JSON encriptado (`%LOCALAPPDATA%\BibliotecaJK\fines.config`) usando AES-256, seguindo padrão BackupConfig.cs. Configurações incluem: valor diário (DailyFine), período de carência (GracePeriodDays), limite máximo (MaxFinePerBook), e status habilitado/desabilitado (Enabled).

#### Scenario: Admin saves fine configuration
- **WHEN** admin abre FormConfiguracoes e altera DailyFine para R$ 3.00
- **AND** clica em "Salvar"
- **THEN** FinesConfig.Salvar() cria/atualiza arquivo fines.config encriptado
- **AND** mensagem "Configurações salvas com sucesso!" é exibida
- **AND** ação é registrada em Log_Acao

#### Scenario: Load configuration on application start
- **WHEN** EmprestimoService é instanciado
- **THEN** FinesService carrega config de fines.config
- **AND** se arquivo não existe, usa padrões (DailyFine=2.00, GracePeriodDays=0, MaxFinePerBook=100, Enabled=true)

### Requirement: FinesService SHALL calculate fines using configuration

FinesService.CalcularMulta(diasAtraso) SHALL usar configurações carregadas para calcular multa: retornar 0 se Enabled=false ou DailyFine=0, aplicar período de carência subtraindo GracePeriodDays, calcular multa como (diasAtraso - grace) × DailyFine, e limitar ao MaxFinePerBook.

#### Scenario: Calculate fine with default settings
- **WHEN** FinesService.CalcularMulta(5) é chamado
- **AND** config é: DailyFine=2.00, GracePeriodDays=0, Enabled=true
- **THEN** retorna R$ 10.00 (5 dias × R$ 2.00)

#### Scenario: Calculate fine with grace period
- **WHEN** FinesService.CalcularMulta(3) é chamado
- **AND** config é: DailyFine=2.00, GracePeriodDays=2, Enabled=true
- **THEN** retorna R$ 2.00 ((3-2) dias × R$ 2.00)

#### Scenario: Calculate fine when disabled
- **WHEN** FinesService.CalcularMulta(10) é chamado
- **AND** config é: Enabled=false
- **THEN** retorna R$ 0.00
- **AND** multa não é aplicada mesmo com 10 dias de atraso

#### Scenario: Calculate fine with cap
- **WHEN** FinesService.CalcularMulta(50) é chamado
- **AND** config é: DailyFine=5.00, MaxFinePerBook=20.00, Enabled=true
- **THEN** retorna R$ 20.00 (limitado ao máximo, não R$ 250.00)

### Requirement: FormConfiguracoes SHALL provide admin UI

FormConfiguracoes SHALL exibir painel de configurações de multas acessível apenas para perfil ADMIN, permitindo editar DailyFine (R$ 0.00-100.00), GracePeriodDays (0-30), MaxFinePerBook (R$ 1.00-1000.00), e Enabled (checkbox). Validações devem prevenir valores fora de range.

#### Scenario: Admin opens fine settings panel
- **WHEN** funcionário com perfil ADMIN clica em "Configurações" na sidebar do FormPrincipal
- **THEN** FormConfiguracoes abre
- **AND** painel "Multas" exibe valores atuais carregados de fines.config
- **AND** campos são editáveis

#### Scenario: Non-admin cannot access settings
- **WHEN** funcionário com perfil BIBLIOTECARIO ou OPERADOR tenta acessar configurações
- **THEN** botão "Configurações" não é visível na sidebar
- **OR** se tentar acessar diretamente, mensagem "Acesso negado: apenas ADMIN" é exibida

#### Scenario: Validate fine value range
- **WHEN** admin tenta salvar DailyFine = R$ 150.00
- **THEN** validação falha com mensagem "Valor deve estar entre R$ 0,00 e R$ 100,00"
- **AND** config não é salva

### Requirement: UI SHALL hide fine elements when disabled

Quando FinesService.EstaHabilitado() retorna false (Enabled=false ou DailyFine=0), formulários SHALL esconder labels, campos e mensagens relacionadas a multas (lblMulta, lblMultaInfo, painéis de multa em relatórios).

#### Scenario: Hide fine UI when disabled
- **WHEN** fines.config tem Enabled=false
- **AND** FormDevolucao é aberto
- **THEN** lblMulta.Visible = false
- **AND** lblMultaInfo.Visible = false
- **AND** seção de multa não aparece em MessageBox de confirmação

#### Scenario: Show fine UI when enabled
- **WHEN** fines.config tem Enabled=true e DailyFine=2.00
- **AND** FormDevolucao é aberto
- **THEN** lblMulta.Visible = true
- **AND** lblMultaInfo.Text = "Multa: R$ 2,00/dia"

## MODIFIED Requirements

### Requirement: EmprestimoService SHALL use FinesService for calculations

EmprestimoService.RegistrarDevolucao() SHALL instanciar FinesService e chamar CalcularMulta(diasAtraso) ao invés de usar constante hardcoded `MULTA_POR_DIA * diasAtraso`. Substituir cálculo manual por serviço configurável.

#### Scenario: Return processing uses FinesService
- **WHEN** EmprestimoService.RegistrarDevolucao() é chamado
- **AND** empréstimo está 5 dias atrasado
- **THEN** finesService.CalcularMulta(5) é invocado
- **AND** multa retornada é armazenada em emprestimo.Multa
- **AND** ResultadoOperacao.OkComMulta(mensagem, multa) é retornado

### Requirement: Forms SHALL use FinesService instead of hardcoded 2.00

FormDevolucao (3 locais), FormConsultaEmprestimos (1 local), FormRelatorios (4 locais), e FormEmprestimo (1 local) SHALL substituir todos cálculos `diasAtraso * 2.00m` por chamadas a `FinesService.CalcularMulta(diasAtraso)`.

#### Scenario: FormDevolucao preview uses service
- **WHEN** FormDevolucao exibe preview de devolução
- **AND** empréstimo está 3 dias atrasado
- **THEN** usa finesService.CalcularMulta(3) para calcular multa
- **AND** lblMulta.Text exibe valor calculado conforme configuração

### Requirement: Database trigger SHALL be updated or removed

Trigger `trigger_emprestimo_devolucao()` em schema-postgresql.sql SHALL ser modificado para não calcular multa (remover linha `NEW.multa = ... * 2.00`) OU manter mas permitir C# sobrescrever valor. Preferência: remover cálculo do trigger, deixar apenas C# calcular.

#### Scenario: Trigger does not override C# calculation
- **WHEN** EmprestimoService.RegistrarDevolucao() salva multa calculada (ex: R$ 15.00)
- **AND** trigger `trigger_emprestimo_devolucao()` é executado
- **THEN** trigger NÃO sobrescreve valor de multa
- **AND** emprestimo.Multa permanece R$ 15.00 como calculado pelo C#

## Testing Requirements

### Requirement: Configuration persistence SHALL be reliable

Configurações devem persistir após reinicialização da aplicação e sobreviver updates. Arquivo deve ser encriptado corretamente e desencriptável.

#### Scenario: Config survives application restart
- **WHEN** admin salva DailyFine=3.50 e fecha aplicação
- **AND** aplicação é reaberta
- **THEN** FinesConfig.Carregar() retorna DailyFine=3.50
- **AND** multas são calculadas usando R$ 3.50/dia

#### Scenario: Config file is encrypted
- **WHEN** admin salva configuração
- **THEN** arquivo `fines.config` é criado em `%LOCALAPPDATA%\BibliotecaJK\`
- **AND** conteúdo do arquivo é base64 encriptado (não plaintext JSON)
- **AND** FinesConfig.Carregar() consegue desencriptar e ler

### Requirement: Backward compatibility SHALL be maintained

Se fines.config não existe (instalação antiga ou primeira execução), sistema SHALL usar valores padrões (DailyFine=2.00, GracePeriodDays=0, MaxFinePerBook=100, Enabled=true) sem quebrar funcionalidade existente.

#### Scenario: First run without config file
- **WHEN** aplicação é executada pela primeira vez ou fines.config não existe
- **THEN** FinesConfig.Carregar() retorna null
- **AND** FinesService usa `new FinesConfig()` com valores padrões
- **AND** multas são calculadas normalmente com R$ 2.00/dia (comportamento atual mantido)
