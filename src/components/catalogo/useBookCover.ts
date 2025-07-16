import { useEffect, useState } from "react";
import { Livro } from "@/types/entities";

/**
 * Hook para obter a URL da capa do livro, com cache localStorage e fallback.
 * - Usa url_capa do livro se disponível.
 * - Busca remota se necessário.
 * - Cache expira em 7 dias.
 */
export function useBookCover(livro: Livro | null) {
  const [coverUrl, setCoverUrl] = useState<string>(livro?.url_capa || "/placeholder-cover.svg");

  useEffect(() => {
    if (!livro) return;

    let isMounted = true;
    const cacheKey = `coverUrl:${livro.titulo}`;
    const cacheExpiry = 1000 * 60 * 60 * 24 * 7; // 7 dias

    async function fetchCover(l: Livro) {
      // 1. Tenta cache localStorage
      const cached = localStorage.getItem(cacheKey);
      if (cached) {
        try {
          const { url, timestamp } = JSON.parse(cached);
          if (Date.now() - timestamp < cacheExpiry) {
            setCoverUrl(url);
            return;
          }
        } catch {
          // ignore parse error
        }
      }
      // 2. Busca remota
      if (!l.url_capa) {
        try {
          const res = await fetch(`/api/book-cover?title=${encodeURIComponent(l.titulo)}`);
          if (res.ok) {
            const data = await res.json();
            if (data.coverUrl && isMounted) {
              setCoverUrl(data.coverUrl);
              // Salva no cache local
              localStorage.setItem(cacheKey, JSON.stringify({ url: data.coverUrl, timestamp: Date.now() }));
            }
          }
        } catch {
          // ignore
        }
      }
    }
    fetchCover(livro);
    return () => { isMounted = false; };
  }, [livro]);

  return coverUrl || "/placeholder-cover.svg";
}
