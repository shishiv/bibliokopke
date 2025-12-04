# Spec: Responsive Layouts

**Capability**: responsive-layouts
**Change**: improve-ui-responsiveness
**Dependencies**: adaptive-form-sizing

## MODIFIED Requirements

### Requirement: FormPrincipal dashboard uses TableLayoutPanel

Dashboard de estatísticas SHALL migrar de posições fixas para TableLayoutPanel com 3 colunas em grid percentual (33.33% cada), permitindo cards se redimensionarem automaticamente baseado no espaço disponível.

#### Scenario: Dashboard cards resize on form resize
- **WHEN** usuário redimensiona FormPrincipal de 1920x1080 para 1366x768
- **THEN** TableLayoutPanel recalcula largura das colunas proporcionalmente
- **AND** cada card fica menor mas mantém proporção 33.33%
- **AND** conteúdo dos cards permanece legível

### Requirement: Form inputs use TableLayoutPanel for label-field pairs

Formulários de cadastro SHALL usar TableLayoutPanel com 2 colunas: coluna fixa para labels (150px) e coluna percentual para inputs (100%).

#### Scenario: Form fields expand on large screen
- **WHEN** FormCadastroAluno aberto em 1920x1080
- **THEN** labels mantêm largura fixa de 150px
- **AND** TextBoxes expandem para preencher espaço restante (~1500px)

### Requirement: Action buttons use FlowLayoutPanel

Painéis com múltiplos botões SHALL usar FlowLayoutPanel com `FlowDirection = LeftToRight` e `WrapContents = true` para reorganizar botões em múltiplas linhas se necessário.

#### Scenario: Action buttons wrap on small screen
- **WHEN** FormConsultaEmprestimos com 8 botões aberto em 1366x768
- **THEN** FlowLayoutPanel coloca 6 botões na primeira linha
- **AND** 2 botões restantes vão para segunda linha
- **AND** height ajusta automaticamente

## ADDED Requirements

### Requirement: SuspendLayout/ResumeLayout pattern

Todos formulários com TableLayoutPanel ou FlowLayoutPanel SHALL implementar SuspendLayout/ResumeLayout para evitar flickering durante inicialização.

#### Scenario: Form loads without flickering
- **WHEN** FormPrincipal é aberto
- **THEN** layout calculado uma única vez após todos controles adicionados
- **AND** não há flickering visível
- **AND** render time < 100ms
