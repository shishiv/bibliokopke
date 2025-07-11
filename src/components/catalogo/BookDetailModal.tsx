import { Livro } from "@/types/entities";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import Image from "next/image";
import { Button } from "@/components/ui/button";

interface BookDetailModalProps {
  livro: Livro | null;
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onReservar?: (livro: Livro) => void;
}

export function BookDetailModal({ livro, open, onOpenChange, onReservar }: BookDetailModalProps) {
  if (!livro) return null;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg">
        <DialogHeader>
          <DialogTitle>{livro.titulo}</DialogTitle>
        </DialogHeader>
        <div className="flex flex-col md:flex-row gap-4">
          <div className="flex-shrink-0 w-full md:w-40 h-60 relative">
            <Image
              src={livro.url_capa || "/placeholder-cover.svg"}
              alt={`Capa do livro ${livro.titulo}`}
              fill
              style={{ objectFit: "cover", borderRadius: 8 }}
              sizes="(max-width: 768px) 100vw, 40vw"
              priority
            />
          </div>
          <div className="flex-1 flex flex-col gap-2">
            <div>
              <span className="font-semibold">Autor:</span> {livro.autor}
            </div>
            <div>
              <span className="font-semibold">Editora:</span> {livro.editora}
            </div>
            <div>
              <span className="font-semibold">Ano:</span> {livro.ano_publicacao}
            </div>
            <div>
              <span className="font-semibold">ISBN:</span> {livro.isbn}
            </div>
            <div>
              <span className="font-semibold">Categoria:</span> {livro.categoria}
            </div>
            <div>
              <span className="font-semibold">Páginas:</span> {livro.numero_paginas}
            </div>
            <div>
              <span className="font-semibold">Sinopse:</span> {livro.sinopse}
            </div>
            {onReservar && (
              <Button className="mt-4" onClick={() => onReservar(livro)}>
                Reservar
              </Button>
            )}
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}