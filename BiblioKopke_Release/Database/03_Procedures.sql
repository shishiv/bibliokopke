-- ==========================================
-- BiblioKopke - Stored Procedures (MySQL)
-- Versão: 1.0
-- Data: 24/11/2025
-- ==========================================

-- IMPORTANTE: SQLite tem suporte limitado a stored procedures.
-- A lógica de negócio está implementada em C# (camada Servicos/).
-- Este arquivo contém procedures para migração futura para MySQL.

-- ==========================================
-- PROCEDURE: sp_RegistrarEmprestimo
-- Descrição: Registra um novo empréstimo
-- ==========================================
DELIMITER $$
CREATE PROCEDURE sp_RegistrarEmprestimo(
    IN p_id_aluno INT,
    IN p_id_livro INT,
    IN p_data_emprestimo DATE,
    IN p_data_prevista DATE
)
BEGIN
    DECLARE v_disponivel INT;

    -- Verifica disponibilidade
    SELECT quantidade_disponivel INTO v_disponivel
    FROM Livro
    WHERE id_livro = p_id_livro;

    IF v_disponivel > 0 THEN
        -- Registra empréstimo
        INSERT INTO Emprestimo (id_aluno, id_livro, data_emprestimo, data_prevista)
        VALUES (p_id_aluno, p_id_livro, p_data_emprestimo, p_data_prevista);

        -- Atualiza disponibilidade
        UPDATE Livro
        SET quantidade_disponivel = quantidade_disponivel - 1
        WHERE id_livro = p_id_livro;
    ELSE
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Livro não disponível para empréstimo';
    END IF;
END$$
DELIMITER ;

-- ==========================================
-- PROCEDURE: sp_RegistrarDevolucao
-- Descrição: Registra devolução e calcula multa
-- ==========================================
DELIMITER $$
CREATE PROCEDURE sp_RegistrarDevolucao(
    IN p_id_emprestimo INT,
    IN p_data_devolucao DATE
)
BEGIN
    DECLARE v_data_prevista DATE;
    DECLARE v_id_livro INT;
    DECLARE v_dias_atraso INT;
    DECLARE v_multa DECIMAL(10,2);

    -- Obtém dados do empréstimo
    SELECT data_prevista, id_livro
    INTO v_data_prevista, v_id_livro
    FROM Emprestimo
    WHERE id_emprestimo = p_id_emprestimo;

    -- Calcula multa (R$ 2,00 por dia de atraso)
    SET v_dias_atraso = DATEDIFF(p_data_devolucao, v_data_prevista);
    IF v_dias_atraso > 0 THEN
        SET v_multa = v_dias_atraso * 2.00;
    ELSE
        SET v_multa = 0;
    END IF;

    -- Registra devolução
    UPDATE Emprestimo
    SET data_devolucao = p_data_devolucao,
        multa = v_multa
    WHERE id_emprestimo = p_id_emprestimo;

    -- Atualiza disponibilidade
    UPDATE Livro
    SET quantidade_disponivel = quantidade_disponivel + 1
    WHERE id_livro = v_id_livro;
END$$
DELIMITER ;

-- ==========================================
-- PROCEDURE: sp_ProcessarReservas
-- Descrição: Processa fila de reservas quando livro fica disponível
-- ==========================================
DELIMITER $$
CREATE PROCEDURE sp_ProcessarReservas(
    IN p_id_livro INT
)
BEGIN
    DECLARE v_id_reserva INT;

    -- Busca primeira reserva ativa (FIFO)
    SELECT id_reserva INTO v_id_reserva
    FROM Reserva
    WHERE id_livro = p_id_livro
      AND status = 'ATIVA'
    ORDER BY data_reserva ASC
    LIMIT 1;

    -- Marca reserva como concluída
    IF v_id_reserva IS NOT NULL THEN
        UPDATE Reserva
        SET status = 'CONCLUIDA'
        WHERE id_reserva = v_id_reserva;
    END IF;
END$$
DELIMITER ;

-- ==========================================
-- NOTAS DE IMPLEMENTAÇÃO
-- ==========================================
-- 1. Implementação atual (SQLite): Lógica em ServicoEmprestimo.cs e ServicoReserva.cs
-- 2. Para migração MySQL: Executar este script após 01_DDL_Create_Tables.sql
-- 3. Validações adicionais estão em Servicos/Validadores.cs
-- 4. Sistema de logs implementado em ServicoLog.cs
