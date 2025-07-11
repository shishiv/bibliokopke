"use client";
import Link from "next/link";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Search } from "lucide-react";

export function Header() {
  return (
    <header className="bg-white shadow-md p-4 flex justify-between items-center">
      <Link href="/" className="text-2xl font-bold text-blue-600">
        Biblioteca Fronteira
      </Link>
      <div className="flex-1 max-w-xl mx-4">
        <div className="relative">
          <Input placeholder="Pesquisar livros, autores, ISBN..." className="pr-10" />
          <Button variant="ghost" size="icon" className="absolute right-0 top-0">
            <Search className="h-5 w-5" />
          </Button>
        </div>
      </div>
      <nav className="flex gap-4">
        <Link href="/aluno" className="text-gray-600 hover:text-blue-600">Aluno</Link>
        <Link href="/professor" className="text-gray-600 hover:text-blue-600">Professor</Link>
        <Link href="/bibliotecario" className="text-gray-600 hover:text-blue-600">Bibliotecário</Link>
      </nav>
    </header>
  );
}
