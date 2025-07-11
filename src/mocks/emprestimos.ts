import { Emprestimo, StatusEmprestimo } from "../types/entities";

export const emprestimos: Emprestimo[] = [
  {
    id_emprestimo: 1,
    codigo_simade: 1001, // Maria Silva (Aluno)
    id_livro: 1, // Dom Casmurro
    data_emprestimo: "2025-06-01",
    data_devolucao_prevista: "2025-06-15",
    data_devolucao_real: "2025-06-14",
    status: StatusEmprestimo.DEVOLVIDO,
    observacoes: "Devolvido em bom estado.",
    renovacoes: 0,
    data_cadastro: "2025-06-01",
  },
  {
    id_emprestimo: 2,
    codigo_simade: 1001, // Maria Silva (Aluno)
    id_livro: 4, // A Hora da Estrela
    data_emprestimo: "2025-06-10",
    data_devolucao_prevista: "2025-06-24",
    status: StatusEmprestimo.ATIVO,
    renovacoes: 0,
    data_cadastro: "2025-06-10",
  },
  {
    id_emprestimo: 3,
    codigo_simade: 2001, // Carlos Souza (Professor)
    id_livro: 2, // O Pequeno Príncipe
    data_emprestimo: "2025-06-05",
    data_devolucao_prevista: "2025-06-19",
    status: StatusEmprestimo.ATRASADO,
    renovacoes: 1,
    data_cadastro: "2025-06-05",
    observacoes: "Aguardando devolução.",
  },
];
