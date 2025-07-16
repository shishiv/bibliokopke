import { useMemo } from "react";
import { livrosBase } from "@/mocks/livros";
import { Livro } from "@/types/entities";

/**
 * Hook para obter a lista de livros (mock).
 * Futuramente pode ser adaptado para fetch real.
 */
export function useLivros(): Livro[] {
  // Memoiza o mock para evitar re-renderizações desnecessárias
  return useMemo(() => livrosBase, []);
}
