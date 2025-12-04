# Spec: Automatic Scrolling

**Capability**: automatic-scrolling
**Change**: improve-ui-responsiveness

## ADDED Requirements

### Requirement: Forms enable AutoScroll for overflow content

Todos formulários e seus panels de conteúdo SHALL ter `AutoScroll = true` para garantir que barras de scroll apareçam automaticamente quando conteúdo excede área visível, especialmente em telas pequenas.

#### Scenario: Form content exceeds screen height
- **WHEN** FormCadastroLivro com 20 campos aberto em 1366x768
- **AND** altura total do conteúdo excede 650px disponíveis
- **THEN** barra de scroll vertical aparece automaticamente
- **AND** usuário pode rolar para acessar campos inferiores
- **AND** scroll com mouse wheel funciona

#### Scenario: Form maximized removes scroll
- **WHEN** FormCadastroAluno em 1366x768 com scroll ativo é maximizado
- **THEN** altura aumenta para ~1020px
- **AND** todo conteúdo cabe sem scroll
- **AND** scrollbar desaparece automaticamente

### Requirement: Preserve scroll position on data refresh

Quando dados são atualizados (ex: refresh em FormConsultaEmprestimos), a posição do scroll SHALL ser preservada para não desorientar o usuário.

#### Scenario: Refresh button maintains scroll position
- **WHEN** usuário rola lista de empréstimos e clica "Atualizar"
- **THEN** dados são recarregados
- **AND** view permanece na mesma posição de scroll

## MODIFIED Requirements

### Requirement: DataGridView uses Dock instead of fixed Size

DataGridView em formulários de consulta SHALL usar `Dock = DockStyle.Fill` ao invés de tamanho fixo, permitindo expandir para preencher espaço disponível.

#### Scenario: DataGridView expands on maximize
- **WHEN** FormConsultaEmprestimos em tamanho normal (1100x700) mostrando 20 linhas é maximizado em 1920x1080
- **THEN** DataGridView expande verticalmente
- **AND** agora mostra ~35 linhas sem scroll
- **AND** colunas expandem proporcionalmente (AutoSizeColumnsMode.Fill)
