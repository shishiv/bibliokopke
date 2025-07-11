import { Livro } from "@/types/entities";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { useState } from "react";

interface ModalReservaLivroProps {
  livro: Livro;
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onReservar: (livro: Livro) => void;
}

export function ModalReservaLivro({ livro, open, onOpenChange, onReservar }: ModalReservaLivroProps) {
  const [loading, setLoading] = useState(false);

  function handleReservar() {
    setLoading(true);
    setTimeout(() => {
      setLoading(false);
      onReservar(livro);
      onOpenChange(false);
    }, 1000); // simula requisição
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Reservar Livro</DialogTitle>
        </DialogHeader>
        <div>
          <p className="font-semibold">{livro.titulo}</p>
          <p className="text-sm text-muted-foreground mb-2">{livro.autor}</p>
          <p className="text-xs">Tem certeza que deseja reservar este livro?</p>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)} disabled={loading}>
            Cancelar
          </Button>
          <Button onClick={handleReservar} disabled={loading}>
            {loading ? "Reservando..." : "Confirmar Reserva"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
