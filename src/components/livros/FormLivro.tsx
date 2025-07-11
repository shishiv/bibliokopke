import { Livro } from "@/types/entities";
import { useState } from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

interface FormLivroProps {
  onSubmit: (livro: Livro) => void;
  initialData?: Partial<Livro>;
}

export function FormLivro({ onSubmit, initialData = {} }: FormLivroProps) {
  const [titulo, setTitulo] = useState(initialData.titulo || "");
  const [autor, setAutor] = useState(initialData.autor || "");
  const [editora, setEditora] = useState(initialData.editora || "");
  const [ano_publicacao, setAnoPublicacao] = useState(
    initialData.ano_publicacao || ""
  );
  const [categoria, setCategoria] = useState(initialData.categoria || "");
  const [localizacao, setLocalizacao] = useState(
    initialData.localizacao || ""
  );
  const [url_capa, setUrlCapa] = useState(initialData.url_capa || "");
  const [isFetchingCover, setIsFetchingCover] = useState(false);

  async function handleFetchCover() {
    if (!titulo) {
      toast.error("Digite um título para buscar a capa.");
      return;
    }
    setIsFetchingCover(true);
    try {
      const response = await fetch(`/api/book-cover?title=${encodeURIComponent(titulo)}`);
      const data = await response.json();
      if (response.ok && data.coverUrl) {
        setUrlCapa(data.coverUrl);
        toast.success("Capa encontrada e preenchida!");
      } else {
        toast.error(data.error || "Nenhuma capa encontrada para este título.");
      }
    } catch {
      toast.error("Falha ao se comunicar com a API de capas.");
    } finally {
      setIsFetchingCover(false);
    }
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    onSubmit({
      id_livro: initialData.id_livro || Math.floor(Math.random() * 10000),
      isbn: initialData.isbn || "",
      titulo,
      autor,
      editora,
      ano_publicacao: Number(ano_publicacao),
      categoria,
      numero_paginas: initialData.numero_paginas || 0,
      idioma: initialData.idioma || "Português",
      sinopse: initialData.sinopse || "",
      localizacao,
      status: initialData.status || "DISPONIVEL",
      quantidade_total: initialData.quantidade_total || 1,
      quantidade_disponivel: initialData.quantidade_disponivel || 1,
      data_cadastro:
        initialData.data_cadastro || new Date().toISOString().slice(0, 10),
      data_atualizacao: new Date().toISOString().slice(0, 10),
      url_capa,
    } as Livro);
  }

  return (
    <form className="space-y-4" onSubmit={handleSubmit}>
      <div>
        <Label htmlFor="titulo">Título</Label>
        <div className="flex items-center gap-2">
          <Input
            id="titulo"
            value={titulo}
            onChange={(e) => setTitulo(e.target.value)}
            required
          />
          <Button
            type="button"
            onClick={handleFetchCover}
            disabled={isFetchingCover}
          >
            {isFetchingCover ? "Buscando..." : "Buscar Capa"}
          </Button>
        </div>
      </div>
      <div>
        <Label htmlFor="url_capa">URL da Capa</Label>
        <Input
          id="url_capa"
          value={url_capa}
          onChange={(e) => setUrlCapa(e.target.value)}
          placeholder="https://exemplo.com/capa.jpg"
        />
      </div>
      <div>
        <Label htmlFor="autor">Autor</Label>
        <Input
          id="autor"
          value={autor}
          onChange={(e) => setAutor(e.target.value)}
          required
        />
      </div>
      <div>
        <Label htmlFor="editora">Editora</Label>
        <Input
          id="editora"
          value={editora}
          onChange={(e) => setEditora(e.target.value)}
        />
      </div>
      <div>
        <Label htmlFor="ano_publicacao">Ano de Publicação</Label>
        <Input
          id="ano_publicacao"
          type="number"
          value={ano_publicacao}
          onChange={(e) => setAnoPublicacao(e.target.value)}
        />
      </div>
      <div>
        <Label htmlFor="categoria">Categoria</Label>
        <Input
          id="categoria"
          value={categoria}
          onChange={(e) => setCategoria(e.target.value)}
        />
      </div>
      <div>
        <Label htmlFor="localizacao">Localização</Label>
        <Input
          id="localizacao"
          value={localizacao}
          onChange={(e) => setLocalizacao(e.target.value)}
        />
      </div>
      <Button type="submit">Salvar Livro</Button>
    </form>
  );
}
