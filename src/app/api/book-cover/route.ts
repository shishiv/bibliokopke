import { NextResponse } from 'next/server';

export async function GET(request: Request) {
  const { searchParams } = new URL(request.url);
  const title = searchParams.get('title');
  const isbn = searchParams.get('isbn');

  if (!title && !isbn) {
    return NextResponse.json({ error: 'O título ou ISBN do livro é obrigatório.' }, { status: 400 });
  }

  // Try Open Library Covers API first if ISBN is available
  if (isbn) {
    const openLibUrl = `https://covers.openlibrary.org/b/isbn/${isbn}-L.jpg?default=false`;
    const openLibRes = await fetch(openLibUrl, { method: "HEAD" });
    if (openLibRes.ok) {
      return NextResponse.json({ coverUrl: openLibUrl });
    }
  }

  // If no ISBN or Open Library fails, try to get ISBN from Google Books
  try {
    let googleUrl = "https://www.googleapis.com/books/v1/volumes?q=";
    if (isbn) {
      googleUrl += `isbn:${encodeURIComponent(isbn)}`;
    } else {
      googleUrl += encodeURIComponent(title!);
    }
    const response = await fetch(googleUrl);
    const data = await response.json();

    if (data.items && data.items.length > 0) {
      const book = data.items[0];
      // Try Open Library with ISBN from Google Books
      type IndustryIdentifier = { type: string; identifier: string };
      const industryIdentifiers: IndustryIdentifier[] = book.volumeInfo.industryIdentifiers || [];
      const isbnFromGoogle =
        industryIdentifiers.find((id) => id.type === "ISBN_13")?.identifier ||
        industryIdentifiers.find((id) => id.type === "ISBN_10")?.identifier;
      if (isbnFromGoogle) {
        const openLibUrl = `https://covers.openlibrary.org/b/isbn/${isbnFromGoogle}-L.jpg?default=false`;
        const openLibRes = await fetch(openLibUrl, { method: "HEAD" });
        if (openLibRes.ok) {
          return NextResponse.json({ coverUrl: openLibUrl });
        }
      }
      // Fallback to best Google Books image
      type ImageLinks = {
        extraLarge?: string;
        large?: string;
        medium?: string;
        thumbnail?: string;
        smallThumbnail?: string;
      };
      const imageLinks: ImageLinks = book.volumeInfo.imageLinks || {};
      const coverUrl =
        imageLinks.extraLarge ||
        imageLinks.large ||
        imageLinks.medium ||
        imageLinks.thumbnail ||
        imageLinks.smallThumbnail;
      if (coverUrl) {
        return NextResponse.json({ coverUrl });
      }
    }
  } catch {
    // Continue to fallback
  }

  // Fallback: improved local SVG placeholder
  return NextResponse.json({
    coverUrl: "/placeholder-cover.svg"
  }, { status: 200 });
}
