# Diagrama de Classes - App Biblioteca Fronteira

```mermaid
classDiagram
    class Usuario {
        +int codigo_simade
        +String codigo_inep
        +String nome_completo
        +Date data_nascimento
        +String cpf
        +String email
        +String telefone
        +String endereco
        +TipoUsuario tipo_usuario
        +String nome_filiacao
        +CorRaca cor_raca
        +Sexo sexo
        +EstadoCivil estado_civil
        +String nacionalidade
        +String uf_nascimento
        +String municipio_nascimento
        +boolean ativo
        +Date data_cadastro
        +Date data_atualizacao
    }

    class Livro {
        +int id_livro
        +String isbn
        +String titulo
        +String autor
        +String editora
        +int ano_publicacao
        +String categoria
        +int numero_paginas
        +String idioma
        +String sinopse
        +String localizacao
        +StatusLivro status
        +int quantidade_total
        +int quantidade_disponivel
        +Date data_cadastro
        +Date data_atualizacao
    }

    class Emprestimo {
        +int id_emprestimo
        +int codigo_simade
        +int id_livro
        +Date data_emprestimo
        +Date data_devolucao_prevista
        +Date data_devolucao_real
        +StatusEmprestimo status
        +String observacoes
        +int renovacoes
        +Date data_cadastro
    }

    class Reserva {
        +int id_reserva
        +int codigo_simade
        +int id_livro
        +Date data_reserva
        +Date data_validade
        +StatusReserva status
        +String motivo_cancelamento
        +Date data_cadastro
    }

    class HistoricoEmprestimo {
        +int id_historico
        +int id_emprestimo
        +int codigo_simade
        +int id_livro
        +Date data_emprestimo
        +Date data_devolucao
        +int dias_atraso
        +decimal multa
        +String observacoes
        +Date data_registro
    }

    class LogSistema {
        +int id_log
        +int codigo_simade
        +String tabela_afetada
        +AcaoLog acao
        +JSON dados_anteriores
        +JSON dados_novos
        +String ip_usuario
        +Date data_acao
    }

    class Relatorio {
        +int id_relatorio
        +String tipo_relatorio
        +String periodo
        +JSON dados_relatorio
        +int codigo_simade
        +Date data_geracao
        +String arquivo_gerado
    }

    Usuario "1" o-- "0..*" Emprestimo : realiza
    Usuario "1" o-- "0..*" Reserva : faz
    Usuario "1" o-- "0..*" HistoricoEmprestimo : possui
    Usuario "1" o-- "0..*" LogSistema : executa
    Usuario "1" o-- "0..*" Relatorio : gera

    Livro "1" o-- "0..*" Emprestimo : é emprestado
    Livro "1" o-- "0..*" Reserva : é reservado
    Livro "1" o-- "0..*" HistoricoEmprestimo : está no histórico

    Emprestimo "1" o-- "0..1" HistoricoEmprestimo : gera

    %% Enums
    class TipoUsuario {
        <<enumeration>>
        ALUNO
        PROFESSOR
        BIBLIOTECARIO
    }
    class CorRaca {
        <<enumeration>>
        Branca
        Preta
        Parda
        Amarela
        Indígena
        Não_declarada
    }
    class Sexo {
        <<enumeration>>
        Masculino
        Feminino
    }
    class EstadoCivil {
        <<enumeration>>
        Solteiro
        Casado
        Divorciado
        Viúvo
    }
    class StatusLivro {
        <<enumeration>>
        DISPONIVEL
        EMPRESTADO
        RESERVADO
        MANUTENCAO
    }
    class StatusEmprestimo {
        <<enumeration>>
        ATIVO
        DEVOLVIDO
        ATRASADO
        RENOVADO
    }
    class StatusReserva {
        <<enumeration>>
        ATIVA
        CANCELADA
        ATENDIDA
        EXPIRADA
    }
    class AcaoLog {
        <<enumeration>>
        INSERT
        UPDATE
        DELETE
        SELECT
    }
