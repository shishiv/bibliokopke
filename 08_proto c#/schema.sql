-- ================================================
-- SCRIPT SQL PARA O PROTÓTIPO C# - BibliotecaJK
-- ================================================
-- Este é o schema esperado pelo código C# do protótipo
-- Database: bibliokopke

CREATE DATABASE IF NOT EXISTS bibliokopke
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE bibliokopke;

-- ================================================
-- TABELA DE ALUNOS
-- ================================================
CREATE TABLE IF NOT EXISTS Aluno (
    id_aluno INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    matricula VARCHAR(50) NOT NULL UNIQUE,
    turma VARCHAR(50) NULL,
    telefone VARCHAR(20) NULL,
    email VARCHAR(255) NULL,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    INDEX idx_nome (nome),
    INDEX idx_cpf (cpf),
    INDEX idx_matricula (matricula)
) COMMENT 'Tabela de alunos do sistema';

-- ================================================
-- TABELA DE FUNCIONÁRIOS
-- ================================================
CREATE TABLE IF NOT EXISTS Funcionario (
    id_funcionario INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    cargo VARCHAR(100) NULL,
    login VARCHAR(50) NOT NULL UNIQUE,
    senha_hash VARCHAR(255) NOT NULL,
    perfil VARCHAR(50) NOT NULL DEFAULT 'OPERADOR', -- ADMIN, BIBLIOTECARIO, OPERADOR
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    INDEX idx_nome (nome),
    INDEX idx_cpf (cpf),
    INDEX idx_login (login),
    INDEX idx_perfil (perfil)
) COMMENT 'Tabela de funcionários do sistema';

-- ================================================
-- TABELA DE LIVROS
-- ================================================
CREATE TABLE IF NOT EXISTS Livro (
    id_livro INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(255) NOT NULL,
    autor VARCHAR(255) NULL,
    isbn VARCHAR(17) NULL UNIQUE,
    editora VARCHAR(100) NULL,
    ano_publicacao INT NULL,
    quantidade_total INT NOT NULL DEFAULT 1,
    quantidade_disponivel INT NOT NULL DEFAULT 1,
    localizacao VARCHAR(50) NULL,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,

    INDEX idx_titulo (titulo),
    INDEX idx_autor (autor),
    INDEX idx_isbn (isbn)
) COMMENT 'Tabela de livros do acervo';

-- ================================================
-- TABELA DE EMPRÉSTIMOS
-- ================================================
CREATE TABLE IF NOT EXISTS Emprestimo (
    id_emprestimo INT AUTO_INCREMENT PRIMARY KEY,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    data_emprestimo DATE NOT NULL,
    data_prevista DATE NOT NULL,
    data_devolucao DATE NULL,
    multa DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno) ON DELETE CASCADE,
    FOREIGN KEY (id_livro) REFERENCES Livro(id_livro) ON DELETE CASCADE,

    INDEX idx_aluno (id_aluno),
    INDEX idx_livro (id_livro),
    INDEX idx_data_emprestimo (data_emprestimo),
    INDEX idx_data_prevista (data_prevista)
) COMMENT 'Tabela de empréstimos de livros';

-- ================================================
-- TABELA DE RESERVAS
-- ================================================
CREATE TABLE IF NOT EXISTS Reserva (
    id_reserva INT AUTO_INCREMENT PRIMARY KEY,
    id_aluno INT NOT NULL,
    id_livro INT NOT NULL,
    data_reserva DATE NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'ATIVA', -- ATIVA, CANCELADA, CONCLUIDA
    data_cadastro TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno) ON DELETE CASCADE,
    FOREIGN KEY (id_livro) REFERENCES Livro(id_livro) ON DELETE CASCADE,

    INDEX idx_aluno (id_aluno),
    INDEX idx_livro (id_livro),
    INDEX idx_status (status),
    INDEX idx_data_reserva (data_reserva)
) COMMENT 'Tabela de reservas de livros';

-- ================================================
-- TABELA DE LOGS DE AÇÕES
-- ================================================
CREATE TABLE IF NOT EXISTS Log_Acao (
    id_log INT AUTO_INCREMENT PRIMARY KEY,
    id_funcionario INT NULL,
    acao VARCHAR(100) NULL,
    descricao TEXT NULL,
    data_hora TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (id_funcionario) REFERENCES Funcionario(id_funcionario) ON DELETE SET NULL,

    INDEX idx_funcionario (id_funcionario),
    INDEX idx_acao (acao),
    INDEX idx_data_hora (data_hora)
) COMMENT 'Tabela de logs de ações do sistema';

-- ================================================
-- DADOS INICIAIS PARA TESTES
-- ================================================

-- Inserir funcionário administrador padrão
-- Senha: "admin123" (hash BCrypt seguro)
-- Hash gerado com BCrypt.Net-Next usando fator de custo 11
INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil)
VALUES ('Administrador', '000.000.000-00', 'Administrador do Sistema', 'admin', '$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', 'ADMIN')
ON DUPLICATE KEY UPDATE nome=nome;

-- Inserir alguns alunos de exemplo
INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email) VALUES
('João Silva', '111.111.111-11', 'MAT001', '1A', '(11) 98888-1111', 'joao@email.com'),
('Maria Santos', '222.222.222-22', 'MAT002', '1A', '(11) 98888-2222', 'maria@email.com'),
('Pedro Oliveira', '333.333.333-33', 'MAT003', '2B', '(11) 98888-3333', 'pedro@email.com')
ON DUPLICATE KEY UPDATE nome=nome;

-- Inserir alguns livros de exemplo
INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, quantidade_total, quantidade_disponivel, localizacao) VALUES
('Dom Casmurro', 'Machado de Assis', '978-85-7326-981-6', 'Editora Garnier', 1899, 3, 3, 'A-001'),
('O Cortiço', 'Aluísio Azevedo', '978-85-08-12345-6', 'Editora Ática', 1890, 2, 2, 'A-002'),
('1984', 'George Orwell', '978-85-359-0277-4', 'Companhia das Letras', 1949, 5, 5, 'B-001'),
('O Pequeno Príncipe', 'Antoine de Saint-Exupéry', '978-85-220-0826-7', 'Editora Agir', 1943, 4, 4, 'C-001'),
('Matemática Básica', 'José Silva', '978-85-16-12345-8', 'Editora Moderna', 2020, 10, 10, 'D-001')
ON DUPLICATE KEY UPDATE titulo=titulo;

-- ================================================
-- VIEWS ÚTEIS
-- ================================================

-- View para empréstimos ativos
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
        WHEN e.data_devolucao IS NULL AND CURDATE() > e.data_prevista
        THEN DATEDIFF(CURDATE(), e.data_prevista)
        ELSE 0
    END as dias_atraso,
    e.multa
FROM Emprestimo e
INNER JOIN Aluno a ON e.id_aluno = a.id_aluno
INNER JOIN Livro l ON e.id_livro = l.id_livro
WHERE e.data_devolucao IS NULL;

-- View para livros disponíveis
CREATE OR REPLACE VIEW vw_livros_disponiveis AS
SELECT
    id_livro,
    titulo,
    autor,
    isbn,
    editora,
    ano_publicacao,
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
