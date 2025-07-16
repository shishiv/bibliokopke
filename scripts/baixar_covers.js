/**
 * Script para baixar capas de livros do mock e salvar em public/covers/
 * Busca por ISBN, título+editora e título puro.
 * Gera relatório com links para busca manual (Google Imagens, Amazon, Estante Virtual).
 * Uso: node scripts/baixar_covers.js
 * Necessário: node-fetch@2, cheerio
 */
const fs = require("fs");
const path = require("path");
const fetch = require("node-fetch");
const cheerio = require("cheerio");

const MOCKS_PATH = path.resolve(__dirname, "../src/mocks/livros.ts");
const COVERS_DIR = path.resolve(__dirname, "../public/covers/");
const RELATORIO_PATH = path.resolve(__dirname, "../public/covers/relatorio_covers.txt");

if (!fs.existsSync(COVERS_DIR)) fs.mkdirSync(COVERS_DIR, { recursive: true });

// Utilitário para sanitizar nomes de arquivos
function sanitize(str) {
  return str
    .toLowerCase()
    .replace(/[^a-z0-9]+/gi, "-")
    .replace(/^-+|-+$/g, "");
}

// Extrai livrosBase do arquivo de mocks (como no popular_covers.js)
function extractLivrosBase(content) {
  const match = content.match(/const livrosBase:.*?=\s*(\[[\s\S]*?\]);/m);
  if (!match) throw new Error("Não foi possível encontrar o array livrosBase.");
  const arrayStr = match[1]
    .replace(/StatusLivro\.([A-Z_]+)/g, '"$1"')
    .replace(/(\w+):/g, '"$1":');
  // eslint-disable-next-line no-eval
  return eval(arrayStr);
}

// Baixa imagem de uma URL e salva no destino
async function downloadImage(url, dest) {
  try {
    const res = await fetch(url);
    if (!res.ok) return false;
    const buffer = await res.buffer();
    if (buffer.length < 1024) return false; // ignora imagens muito pequenas (provavelmente placeholder)
    fs.writeFileSync(dest, buffer);
    return true;
  } catch {
    return false;
  }
}

// Busca capa no Skoob (scraping leve)
async function getSkoobCover(query) {
  try {
    const searchUrl = `https://www.skoob.com.br/livro/busca?busca=${encodeURIComponent(query)}`;
    const res = await fetch(searchUrl);
    if (!res.ok) return null;
    const html = await res.text();
    const $ = cheerio.load(html);
    const link = $(".box-livro a").first().attr("href");
    if (!link) return null;
    const idMatch = link.match(/\/(\d+)[^\/]*$/);
    if (!idMatch) return null;
    const id = idMatch[1];
    return `https://skoob.com.br/img/livros_new/${id}.jpg`;
  } catch {
    return null;
  }
}

// Busca capa no Google Books
async function getGoogleBooksCover(query) {
  try {
    const url = `https://www.googleapis.com/books/v1/volumes?q=${encodeURIComponent(query)}`;
    const res = await fetch(url);
    if (!res.ok) return null;
    const data = await res.json();
    const img = data.items?.[0]?.volumeInfo?.imageLinks?.thumbnail;
    if (img) return img.replace("http://", "https://");
    return null;
  } catch {
    return null;
  }
}

// Busca capa no Open Library
function getOpenLibraryCover(isbn) {
  if (!isbn) return null;
  // Remove traços do ISBN
  const isbnNum = isbn.replace(/[^0-9Xx]/g, "");
  return `https://covers.openlibrary.org/b/isbn/${isbnNum}-L.jpg`;
}

// Gera links para busca manual
function gerarLinksBusca(titulo, editora) {
  const query = encodeURIComponent(`${titulo} ${editora || ""}`.trim());
  return {
    google: `https://www.google.com/search?tbm=isch&q=capa+${query}`,
    amazon: `https://www.amazon.com.br/s?k=${query}`,
    estante: `https://www.estantevirtual.com.br/busca?q=${query}`,
  };
}

(async () => {
  let content = fs.readFileSync(MOCKS_PATH, "utf-8");
  let livrosBase = extractLivrosBase(content);

  let notFound = [];

  for (let livro of livrosBase) {
    const isbn = livro.isbn;
    const titulo = livro.titulo;
    const editora = livro.editora || "";
    let filename = isbn
      ? `${isbn.replace(/[^0-9Xx]/g, "")}.jpg`
      : `${sanitize(titulo)}.jpg`;
    let dest = path.join(COVERS_DIR, filename);

    // Pula se já existe
    if (fs.existsSync(dest)) {
      console.log(`Capa já existe: ${filename}`);
      continue;
    }

    // 1. Skoob por ISBN
    let url = isbn && (await getSkoobCover(isbn));
    if (url && (await downloadImage(url, dest))) {
      console.log(`Baixada do Skoob (ISBN): ${filename}`);
      continue;
    }

    // 2. Skoob por Título+Editora
    url = await getSkoobCover(`${titulo} ${editora}`);
    if (url && (await downloadImage(url, dest))) {
      console.log(`Baixada do Skoob (Título+Editora): ${filename}`);
      continue;
    }

    // 3. Skoob por Título puro
    url = await getSkoobCover(titulo);
    if (url && (await downloadImage(url, dest))) {
      console.log(`Baixada do Skoob (Título): ${filename}`);
      continue;
    }

    // 4. Google Books por ISBN
    url = isbn && (await getGoogleBooksCover(`isbn:${isbn}`));
    if (url && (await downloadImage(url, dest))) {
      console.log(`Baixada do Google Books (ISBN): ${filename}`);
      continue;
    }

    // 5. Google Books por Título+Editora
    url = await getGoogleBooksCover(`${titulo} ${editora}`);
    if (url && (await downloadImage(url, dest))) {
      console.log(`Baixada do Google Books (Título+Editora): ${filename}`);
      continue;
    }

    // 6. Google Books por Título puro
    url = await getGoogleBooksCover(titulo);
    if (url && (await downloadImage(url, dest))) {
      console.log(`Baixada do Google Books (Título): ${filename}`);
      continue;
    }

    // 7. Open Library por ISBN
    url = getOpenLibraryCover(isbn);
    if (url && (await downloadImage(url, dest))) {
      console.log(`Baixada do Open Library (ISBN): ${filename}`);
      continue;
    }

    // Não encontrou
    notFound.push({ titulo, editora, isbn, filename });
    console.log(`Capa não encontrada: ${filename}`);
  }

  // Gera relatório de busca manual
  if (notFound.length) {
    let relatorio = "# Capas não encontradas automaticamente\n\n";
    for (const livro of notFound) {
      const { titulo, editora, isbn, filename } = livro;
      const links = gerarLinksBusca(titulo, editora);
      relatorio += `## ${titulo} (${isbn || "sem ISBN"})\n`;
      relatorio += `Arquivo sugerido: ${filename}\n\n`;
      relatorio += `- [Google Imagens](${links.google})\n`;
      relatorio += `- [Amazon Brasil](${links.amazon})\n`;
      relatorio += `- [Estante Virtual](${links.estante})\n\n`;
    }
    fs.writeFileSync(RELATORIO_PATH, relatorio, "utf-8");
    console.log(`\nRelatório gerado em: ${RELATORIO_PATH}`);
  } else {
    console.log("\nTodas as capas foram baixadas com sucesso!");
  }
})();
