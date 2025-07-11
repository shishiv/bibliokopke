import { Reserva, StatusReserva } from "../types/entities";

export const reservas: Reserva[] = [
  {
    id_reserva: 1,
    codigo_simade: 1001, // Maria Silva (Aluno)
    id_livro: 3, // Capitães da Areia
    data_reserva: "2025-06-15",
    data_validade: "2025-06-22",
    status: StatusReserva.ATIVA,
    data_cadastro: "2025-06-15",
  },
  {
    id_reserva: 2,
    codigo_simade: 2001, // Carlos Souza (Professor)
    id_livro: 1, // Dom Casmurro
    data_reserva: "2025-06-10",
    data_validade: "2025-06-17",
    status: StatusReserva.CANCELADA,
    motivo_cancelamento: "Solicitação do usuário",
    data_cadastro: "2025-06-10",
  },
  {
    id_reserva: 3,
    codigo_simade: 3001, // Fernanda Lima (Bibliotecária)
    id_livro: 2, // O Pequeno Príncipe
    data_reserva: "2025-06-05",
    data_validade: "2025-06-12",
    status: StatusReserva.ATENDIDA,
    data_cadastro: "2025-06-05",
  },
];
