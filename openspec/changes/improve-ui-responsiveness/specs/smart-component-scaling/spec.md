# Spec: Smart Component Scaling

**Capability**: smart-component-scaling
**Change**: improve-ui-responsiveness
**Dependencies**: responsive-layouts

## MODIFIED Requirements

### Requirement: Replace absolute positioning with Anchor/Dock

Controles dentro de panels SHALL usar `Anchor` ou `Dock` ao invés de `Location` e `Size` fixos para se adaptar ao redimensionamento do container pai.

#### Scenario: Button anchored to bottom-right
- **WHEN** FormCadastroAluno com botão "Salvar" ancorado bottom-right é maximizado de 900x550 para 1920x1080
- **THEN** botão permanece no canto inferior direito
- **AND** distância até bordas permanece constante
- **AND** posição ajusta automaticamente

#### Scenario: TextBox expands horizontally
- **WHEN** FormCadastroAluno expande de 900px para 1400px largura
- **AND** TextBox "Nome" tem Anchor = Left | Right
- **THEN** TextBox expande proporcionalmente de 400px para ~900px
- **AND** labels à esquerda permanecem fixos em 150px

### Requirement: Sidebar uses Dock.Left with fixed width

Sidebar de navegação no FormPrincipal SHALL usar `Dock = DockStyle.Left` com `Width` fixo (250px), permitindo área de conteúdo usar o espaço restante automaticamente.

#### Scenario: Sidebar stays fixed, content expands
- **WHEN** FormPrincipal redimensionado de 1200px para 1920px largura
- **THEN** sidebar permanece 250px
- **AND** content area expande de 950px para 1670px
- **AND** dashboard cards se redistribuem

## ADDED Requirements

### Requirement: Button panels anchor to specific edges

Panels contendo botões SHALL ancorar à borda apropriada do formulário (geralmente bottom ou bottom-right).

#### Scenario: Button panel stays at bottom on resize
- **WHEN** FormCadastroLivro com FlowLayoutPanel de botões docked bottom é redimensionado verticalmente de 550px para 800px
- **THEN** button panel permanece grudado na parte inferior
- **AND** altura do panel permanece 60px
- **AND** content area acima expande para usar espaço adicional
