import { Livro, StatusLivro } from "@/types/entities";

export async function getLivrosComCapas(): Promise<Livro[]> {
  // Helper to fetch cover from the local API route
  async function fetchCapa(titulo: string): Promise<string> {
    try {
      const res = await fetch(`/api/book-cover?title=${encodeURIComponent(titulo)}`);
      if (!res.ok) return "";
      const data = await res.json();
      return data.coverUrl || "";
    } catch {
      return "";
    }
  }

  const livrosBase: Omit<Livro, "url_capa">[] = [
    {
      id_livro: 1,
      titulo: "Dom Casmurro",
      autor: "Machado de Assis",
      editora: "Editora Garnier",
      ano_publicacao: 1899,
      isbn: "978-85-7326-981-6",
      categoria: "Literatura Brasileira",
      numero_paginas: 256,
      idioma: "Português",
      sinopse: "Um clássico da literatura brasileira que narra a história de Bentinho e Capitu, explorando temas como ciúme e traição.",
      localizacao: "A-001",
      status: StatusLivro.DISPONIVEL,
      quantidade_total: 5,
      quantidade_disponivel: 3,
      data_cadastro: "2023-01-15",
      data_atualizacao: "2024-06-20",
    },
    {
      id_livro: 2,
      titulo: "O Cortiço",
      autor: "Aluísio Azevedo",
      editora: "Editora Ática",
      ano_publicacao: 1890,
      isbn: "978-85-08-12345-6",
      categoria: "Literatura Brasileira",
      numero_paginas: 320,
      idioma: "Português",
      sinopse: "Um romance naturalista que retrata a vida em um cortiço no Rio de Janeiro do século XIX.",
      localizacao: "A-002",
      status: StatusLivro.EMPRESTADO,
      quantidade_total: 3,
      quantidade_disponivel: 0,
      data_cadastro: "2023-02-10",
      data_atualizacao: "2024-06-25",
    },
    {
      id_livro: 3,
      titulo: "1984",
      autor: "George Orwell",
      editora: "Companhia das Letras",
      ano_publicacao: 1949,
      isbn: "978-85-359-0277-4",
      categoria: "Ficção Científica",
      numero_paginas: 416,
      idioma: "Português",
      sinopse: "Uma distopia sombria sobre um regime totalitário que controla todos os aspectos da vida humana.",
      localizacao: "B-001",
      status: StatusLivro.DISPONIVEL,
      quantidade_total: 7,
      quantidade_disponivel: 4,
      data_cadastro: "2023-03-01",
      data_atualizacao: "2024-06-18",
    },
    {
      id_livro: 4,
      titulo: "O Pequeno Príncipe",
      autor: "Antoine de Saint-Exupéry",
      editora: "Editora Agir",
      ano_publicacao: 1943,
      isbn: "978-85-220-0826-7",
      categoria: "Infantil",
      numero_paginas: 96,
      idioma: "Português",
      sinopse: "Uma fábula poética que encanta leitores de todas as idades com suas reflexões sobre a vida e a amizade.",
      localizacao: "C-001",
      status: StatusLivro.RESERVADO,
      quantidade_total: 10,
      quantidade_disponivel: 2,
      data_cadastro: "2023-04-05",
      data_atualizacao: "2024-06-22",
    },
    {
      id_livro: 5,
      titulo: "Matemática Básica",
      autor: "José Silva",
      editora: "Editora Moderna",
      ano_publicacao: 2020,
      isbn: "978-85-16-12345-8",
      categoria: "Didático",
      numero_paginas: 400,
      idioma: "Português",
      sinopse: "Um livro didático completo para o ensino de matemática básica, com exercícios e exemplos práticos.",
      localizacao: "D-001",
      status: StatusLivro.DISPONIVEL,
      quantidade_total: 8,
      quantidade_disponivel: 8,
      data_cadastro: "2023-05-01",
      data_atualizacao: "2024-06-10",
    },
  ];

  // Fetch covers in parallel
  const livros: Livro[] = await Promise.all(
    livrosBase.map(async (livro) => ({
      ...livro,
      url_capa: await fetchCapa(livro.titulo),
    }))
  );
  return livros;
}