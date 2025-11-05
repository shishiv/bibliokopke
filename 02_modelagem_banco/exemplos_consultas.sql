-- ================================================
-- EXEMPLOS DE CONSULTAS SQL (DML)
-- Sistema de Biblioteca - App Biblioteca Fronteira
-- ================================================

-- INSERÇÃO DE NOVO USUÁRIO (ALUNO)
INSERT INTO usuario (
    codigo_simade, codigo_inep, nome_completo, data_nascimento, cpf, email, telefone, endereco, tipo_usuario, sexo, ativo
) VALUES (
    123456, '12345678', 'João da Silva Teles', '2008-05-10', '123.456.789-00', 'joao.123456@aluno.mg.gov.br', '(34) 99999-0000', 'Rua das Flores, 100', 'ALUNO', 'Masculino', TRUE
);

-- ATUALIZAÇÃO DE DADOS DO USUÁRIO
UPDATE usuario
SET telefone = '(34) 98888-0000', endereco = 'Rua Nova, 200'
WHERE codigo_simade = 123456;

-- EXCLUSÃO DE USUÁRIO (apenas se não houver empréstimos/reservas vinculados)
DELETE FROM usuario
WHERE codigo_simade = 123456;

-- CONSULTA DE USUÁRIOS ATIVOS
SELECT codigo_simade, nome_completo, tipo_usuario, email
FROM usuario
WHERE ativo = TRUE;

-- INSERÇÃO DE NOVO LIVRO
INSERT INTO livro (
    isbn, titulo, autor, editora, ano_publicacao, categoria, numero_paginas, idioma, sinopse, localizacao
) VALUES (
    '978-85-99999-999-9', 'Aprendendo SQL', 'Maria Souza', 'Editora Exemplo', 2022, 'Tecnologia', 350, 'Português', 'Livro introdutório sobre SQL.', 'E-001'
);

-- CONSULTA DE LIVROS DISPONÍVEIS
SELECT id_livro, titulo, autor, categoria, quantidade_disponivel
FROM livro
WHERE status = 'DISPONIVEL' AND quantidade_disponivel > 0;

-- REGISTRO DE EMPRÉSTIMO DE LIVRO
INSERT INTO emprestimo (
    codigo_simade, id_livro, data_emprestimo, data_devolucao_prevista, status
) VALUES (
    123456, 1, CURRENT_DATE, DATE_ADD(CURRENT_DATE, INTERVAL 14 DAY), 'ATIVO'
);

-- DEVOLUÇÃO DE LIVRO (atualização do status e data de devolução real)
UPDATE emprestimo
SET status = 'DEVOLVIDO', data_devolucao_real = CURRENT_DATE
WHERE id_emprestimo = 1;

-- CONSULTA DE EMPRÉSTIMOS ATIVOS DE UM USUÁRIO
SELECT e.id_emprestimo, l.titulo, e.data_emprestimo, e.data_devolucao_prevista, e.status
FROM emprestimo e
JOIN livro l ON e.id_livro = l.id_livro
WHERE e.codigo_simade = 123456 AND e.status = 'ATIVO';

-- RESERVA DE LIVRO
INSERT INTO reserva (
    codigo_simade, id_livro, data_reserva, data_validade, status
) VALUES (
    123456, 1, CURRENT_DATE, DATE_ADD(CURRENT_DATE, INTERVAL 3 DAY), 'ATIVA'
);

-- CANCELAMENTO DE RESERVA
UPDATE reserva
SET status = 'CANCELADA', motivo_cancelamento = 'Solicitação do usuário'
WHERE id_reserva = 1;

-- CONSULTA DE HISTÓRICO DE EMPRÉSTIMOS DE UM USUÁRIO
SELECT h.id_historico, l.titulo, h.data_emprestimo, h.data_devolucao, h.dias_atraso, h.multa
FROM historico_emprestimo h
JOIN livro l ON h.id_livro = l.id_livro
WHERE h.codigo_simade = 123456
ORDER BY h.data_emprestimo DESC;

-- GERAÇÃO DE RELATÓRIO DE LIVROS MAIS EMPRESTADOS
SELECT l.titulo, COUNT(h.id_historico) AS total_emprestimos
FROM livro l
LEFT JOIN historico_emprestimo h ON l.id_livro = h.id_livro
GROUP BY l.id_livro, l.titulo
ORDER BY total_emprestimos DESC
LIMIT 10;

-- CONSULTA DE LOGS DE AUDITORIA
SELECT id_log, tabela_afetada, acao, data_acao, ip_usuario
FROM log_sistema
ORDER BY data_acao DESC
LIMIT 20;
