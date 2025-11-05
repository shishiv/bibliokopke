-- ================================================
-- SCRIPT DE CRIAÇÃO DO BANCO DE DADOS (DDL)
-- Sistema de Biblioteca - BiblioKopke
-- Integração com SIMADE
-- ================================================

-- Criação do banco de dados
CREATE DATABASE IF NOT EXISTS biblioteca_fronteira
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE biblioteca_fronteira;

-- ================================================
-- TABELA DE USUÁRIOS (integração com SIMADE)
-- ================================================
CREATE TABLE usuario (
    codigo_simade INT PRIMARY KEY COMMENT 'Código único do SIMADE',
    codigo_inep VARCHAR(20) COMMENT 'Código INEP da escola',
    nome_completo VARCHAR(255) NOT NULL COMMENT 'Nome completo do usuário',
    data_nascimento DATE NOT NULL COMMENT 'Data de nascimento',
    cpf VARCHAR(14) UNIQUE COMMENT 'CPF do usuário',
    email VARCHAR(255) UNIQUE COMMENT 'Email automático do SIMADE',
    telefone VARCHAR(20) COMMENT 'Telefone de contato',
    endereco TEXT COMMENT 'Endereço completo',
    tipo_usuario ENUM('ALUNO', 'PROFESSOR', 'BIBLIOTECARIO') NOT NULL DEFAULT 'ALUNO',
    nome_filiacao VARCHAR(500) COMMENT 'Nome dos pais/responsáveis',
    cor_raca ENUM('Branca', 'Preta', 'Parda', 'Amarela', 'Indígena', 'Não declarada') COMMENT 'Cor/Raça conforme SIMADE',
    sexo ENUM('Masculino', 'Feminino') NOT NULL,
    estado_civil ENUM('Solteiro(a)', 'Casado(a)', 'Divorciado(a)', 'Viúvo(a)') DEFAULT 'Solteiro(a)',
    nacionalidade VARCHAR(50) DEFAULT 'Brasileira',
    uf_nascimento VARCHAR(2) COMMENT 'UF de nascimento',
    municipio_nascimento VARCHAR(100) COMMENT 'Município de nascimento',
    ativo BOOLEAN DEFAULT TRUE COMMENT 'Status do usuário',
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_nome (nome_completo),
    INDEX idx_tipo (tipo_usuario),
    INDEX idx_ativo (ativo),
    INDEX idx_email (email)
) COMMENT 'Tabela de usuários integrada com SIMADE';

-- ================================================
-- TABELA DE LIVROS
-- ================================================
CREATE TABLE livro (
    id_livro INT AUTO_INCREMENT PRIMARY KEY,
    isbn VARCHAR(17) UNIQUE COMMENT 'ISBN do livro',
    titulo VARCHAR(255) NOT NULL COMMENT 'Título do livro',
    autor VARCHAR(255) NOT NULL COMMENT 'Autor principal',
    editora VARCHAR(100) COMMENT 'Nome da editora',
    ano_publicacao YEAR COMMENT 'Ano de publicação',
    categoria VARCHAR(50) COMMENT 'Categoria/Gênero',
    numero_paginas INT COMMENT 'Número de páginas',
    idioma VARCHAR(20) DEFAULT 'Português' COMMENT 'Idioma da publicação',
    sinopse TEXT COMMENT 'Resumo do livro',
    localizacao VARCHAR(50) COMMENT 'Localização física na biblioteca',
    status ENUM('DISPONIVEL', 'EMPRESTADO', 'RESERVADO', 'MANUTENCAO') DEFAULT 'DISPONIVEL',
    quantidade_total INT DEFAULT 1 COMMENT 'Quantidade total de exemplares',
    quantidade_disponivel INT DEFAULT 1 COMMENT 'Quantidade disponível',
    url_capa VARCHAR(255) COMMENT 'URL/local da imagem da capa',
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    INDEX idx_titulo (titulo),
    INDEX idx_autor (autor),
    INDEX idx_categoria (categoria),
    INDEX idx_status (status),
    INDEX idx_isbn (isbn),
    FULLTEXT idx_busca (titulo, autor, sinopse)
) COMMENT 'Tabela de livros do acervo';

-- ================================================
-- TABELA DE RECOMENDAÇÕES
-- ================================================
CREATE TABLE recomendacao (
    id_recomendacao INT AUTO_INCREMENT PRIMARY KEY,
    codigo_simade_professor INT NOT NULL COMMENT 'Professor que recomendou',
    id_livro INT NOT NULL COMMENT 'Livro recomendado',
    codigo_simade_aluno INT NULL COMMENT 'Aluno recomendado (opcional)',
    turma VARCHAR(50) NULL COMMENT 'Turma recomendada (opcional)',
    observacoes TEXT COMMENT 'Observações da recomendação',
    data_recomendacao DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (codigo_simade_professor) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
    FOREIGN KEY (codigo_simade_aluno) REFERENCES usuario(codigo_simade) ON DELETE SET NULL,
    FOREIGN KEY (id_livro) REFERENCES livro(id_livro) ON DELETE CASCADE,
    
    INDEX idx_professor (codigo_simade_professor),
    INDEX idx_aluno (codigo_simade_aluno),
    INDEX idx_livro (id_livro),
    INDEX idx_turma (turma)
) COMMENT 'Recomendações de livros feitas por professores';

-- ================================================
-- TABELA DE EMPRÉSTIMOS
-- ================================================
CREATE TABLE emprestimo (
    id_emprestimo INT AUTO_INCREMENT PRIMARY KEY,
    codigo_simade INT NOT NULL COMMENT 'Código do usuário no SIMADE',
    id_livro INT NOT NULL COMMENT 'ID do livro emprestado',
    data_emprestimo DATE NOT NULL DEFAULT (CURRENT_DATE),
    data_devolucao_prevista DATE NOT NULL,
    data_devolucao_real DATE NULL,
    status ENUM('ATIVO', 'DEVOLVIDO', 'ATRASADO', 'RENOVADO') DEFAULT 'ATIVO',
    observacoes TEXT COMMENT 'Observações sobre o empréstimo',
    renovacoes INT DEFAULT 0 COMMENT 'Número de renovações',
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (codigo_simade) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
    FOREIGN KEY (id_livro) REFERENCES livro(id_livro) ON DELETE CASCADE,
    
    INDEX idx_usuario (codigo_simade),
    INDEX idx_livro (id_livro),
    INDEX idx_status (status),
    INDEX idx_data_emprestimo (data_emprestimo),
    INDEX idx_data_devolucao (data_devolucao_prevista)
) COMMENT 'Tabela de empréstimos de livros';

-- ================================================
-- TABELA DE RESERVAS
-- ================================================
CREATE TABLE reserva (
    id_reserva INT AUTO_INCREMENT PRIMARY KEY,
    codigo_simade INT NOT NULL COMMENT 'Código do usuário no SIMADE',
    id_livro INT NOT NULL COMMENT 'ID do livro reservado',
    data_reserva DATE NOT NULL DEFAULT (CURRENT_DATE),
    data_validade DATE NOT NULL,
    status ENUM('ATIVA', 'CANCELADA', 'ATENDIDA', 'EXPIRADA') DEFAULT 'ATIVA',
    motivo_cancelamento TEXT COMMENT 'Motivo do cancelamento (se aplicável)',
    data_cadastro DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (codigo_simade) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
    FOREIGN KEY (id_livro) REFERENCES livro(id_livro) ON DELETE CASCADE,
    
    INDEX idx_usuario (codigo_simade),
    INDEX idx_livro (id_livro),
    INDEX idx_status (status),
    INDEX idx_data_validade (data_validade)
) COMMENT 'Tabela de reservas de livros';

-- ================================================
-- TABELA DE HISTÓRICO DE EMPRÉSTIMOS
-- ================================================
CREATE TABLE historico_emprestimo (
    id_historico INT AUTO_INCREMENT PRIMARY KEY,
    id_emprestimo INT NOT NULL COMMENT 'ID do empréstimo original',
    codigo_simade INT NOT NULL COMMENT 'Código do usuário',
    id_livro INT NOT NULL COMMENT 'ID do livro',
    data_emprestimo DATE NOT NULL,
    data_devolucao DATE NOT NULL,
    dias_atraso INT DEFAULT 0 COMMENT 'Dias de atraso (se houver)',
    multa DECIMAL(10,2) DEFAULT 0.00 COMMENT 'Valor da multa (se aplicável)',
    observacoes TEXT COMMENT 'Observações do histórico',
    data_registro DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (codigo_simade) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
    FOREIGN KEY (id_livro) REFERENCES livro(id_livro) ON DELETE CASCADE,
    
    INDEX idx_usuario (codigo_simade),
    INDEX idx_livro (id_livro),
    INDEX idx_data_emprestimo (data_emprestimo),
    INDEX idx_multa (multa)
) COMMENT 'Histórico de todos os empréstimos realizados';

-- ================================================
-- TABELA DE LOGS DO SISTEMA
-- ================================================
CREATE TABLE log_sistema (
    id_log INT AUTO_INCREMENT PRIMARY KEY,
    codigo_simade INT COMMENT 'Usuário que executou a ação',
    tabela_afetada VARCHAR(50) NOT NULL COMMENT 'Tabela que foi modificada',
    acao ENUM('INSERT', 'UPDATE', 'DELETE', 'SELECT') NOT NULL COMMENT 'Tipo de ação',
    dados_anteriores JSON COMMENT 'Dados antes da modificação',
    dados_novos JSON COMMENT 'Dados após a modificação',
    ip_usuario VARCHAR(45) COMMENT 'IP do usuário',
    data_acao DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (codigo_simade) REFERENCES usuario(codigo_simade) ON DELETE SET NULL,
    
    INDEX idx_usuario (codigo_simade),
    INDEX idx_tabela (tabela_afetada),
    INDEX idx_acao (acao),
    INDEX idx_data (data_acao)
) COMMENT 'Log de auditoria do sistema';

-- ================================================
-- TABELA DE RELATÓRIOS
-- ================================================
CREATE TABLE relatorio (
    id_relatorio INT AUTO_INCREMENT PRIMARY KEY,
    tipo_relatorio VARCHAR(100) NOT NULL COMMENT 'Tipo do relatório gerado',
    periodo VARCHAR(50) COMMENT 'Período do relatório',
    dados_relatorio JSON COMMENT 'Dados do relatório em JSON',
    codigo_simade INT NOT NULL COMMENT 'Usuário que gerou o relatório',
    data_geracao DATETIME DEFAULT CURRENT_TIMESTAMP,
    arquivo_gerado VARCHAR(255) COMMENT 'Caminho do arquivo gerado',
    
    FOREIGN KEY (codigo_simade) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
    
    INDEX idx_tipo (tipo_relatorio),
    INDEX idx_usuario (codigo_simade),
    INDEX idx_data (data_geracao)
) COMMENT 'Relatórios gerados pelo sistema';

-- ================================================
-- TRIGGERS PARA AUDITORIA
-- ================================================

-- Trigger para log de inserção de usuários
DELIMITER //
CREATE TRIGGER tr_usuario_insert 
AFTER INSERT ON usuario
FOR EACH ROW
BEGIN
    INSERT INTO log_sistema (codigo_simade, tabela_afetada, acao, dados_novos, ip_usuario)
    VALUES (NEW.codigo_simade, 'usuario', 'INSERT', JSON_OBJECT(
        'codigo_simade', NEW.codigo_simade,
        'nome_completo', NEW.nome_completo,
        'tipo_usuario', NEW.tipo_usuario
    ), CONNECTION_ID());
END//

-- Trigger para log de atualização de usuários
CREATE TRIGGER tr_usuario_update 
AFTER UPDATE ON usuario
FOR EACH ROW
BEGIN
    INSERT INTO log_sistema (codigo_simade, tabela_afetada, acao, dados_anteriores, dados_novos, ip_usuario)
    VALUES (NEW.codigo_simade, 'usuario', 'UPDATE', 
        JSON_OBJECT('nome_completo', OLD.nome_completo, 'tipo_usuario', OLD.tipo_usuario),
        JSON_OBJECT('nome_completo', NEW.nome_completo, 'tipo_usuario', NEW.tipo_usuario),
        CONNECTION_ID());
END//

-- Trigger para atualizar quantidade disponível de livros
CREATE TRIGGER tr_emprestimo_insert 
AFTER INSERT ON emprestimo
FOR EACH ROW
BEGIN
    UPDATE livro 
    SET quantidade_disponivel = quantidade_disponivel - 1,
        status = CASE 
            WHEN quantidade_disponivel - 1 = 0 THEN 'EMPRESTADO'
            ELSE status
        END
    WHERE id_livro = NEW.id_livro;
END//

-- Trigger para devolver livro
CREATE TRIGGER tr_emprestimo_update 
AFTER UPDATE ON emprestimo
FOR EACH ROW
BEGIN
    IF OLD.status != 'DEVOLVIDO' AND NEW.status = 'DEVOLVIDO' THEN
        UPDATE livro 
        SET quantidade_disponivel = quantidade_disponivel + 1,
            status = CASE 
                WHEN quantidade_disponivel + 1 > 0 THEN 'DISPONIVEL'
                ELSE status
            END
        WHERE id_livro = NEW.id_livro;
        
        -- Inserir no histórico
        INSERT INTO historico_emprestimo (
            id_emprestimo, codigo_simade, id_livro, 
            data_emprestimo, data_devolucao, dias_atraso, observacoes
        ) VALUES (
            NEW.id_emprestimo, NEW.codigo_simade, NEW.id_livro,
            NEW.data_emprestimo, NEW.data_devolucao_real,
            DATEDIFF(NEW.data_devolucao_real, NEW.data_devolucao_prevista),
            NEW.observacoes
        );
    END IF;
END//

DELIMITER ;

-- ================================================
-- INSERÇÃO DE DADOS INICIAIS (DML)
-- ================================================

-- Inserir bibliotecário padrão
INSERT INTO usuario (
    codigo_simade, nome_completo, data_nascimento, cpf, email, 
    tipo_usuario, sexo, ativo
) VALUES (
    999999, 'Administrador do Sistema', '1980-01-01', '000.000.000-00', 
    'admin.999999@aluno.mg.gov.br', 'BIBLIOTECARIO', 'Masculino', TRUE
);

-- Inserir categorias de exemplo
INSERT INTO livro (titulo, autor, editora, ano_publicacao, categoria, isbn, sinopse, localizacao, url_capa) VALUES
('Dom Casmurro', 'Machado de Assis', 'Editora Garnier', 1899, 'Literatura Brasileira', '978-85-7326-981-6', 'Romance clássico da literatura brasileira que narra a história de Bentinho e Capitu.', 'A-001', '/public/covers/dom-casmurro.jpg'),
('O Cortiço', 'Aluísio Azevedo', 'Editora Ática', 1890, 'Literatura Brasileira', '978-85-08-12345-6', 'Romance naturalista que retrata a vida em um cortiço no Rio de Janeiro.', 'A-002', '/public/covers/o-cortico.jpg'),
('1984', 'George Orwell', 'Companhia das Letras', 1949, 'Ficção Científica', '978-85-359-0277-4', 'Distopia sobre um regime totalitário que controla todos os aspectos da vida.', 'B-001', '/public/covers/1984.jpg'),
('O Pequeno Príncipe', 'Antoine de Saint-Exupéry', 'Editora Agir', 1943, 'Infantil', '978-85-220-0826-7', 'Fábula poética sobre amizade, amor e crítica social.', 'C-001', '/public/covers/o-pequeno-principe.jpg'),
('Matemática Básica', 'José Silva', 'Editora Moderna', 2020, 'Didático', '978-85-16-12345-8', 'Livro didático de matemática para ensino médio.', 'D-001', NULL);

-- ================================================
-- VIEWS ÚTEIS
-- ================================================

-- View para empréstimos ativos
CREATE VIEW vw_emprestimos_ativos AS
SELECT 
    e.id_emprestimo,
    u.nome_completo,
    u.codigo_simade,
    l.titulo,
    l.autor,
    e.data_emprestimo,
    e.data_devolucao_prevista,
    DATEDIFF(CURRENT_DATE, e.data_devolucao_prevista) as dias_atraso,
    e.status
FROM emprestimo e
JOIN usuario u ON e.codigo_simade = u.codigo_simade
JOIN livro l ON e.id_livro = l.id_livro
WHERE e.status IN ('ATIVO', 'ATRASADO', 'RENOVADO');

-- View para livros mais emprestados
CREATE VIEW vw_livros_populares AS
SELECT 
    l.id_livro,
    l.titulo,
    l.autor,
    l.categoria,
    COUNT(h.id_historico) as total_emprestimos
FROM livro l
LEFT JOIN historico_emprestimo h ON l.id_livro = h.id_livro
GROUP BY l.id_livro, l.titulo, l.autor, l.categoria
ORDER BY total_emprestimos DESC;

-- View para usuários com mais empréstimos
-- (continuação pode ser feita conforme necessidade)
