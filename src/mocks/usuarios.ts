import { Usuario, TipoUsuario, CorRaca, Sexo, EstadoCivil } from "../types/entities";

export const usuarios: Usuario[] = [
  {
    codigo_simade: 1001,
    codigo_inep: "INEP001",
    nome_completo: "Maria Silva",
    data_nascimento: "2005-03-15",
    cpf: "123.456.789-00",
    email: "maria.silva@aluno.joaokopke.edu",
    telefone: "(21) 99999-1111",
    endereco: "Rua das Flores, 123",
    tipo_usuario: TipoUsuario.ALUNO,
    nome_filiacao: "João Silva",
    cor_raca: CorRaca.PARDA,
    sexo: Sexo.FEMININO,
    estado_civil: EstadoCivil.SOLTEIRO,
    nacionalidade: "Brasileira",
    uf_nascimento: "RJ",
    municipio_nascimento: "Rio de Janeiro",
    ativo: true,
    data_cadastro: "2024-01-10",
    data_atualizacao: "2025-06-20",
  },
  {
    codigo_simade: 2001,
    codigo_inep: "INEP002",
    nome_completo: "Carlos Souza",
    data_nascimento: "1980-08-22",
    cpf: "987.654.321-00",
    email: "carlos.souza@prof.joaokopke.edu",
    telefone: "(21) 98888-2222",
    endereco: "Av. Central, 456",
    tipo_usuario: TipoUsuario.PROFESSOR,
    nome_filiacao: "Ana Souza",
    cor_raca: CorRaca.BRANCA,
    sexo: Sexo.MASCULINO,
    estado_civil: EstadoCivil.CASADO,
    nacionalidade: "Brasileira",
    uf_nascimento: "RJ",
    municipio_nascimento: "Niterói",
    ativo: true,
    data_cadastro: "2023-02-15",
    data_atualizacao: "2025-06-20",
  },
  {
    codigo_simade: 3001,
    codigo_inep: "INEP003",
    nome_completo: "Fernanda Lima",
    data_nascimento: "1975-11-05",
    cpf: "321.654.987-00",
    email: "fernanda.lima@biblio.joaokopke.edu",
    telefone: "(21) 97777-3333",
    endereco: "Praça da Liberdade, 789",
    tipo_usuario: TipoUsuario.BIBLIOTECARIO,
    nome_filiacao: "Pedro Lima",
    cor_raca: CorRaca.PRETA,
    sexo: Sexo.FEMININO,
    estado_civil: EstadoCivil.DIVORCIADO,
    nacionalidade: "Brasileira",
    uf_nascimento: "RJ",
    municipio_nascimento: "São Gonçalo",
    ativo: true,
    data_cadastro: "2022-09-01",
    data_atualizacao: "2025-06-20",
  },
];
