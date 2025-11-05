# BiblioKopke 📚

Sistema de Gestão de Biblioteca Escolar para a Escola Estadual João Kopke.

**Stack:** Next.js 15 + TypeScript + Supabase (PostgreSQL) + Tailwind CSS

---

## 📋 Visão Geral

O BiblioKopke é um sistema completo de gestão de biblioteca desenvolvido como **Projeto Interdisciplinar IV**, adaptado para stack web moderna (Next.js + Supabase) ao invés da proposta original de desktop em C#.

### Funcionalidades Principais

- **Gestão de Livros:** Cadastro, busca full-text, categorização
- **Empréstimos e Devoluções:** Controle automático de disponibilidade
- **Reservas:** Sistema de fila para livros indisponíveis
- **Recomendações:** Professores recomendam livros para alunos/turmas
- **Relatórios:** Estatísticas e relatórios exportáveis (PDF/CSV)
- **Controle de Acesso:** 3 perfis (Aluno, Professor, Bibliotecário)
- **Preparado para SIMADE:** Integração futura com sistema do governo

---

## 🏗️ Arquitetura

### Desenvolvimento Local
```
Docker Compose
└── PostgreSQL 15
    └── pgAdmin (interface web)

Next.js (localhost:3000)
└── Mock Data → PostgreSQL local
```

### Produção
```
Internet
    ↓
Traefik (Easypanel)
    ↓
Next.js Container
    ↓
Supabase Cloud
    ├── PostgreSQL
    ├── Auth
    └── Storage
```

---

## 🚀 Quick Start

### Pré-requisitos
- Node.js 18+
- Docker Desktop
- Git

### Instalação

```bash
# 1. Clone o repositório
git clone <url-do-repo>
cd BiblioKopke/06_proto

# 2. Instale dependências
npm install

# 3. Configure variáveis de ambiente
cp .env.example .env.local

# 4. Suba o PostgreSQL local
docker-compose up -d

# 5. Inicie o servidor de desenvolvimento
npm run dev
```

Acesse: **http://localhost:3000**

---

## 📁 Estrutura do Projeto

```
BiblioKopke/
├── 01_planejamento/           # Documentos de planejamento
│   └── Projeto Interdisciplinar IV.pdf
├── 02_modelagem_banco/        # Schema SQL de referência
├── 03_requisitos/             # Requisitos e user stories
├── 04_diagramas/              # UML (casos de uso, classes, etc.)
├── 05_relatorios/             # Relatórios acadêmicos
├── 06_proto/                  # ⭐ APLICAÇÃO NEXT.JS
│   ├── src/                   # Código-fonte
│   │   ├── app/              # Pages (App Router)
│   │   ├── components/       # Componentes React
│   │   ├── lib/              # Utilities + Supabase client
│   │   ├── services/         # Data access layer
│   │   ├── hooks/            # Custom hooks
│   │   └── types/            # TypeScript types
│   ├── database/             # SQL migrations e seeds
│   ├── docker-compose.yml    # PostgreSQL + pgAdmin
│   └── .env.example          # Template de configuração
├── PRD.md                     # Product Requirements Document
├── SPRINT_1.md                # Guia da Sprint 1
├── DEPLOY_EASYPANEL.md        # Guia de deploy em produção
└── README.md                  # Este arquivo
```

---

## 📖 Documentação

### Para Desenvolvedores

- **[06_proto/README.md](06_proto/README.md)** - Setup completo do ambiente de desenvolvimento
- **[PRD.md](PRD.md)** - Requisitos completos, arquitetura, roadmap
- **[SPRINT_1.md](SPRINT_1.md)** - Checklist detalhado da Sprint 1
- **[database/README.md](06_proto/database/README.md)** - Guia do banco de dados

### Para Deploy

- **[DEPLOY_EASYPANEL.md](DEPLOY_EASYPANEL.md)** - Deploy em produção (Easypanel + Supabase Cloud)

### Para Professores/Avaliação

- **[01_planejamento/Projeto Interdisciplinar IV.pdf](01_planejamento/Projeto%20Interdisciplinar%20IV.pdf)** - Requisitos originais do projeto
- **[05_relatorios/](05_relatorios/)** - Relatórios e apresentações

---

## 🎯 Status do Projeto

### ✅ Concluído (Sprint 0)

- [x] Levantamento de requisitos
- [x] Modelagem UML completa
- [x] Design de banco de dados
- [x] Protótipo Next.js com mock data
- [x] Interface completa e responsiva
- [x] Documentação técnica (PRD, guias)

### 🚧 Em Andamento (Sprint 1)

- [ ] Infraestrutura de desenvolvimento (Docker + PostgreSQL)
- [ ] Migrations do banco de dados
- [ ] Integração Next.js ↔ Supabase
- [ ] CRUD de livros funcionando
- [ ] Sistema de autenticação

### 📅 Próximas Sprints

**Sprint 2** (22/out - 06/nov): Autenticação + Empréstimos
**Sprint 3** (06/nov - 21/nov): Relatórios + UX
**Sprint 4** (21/nov - 30/nov): Deploy + Documentação final

Ver roadmap completo em [PRD.md](PRD.md).

---

## 🧑‍💻 Tecnologias

### Frontend
- Next.js 15.3.4 (App Router)
- React 19.1.0
- TypeScript 5.8.3
- Tailwind CSS 4.1.11
- shadcn/ui (Radix UI)
- React Hook Form + Zod

### Backend
- Supabase (PostgreSQL + Auth + Storage)
- PostgreSQL 15+
- Row Level Security (RLS)

### DevOps
- Docker + Docker Compose
- Easypanel (produção)
- Traefik (reverse proxy)
- Git

---

## 👥 Perfis de Usuário

### 🎓 Aluno
- Buscar e visualizar livros
- Reservar livros
- Ver histórico de empréstimos
- Ver recomendações de professores

### 👨‍🏫 Professor
- Tudo do aluno +
- Recomendar livros para alunos/turmas

### 📖 Bibliotecário
- Tudo anterior +
- Gerenciar livros (CRUD)
- Registrar empréstimos e devoluções
- Gerenciar usuários
- Gerar relatórios gerenciais

---

## 🛠️ Comandos Úteis

### Desenvolvimento

```bash
# Servidor de desenvolvimento
npm run dev

# Build de produção
npm run build

# Rodar testes (quando implementados)
npm test

# Lint
npm run lint
```

### Docker

```bash
# Subir PostgreSQL + pgAdmin
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar containers
docker-compose down

# Resetar banco de dados (CUIDADO!)
docker-compose down -v
docker-compose up -d
```

### Banco de Dados

```bash
# Conectar ao PostgreSQL
psql -h localhost -p 5432 -U postgres -d bibliokopke

# Acessar pgAdmin
# http://localhost:5050

# Executar migration
psql -h localhost -p 5432 -U postgres -d bibliokopke -f database/migrations/001_create_tables.sql
```

---

## 📊 Modelo de Dados

### Principais Entidades

- **usuario** - Alunos, professores e bibliotecários (integração SIMADE)
- **livro** - Acervo da biblioteca
- **emprestimo** - Empréstimos ativos e histórico
- **reserva** - Fila de espera para livros
- **recomendacao** - Sugestões de professores
- **log_sistema** - Auditoria de ações

### Triggers Automatizados

✅ Atualização automática de disponibilidade ao emprestar
✅ Atualização automática de disponibilidade ao devolver
✅ Busca full-text em português (título + autor + sinopse)
✅ Timestamps automáticos (created_at, updated_at)
✅ Auditoria de mudanças críticas

Ver schema completo em [database/migrations/](06_proto/database/migrations/).

---

## 🚀 Deploy em Produção

O deploy é feito via **Easypanel + Supabase Cloud**:

1. **Criar conta no Supabase Cloud** (gratuita)
2. **Executar migrations** no Supabase
3. **Configurar Easypanel** com repo Git
4. **Configurar domínio** (Traefik + SSL automático)
5. **Variáveis de ambiente** (Supabase URL + Keys)

Guia completo: **[DEPLOY_EASYPANEL.md](DEPLOY_EASYPANEL.md)**

---

## 🔒 Segurança

- ✅ HTTPS obrigatório em produção (Let's Encrypt)
- ✅ Row Level Security (RLS) no PostgreSQL
- ✅ Autenticação via Supabase Auth
- ✅ Validação de entrada (Zod)
- ✅ SQL Injection protection (prepared statements)
- ✅ Variáveis sensíveis como secrets
- ✅ CORS configurado

---

## 📝 Licença e Uso

Este projeto é desenvolvido como trabalho acadêmico para:

**Escola Estadual João Kopke**
**Projeto Interdisciplinar IV - 2025**

---

## 🙋 Suporte e Contribuição

### Reportar Problemas
- Abra uma issue no repositório
- Inclua screenshots e logs

### Contribuir
```bash
1. Fork o projeto
2. Crie uma branch: git checkout -b feature/nova-feature
3. Commit: git commit -m 'feat: adiciona nova feature'
4. Push: git push origin feature/nova-feature
5. Abra um Pull Request
```

Seguimos [Conventional Commits](https://www.conventionalcommits.org/).

---

## 📚 Recursos e Links

### Documentação Oficial
- [Next.js](https://nextjs.org/docs)
- [Supabase](https://supabase.com/docs)
- [PostgreSQL](https://www.postgresql.org/docs/)
- [Tailwind CSS](https://tailwindcss.com/docs)
- [shadcn/ui](https://ui.shadcn.com/)

### Deploy
- [Easypanel](https://easypanel.io/docs)
- [Traefik](https://doc.traefik.io/traefik/)

---

## ✨ Features Futuras (Pós-MVP)

- 📱 App mobile (React Native)
- 🔔 Notificações push em tempo real
- 📊 Dashboard analytics avançado
- 🤖 Recomendações por IA
- 📖 Clubes de leitura virtuais
- ⭐ Sistema de avaliações
- 🏆 Gamificação (badges, rankings)
- 🔗 Integração oficial com SIMADE
- 📷 Scanner de código de barras (ISBN)

---

**Desenvolvido com ❤️ para a Escola Estadual João Kopke**

**Stack:** Next.js + TypeScript + Supabase + Tailwind CSS

**Ano:** 2025
