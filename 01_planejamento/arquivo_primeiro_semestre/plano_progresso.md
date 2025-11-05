# Plano e Progresso — Protótipo Biblioteca João Kopke

## Macroetapas

- [x] 1. Inicialização do Projeto
  - [x] Criar projeto Next.js com TailwindCSS
  - [x] Instalar e configurar shadcn/ui
  - [x] Configurar estrutura de pastas

- [x] 2. Modelagem de Tipos e Mocks
  - [x] Definir tipagens das entidades (usuário, livro, empréstimo, reserva, etc)
  - [x] Criar arquivos de dados mockados

- [ ] 3. Estruturação das Páginas e Navegação
  - [x] Implementar página de login/seleção de perfil
  - [x] Criar dashboards para cada perfil
  - [x] Configurar navegação entre páginas

- [ ] 4. Implementação dos Componentes Interativos
  - [x] Listas e cards de livros (catálogo)
  - [x] Modais de reserva, devolução, confirmação
  - [x] Tabelas de relatórios e históricos
  - [x] Formulários de cadastro/edição de livros
  - [x] Menus e navegação

- [ ] 5. Integração dos Mocks nas Interfaces
- [x] Conectar dados mockados às páginas e componentes
- [x] Simular ações (reserva, empréstimo, devolução, etc)

- [ ] 6. Ajustes de UX e Responsividade
- [x] Refinar layout e responsividade
- [x] Adicionar feedbacks visuais (toasts, alertas)

- [ ] 7. Documentação e Finalização
- [x] Documentar o protótipo e instruções de uso
- [x] Revisar checklist de funcionalidades

---

## Progresso Detalhado

| Etapa                       | Status    | Observações |
|-----------------------------|-----------|-------------|
| Inicialização do Projeto    | Concluído | Projeto Next.js criado com TailwindCSS, shadcn/ui inicializado e estrutura pronta. |
| Modelagem de Tipos e Mocks  | Concluído | Tipagens e arquivos de mocks criados para todas as entidades principais. |
| Estruturação das Páginas    | Concluído | Páginas de login/seleção de perfil e dashboards de Aluno, Professor e Bibliotecário criadas. Navegação básica pronta. |
| Componentes Interativos     | Concluído | Catálogo de livros implementado como componente interativo (ListaLivros) usando shadcn/ui e Tailwind. Integrado à dashboard do Aluno. Modais de reserva implementados e integrados ao catálogo. Tabela de histórico de empréstimos criada e integrada à dashboard do Aluno. Formulário de cadastro de livros criado e integrado à dashboard do Bibliotecário. Menu de navegação criado e integrado ao layout principal, permitindo acesso rápido a todas as páginas principais. UI/UX aprimorada com novos componentes de card para livros e seções de destaque nos dashboards.
| Integração dos Mocks        | Concluído | Catálogo, histórico e ações de reserva integrados aos mocks. Simulação de reserva, empréstimo e devolução implementada nas interfaces. |
| Ajustes de UX               | Concluído | Layout refinado para responsividade, feedbacks visuais (toasts) integrados às principais ações. |
| Documentação                | Concluído | README.md criado com instruções de uso, funcionalidades, tecnologias, contribuição e contato. Checklist revisado e validado. |

---

## Próximos Passos

- [ ] Implementar os modais de reserva, devolução e confirmação
- [ ] Implementar os formulários de cadastro e edição de livros
- [x] Conectar os formulários e modais às ações do sistema (simuladas com mocks)
- [ ] Atualizar este arquivo conforme o progresso
