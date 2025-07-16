/**
 * Script para popular o campo url_capa em livrosBase de src/mocks/livros.ts
 * Uso: node scripts/popular_covers.js
 */
const fs = require("fs");
const path = require("path");
const fetch = require("node-fetch");

// Caminho do arquivo de mocks
const MOCKS_PATH = path.resolve(__dirname, "../src/mocks/livros.ts");

// Função para extrair o array livrosBase do arquivo
function extractLivrosBase(content) {
  const match = content.match(/const livrosBase:.*?=\s*(\[[\s\S]*?\]);/m);
  if (!match) throw new Error("Não foi possível encontrar o array livrosBase.");
  // Substitui StatusLivro.XYZ por "XYZ" para permitir eval seguro
  const arrayStr = match[1]
    .replace(/StatusLivro\.([A-Z_]+)/g, '"$1"')
    .replace(/(\w+):/g, '"$1":');
  // eslint-disable-next-line no-eval
  return eval(arrayStr);
}

// Função para substituir o array livrosBase no arquivo
function replaceLivrosBase(content, newArrayStr) {
  return content.replace(
    /const livrosBase:.*?=\s*\[[\s\S]*?\];/m,
    `const livrosBase: Omit<Livro, "url_capa">[] = ${newArrayStr};`
  );
}

// Função para buscar a capa
async function fetchCapa(titulo, isbn) {
  let url = `http://localhost:3001/api/book-cover?title=${encodeURIComponent(titulo)}`;
  if (isbn) url += `&isbn=${encodeURIComponent(isbn)}`;
  try {
    const res = await fetch(url);
    if (!res.ok) return "";
    const data = await res.json();
    return data.coverUrl || "";
  } catch {
    return "";
  }
}

(async () => {
  let content = fs.readFileSync(MOCKS_PATH, "utf-8");
  let livrosBase = extractLivrosBase(content);

  // Para cada livro, buscar a capa e adicionar url_capa
  for (let livro of livrosBase) {
    if (!livro.url_capa || livro.url_capa === "") {
      livro.url_capa = await fetchCapa(livro.titulo, livro.isbn);
      console.log(`Capa de "${livro.titulo}": ${livro.url_capa}`);
    }
  }

  // Gerar string do novo array (com url_capa)
  const newArrayStr = JSON.stringify(livrosBase, null, 2)
    .replace(/"(\w+)":/g, "$1:") // remover aspas das chaves
    .replace(/"([^"]+)"/g, (m, p1) => {
      // manter aspas só em strings, não em números/booleans
      if (/^\d+$/.test(p1)) return p1;
      return `"${p1}"`;
    });

  // Substituir no arquivo
  const newContent = replaceLivrosBase(content, newArrayStr);
  fs.writeFileSync(MOCKS_PATH, newContent, "utf-8");
  console.log("Arquivo de mocks atualizado com url_capa.");
})();
