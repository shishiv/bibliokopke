# Protótipo Biblioteca João Kopke

## Visão Geral

Este projeto é um protótipo interativo de um sistema de biblioteca escolar, desenvolvido em Next.js, TailwindCSS e shadcn/ui, com dados mockados e interfaces responsivas para simulação e apresentação.

## Funcionalidades Implementadas

- **Login/Seleção de Perfil:** Escolha entre Aluno, Professor ou Bibliotecário.
- **Dashboard do Aluno:** Catálogo de livros, reserva simulada, histórico de empréstimos dinâmico.
- **Dashboard do Professor:** Estrutura pronta para futuras integrações.
- **Dashboard do Bibliotecário:** Cadastro de novos livros, listagem dinâmica.
- **Menus e Navegação:** Acesso rápido a todas as páginas principais.
- **Feedbacks Visuais:** Toasts para ações importantes.
- **Responsividade:** Layout adaptado para desktop e mobile.
- **Dados Mockados:** Todas as operações são simuladas localmente.

## Como Executar

1. Instale as dependências:
   ```bash
   npm install
   ```
2. Rode o servidor de desenvolvimento:
   ```bash
   npm run dev
   ```
3. Acesse [http://localhost:3000](http://localhost:3000) no navegador.

## Estrutura de Pastas

- `src/app/` — Páginas e layouts principais.
- `src/components/` — Componentes reutilizáveis (catálogo, formulários, tabelas, modais, navegação).
- `src/mocks/` — Dados simulados (livros, usuários, histórico, etc).
- `src/types/` — Tipagens TypeScript das entidades.

## Apresentação

- Demonstre a navegação entre perfis.
- Simule reservas e cadastros para mostrar feedbacks e atualização dinâmica.
- Mostre a responsividade redimensionando a janela.
- Explique que todos os dados são mockados, mas a estrutura está pronta para integração real.

## Checklist de Funcionalidades

- [x] Estrutura Next.js, TailwindCSS, shadcn/ui
- [x] Tipagens e mocks completos
- [x] Catálogo de livros interativo
- [x] Modais e formulários funcionais
- [x] Histórico dinâmico do aluno
- [x] Cadastro de livros pelo bibliotecário
- [x] Navegação e layout responsivo
- [x] Feedbacks visuais (toasts)
- [x] Documentação

## Tecnologias e Dependências

- [Next.js](https://nextjs.org/)
- [TailwindCSS](https://tailwindcss.com/)
- [shadcn/ui](https://ui.shadcn.com/)
- [sonner](https://sonner.emilkowal.ski/) (toasts)
- [TypeScript](https://www.typescriptlang.org/)

## Contribuição

Sugestões, melhorias ou correções podem ser enviadas via pull request ou contato direto.

## Contato

Dúvidas ou suporte: [equipe@joaokopke.edu.br](mailto:equipe@joaokopke.edu.br)

---

Protótipo desenvolvido para a Escola Estadual João Kopke.
