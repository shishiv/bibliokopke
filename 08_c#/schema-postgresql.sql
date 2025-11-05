-- ================================================
-- SCRIPT SQL PARA PostgreSQL/Supabase - BibliotecaJK
-- ================================================
-- Este e o schema para usar com Supabase ou PostgreSQL local
-- Database: bibliokopke (ou o nome do seu projeto Supabase)

-- ================================================
-- EXTENSOES (se necessario)
-- ================================================
-- No Supabase, essas extensoes ja estao habilitadas
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

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
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_aluno_nome ON Aluno(nome);
CREATE INDEX IF NOT EXISTS idx_aluno_cpf ON Aluno(cpf);
CREATE INDEX IF NOT EXISTS idx_aluno_matricula ON Aluno(matricula);

COMMENT ON TABLE Aluno IS 'Tabela de alunos do sistema';

-- Trigger para atualizar data_atualizacao automaticamente
CREATE OR REPLACE FUNCTION update_data_atualizacao()
RETURNS TRIGGER AS $$
BEGIN
    NEW.data_atualizacao = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

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
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_funcionario_nome ON Funcionario(nome);
CREATE INDEX IF NOT EXISTS idx_funcionario_cpf ON Funcionario(cpf);
CREATE INDEX IF NOT EXISTS idx_funcionario_login ON Funcionario(login);
CREATE INDEX IF NOT EXISTS idx_funcionario_perfil ON Funcionario(perfil);

COMMENT ON TABLE Funcionario IS 'Tabela de funcionarios do sistema';

CREATE TRIGGER trigger_funcionario_data_atualizacao
    BEFORE UPDATE ON Funcionario
    FOR EACH ROW
    EXECUTE FUNCTION update_data_atualizacao();

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
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS idx_livro_titulo ON Livro(titulo);
CREATE INDEX IF NOT EXISTS idx_livro_autor ON Livro(autor);
CREATE INDEX IF NOT EXISTS idx_livro_isbn ON Livro(isbn);
CREATE INDEX IF NOT EXISTS idx_livro_categoria ON Livro(categoria);

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
    data_emprestimo DATE NOT NULL,
    data_prevista DATE NOT NULL,
    data_devolucao DATE NULL,
    multa DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_emprestimo_aluno FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno) ON DELETE CASCADE,
    CONSTRAINT fk_emprestimo_livro FOREIGN KEY (id_livro) REFERENCES Livro(id_livro) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_emprestimo_aluno ON Emprestimo(id_aluno);
CREATE INDEX IF NOT EXISTS idx_emprestimo_livro ON Emprestimo(id_livro);
CREATE INDEX IF NOT EXISTS idx_emprestimo_data_emprestimo ON Emprestimo(data_emprestimo);
CREATE INDEX IF NOT EXISTS idx_emprestimo_data_prevista ON Emprestimo(data_prevista);

COMMENT ON TABLE Emprestimo IS 'Tabela de emprestimos de livros';

-- ================================================
-- TABELA DE RESERVAS
-- ================================================
CREATE TABLE IF NOT EXISTS Reserva (
    id_reserva SERIAL PRIMARY KEY,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    data_reserva DATE NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'ATIVA', -- ATIVA, CANCELADA, CONCLUIDA
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_reserva_aluno FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno) ON DELETE CASCADE,
    CONSTRAINT fk_reserva_livro FOREIGN KEY (id_livro) REFERENCES Livro(id_livro) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_reserva_aluno ON Reserva(id_aluno);
CREATE INDEX IF NOT EXISTS idx_reserva_livro ON Reserva(id_livro);
CREATE INDEX IF NOT EXISTS idx_reserva_status ON Reserva(status);
CREATE INDEX IF NOT EXISTS idx_reserva_data_reserva ON Reserva(data_reserva);

COMMENT ON TABLE Reserva IS 'Tabela de reservas de livros';

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
-- DADOS INICIAIS PARA TESTES
-- ================================================

-- Inserir funcionario administrador padrao
-- Senha: "admin123" (hash BCrypt seguro)
-- Hash gerado com BCrypt.Net-Next usando fator de custo 11
INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil)
VALUES ('Administrador', '000.000.000-00', 'Administrador do Sistema', 'admin', '$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'ADMIN')
ON CONFLICT (login) DO NOTHING;

-- Inserir alguns alunos de exemplo
INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email) VALUES
('Joao Silva', '111.111.111-11', 'MAT001', '1A', '(11) 98888-1111', 'joao@email.com'),
('Maria Santos', '222.222.222-22', 'MAT002', '1A', '(11) 98888-2222', 'maria@email.com'),
('Pedro Oliveira', '333.333.333-33', 'MAT003', '2B', '(11) 98888-3333', 'pedro@email.com')
ON CONFLICT (cpf) DO NOTHING;

-- Inserir alguns livros de exemplo
INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, categoria, quantidade_total, quantidade_disponivel, localizacao) VALUES
('Dom Casmurro', 'Machado de Assis', '978-85-7326-981-6', 'Editora Garnier', 1899, 'Literatura Brasileira', 3, 3, 'A-001'),
('O Cortico', 'Aluisio Azevedo', '978-85-08-12345-6', 'Editora Atica', 1890, 'Literatura Brasileira', 2, 2, 'A-002'),
('1984', 'George Orwell', '978-85-359-0277-4', 'Companhia das Letras', 1949, 'Ficcao Cientifica', 5, 5, 'B-001'),
('O Pequeno Principe', 'Antoine de Saint-Exupery', '978-85-220-0826-7', 'Editora Agir', 1943, 'Literatura Infantil', 4, 4, 'C-001'),
('Matematica Basica', 'Jose Silva', '978-85-16-12345-8', 'Editora Moderna', 2020, 'Didatico', 10, 10, 'D-001')
ON CONFLICT (isbn) DO NOTHING;

-- ================================================
-- VIEWS UTEIS
-- ================================================

-- View para emprestimos ativos
CREATE OR REPLACE VIEW vw_emprestimos_ativos AS
SELECT
    e.id_emprestimo,
    a.nome as nome_aluno,
    a.matricula,
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
    e.multa
FROM Emprestimo e
INNER JOIN Aluno a ON e.id_aluno = a.id_aluno
INNER JOIN Livro l ON e.id_livro = l.id_livro
WHERE e.data_devolucao IS NULL;

-- View para livros disponiveis
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
    localizacao
FROM Livro
WHERE quantidade_disponivel > 0;

-- View para reservas ativas
CREATE OR REPLACE VIEW vw_reservas_ativas AS
SELECT
    r.id_reserva,
    a.nome as nome_aluno,
    a.matricula,
    l.titulo as titulo_livro,
    l.autor,
    r.data_reserva,
    r.status
FROM Reserva r
INNER JOIN Aluno a ON r.id_aluno = a.id_aluno
INNER JOIN Livro l ON r.id_livro = l.id_livro
WHERE r.status = 'ATIVA';

-- ================================================
-- FINALIZADO
-- ================================================
-- Execute este script no Supabase SQL Editor ou no psql
-- Para Supabase: Va em "SQL Editor" e cole todo este script
-- Para PostgreSQL local: psql -U usuario -d bibliokopke -f schema-postgresql.sql
