# BiblioKopke – Sistema de Gestão de Biblioteca Escolar

## Visão Geral

O BiblioKopke é um sistema web moderno para gestão de bibliotecas escolares, focado em usabilidade, performance e identidade visual forte. Ele atende a três perfis principais de usuários: alunos, professores e bibliotecários, oferecendo funcionalidades específicas para cada papel, além de um catálogo público de livros.

## Principais Funcionalidades

### 1. Login e Identidade Visual
- Tela de login com identidade visual personalizada (logo BiblioKopke, cores institucionais).
- Autenticação de usuários por perfil (aluno, professor, bibliotecário).
- Interface responsiva e acessível.

### 2. Catálogo de Livros
- Exibição de catálogo completo com busca, filtros e destaques.
- Capas dos livros exibidas em alta qualidade, carregadas localmente para máxima performance.
- Modal de detalhes do livro com informações completas (sinopse, autor, editora, status, etc).
- Slider/carrossel de destaques na dashboard do aluno.

### 3. Reserva e Empréstimo
- Alunos podem reservar livros disponíveis diretamente pelo catálogo.
- Professores podem recomendar livros para turmas ou alunos.
- Bibliotecários podem gerenciar reservas, empréstimos e devoluções.

### 4. Histórico e Recomendações
- Alunos visualizam histórico de empréstimos e reservas.
- Professores têm acesso a relatórios de recomendações e podem acompanhar o engajamento dos alunos.
- Recomendações personalizadas baseadas em perfil e histórico.

### 5. Gestão de Acervo (Bibliotecário)
- Cadastro, edição e exclusão de livros.
- Gerenciamento de status (disponível, emprestado, reservado).
- Visualização de relatórios de uso do acervo.

### 6. Performance e Experiência
- Todas as imagens de capas são servidas localmente, eliminando dependências externas e acelerando o carregamento.
- UI/UX refinada, com navegação fluida, feedback visual e componentes reutilizáveis.
- Otimização de carregamento de dados, evitando requisições desnecessárias.

### 7. Branding e Comunicação
- Todas as referências à antiga marca foram substituídas por BiblioKopke.
- Logo e identidade visual aplicadas em todas as telas principais.

## Arquitetura e Tecnologias

- **Frontend:** React/Next.js (App Router), componentes funcionais, hooks, TypeScript.
- **Mock de Dados:** Utilização de arquivos locais para simular banco de dados e API.
- **Imagens:** Capas baixadas e salvas em `/public/covers/`, referenciadas diretamente no mock.
- **Componentização:** Separação clara entre componentes de catálogo, dashboards, histórico, etc.
- **Estilo:** CSS-in-JS ou Tailwind, com foco em responsividade e acessibilidade.

## Fluxo de Usuário

1. **Login:** Usuário acessa a tela de login, insere credenciais e é redirecionado para seu dashboard.
2. **Dashboard:** Visualiza destaques, histórico e pode navegar pelo catálogo.
3. **Catálogo:** Busca, filtra e visualiza detalhes dos livros. Pode reservar se for aluno.
4. **Gestão:** Bibliotecário acessa painel de administração para gerenciar acervo e usuários.
5. **Recomendações:** Professores recomendam livros, alunos recebem sugestões personalizadas.
6. **Histórico:** Usuário acompanha suas reservas, empréstimos e devoluções.

## Diferenciais

- **Performance:** Carregamento instantâneo das capas e dados, mesmo em conexões lentas.
- **Experiência do Usuário:** Interface intuitiva, moderna e adaptada para dispositivos móveis.
- **Foco no Brasil:** Busca e exibição de capas priorizando obras brasileiras, com possibilidade de ajuste manual.
- **Extensibilidade:** Estrutura pronta para integração futura com banco de dados real e autenticação robusta.
