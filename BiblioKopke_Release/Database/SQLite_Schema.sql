-- ==========================================
-- BiblioKopke - SQLite Database Schema
-- Versão: 1.0
-- Data: 24/11/2025
-- ==========================================

-- ==========================================
-- CRIAÇÃO DE TABELAS
-- ==========================================

-- Tabela: Aluno
CREATE TABLE IF NOT EXISTS Aluno (
    id_aluno INTEGER PRIMARY KEY AUTOINCREMENT,
    nome TEXT NOT NULL,
    cpf TEXT UNIQUE NOT NULL,
    matricula TEXT NOT NULL,
    turma TEXT,
    telefone TEXT,
    email TEXT
);

-- Tabela: Funcionario
CREATE TABLE IF NOT EXISTS Funcionario (
    id_funcionario INTEGER PRIMARY KEY AUTOINCREMENT,
    nome TEXT NOT NULL,
    cpf TEXT UNIQUE NOT NULL,
    cargo TEXT,
    login TEXT UNIQUE NOT NULL,
    senha_hash TEXT NOT NULL,
    perfil TEXT NOT NULL
);

-- Tabela: Livro
CREATE TABLE IF NOT EXISTS Livro (
    id_livro INTEGER PRIMARY KEY AUTOINCREMENT,
    titulo TEXT NOT NULL,
    autor TEXT,
    isbn TEXT UNIQUE,
    editora TEXT,
    ano_publicacao INTEGER,
    quantidade_total INTEGER NOT NULL,
    quantidade_disponivel INTEGER NOT NULL,
    localizacao TEXT
);

-- Tabela: Emprestimo
CREATE TABLE IF NOT EXISTS Emprestimo (
    id_emprestimo INTEGER PRIMARY KEY AUTOINCREMENT,
    id_aluno INTEGER NOT NULL,
    id_livro INTEGER NOT NULL,
    data_emprestimo TEXT NOT NULL,
    data_prevista TEXT NOT NULL,
    data_devolucao TEXT,
    multa REAL DEFAULT 0,
    FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno),
    FOREIGN KEY (id_livro) REFERENCES Livro(id_livro)
);

-- Tabela: Reserva
CREATE TABLE IF NOT EXISTS Reserva (
    id_reserva INTEGER PRIMARY KEY AUTOINCREMENT,
    id_aluno INTEGER NOT NULL,
    id_livro INTEGER NOT NULL,
    data_reserva TEXT NOT NULL,
    status TEXT DEFAULT 'ATIVA',
    FOREIGN KEY (id_aluno) REFERENCES Aluno(id_aluno),
    FOREIGN KEY (id_livro) REFERENCES Livro(id_livro)
);

-- Tabela: Log_Acao
CREATE TABLE IF NOT EXISTS Log_Acao (
    id_log INTEGER PRIMARY KEY AUTOINCREMENT,
    id_funcionario INTEGER,
    acao TEXT,
    descricao TEXT,
    data_hora TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_funcionario) REFERENCES Funcionario(id_funcionario)
);

-- ==========================================
-- DADOS INICIAIS (SEEDS)
-- ==========================================

-- Alunos de teste
INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email)
SELECT 'João Silva', '12345678901', '2025A001', '3A', '11999998888', 'joao@email.com'
WHERE NOT EXISTS (SELECT 1 FROM Aluno WHERE cpf = '12345678901');

INSERT INTO Aluno (nome, cpf, matricula, turma, telefone, email)
SELECT 'Maria Souza', '98765432100', '2025B002', '2B', '11988887777', 'maria@email.com'
WHERE NOT EXISTS (SELECT 1 FROM Aluno WHERE cpf = '98765432100');

-- Funcionário Administrador
-- Login: admin
-- Senha: admin123 (senha_hash: 7676AAAFB027C825BD9ABAB78B234070E702752F625B752E55E55B48E607E358)
INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil)
SELECT 'Administrador', '11122233344', 'Administrador', 'admin',
       '7676AAAFB027C825BD9ABAB78B234070E702752F625B752E55E55B48E607E358', 'ADMIN'
WHERE NOT EXISTS (SELECT 1 FROM Funcionario WHERE login = 'admin');

-- Livros de teste
INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, quantidade_total, quantidade_disponivel, localizacao)
SELECT 'Dom Casmurro', 'Machado de Assis', '9788535910665', 'Companhia das Letras', 2019, 3, 3, 'Estante A'
WHERE NOT EXISTS (SELECT 1 FROM Livro WHERE isbn = '9788535910665');

INSERT INTO Livro (titulo, autor, isbn, editora, ano_publicacao, quantidade_total, quantidade_disponivel, localizacao)
SELECT 'O Pequeno Príncipe', 'Antoine de Saint-Exupéry', '9788525056019', 'HarperCollins', 2020, 2, 2, 'Estante B'
WHERE NOT EXISTS (SELECT 1 FROM Livro WHERE isbn = '9788525056019');

-- ==========================================
-- NOTAS
-- ==========================================
-- 1. Este schema é automaticamente criado pela aplicação ao iniciar
-- 2. O arquivo de banco de dados fica em ./dados/biblioteca.sqlite
-- 3. SQLite usa INTEGER PRIMARY KEY AUTOINCREMENT ao invés de AUTO_INCREMENT do MySQL
-- 4. Senhas são armazenadas como SHA-256 hash
-- 5. Dados iniciais são inseridos apenas se não existirem (usando WHERE NOT EXISTS)
