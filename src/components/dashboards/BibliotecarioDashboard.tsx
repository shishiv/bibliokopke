"use client";
import { useState, useEffect } from "react";
import { getLivrosComCapas } from "@/mocks/livros";
import { ListaLivros } from "@/components/catalogo/ListaLivros";
import { historicoEmprestimos as historicoMock } from "@/mocks/historicoEmprestimos";
import { TabelaHistoricoEmprestimos } from "@/components/historico/TabelaHistoricoEmprestimos";
import { Livro, HistoricoEmprestimo, StatusLivro } from "@/types/entities";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { FormLivro } from "@/components/livros/FormLivro";
import { ModalDevolucao } from "@/components/emprestimos/ModalDevolucao";
import { toast } from "sonner";

export function BibliotecarioDashboard() {
  const [livros, setLivros] = useState<Livro[]>([]);

  useEffect(() => {
    getLivrosComCapas().then(setLivros);
  }, []);
  const [historico, setHistorico] = useState<HistoricoEmprestimo[]>(historicoMock);
  const [isAddBookModalOpen, setIsAddBookModalOpen] = useState(false);
  const [isDevolucaoModalOpen, setIsDevolucaoModalOpen] = useState(false);
  const [livroParaDevolucao, setLivroParaDevolucao] = useState<Livro | null>(null);

  function handleGerarRelatorio() {
    toast.success("Relatório de empréstimos gerado com sucesso!");
  }

  function handleAddLivro(novoLivro: Livro) {
    setLivros((prev) => [...prev, novoLivro]);
    toast.success(`Livro "${novoLivro.titulo}" adicionado com sucesso!`);
    setIsAddBookModalOpen(false);
  }

  function handleOpenDevolucaoModal(livro: Livro) {
    setLivroParaDevolucao(livro);
    setIsDevolucaoModalOpen(true);
  }

  function handleConfirmDevolucao(livro: Livro, observacoes?: string) {
    setLivros((prev) =>
      prev.map((l) =>
        l.id_livro === livro.id_livro ? { ...l, status: StatusLivro.DISPONIVEL, quantidade_disponivel: l.quantidade_disponivel + 1 } : l
      )
    );
    // Simular adição ao histórico de empréstimos
    setHistorico((prev) => [
      ...prev,
      {
        id_historico: Math.floor(Math.random() * 100000),
        id_emprestimo: Math.floor(Math.random() * 100000),
        codigo_simade: 999999, // Bibliotecário
        id_livro: livro.id_livro,
        data_emprestimo: "2025-06-01", // Data simulada
        data_devolucao: new Date().toISOString().slice(0, 10),
        dias_atraso: 0,
        multa: 0,
        observacoes: observacoes || "Devolução registrada pelo bibliotecário",
        data_registro: new Date().toISOString(),
      },
    ]);
    toast.success(`Livro "${livro.titulo}" devolvido com sucesso!`);
    setIsDevolucaoModalOpen(false);
    setLivroParaDevolucao(null);
  }

  return (
    <main className="flex min-h-screen flex-col items-center bg-slate-50 py-8">
      <h1 className="text-2xl font-bold mb-4">Dashboard do Bibliotecário</h1>
      <p className="mb-6">Bem-vindo(a), Administrador!</p>

      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-xl font-semibold mb-4">Destaques da Biblioteca</h2>
        <ListaLivros livros={livros.slice(0, 3)} />
      </section>

      <section className="w-full max-w-5xl mb-10">
        <h2 className="text-xl font-semibold mb-4">Novas Aquisições</h2>
        <ListaLivros livros={livros.slice(-3)} />
      </section>

      <section className="w-full max-w-5xl mb-10">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-semibold">Gerenciamento do Acervo</h2>
          <Dialog open={isAddBookModalOpen} onOpenChange={setIsAddBookModalOpen}>
            <DialogTrigger asChild>
              <Button>Adicionar Novo Livro</Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
              <DialogHeader>
                <DialogTitle>Adicionar Novo Livro</DialogTitle>
              </DialogHeader>
              <FormLivro onSubmit={handleAddLivro} />
            </DialogContent>
          </Dialog>
        </div>
        <ListaLivros livros={livros} />
      </section>

      <section className="w-full max-w-5xl mb-10">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-semibold">Gerenciamento de Empréstimos e Devoluções</h2>
          <Button onClick={() => handleOpenDevolucaoModal(livros[0])}>Registrar Devolução (Exemplo)</Button>
        </div>
        {livroParaDevolucao && (
          <ModalDevolucao
            livro={livroParaDevolucao}
            open={isDevolucaoModalOpen}
            onOpenChange={setIsDevolucaoModalOpen}
            onConfirmDevolucao={handleConfirmDevolucao}
          />
        )}
      </section>

      <section className="w-full max-w-5xl">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-xl font-semibold">Histórico Geral de Empréstimos</h2>
          <Button onClick={handleGerarRelatorio}>Gerar Relatório</Button>
        </div>
        <TabelaHistoricoEmprestimos historico={historico} />
      </section>
    </main>
  );
}
