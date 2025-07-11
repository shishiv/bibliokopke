"use client";
import { useState, useEffect } from "react";
import { getLivrosComCapas } from "@/mocks/livros";
import { ListaLivros } from "@/components/catalogo/ListaLivros";
import { historicoEmprestimos as historicoMock } from "@/mocks/historicoEmprestimos";
import { TabelaHistoricoEmprestimos } from "@/components/historico/TabelaHistoricoEmprestimos";
import { Livro, StatusLivro, HistoricoEmprestimo } from "@/types/entities";
import { toast } from "sonner";
import Image from "next/image";
import { BookDetailModal } from "@/components/catalogo/BookDetailModal";

export function AlunoDashboard() {
  const [livros, setLivros] = useState<Livro[]>([]);
  const [historico, setHistorico] = useState<HistoricoEmprestimo[]>(
    historicoMock.filter((h) => h.codigo_simade === 1001)
  );
  const [carouselIndex, setCarouselIndex] = useState(0);
  const [modalLivro, setModalLivro] = useState<Livro | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

  useEffect(() => {
    getLivrosComCapas().then(setLivros);
  }, []);

  function handleReservarLivro(livro: Livro) {
    setLivros((prev) =>
      prev.map((l) =>
        l.id_livro === livro.id_livro ? { ...l, status: StatusLivro.RESERVADO } : l
      )
    );
    setHistorico((prev) => [
      ...prev,
      {
        id_historico: Math.floor(Math.random() * 100000),
        id_emprestimo: Math.floor(Math.random() * 100000),
        codigo_simade: 1001,
        id_livro: livro.id_livro,
        data_emprestimo: new Date().toISOString().slice(0, 10),
        data_devolucao: "",
        dias_atraso: 0,
        multa: 0,
        observacoes: "Reserva simulada",
        data_registro: new Date().toISOString(),
      },
    ]);
    toast.success(`Livro "${livro.titulo}" reservado com sucesso!`);
  }

  // Carousel logic
  const destaques = livros.slice(0, 5);
  function nextSlide() {
    setCarouselIndex((prev) => (prev + 1) % destaques.length);
  }
  function prevSlide() {
    setCarouselIndex((prev) => (prev - 1 + destaques.length) % destaques.length);
  }

  return (
    <main className="flex min-h-screen flex-col items-center bg-gradient-to-br from-blue-50 to-indigo-100 py-8">
      {/* Hero Section */}
      <section className="w-full max-w-5xl mb-10 flex flex-col md:flex-row items-center justify-between gap-8 p-6 bg-white rounded-xl shadow-lg">
        <div className="flex-1">
          <h1 className="text-4xl font-extrabold text-indigo-800 mb-2">Bem-vindo(a), Maria Silva!</h1>
          <p className="text-lg text-gray-700 mb-4">
            Descubra, reserve e acompanhe seus livros favoritos com facilidade.
          </p>
          <div className="flex gap-2">
            <button
              className="bg-indigo-600 text-white px-4 py-2 rounded-lg font-semibold hover:bg-indigo-700 transition"
              onClick={() => document.getElementById("catalogo")?.scrollIntoView({ behavior: "smooth" })}
            >
              Ver Catálogo
            </button>
            <button
              className="bg-white border border-indigo-600 text-indigo-700 px-4 py-2 rounded-lg font-semibold hover:bg-indigo-50 transition"
              onClick={() => document.getElementById("historico")?.scrollIntoView({ behavior: "smooth" })}
            >
              Ver Histórico
            </button>
          </div>
        </div>
        {/* Carousel */}
        <div className="w-full md:w-64 flex flex-col items-center">
          <div className="relative w-full h-80 rounded-lg overflow-hidden shadow-md bg-slate-100">
            {destaques.length > 0 && (
              <Image
                src={destaques[carouselIndex].url_capa || "/placeholder-cover.svg"}
                alt={destaques[carouselIndex].titulo}
                fill
                style={{ objectFit: "cover" }}
                className="cursor-pointer"
                onClick={() => {
                  setModalLivro(destaques[carouselIndex]);
                  setModalOpen(true);
                }}
                sizes="(max-width: 768px) 100vw, 256px"
                priority
              />
            )}
            <button
              className="absolute left-2 top-1/2 -translate-y-1/2 bg-white/80 rounded-full p-2 shadow hover:bg-white"
              onClick={prevSlide}
              aria-label="Anterior"
              style={{ zIndex: 2 }}
            >
              &#8592;
            </button>
            <button
              className="absolute right-2 top-1/2 -translate-y-1/2 bg-white/80 rounded-full p-2 shadow hover:bg-white"
              onClick={nextSlide}
              aria-label="Próximo"
              style={{ zIndex: 2 }}
            >
              &#8594;
            </button>
          </div>
          <div className="mt-2 text-center font-semibold text-indigo-700">
            {destaques.length > 0 && destaques[carouselIndex].titulo}
          </div>
        </div>
      </section>

      {/* Destaques */}
      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-2xl font-bold mb-4 text-indigo-900">Destaques</h2>
        <ListaLivros livros={destaques} onReservar={(livro) => {
          setModalLivro(livro);
          setModalOpen(true);
        }} />
      </section>

      {/* Novas Aquisições */}
      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-2xl font-bold mb-4 text-indigo-900">Novas Aquisições</h2>
        <ListaLivros livros={livros.slice(-3)} onReservar={(livro) => {
          setModalLivro(livro);
          setModalOpen(true);
        }} />
      </section>

      {/* Catálogo Completo */}
      <section id="catalogo" className="w-full max-w-5xl">
        <h2 className="text-2xl font-bold mb-4 text-indigo-900">Catálogo Completo de Livros</h2>
        <ListaLivros livros={livros} onReservar={(livro) => {
          setModalLivro(livro);
          setModalOpen(true);
        }} />
      </section>

      {/* Histórico de Empréstimos */}
      <section id="historico" className="w-full max-w-5xl mt-10">
        <h2 className="text-2xl font-bold mb-4 text-indigo-900">Histórico de Empréstimos</h2>
        <TabelaHistoricoEmprestimos historico={historico} />
      </section>

      {/* Book Detail Modal */}
      <BookDetailModal
        livro={modalLivro}
        open={modalOpen}
        onOpenChange={setModalOpen}
        onReservar={handleReservarLivro}
      />
    </main>
  );
}
