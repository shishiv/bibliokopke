import { NextResponse } from "next/server";

// In-memory mock for recommendations (replace with DB integration as needed)
let recommendations: {
  id: number;
  codigo_simade: number;
  id_livro: number;
  turma?: string;
  disciplina?: string;
  destaque?: string[];
  data: string;
}[] = [];

export async function GET(request: Request) {
  const { searchParams } = new URL(request.url);
  const codigo_simade = searchParams.get("codigo_simade");
  const id_livro = searchParams.get("id_livro");

  let result = recommendations;
  if (codigo_simade) {
    result = result.filter((r) => r.codigo_simade === Number(codigo_simade));
  }
  if (id_livro) {
    result = result.filter((r) => r.id_livro === Number(id_livro));
  }
  return NextResponse.json(result);
}

export async function POST(request: Request) {
  const body = await request.json();
  const newRec = {
    id: Date.now(),
    ...body,
    data: new Date().toISOString(),
  };
  recommendations.push(newRec);
  return NextResponse.json(newRec, { status: 201 });
}

export async function DELETE(request: Request) {
  const { searchParams } = new URL(request.url);
  const id = searchParams.get("id");
  if (!id) {
    return NextResponse.json({ error: "ID obrigatório" }, { status: 400 });
  }
  recommendations = recommendations.filter((r) => r.id !== Number(id));
  return NextResponse.json({ success: true });
}