-- ================================================
-- SCRIPT SQL PARA PostgreSQL/Supabase - BibliotecaJK v3.1
-- ================================================
-- NOVA VERSÃO: Com pgcrypto, triggers inteligentes e notificações
-- Database: bibliokopke (ou o nome do seu projeto Supabase)

-- ================================================
-- EXTENSÕES
-- ================================================
CREATE EXTENSION IF NOT EXISTS pgcrypto;

-- ================================================
-- FUNÇÕES AUXILIARES
-- ================================================

-- Função para atualizar data_atualizacao automaticamente
CREATE OR REPLACE FUNCTION update_data_atualizacao()
RETURNS TRIGGER AS $$
BEGIN
    NEW.data_atualizacao = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Função para hash de senha usando crypt (bcrypt)
CREATE OR REPLACE FUNCTION hash_senha(senha TEXT)
RETURNS TEXT AS $$
BEGIN
    RETURN crypt(senha, gen_salt('bf', 11));
END;
$$ LANGUAGE plpgsql;

-- Função para verificar senha
CREATE OR REPLACE FUNCTION verificar_senha(senha TEXT, senha_hash TEXT)
RETURNS BOOLEAN AS $$
BEGIN
    RETURN senha_hash = crypt(senha, senha_hash);
END;
$$ LANGUAGE plpgsql;

-- ================================================
-- TABELA DE ALUNOS
-- ================================================
CREATE TABLE IF NOT EXISTS Aluno (
    id_aluno SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    matricula VARCHAR(50) NOT NULL UNIQUE,
    turma VARCHAR(50) NULL,
    telefone VARCHAR(20) NULL,
    email VARCHAR(255) NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_aluno_nome ON Aluno(nome);
CREATE INDEX IF NOT EXISTS idx_aluno_cpf ON Aluno(cpf);
CREATE INDEX IF NOT EXISTS idx_aluno_matricula ON Aluno(matricula);
CREATE INDEX IF NOT EXISTS idx_aluno_ativo ON Aluno(ativo);

COMMENT ON TABLE Aluno IS 'Tabela de alunos do sistema';

CREATE TRIGGER trigger_aluno_data_atualizacao
    BEFORE UPDATE ON Aluno
    FOR EACH ROW
    EXECUTE FUNCTION update_data_atualizacao();

-- ================================================
-- TABELA DE FUNCIONARIOS
-- ================================================
CREATE TABLE IF NOT EXISTS Funcionario (
    id_funcionario SERIAL PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    cargo VARCHAR(100) NULL,
    login VARCHAR(50) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL,
    perfil VARCHAR(50) NOT NULL DEFAULT 'OPERADOR', -- ADMIN, BIBLIOTECARIO, OPERADOR
    primeiro_login BOOLEAN NOT NULL DEFAULT TRUE,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_funcionario_nome ON Funcionario(nome);
CREATE INDEX IF NOT EXISTS idx_funcionario_cpf ON Funcionario(cpf);
CREATE INDEX IF NOT EXISTS idx_funcionario_login ON Funcionario(login);
CREATE INDEX IF NOT EXISTS idx_funcionario_perfil ON Funcionario(perfil);
CREATE INDEX IF NOT EXISTS idx_funcionario_ativo ON Funcionario(ativo);

COMMENT ON TABLE Funcionario IS 'Tabela de funcionarios do sistema';

CREATE TRIGGER trigger_funcionario_data_atualizacao
    BEFORE UPDATE ON Funcionario
    FOR EACH ROW
    EXECUTE FUNCTION update_data_atualizacao();

-- Trigger para hash automático de senha ao inserir/atualizar
CREATE OR REPLACE FUNCTION trigger_hash_senha_funcionario()
RETURNS TRIGGER AS $$
BEGIN
    -- Se a senha foi alterada e não é um hash bcrypt
    IF NEW.senha_hash IS DISTINCT FROM OLD.senha_hash OR TG_OP = 'INSERT' THEN
        -- Verificar se já não é um hash (começa com $2a$ ou $2b$)
        IF NEW.senha_hash NOT LIKE '$2a$%' AND NEW.senha_hash NOT LIKE '$2b$%' THEN
            NEW.senha_hash = hash_senha(NEW.senha_hash);
        END IF;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_hash_senha_funcionario
    BEFORE INSERT OR UPDATE ON Funcionario
    FOR EACH ROW
    EXECUTE FUNCTION trigger_hash_senha_funcionario();

-- ================================================
-- TABELA DE LIVROS
-- ================================================
CREATE TABLE IF NOT EXISTS Livro (
    id_livro SERIAL PRIMARY KEY,
    titulo VARCHAR(255) NOT NULL,
    autor VARCHAR(255) NULL,
    isbn VARCHAR(17) NULL UNIQUE,
    editora VARCHAR(100) NULL,
    ano_publicacao INT NULL,
    categoria VARCHAR(100) NULL,
    quantidade_total INT NOT NULL DEFAULT 1,
    quantidade_disponivel INT NOT NULL DEFAULT 1,
    localizacao VARCHAR(50) NULL,
    ativo BOOLEAN NOT NULL DEFAULT TRUE,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT chk_quantidade_total CHECK (quantidade_total >= 0),
    CONSTRAINT chk_quantidade_disponivel CHECK (quantidade_disponivel >= 0),
    CONSTRAINT chk_quantidade_logica CHECK (quantidade_disponivel <= quantidade_total)
);

CREATE INDEX IF NOT EXISTS idx_livro_titulo ON Livro(titulo);
CREATE INDEX IF NOT EXISTS idx_livro_autor ON Livro(autor);
CREATE INDEX IF NOT EXISTS idx_livro_isbn ON Livro(isbn);
CREATE INDEX IF NOT EXISTS idx_livro_categoria ON Livro(categoria);
CREATE INDEX IF NOT EXISTS idx_livro_ativo ON Livro(ativo);

COMMENT ON TABLE Livro IS 'Tabela de livros do acervo';

CREATE TRIGGER trigger_livro_data_atualizacao
    BEFORE UPDATE ON Livro
    FOR EACH ROW
    EXECUTE FUNCTION update_data_atualizacao();

-- ================================================
-- TABELA DE EMPRESTIMOS
-- ================================================
CREATE TABLE IF NOT EXISTS Emprestimo (
    id_emprestimo SERIAL PRIMARY KEY,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    id_funcionario INT NULL, -- Funcionário que realizou o empréstimo
    data_emprestimo DATE NOT NULL DEFAULT CURRENT_DATE,
    data_prevista DATE NOT NULL,
    data_devolucao DATE NULL,
    multa DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    status VARCHAR(20) NOT NULL DEFAULT 'ATIVO', -- ATIVO, DEVOLVIDO, ATRASADO
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_emprestimo_aluno FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno) ON DELETE CASCADE,
    CONSTRAINT fk_emprestimo_livro FOREIGN KEY (id_livro) REFERENCES Livro(id_livro) ON DELETE CASCADE,
    CONSTRAINT fk_emprestimo_funcionario FOREIGN KEY (id_funcionario) REFERENCES Funcionario(id_funcionario) ON DELETE SET NULL,
    CONSTRAINT chk_data_prevista CHECK (data_prevista >= data_emprestimo),
    CONSTRAINT chk_data_devolucao CHECK (data_devolucao IS NULL OR data_devolucao >= data_emprestimo)
);

CREATE INDEX IF NOT EXISTS idx_emprestimo_aluno ON Emprestimo(id_aluno);
CREATE INDEX IF NOT EXISTS idx_emprestimo_livro ON Emprestimo(id_livro);
CREATE INDEX IF NOT EXISTS idx_emprestimo_funcionario ON Emprestimo(id_funcionario);
CREATE INDEX IF NOT EXISTS idx_emprestimo_status ON Emprestimo(status);
CREATE INDEX IF NOT EXISTS idx_emprestimo_data_emprestimo ON Emprestimo(data_emprestimo);
CREATE INDEX IF NOT EXISTS idx_emprestimo_data_prevista ON Emprestimo(data_prevista);

COMMENT ON TABLE Emprestimo IS 'Tabela de emprestimos de livros';

-- Trigger para atualizar quantidade disponível ao criar empréstimo
CREATE OR REPLACE FUNCTION trigger_emprestimo_insert()
RETURNS TRIGGER AS $$
BEGIN
    -- Verificar se há livros disponíveis
    IF (SELECT quantidade_disponivel FROM Livro WHERE id_livro = NEW.id_livro) <= 0 THEN
        RAISE EXCEPTION 'Livro sem exemplares disponíveis';
    END IF;

    -- Decrementar quantidade disponível
    UPDATE Livro
    SET quantidade_disponivel = quantidade_disponivel - 1
    WHERE id_livro = NEW.id_livro;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_emprestimo_insert
    AFTER INSERT ON Emprestimo
    FOR EACH ROW
    EXECUTE FUNCTION trigger_emprestimo_insert();

-- Trigger para atualizar quantidade disponível ao devolver
CREATE OR REPLACE FUNCTION trigger_emprestimo_devolucao()
RETURNS TRIGGER AS $$
BEGIN
    -- Se está devolvendo (data_devolucao foi preenchida)
    IF NEW.data_devolucao IS NOT NULL AND OLD.data_devolucao IS NULL THEN
        -- Incrementar quantidade disponível
        UPDATE Livro
        SET quantidade_disponivel = quantidade_disponivel + 1
        WHERE id_livro = NEW.id_livro;

        -- Atualizar status
        NEW.status = 'DEVOLVIDO';

        -- Calcular multa se atrasado (R$ 2,00 por dia)
        IF NEW.data_devolucao > NEW.data_prevista THEN
            NEW.multa = (NEW.data_devolucao - NEW.data_prevista) * 2.00;
        END IF;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_emprestimo_devolucao
    BEFORE UPDATE ON Emprestimo
    FOR EACH ROW
    EXECUTE FUNCTION trigger_emprestimo_devolucao();

-- ================================================
-- TABELA DE RESERVAS
-- ================================================
CREATE TABLE IF NOT EXISTS Reserva (
    id_reserva SERIAL PRIMARY KEY,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    data_reserva DATE NOT NULL DEFAULT CURRENT_DATE,
    data_expiracao DATE NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'ATIVA', -- ATIVA, CANCELADA, CONCLUIDA, EXPIRADA
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_reserva_aluno FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno) ON DELETE CASCADE,
    CONSTRAINT fk_reserva_livro FOREIGN KEY (id_livro) REFERENCES Livro(id_livro) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_reserva_aluno ON Reserva(id_aluno);
CREATE INDEX IF NOT EXISTS idx_reserva_livro ON Reserva(id_livro);
CREATE INDEX IF NOT EXISTS idx_reserva_status ON Reserva(status);
CREATE INDEX IF NOT EXISTS idx_reserva_data_reserva ON Reserva(data_reserva);
CREATE INDEX IF NOT EXISTS idx_reserva_data_expiracao ON Reserva(data_expiracao);

COMMENT ON TABLE Reserva IS 'Tabela de reservas de livros';

-- Trigger para definir data de expiração (7 dias após reserva)
CREATE OR REPLACE FUNCTION trigger_reserva_expiracao()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.data_expiracao IS NULL THEN
        NEW.data_expiracao = NEW.data_reserva + INTERVAL '7 days';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_reserva_expiracao
    BEFORE INSERT ON Reserva
    FOR EACH ROW
    EXECUTE FUNCTION trigger_reserva_expiracao();

-- ================================================
-- TABELA DE LOGS DE ACOES
-- ================================================
CREATE TABLE IF NOT EXISTS Log_Acao (
    id_log SERIAL PRIMARY KEY,
    id_funcionario INT NULL,
    acao VARCHAR(100) NULL,
    descricao TEXT NULL,
    data_hora TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_log_funcionario FOREIGN KEY (id_funcionario) REFERENCES Funcionario(id_funcionario) ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS idx_log_funcionario ON Log_Acao(id_funcionario);
CREATE INDEX IF NOT EXISTS idx_log_acao ON Log_Acao(acao);
CREATE INDEX IF NOT EXISTS idx_log_data_hora ON Log_Acao(data_hora);

COMMENT ON TABLE Log_Acao IS 'Tabela de logs de acoes do sistema';

-- ================================================
-- TABELA DE NOTIFICAÇÕES
-- ================================================
CREATE TABLE IF NOT EXISTS Notificacao (
    id_notificacao SERIAL PRIMARY KEY,
    tipo VARCHAR(50) NOT NULL, -- EMPRESTIMO_ATRASADO, RESERVA_EXPIRADA, LIVRO_DISPONIVEL, etc
    titulo VARCHAR(255) NOT NULL,
    mensagem TEXT NOT NULL,
    id_aluno INT NULL,
    id_funcionario INT NULL,
    id_emprestimo INT NULL,
    id_reserva INT NULL,
    lida BOOLEAN NOT NULL DEFAULT FALSE,
    prioridade VARCHAR(20) NOT NULL DEFAULT 'NORMAL', -- BAIXA, NORMAL, ALTA, URGENTE
    data_criacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_leitura TIMESTAMP NULL,

    CONSTRAINT fk_notificacao_aluno FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno) ON DELETE CASCADE,
    CONSTRAINT fk_notificacao_funcionario FOREIGN KEY (id_funcionario) REFERENCES Funcionario(id_funcionario) ON DELETE CASCADE,
    CONSTRAINT fk_notificacao_emprestimo FOREIGN KEY (id_emprestimo) REFERENCES Emprestimo(id_emprestimo) ON DELETE CASCADE,
    CONSTRAINT fk_notificacao_reserva FOREIGN KEY (id_reserva) REFERENCES Reserva(id_reserva) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_notificacao_tipo ON Notificacao(tipo);
CREATE INDEX IF NOT EXISTS idx_notificacao_lida ON Notificacao(lida);
CREATE INDEX IF NOT EXISTS idx_notificacao_prioridade ON Notificacao(prioridade);
CREATE INDEX IF NOT EXISTS idx_notificacao_data_criacao ON Notificacao(data_criacao);
CREATE INDEX IF NOT EXISTS idx_notificacao_aluno ON Notificacao(id_aluno);
CREATE INDEX IF NOT EXISTS idx_notificacao_funcionario ON Notificacao(id_funcionario);

COMMENT ON TABLE Notificacao IS 'Tabela de notificações do sistema';

-- ================================================
-- FUNÇÃO PARA CRIAR NOTIFICAÇÕES
-- ================================================
CREATE OR REPLACE FUNCTION criar_notificacao(
    p_tipo VARCHAR,
    p_titulo VARCHAR,
    p_mensagem TEXT,
    p_id_aluno INT DEFAULT NULL,
    p_id_funcionario INT DEFAULT NULL,
    p_id_emprestimo INT DEFAULT NULL,
    p_id_reserva INT DEFAULT NULL,
    p_prioridade VARCHAR DEFAULT 'NORMAL'
)
RETURNS INT AS $$
DECLARE
    v_id_notificacao INT;
BEGIN
    INSERT INTO Notificacao (
        tipo, titulo, mensagem,
        id_aluno, id_funcionario, id_emprestimo, id_reserva,
        prioridade
    ) VALUES (
        p_tipo, p_titulo, p_mensagem,
        p_id_aluno, p_id_funcionario, p_id_emprestimo, p_id_reserva,
        p_prioridade
    )
    RETURNING id_notificacao INTO v_id_notificacao;

    RETURN v_id_notificacao;
END;
$$ LANGUAGE plpgsql;

-- ================================================
-- PROCEDURE PARA ATUALIZAR STATUS DE EMPRÉSTIMOS
-- ================================================
CREATE OR REPLACE FUNCTION atualizar_status_emprestimos()
RETURNS void AS $$
DECLARE
    v_emprestimo RECORD;
BEGIN
    -- Atualizar empréstimos atrasados
    FOR v_emprestimo IN
        SELECT e.*, a.nome as nome_aluno, l.titulo as titulo_livro
        FROM Emprestimo e
        JOIN Aluno a ON e.id_aluno = a.id_aluno
        JOIN Livro l ON e.id_livro = l.id_livro
        WHERE e.status = 'ATIVO'
        AND e.data_prevista < CURRENT_DATE
        AND e.data_devolucao IS NULL
    LOOP
        -- Atualizar status
        UPDATE Emprestimo
        SET status = 'ATRASADO'
        WHERE id_emprestimo = v_emprestimo.id_emprestimo;

        -- Criar notificação se não existir uma recente
        IF NOT EXISTS (
            SELECT 1 FROM Notificacao
            WHERE id_emprestimo = v_emprestimo.id_emprestimo
            AND tipo = 'EMPRESTIMO_ATRASADO'
            AND data_criacao > CURRENT_DATE - INTERVAL '1 day'
        ) THEN
            PERFORM criar_notificacao(
                'EMPRESTIMO_ATRASADO',
                'Empréstimo Atrasado!',
                format('O aluno %s está com o livro "%s" atrasado há %s dia(s). Multa acumulada: R$ %.2f',
                    v_emprestimo.nome_aluno,
                    v_emprestimo.titulo_livro,
                    CURRENT_DATE - v_emprestimo.data_prevista,
                    (CURRENT_DATE - v_emprestimo.data_prevista) * 2.00
                ),
                v_emprestimo.id_aluno,
                NULL,
                v_emprestimo.id_emprestimo,
                NULL,
                'ALTA'
            );
        END IF;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

-- ================================================
-- PROCEDURE PARA VERIFICAR RESERVAS EXPIRADAS
-- ================================================
CREATE OR REPLACE FUNCTION atualizar_status_reservas()
RETURNS void AS $$
DECLARE
    v_reserva RECORD;
BEGIN
    -- Atualizar reservas expiradas
    FOR v_reserva IN
        SELECT r.*, a.nome as nome_aluno, l.titulo as titulo_livro
        FROM Reserva r
        JOIN Aluno a ON r.id_aluno = a.id_aluno
        JOIN Livro l ON r.id_livro = l.id_livro
        WHERE r.status = 'ATIVA'
        AND r.data_expiracao < CURRENT_DATE
    LOOP
        -- Atualizar status
        UPDATE Reserva
        SET status = 'EXPIRADA'
        WHERE id_reserva = v_reserva.id_reserva;

        -- Criar notificação
        PERFORM criar_notificacao(
            'RESERVA_EXPIRADA',
            'Reserva Expirada',
            format('A reserva do livro "%s" para o aluno %s expirou.',
                v_reserva.titulo_livro,
                v_reserva.nome_aluno
            ),
            v_reserva.id_aluno,
            NULL,
            NULL,
            v_reserva.id_reserva,
            'NORMAL'
        );
    END LOOP;
END;
$$ LANGUAGE plpgsql;

-- ================================================
-- VIEWS ÚTEIS
-- ================================================

-- View para empréstimos ativos/atrasados
CREATE OR REPLACE VIEW vw_emprestimos_status AS
SELECT
    e.id_emprestimo,
    e.status,
    a.id_aluno,
    a.nome as nome_aluno,
    a.matricula,
    l.id_livro,
    l.titulo as titulo_livro,
    l.autor,
    e.data_emprestimo,
    e.data_prevista,
    e.data_devolucao,
    CASE
        WHEN e.data_devolucao IS NULL AND CURRENT_DATE > e.data_prevista
        THEN (CURRENT_DATE - e.data_prevista)
        ELSE 0
    END as dias_atraso,
    CASE
        WHEN e.data_devolucao IS NULL AND CURRENT_DATE > e.data_prevista
        THEN (CURRENT_DATE - e.data_prevista) * 2.00
        ELSE e.multa
    END as multa_calculada,
    e.multa as multa_registrada
FROM Emprestimo e
INNER JOIN Aluno a ON e.id_aluno = a.id_aluno
INNER JOIN Livro l ON e.id_livro = l.id_livro;

-- View para livros disponíveis
CREATE OR REPLACE VIEW vw_livros_disponiveis AS
SELECT
    id_livro,
    titulo,
    autor,
    isbn,
    editora,
    ano_publicacao,
    categoria,
    quantidade_total,
    quantidade_disponivel,
    localizacao,
    ativo
FROM Livro
WHERE quantidade_disponivel > 0 AND ativo = TRUE;

-- View para notificações não lidas
CREATE OR REPLACE VIEW vw_notificacoes_pendentes AS
SELECT
    n.*,
    a.nome as nome_aluno,
    f.nome as nome_funcionario
FROM Notificacao n
LEFT JOIN Aluno a ON n.id_aluno = a.id_aluno
LEFT JOIN Funcionario f ON n.id_funcionario = f.id_funcionario
WHERE n.lida = FALSE
ORDER BY
    CASE n.prioridade
        WHEN 'URGENTE' THEN 1
        WHEN 'ALTA' THEN 2
        WHEN 'NORMAL' THEN 3
        WHEN 'BAIXA' THEN 4
    END,
    n.data_criacao DESC;

-- ================================================
-- DADOS INICIAIS
-- ================================================

-- Inserir funcionário administrador padrão
-- Senha será "admin123" (será hashada automaticamente pelo trigger)
INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil)
VALUES ('Administrador', '000.000.000-00', 'Administrador do Sistema', 'admin', 'admin123', 'ADMIN')
ON CONFLICT (login) DO NOTHING;

-- Inserir alguns alunos de exemplo
INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email) VALUES
('João Silva', '111.111.111-11', 'MAT001', '1A', '(11) 98888-1111', 'joao@email.com'),
('Maria Santos', '222.222.222-22', 'MAT002', '1A', '(11) 98888-2222', 'maria@email.com'),
('Pedro Oliveira', '333.333.333-33', 'MAT003', '2B', '(11) 98888-3333', 'pedro@email.com')
ON CONFLICT (cpf) DO NOTHING;

-- Inserir alguns livros de exemplo
INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, categoria, quantidade_total, quantidade_disponivel, localizacao) VALUES
('Dom Casmurro', 'Machado de Assis', '978-85-7326-981-6', 'Editora Garnier', 1899, 'Literatura Brasileira', 3, 3, 'A-001'),
('O Cortiço', 'Aluísio Azevedo', '978-85-08-12345-6', 'Editora Ática', 1890, 'Literatura Brasileira', 2, 2, 'A-002'),
('1984', 'George Orwell', '978-85-359-0277-4', 'Companhia das Letras', 1949, 'Ficção Científica', 5, 5, 'B-001'),
('O Pequeno Príncipe', 'Antoine de Saint-Exupéry', '978-85-220-0826-7', 'Editora Agir', 1943, 'Literatura Infantil', 4, 4, 'C-001'),
('Matemática Básica', 'José Silva', '978-85-16-12345-8', 'Editora Moderna', 2020, 'Didático', 10, 10, 'D-001')
ON CONFLICT (isbn) DO NOTHING;

-- ================================================
-- FINALIZADO
-- ================================================
-- Execute este script no Supabase SQL Editor ou no psql
-- Para Supabase: Vá em "SQL Editor" e cole todo este script
-- Para PostgreSQL local: psql -U usuario -d bibliokopke -f schema-postgresql-v2.sql
