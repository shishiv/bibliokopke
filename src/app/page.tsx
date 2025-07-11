"use client";

import { useRouter } from "next/navigation";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Book, GraduationCap, Briefcase } from "lucide-react";

export default function Home() {
  const router = useRouter();

  return (
    <main className="flex min-h-screen flex-col items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100 p-8">
      <section className="text-center mb-12">
        <h1 className="text-5xl font-extrabold text-gray-900 mb-4 leading-tight">
          Bem-vindo à Biblioteca João Kopke
        </h1>
        <p className="text-xl text-gray-600 max-w-2xl mx-auto">
          Seu portal completo para o conhecimento. Acesse o catálogo, gerencie seus empréstimos e explore um mundo de livros.
        </p>
      </section>

      <section className="w-full max-w-4xl">
        <h2 className="text-3xl font-bold text-center text-gray-800 mb-8">Selecione seu Perfil</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          <Card
            className="cursor-pointer hover:shadow-lg transition-shadow duration-300 transform hover:-translate-y-1"
            onClick={() => router.push("/aluno")}
          >
            <CardHeader className="flex flex-col items-center justify-center p-6">
              <GraduationCap className="h-12 w-12 text-blue-500 mb-3" />
              <CardTitle className="text-xl font-semibold text-gray-800">Aluno</CardTitle>
            </CardHeader>
            <CardContent className="text-center text-gray-600">
              Acesse seu histórico de empréstimos e reserve livros.
            </CardContent>
          </Card>

          <Card
            className="cursor-pointer hover:shadow-lg transition-shadow duration-300 transform hover:-translate-y-1"
            onClick={() => router.push("/professor")}
          >
            <CardHeader className="flex flex-col items-center justify-center p-6">
              <Briefcase className="h-12 w-12 text-green-500 mb-3" />
              <CardTitle className="text-xl font-semibold text-gray-800">Professor</CardTitle>
            </CardHeader>
            <CardContent className="text-center text-gray-600">
              Solicite materiais para suas aulas e consulte o acervo.
            </CardContent>
          </Card>

          <Card
            className="cursor-pointer hover:shadow-lg transition-shadow duration-300 transform hover:-translate-y-1"
            onClick={() => router.push("/bibliotecario")}
          >
            <CardHeader className="flex flex-col items-center justify-center p-6">
              <Book className="h-12 w-12 text-purple-500 mb-3" />
              <CardTitle className="text-xl font-semibold text-gray-800">Bibliotecário</CardTitle>
            </CardHeader>
            <CardContent className="text-center text-gray-600">
              Gerencie o acervo, empréstimos e relatórios.
            </CardContent>
          </Card>
        </div>
      </section>
    </main>
  );
}