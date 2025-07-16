import { Livro, StatusLivro } from "@/types/entities";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import Image from "next/image";
import { useBookCover } from "./useBookCover";

interface LivroCardProps {
  livro: Livro;
  onReservar?: (livro: Livro) => void;
}

export function LivroCard({ livro, onReservar }: LivroCardProps) {
  const coverUrl = useBookCover(livro);

  return (
    <Card className="overflow-hidden hover:shadow-xl transition-shadow duration-300">
      <div className="relative h-48 w-full">
        <Image
          src={coverUrl}
          alt={`Capa do livro ${livro.titulo}`}
          fill
          style={{ objectFit: "contain", background: "#fff" }}
          loading="lazy"
        />
      </div>
      <CardContent className="p-4">
        <h3 className="font-bold text-lg truncate">{livro.titulo}</h3>
        <p className="text-sm text-muted-foreground mb-2">{livro.autor}</p>
        <div className="flex justify-between items-center mt-4">
          <span
            className={`text-xs font-semibold px-2 py-1 rounded-full ${{
              [StatusLivro.DISPONIVEL]: "bg-green-100 text-green-800",
              [StatusLivro.EMPRESTADO]: "bg-yellow-100 text-yellow-800",
              [StatusLivro.RESERVADO]: "bg-blue-100 text-blue-800",
              [StatusLivro.MANUTENCAO]: "bg-red-100 text-red-800",
            }[livro.status]}`}
          >
            {livro.status}
          </span>
          {livro.status === StatusLivro.DISPONIVEL && onReservar && (
            <Button size="sm" onClick={() => onReservar(livro)}>
              Reservar
            </Button>
          )}
        </div>
      </CardContent>
    </Card>
  );
}
