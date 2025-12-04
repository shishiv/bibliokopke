# Spec: Adaptive Form Sizing

**Capability**: adaptive-form-sizing
**Change**: improve-ui-responsiveness

## MODIFIED Requirements

### Requirement: Forms adapt to screen resolution

Todos os formulários SHALL detectar a resolução da tela usando `Screen.PrimaryScreen.WorkingArea` e ajustar `ClientSize` automaticamente para 90% da área disponível (mínimo 1200x650), garantindo visibilidade de todos elementos sem scroll inicial quando possível. Afeta todos os 13 formulários principais em `06_bibliotecaJK/Forms/*.cs`.

#### Scenario: Form opens on 1366x768 notebook
- **WHEN** usuário abre FormPrincipal em notebook 1366x768
- **THEN** formulário abre com tamanho ~1316x680 (90% da área de trabalho)
- **AND** todos elementos visíveis sem scroll
- **AND** formulário centralizado na tela

#### Scenario: Form opens on 1920x1080 desktop
- **WHEN** usuário abre FormPrincipal em desktop 1920x1080
- **THEN** formulário abre com tamanho ~1728x972 (90% da área)
- **AND** elementos usam espaço adicional

### Requirement: Form minimum size supports target resolution

MinimumSize de cada formulário SHALL ser reduzido para caber em 1366x768. FormPrincipal: 1200x650 (era 1200x700). FormConsultaEmprestimos: 1000x600. Formulários de cadastro: 900x550.

#### Scenario: User resizes below minimum
- **WHEN** usuário tenta redimensionar FormPrincipal para menor que 1200x650
- **THEN** Windows impede redimensionamento além do MinimumSize
- **AND** conteúdo permanece acessível

## ADDED Requirements

### Requirement: StartPosition set to CenterScreen

Todos formulários principais SHALL usar `StartPosition = FormStartPosition.CenterScreen` para centralização automática independente de resolução.

#### Scenario: Form opens centered
- **WHEN** usuário abre qualquer formulário principal
- **THEN** formulário aparece centralizado horizontal e verticalmente
