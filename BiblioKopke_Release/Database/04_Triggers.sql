-- ==========================================
-- BiblioKopke - Database Triggers (MySQL)
-- Versão: 1.0
-- Data: 24/11/2025
-- ==========================================

-- IMPORTANTE: SQLite tem suporte limitado a triggers complexos.
-- A lógica de auditoria e validação está implementada em C# (camada Servicos/).
-- Este arquivo contém triggers para migração futura para MySQL.

-- ==========================================
-- TRIGGER: trg_AuditarEmprestimo
-- Descrição: Registra log quando empréstimo é criado
-- ==========================================
DELIMITER $$
CREATE TRIGGER trg_AuditarEmprestimo
AFTER INSERT ON Emprestimo
FOR EACH ROW
BEGIN
    INSERT INTO Log_Acao (id_funcionario, acao, descricao)
    VALUES (
        NULL,
        'EMPRESTIMO_CRIADO',
        CONCAT('Empréstimo ID ', NEW.id_emprestimo, ' criado para aluno ', NEW.id_aluno)
    );
END$$
DELIMITER ;

-- ==========================================
-- TRIGGER: trg_AuditarDevolucao
-- Descrição: Registra log quando devolução é realizada
-- ==========================================
DELIMITER $$
CREATE TRIGGER trg_AuditarDevolucao
AFTER UPDATE ON Emprestimo
FOR EACH ROW
BEGIN
    IF OLD.data_devolucao IS NULL AND NEW.data_devolucao IS NOT NULL THEN
        INSERT INTO Log_Acao (id_funcionario, acao, descricao)
        VALUES (
            NULL,
            'DEVOLUCAO_REGISTRADA',
            CONCAT('Devolução do empréstimo ID ', NEW.id_emprestimo, ' registrada')
        );
    END IF;
END$$
DELIMITER ;

-- ==========================================
-- TRIGGER: trg_ValidarDisponibilidade
-- Descrição: Valida que quantidade disponível não fica negativa
-- ==========================================
DELIMITER $$
CREATE TRIGGER trg_ValidarDisponibilidade
BEFORE UPDATE ON Livro
FOR EACH ROW
BEGIN
    IF NEW.quantidade_disponivel < 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Quantidade disponível não pode ser negativa';
    END IF;

    IF NEW.quantidade_disponivel > NEW.quantidade_total THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Quantidade disponível não pode ser maior que quantidade total';
    END IF;
END$$
DELIMITER ;

-- ==========================================
-- TRIGGER: trg_ExpirarReservas
-- Descrição: Marca reservas como expiradas após 7 dias
-- ==========================================
DELIMITER $$
CREATE TRIGGER trg_ExpirarReservas
BEFORE UPDATE ON Reserva
FOR EACH ROW
BEGIN
    DECLARE v_dias_reserva INT;

    IF NEW.status = 'ATIVA' THEN
        SET v_dias_reserva = DATEDIFF(CURRENT_DATE, NEW.data_reserva);

        IF v_dias_reserva > 7 THEN
            SET NEW.status = 'EXPIRADA';
        END IF;
    END IF;
END$$
DELIMITER ;

-- ==========================================
-- TRIGGER: trg_AuditarReserva
-- Descrição: Registra log quando reserva é criada
-- ==========================================
DELIMITER $$
CREATE TRIGGER trg_AuditarReserva
AFTER INSERT ON Reserva
FOR EACH ROW
BEGIN
    INSERT INTO Log_Acao (id_funcionario, acao, descricao)
    VALUES (
        NULL,
        'RESERVA_CRIADA',
        CONCAT('Reserva ID ', NEW.id_reserva, ' criada para aluno ', NEW.id_aluno)
    );
END$$
DELIMITER ;

-- ==========================================
-- TRIGGER: trg_PrevenirExclusaoComEmprestimos
-- Descrição: Impede exclusão de livro com empréstimos ativos
-- ==========================================
DELIMITER $$
CREATE TRIGGER trg_PrevenirExclusaoComEmprestimos
BEFORE DELETE ON Livro
FOR EACH ROW
BEGIN
    DECLARE v_emprestimos_ativos INT;

    SELECT COUNT(*) INTO v_emprestimos_ativos
    FROM Emprestimo
    WHERE id_livro = OLD.id_livro
      AND data_devolucao IS NULL;

    IF v_emprestimos_ativos > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Não é possível excluir livro com empréstimos ativos';
    END IF;
END$$
DELIMITER ;

-- ==========================================
-- NOTAS DE IMPLEMENTAÇÃO
-- ==========================================
-- 1. Implementação atual (SQLite): Lógica em classes de Servico (C#)
-- 2. Sistema de logs: RepositorioLogAcao.cs
-- 3. Validações: Validadores.cs e ServicoEmprestimo.cs
-- 4. Expiração de reservas: ServicoReserva.cs (verificação periódica)
-- 5. Para migração MySQL: Executar após 01_DDL e 03_Procedures
