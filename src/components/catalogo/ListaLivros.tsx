import { Livro } from "@/types/entities";
import { LivroCard } from "./LivroCard";
import { BookDetailModal } from "./BookDetailModal";
import { useState } from "react";

interface ListaLivrosProps {
  livros: Livro[];
  onReservar?: (livro: Livro) => void;
}

export function ListaLivros({ livros, onReservar }: ListaLivrosProps) {
  const [livroSelecionado, setLivroSelecionado] = useState<Livro | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

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
        {livros.map((livro: Livro) => (
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
