-- ================================================
-- TABELA DE RECOMENDAÇÕES DE PROFESSOR
-- ================================================
CREATE TABLE recomendacao_professor (
    id_recomendacao INT AUTO_INCREMENT PRIMARY KEY,
    codigo_simade INT NOT NULL COMMENT 'Professor que fez a recomendação',
    id_livro INT NOT NULL COMMENT 'Livro recomendado',
    turma VARCHAR(50) COMMENT 'Turma ou classe associada',
    disciplina VARCHAR(100) COMMENT 'Disciplina associada',
    destaques VARCHAR(255) COMMENT 'Tags ou destaques pedagógicos (separados por vírgula)',
    data_recomendacao DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    FOREIGN KEY (codigo_simade) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
    FOREIGN KEY (id_livro) REFERENCES livro(id_livro) ON DELETE CASCADE,
    
    INDEX idx_professor (codigo_simade),
    INDEX idx_livro (id_livro),
    INDEX idx_turma (turma),
    INDEX idx_disciplina (disciplina)
) COMMENT 'Recomendações e destaques de livros feitas por professores';

-- ================================================
-- ALTERAÇÃO NA TABELA LIVRO PARA TAGS/DESTAQUE
-- ================================================
ALTER TABLE livro
ADD COLUMN tags VARCHAR(255) COMMENT 'Tags ou destaques pedagógicos para o livro' AFTER categoria;