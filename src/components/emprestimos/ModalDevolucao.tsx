import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Livro } from "@/types/entities";
import { useState } from "react";

interface ModalDevolucaoProps {
  livro: Livro;
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onConfirmDevolucao: (livro: Livro, observacoes?: string) => void;
}

export function ModalDevolucao({
  livro,
  open,
  onOpenChange,
  onConfirmDevolucao,
}: ModalDevolucaoProps) {
  const [observacoes, setObservacoes] = useState("");

  const handleConfirm = () => {
    onConfirmDevolucao(livro, observacoes);
    setObservacoes(""); // Limpa as observações
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Confirmar Devolução</DialogTitle>
          <DialogDescription>
            Confirme a devolução do livro &apos;{livro.titulo}&apos;.
          </DialogDescription>
        </DialogHeader>
        <div className="grid gap-4 py-4">
          <div className="grid grid-cols-4 items-center gap-4">
            <Label htmlFor="observacoes" className="text-right">
              Observações
            </Label>
            <Input
              id="observacoes"
              value={observacoes}
              onChange={(e) => setObservacoes(e.target.value)}
              className="col-span-3"
            />
          </div>
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Cancelar
          </Button>
          <Button onClick={handleConfirm}>Confirmar Devolução</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
