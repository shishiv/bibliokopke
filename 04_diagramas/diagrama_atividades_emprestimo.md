# Diagrama de Atividades - Fluxo de Empréstimo de Livro

```mermaid
flowchart TD
    A[Início] --> B[Usuário solicita empréstimo ao bibliotecário]
    B --> C[Verificar disponibilidade do livro]
    C -->|Disponível| D[Registrar empréstimo no sistema]
    D --> E[Atualizar quantidade disponível e status do livro]
    E --> F[Gerar log de auditoria]
    F --> G[Entregar livro ao usuário]
    G --> H[Fim]

    C -->|Indisponível| I[Informar usuário sobre indisponibilidade]
    I --> H
