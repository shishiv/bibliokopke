# Diagrama de Sequência - Fluxo de Empréstimo de Livro

```mermaid
sequenceDiagram
    participant U as Usuário
    participant B as Bibliotecário
    participant S as Sistema
    participant DB as Banco de Dados

    U->>B: Solicita empréstimo de livro
    B->>S: Inicia processo de empréstimo
    S->>DB: Verifica disponibilidade do livro
    DB-->>S: Livro disponível?
    alt Disponível
        S->>DB: Registra empréstimo (tabela emprestimo)
        S->>DB: Atualiza quantidade_disponivel e status do livro
        S->>DB: Gera log de auditoria
        S-->>B: Confirmação de empréstimo realizado
        B-->>U: Entrega livro e confirma empréstimo
    else Indisponível
        S-->>B: Informa indisponibilidade do livro
        B-->>U: Notifica usuário sobre indisponibilidade
    end
