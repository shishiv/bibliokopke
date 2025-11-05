# Diagrama de Casos de Uso - App Biblioteca Fronteira

```mermaid
graph TD
    %% Atores
    A[Aluno] 
    P[Professor]
    B[Bibliotecário]
    S[Sistema SIMADE]

    %% Casos de Uso - Aluno
    subgraph "Aluno"
        UC1[Visualizar Catálogo de Livros]
        UC2[Pesquisar Livros]
        UC3[Reservar Livro]
        UC4[Visualizar Seus Empréstimos]
        UC5[Renovar Empréstimo]
        UC6[Consultar Histórico]
        UC7[Cancelar Reserva]
    end

    %% Casos de Uso - Professor
    subgraph "Professor"
        UC8[Solicitar Reserva para Aulas]
        UC9[Visualizar Histórico de Empréstimos]
        UC10[Sugerir Aquisição de Livros]
        UC11[Acessar Relatórios Básicos]
    end

    %% Casos de Uso - Bibliotecário
    subgraph "Bibliotecário"
        UC12[Cadastrar Livro]
        UC13[Editar Informações do Livro]
        UC14[Remover Livro do Acervo]
        UC15[Registrar Empréstimo]
        UC16[Registrar Devolução]
        UC17[Gerenciar Reservas]
        UC18[Gerar Relatórios Gerenciais]
        UC19[Consultar Usuários]
        UC20[Aplicar Multas]
        UC21[Configurar Sistema]
        UC22[Fazer Backup]
        UC23[Importar Dados do SIMADE]
    end

    %% Casos de Uso - Sistema
    subgraph "Sistema"
        UC24[Sincronizar com SIMADE]
        UC25[Enviar Notificações]
        UC26[Calcular Multas Automaticamente]
        UC27[Expirar Reservas]
        UC28[Gerar Logs de Auditoria]
    end

    %% Relacionamentos Aluno
    A --> UC1
    A --> UC2
    A --> UC3
    A --> UC4
    A --> UC5
    A --> UC6
    A --> UC7

    %% Relacionamentos Professor
    P --> UC1
    P --> UC2
    P --> UC8
    P --> UC9
    P --> UC10
    P --> UC11

    %% Relacionamentos Bibliotecário
    B --> UC12
    B --> UC13
    B --> UC14
    B --> UC15
    B --> UC16
    B --> UC17
    B --> UC18
    B --> UC19
    B --> UC20
    B --> UC21
    B --> UC22
    B --> UC23
    B --> UC1
    B --> UC2

    %% Relacionamentos Sistema
    S --> UC24
    UC24 --> UC23
    UC25 --> UC4
    UC26 --> UC20
    UC27 --> UC17
    UC28 --> UC21

    %% Extends e Includes
    UC3 -.->|extends| UC25
    UC15 -.->|extends| UC25
    UC16 -.->|extends| UC26
    UC5 -.->|includes| UC4
    UC7 -.->|includes| UC3
    UC18 -.->|includes| UC19
