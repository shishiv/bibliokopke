/**
 * Tipagens principais do sistema de biblioteca — Escola João Kopke
 * Gerado a partir do diagrama de classes
 */

export enum TipoUsuario {
  ALUNO = "ALUNO",
  PROFESSOR = "PROFESSOR",
  BIBLIOTECARIO = "BIBLIOTECARIO",
}

export enum CorRaca {
  BRANCA = "Branca",
  PRETA = "Preta",
  PARDA = "Parda",
  AMARELA = "Amarela",
  INDIGENA = "Indígena",
  NAO_DECLARADA = "Não_declarada",
}

export enum Sexo {
  MASCULINO = "Masculino",
  FEMININO = "Feminino",
}

export enum EstadoCivil {
  SOLTEIRO = "Solteiro",
  CASADO = "Casado",
  DIVORCIADO = "Divorciado",
  VIUVO = "Viúvo",
}

export enum StatusLivro {
  DISPONIVEL = "DISPONIVEL",
  EMPRESTADO = "EMPRESTADO",
  RESERVADO = "RESERVADO",
  MANUTENCAO = "MANUTENCAO",
}

export enum StatusEmprestimo {
  ATIVO = "ATIVO",
  DEVOLVIDO = "DEVOLVIDO",
  ATRASADO = "ATRASADO",
  RENOVADO = "RENOVADO",
}

export enum StatusReserva {
  ATIVA = "ATIVA",
  CANCELADA = "CANCELADA",
  ATENDIDA = "ATENDIDA",
  EXPIRADA = "EXPIRADA",
}

export interface Usuario {
  codigo_simade: number;
  codigo_inep: string;
  nome_completo: string;
  data_nascimento: string;
  cpf: string;
  email: string;
  telefone: string;
  endereco: string;
  tipo_usuario: TipoUsuario;
  nome_filiacao: string;
  cor_raca: CorRaca;
  sexo: Sexo;
  estado_civil: EstadoCivil;
  nacionalidade: string;
  uf_nascimento: string;
  municipio_nascimento: string;
  ativo: boolean;
  data_cadastro: string;
  data_atualizacao: string;
}

export interface Livro {
  id_livro: number;
  isbn: string;
  titulo: string;
  autor: string;
  editora: string;
  ano_publicacao: number;
  categoria: string;
  numero_paginas: number;
  idioma: string;
  sinopse: string;
  localizacao: string;
  status: StatusLivro;
  quantidade_total: number;
  quantidade_disponivel: number;
  data_cadastro: string;
  data_atualizacao: string;
  url_capa?: string;
}

export interface Emprestimo {
  id_emprestimo: number;
  codigo_simade: number;
  id_livro: number;
  data_emprestimo: string;
  data_devolucao_prevista: string;
  data_devolucao_real?: string;
  status: StatusEmprestimo;
  observacoes?: string;
  renovacoes: number;
  data_cadastro: string;
}

export interface Reserva {
  id_reserva: number;
  codigo_simade: number;
  id_livro: number;
  data_reserva: string;
  data_validade: string;
  status: StatusReserva;
  motivo_cancelamento?: string;
  data_cadastro: string;
}

export interface HistoricoEmprestimo {
  id_historico: number;
  id_emprestimo: number;
  codigo_simade: number;
  id_livro: number;
  data_emprestimo: string;
  data_devolucao: string;
  dias_atraso: number;
  multa: number;
  observacoes?: string;
  data_registro: string;
}

export interface Relatorio {
  id_relatorio: number;
  tipo_relatorio: string;
  periodo: string;
  dados_relatorio: unknown;
  codigo_simade: number;
  data_geracao: string;
  arquivo_gerado: string;
}
