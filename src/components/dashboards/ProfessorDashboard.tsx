"use client";
import { useState } from "react";
import { useLivros } from "./useLivros";
import { ListaLivros } from "@/components/catalogo/ListaLivros";
import { RecommendationManager } from "@/components/catalogo/RecommendationManager";
import { historicoEmprestimos as historicoMock } from "@/mocks/historicoEmprestimos";
import { TabelaHistoricoEmprestimos } from "@/components/historico/TabelaHistoricoEmprestimos";
import { Livro, HistoricoEmprestimo } from "@/types/entities";
import { toast } from "sonner";

export function ProfessorDashboard() {
  const livros = useLivros();
  const [recommended, setRecommended] = useState<{ [id: number]: string[] }>({});
  const [historico] = useState<HistoricoEmprestimo[]>(
    historicoMock.filter((h) => h.codigo_simade === 2001)
  );


  function handleSolicitarReserva(livro: Livro) {
    toast.info(`Solicitação de reserva para "${livro.titulo}" enviada para a biblioteca.`);
  }

  function handleRecommend(livro: Livro, tags: string[]) {
    setRecommended((prev) => ({
      ...prev,
      [livro.id_livro]: tags,
    }));
    toast.success(`Livro "${livro.titulo}" marcado como: ${tags.join(", ")}`);
  }

  // Filter for recommended books
  const livrosRecomendados = livros.filter((livro) => recommended[livro.id_livro]?.length);

  return (
    <main className="flex min-h-screen flex-col items-center bg-gradient-to-br from-green-50 to-blue-100 py-8">
      <h1 className="text-3xl font-extrabold mb-4 text-green-900">Dashboard do Professor</h1>
      <p className="mb-6 text-lg">Bem-vindo(a), Professor Carlos! Gerencie recomendações e destaques para suas turmas.</p>

      {/* Curadoria e Recomendações */}
      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-2xl font-bold mb-4 text-green-900">Curadoria e Recomendações</h2>
        <RecommendationManager livros={livros} onRecommend={handleRecommend} recommended={recommended} />
      </section>

      {/* Livros Recomendados */}
      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-xl font-semibold mb-4">Livros Recomendados</h2>
        <ListaLivros livros={livrosRecomendados} onReservar={handleSolicitarReserva} />
      </section>

      {/* Destaques para Professores */}
      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-xl font-semibold mb-4">Destaques para Professores</h2>
        <ListaLivros livros={livros.filter(livro => livro.categoria === 'Didático').slice(0, 3)} onReservar={handleSolicitarReserva} />
      </section>

      {/* Novas Aquisições */}
      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-xl font-semibold mb-4">Novas Aquisições</h2>
        <ListaLivros livros={livros.slice(-3)} onReservar={handleSolicitarReserva} />
      </section>

      {/* Catálogo Completo */}
      <section className="w-full max-w-5xl">
        <h2 className="text-xl font-semibold mb-4">Catálogo Completo de Livros</h2>
        <ListaLivros livros={livros} onReservar={handleSolicitarReserva} />
      </section>

      {/* Histórico de Empréstimos */}
      <section className="w-full max-w-5xl mt-10">
        <h2 className="text-xl font-semibold mb-4">Seu Histórico de Empréstimos</h2>
        <TabelaHistoricoEmprestimos historico={historico} />
      </section>
    </main>
  );
}
