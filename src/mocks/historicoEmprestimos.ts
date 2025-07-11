import { HistoricoEmprestimo } from "../types/entities";

export const historicoEmprestimos: HistoricoEmprestimo[] = [
  {
    id_historico: 1,
    id_emprestimo: 1,
    codigo_simade: 1001, // Maria Silva
    id_livro: 1, // Dom Casmurro
    data_emprestimo: "2025-06-01",
    data_devolucao: "2025-06-14",
    dias_atraso: 0,
    multa: 0,
    observacoes: "Devolução no prazo.",
    data_registro: "2025-06-14",
  },
  {
    id_historico: 2,
    id_emprestimo: 3,
    codigo_simade: 2001, // Carlos Souza
    id_livro: 2, // O Pequeno Príncipe
    data_emprestimo: "2025-06-05",
    data_devolucao: "2025-06-25",
    dias_atraso: 6,
    multa: 12.00,
    observacoes: "Devolução com atraso.",
    data_registro: "2025-06-25",
  },
];
