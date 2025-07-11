import { useState } from "react";
import { Livro } from "@/types/entities";
import { Button } from "@/components/ui/button";

interface RecommendationManagerProps {
  livros: Livro[];
  onRecommend: (livro: Livro, tags: string[]) => void;
  recommended: { [id: number]: string[] };
}

export function RecommendationManager({ livros, onRecommend, recommended }: RecommendationManagerProps) {
  const [selectedLivro, setSelectedLivro] = useState<Livro | null>(null);
  const [tags, setTags] = useState<string[]>([]);

  function handleTagChange(tag: string) {
    setTags((prev) =>
      prev.includes(tag) ? prev.filter((t) => t !== tag) : [...prev, tag]
    );
  }

  function handleRecommend() {
    if (selectedLivro) {
      onRecommend(selectedLivro, tags);
      setSelectedLivro(null);
      setTags([]);
    }
  }

  return (
    <div>
      <h3 className="font-bold mb-2">Curadoria de Livros</h3>
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-4">
        {livros.map((livro) => (
          <div
            key={livro.id_livro}
            className={`p-2 border rounded cursor-pointer ${selectedLivro?.id_livro === livro.id_livro ? "border-indigo-600" : "border-gray-300"}`}
            onClick={() => setSelectedLivro(livro)}
          >
            <div className="font-semibold">{livro.titulo}</div>
            <div className="text-xs text-gray-500">{livro.autor}</div>
            <div className="mt-1">
              {recommended[livro.id_livro]?.map((tag) => (
                <span key={tag} className="inline-block bg-indigo-100 text-indigo-700 px-2 py-0.5 rounded text-xs mr-1">{tag}</span>
              ))}
            </div>
          </div>
        ))}
      </div>
      {selectedLivro && (
        <div className="mb-4">
          <div className="mb-2">Selecione tags para recomendação:</div>
          <div className="flex gap-2 mb-2">
            {["Recomendado", "Destaque Pedagógico", "Material de Apoio"].map((tag) => (
              <Button
                key={tag}
                variant={tags.includes(tag) ? "default" : "outline"}
                onClick={() => handleTagChange(tag)}
                size="sm"
              >
                {tag}
              </Button>
            ))}
          </div>
          <Button onClick={handleRecommend} disabled={tags.length === 0}>
            Salvar Recomendações
          </Button>
        </div>
      )}
    </div>
  );
}