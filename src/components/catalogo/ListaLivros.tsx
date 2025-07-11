import { Livro } from "@/types/entities";
import { LivroCard } from "./LivroCard";
import { BookDetailModal } from "./BookDetailModal";
import { useState, useEffect } from "react";

interface ListaLivrosProps {
  livros: Livro[];
  onReservar?: (livro: Livro) => void;
}

export function ListaLivros({ livros, onReservar }: ListaLivrosProps) {
  const [livroSelecionado, setLivroSelecionado] = useState<Livro | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [livrosComCapa, setLivrosComCapa] = useState<Livro[]>(livros);

  useEffect(() => {
    const fetchCovers = async () => {
      const updatedLivros = await Promise.all(
        livros.map(async (livro) => {
          if (!livro.url_capa) {
            try {
              const response = await fetch(`/api/book-cover?title=${encodeURIComponent(livro.titulo)}`);
              const data = await response.json();
              if (response.ok && data.coverUrl) {
                return { ...livro, url_capa: data.coverUrl };
              }
            } catch (error) {
              console.error(`Erro ao buscar capa para ${livro.titulo}:`, error);
            }
          }
          return livro;
        })
      );
      setLivrosComCapa(updatedLivros);
    };

    fetchCovers();
  }, [livros]);

  function handleOpenModal(livro: Livro) {
    setLivroSelecionado(livro);
    setModalOpen(true);
  }

  function handleReservar(livro: Livro) {
    if (onReservar) onReservar(livro);
    setModalOpen(false);
  }

  return (
    <>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {livrosComCapa.map((livro) => (
          <LivroCard key={livro.id_livro} livro={livro} onReservar={handleOpenModal} />
        ))}
      </div>
      {livroSelecionado && (
        <BookDetailModal
          livro={livroSelecionado}
          open={modalOpen}
          onOpenChange={setModalOpen}
          onReservar={onReservar ? handleReservar : undefined}
        />
      )}
    </>
  );
}