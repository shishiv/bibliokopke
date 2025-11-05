# PRD - BiblioKopke
## Product Requirements Document

**Versão:** 1.0
**Data:** Novembro 2025
**Projeto:** Sistema de Gestão de Biblioteca Escolar João Kopke
**Stack:** Next.js + Supabase (PostgreSQL)

---

## 1. Visão Geral

### 1.1 Contexto

O **BiblioKopke** é um sistema de gestão de biblioteca escolar desenvolvido para a Escola Estadual João Kopke. O projeto teve início com levantamento de requisitos, modelagem UML e design de banco de dados no semestre anterior.

Atualmente existe um **protótipo funcional em Next.js** (`06_proto/`) com interface completa e mock data, mas sem integração com backend real.

### 1.2 Objetivo do Projeto

Transformar o protótipo Next.js em uma **aplicação full-stack funcional**, substituindo os dados mockados por um backend real usando **Supabase local (via Docker)**, mantendo a stack web moderna e descartando completamente a proposta original de aplicação desktop em C#.

### 1.3 Escopo da Primeira Sprint

Adaptar e cumprir os requisitos do **Projeto Interdisciplinar IV** para stack web:
- ✅ Cadastro de livros, reservas e empréstimos
- ✅ Controle de acesso por perfil (Aluno, Professor, Bibliotecário)
- ✅ Regras de negócio automatizadas (triggers/functions)
- ✅ Integração preparada para SIMADE (futuro)
- ✅ Relatórios de empréstimos e acervo

---

## 2. Arquitetura Técnica

### 2.1 Stack Tecnológica

#### Frontend
- **Framework:** Next.js 15.3.4 (App Router)
- **Linguagem:** TypeScript 5.8.3
- **UI:** React 19.1.0 + Tailwind CSS 4.1.11
- **Componentes:** shadcn/ui (Radix UI)
- **Formulários:** React Hook Form + Zod
- **Estado:** React Context + Hooks customizados
- **Notificações:** Sonner

#### Backend
- **BaaS:** Supabase Cloud (produção) / PostgreSQL local (desenvolvimento)
- **Banco de Dados:** PostgreSQL 15+
- **Autenticação:** Supabase Auth
- **Storage:** Supabase Storage (capas de livros)
- **Realtime:** Supabase Realtime (opcional - notificações)
- **Edge Functions:** Supabase Functions (regras complexas)

#### Infraestrutura
- **Desenvolvimento:** Docker Compose (PostgreSQL + pgAdmin)
- **Produção:**
  - **Frontend:** Easypanel (Next.js com Docker)
  - **Proxy:** Traefik (gerenciamento de domínios e SSL)
  - **Backend:** Supabase Cloud (PostgreSQL + Auth + Storage)
- **Versionamento:** Git

### 2.2 Arquitetura da Aplicação

```
bibliokopke/
├── 06_proto/                      # Aplicação Next.js
│   ├── src/
│   │   ├── app/                   # Pages (App Router)
│   │   ├── components/            # Componentes React
│   │   ├── lib/                   # Utilities + Supabase Client
│   │   ├── hooks/                 # Custom hooks
│   │   ├── types/                 # TypeScript types
│   │   └── services/              # Camada de acesso a dados
│   ├── database/                  # PostgreSQL local
│   │   ├── migrations/            # SQL migrations
│   │   └── seeds/                 # Dados iniciais
│   ├── docker-compose.yml         # PostgreSQL + pgAdmin (dev)
│   ├── .env.local                 # Variáveis de ambiente (dev)
│   └── .env.example               # Template de variáveis
├── 02_modelagem_banco/            # Schema SQL (referência)
├── 03_requisitos/                 # Requisitos e user stories
├── 04_diagramas/                  # UML diagrams
├── PRD.md                         # Este documento
└── DEPLOY_EASYPANEL.md            # Guia de deploy
```

### 2.3 Camadas da Aplicação

1. **Presentation Layer** (React Components)
   - Componentes UI reutilizáveis
   - Pages do App Router
   - Formulários com validação

2. **Business Logic Layer** (Hooks + Services)
   - Custom hooks para lógica de negócio
   - Validações client-side
   - Transformação de dados

3. **Data Access Layer** (Supabase Client)
   - Queries via Supabase JS Client
   - Abstração de acesso ao banco
   - Gerenciamento de cache

4. **Database Layer** (PostgreSQL + Supabase)
   - Schema com constraints
   - Row Level Security (RLS)
   - Triggers e Functions
   - Views para queries complexas

---

## 3. Modelo de Dados

### 3.1 Migração MySQL → PostgreSQL

O schema atual está em MySQL (`02_modelagem_banco/banco_de_dados.sql`). Principais adaptações necessárias:

| MySQL | PostgreSQL | Observação |
|-------|------------|------------|
| `INT AUTO_INCREMENT` | `SERIAL` ou `BIGSERIAL` | Chaves primárias |
| `VARCHAR(n)` | `VARCHAR(n)` ou `TEXT` | Strings |
| `DATETIME` | `TIMESTAMP WITH TIME ZONE` | Datas/horas |
| `ENUM('A','B')` | `TEXT CHECK (...)` ou custom ENUM | Enumerações |
| `TINYINT(1)` | `BOOLEAN` | Booleanos |
| `FULLTEXT INDEX` | `GIN` index com `tsvector` | Busca full-text |
| `JSON` | `JSONB` | JSON (binário é mais eficiente) |

### 3.2 Entidades Principais

#### 3.2.1 usuario
```sql
CREATE TABLE usuario (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  codigo_simade VARCHAR(50) UNIQUE NOT NULL,  -- Integração SIMADE
  nome_completo VARCHAR(150) NOT NULL,
  email VARCHAR(100) UNIQUE NOT NULL,
  tipo_usuario VARCHAR(20) CHECK (tipo_usuario IN ('ALUNO', 'PROFESSOR', 'BIBLIOTECARIO')),
  data_nascimento DATE,
  telefone VARCHAR(20),
  endereco TEXT,
  turma VARCHAR(10),  -- Para alunos
  ativo BOOLEAN DEFAULT true,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 3.2.2 livro
```sql
CREATE TABLE livro (
  id_livro UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  isbn VARCHAR(20) UNIQUE,
  titulo VARCHAR(200) NOT NULL,
  autor VARCHAR(150),
  editora VARCHAR(100),
  ano_publicacao INTEGER,
  categoria VARCHAR(50),
  sinopse TEXT,
  quantidade_total INTEGER DEFAULT 1,
  quantidade_disponivel INTEGER DEFAULT 1,
  capa_url TEXT,  -- Supabase Storage URL
  status VARCHAR(20) CHECK (status IN ('DISPONIVEL', 'INDISPONIVEL', 'MANUTENCAO')),
  titulo_autor_tsvector TSVECTOR,  -- Full-text search
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Index para busca full-text
CREATE INDEX livro_search_idx ON livro USING GIN(titulo_autor_tsvector);
```

#### 3.2.3 emprestimo
```sql
CREATE TABLE emprestimo (
  id_emprestimo UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  id_livro UUID REFERENCES livro(id_livro) ON DELETE CASCADE,
  codigo_simade VARCHAR(50) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
  data_emprestimo TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  data_devolucao_prevista TIMESTAMP WITH TIME ZONE NOT NULL,
  data_devolucao_real TIMESTAMP WITH TIME ZONE,
  renovacoes INTEGER DEFAULT 0,
  status VARCHAR(20) CHECK (status IN ('ATIVO', 'DEVOLVIDO', 'ATRASADO', 'PERDIDO')),
  observacoes TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 3.2.4 reserva
```sql
CREATE TABLE reserva (
  id_reserva UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  id_livro UUID REFERENCES livro(id_livro) ON DELETE CASCADE,
  codigo_simade VARCHAR(50) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
  data_reserva TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  data_expiracao TIMESTAMP WITH TIME ZONE NOT NULL,
  status VARCHAR(20) CHECK (status IN ('ATIVA', 'CANCELADA', 'EXPIRADA', 'CONCLUIDA')),
  motivo_cancelamento TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 3.2.5 historico_emprestimo
```sql
CREATE TABLE historico_emprestimo (
  id_historico UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  id_emprestimo UUID REFERENCES emprestimo(id_emprestimo),
  id_livro UUID,
  codigo_simade VARCHAR(50),
  data_emprestimo TIMESTAMP WITH TIME ZONE,
  data_devolucao TIMESTAMP WITH TIME ZONE,
  dias_atraso INTEGER DEFAULT 0,
  multa DECIMAL(10,2) DEFAULT 0,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 3.2.6 recomendacao
```sql
CREATE TABLE recomendacao (
  id_recomendacao UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  codigo_professor VARCHAR(50) REFERENCES usuario(codigo_simade),
  id_livro UUID REFERENCES livro(id_livro),
  codigo_aluno VARCHAR(50) REFERENCES usuario(codigo_simade),  -- NULL se for para turma
  turma VARCHAR(10),  -- NULL se for individual
  tipo VARCHAR(20) CHECK (tipo IN ('INDIVIDUAL', 'TURMA')),
  justificativa TEXT,
  data_recomendacao TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 3.2.7 log_sistema
```sql
CREATE TABLE log_sistema (
  id_log UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  codigo_usuario VARCHAR(50),
  acao VARCHAR(100) NOT NULL,
  tabela_afetada VARCHAR(50),
  registro_id UUID,
  dados_antes JSONB,
  dados_depois JSONB,
  ip_address INET,
  timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

#### 3.2.8 relatorio
```sql
CREATE TABLE relatorio (
  id_relatorio UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  tipo VARCHAR(50) NOT NULL,
  parametros JSONB,
  dados JSONB,
  gerado_por VARCHAR(50) REFERENCES usuario(codigo_simade),
  data_geracao TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

### 3.3 Triggers e Functions

#### 3.3.1 Atualização Automática de Disponibilidade
```sql
-- Trigger para diminuir quantidade disponível ao emprestar
CREATE OR REPLACE FUNCTION atualizar_disponibilidade_emprestimo()
RETURNS TRIGGER AS $$
BEGIN
  IF NEW.status = 'ATIVO' THEN
    UPDATE livro
    SET quantidade_disponivel = quantidade_disponivel - 1,
        status = CASE
          WHEN quantidade_disponivel - 1 <= 0 THEN 'INDISPONIVEL'::VARCHAR
          ELSE status
        END
    WHERE id_livro = NEW.id_livro;
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_emprestimo_insert
AFTER INSERT ON emprestimo
FOR EACH ROW
EXECUTE FUNCTION atualizar_disponibilidade_emprestimo();

-- Trigger para aumentar quantidade disponível ao devolver
CREATE OR REPLACE FUNCTION atualizar_disponibilidade_devolucao()
RETURNS TRIGGER AS $$
BEGIN
  IF NEW.status = 'DEVOLVIDO' AND OLD.status = 'ATIVO' THEN
    UPDATE livro
    SET quantidade_disponivel = quantidade_disponivel + 1,
        status = 'DISPONIVEL'
    WHERE id_livro = NEW.id_livro;
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_emprestimo_update
AFTER UPDATE ON emprestimo
FOR EACH ROW
EXECUTE FUNCTION atualizar_disponibilidade_devolucao();
```

#### 3.3.2 Atualização de Timestamps
```sql
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Aplicar em todas as tabelas com updated_at
CREATE TRIGGER update_usuario_updated_at BEFORE UPDATE ON usuario
  FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_livro_updated_at BEFORE UPDATE ON livro
  FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_emprestimo_updated_at BEFORE UPDATE ON emprestimo
  FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
```

#### 3.3.3 Full-Text Search
```sql
CREATE OR REPLACE FUNCTION livro_search_trigger()
RETURNS TRIGGER AS $$
BEGIN
  NEW.titulo_autor_tsvector :=
    setweight(to_tsvector('portuguese', COALESCE(NEW.titulo, '')), 'A') ||
    setweight(to_tsvector('portuguese', COALESCE(NEW.autor, '')), 'B') ||
    setweight(to_tsvector('portuguese', COALESCE(NEW.sinopse, '')), 'C');
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tsvector_update BEFORE INSERT OR UPDATE ON livro
  FOR EACH ROW EXECUTE FUNCTION livro_search_trigger();
```

#### 3.3.4 Auditoria Automática
```sql
CREATE OR REPLACE FUNCTION log_changes()
RETURNS TRIGGER AS $$
BEGIN
  INSERT INTO log_sistema (
    codigo_usuario,
    acao,
    tabela_afetada,
    registro_id,
    dados_antes,
    dados_depois
  ) VALUES (
    COALESCE(current_setting('app.current_user', true), 'system'),
    TG_OP,
    TG_TABLE_NAME,
    COALESCE(NEW.id_livro, NEW.id_emprestimo, NEW.id_reserva),
    CASE WHEN TG_OP = 'DELETE' THEN row_to_json(OLD) ELSE NULL END,
    CASE WHEN TG_OP IN ('INSERT', 'UPDATE') THEN row_to_json(NEW) ELSE NULL END
  );
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Aplicar em tabelas críticas
CREATE TRIGGER audit_livro AFTER INSERT OR UPDATE OR DELETE ON livro
  FOR EACH ROW EXECUTE FUNCTION log_changes();

CREATE TRIGGER audit_emprestimo AFTER INSERT OR UPDATE OR DELETE ON emprestimo
  FOR EACH ROW EXECUTE FUNCTION log_changes();
```

### 3.4 Row Level Security (RLS)

#### 3.4.1 Políticas para usuario
```sql
ALTER TABLE usuario ENABLE ROW LEVEL SECURITY;

-- Usuários podem ver seus próprios dados
CREATE POLICY "Usuarios veem proprios dados"
  ON usuario FOR SELECT
  USING (auth.uid()::text = codigo_simade OR tipo_usuario = 'BIBLIOTECARIO');

-- Bibliotecários podem fazer tudo
CREATE POLICY "Bibliotecarios acesso total"
  ON usuario FOR ALL
  USING (
    EXISTS (
      SELECT 1 FROM usuario
      WHERE codigo_simade = auth.uid()::text
      AND tipo_usuario = 'BIBLIOTECARIO'
    )
  );
```

#### 3.4.2 Políticas para livro
```sql
ALTER TABLE livro ENABLE ROW LEVEL SECURITY;

-- Todos podem ver livros
CREATE POLICY "Todos veem livros"
  ON livro FOR SELECT
  USING (true);

-- Apenas bibliotecários podem modificar
CREATE POLICY "Bibliotecarios modificam livros"
  ON livro FOR ALL
  USING (
    EXISTS (
      SELECT 1 FROM usuario
      WHERE codigo_simade = auth.uid()::text
      AND tipo_usuario = 'BIBLIOTECARIO'
    )
  );
```

#### 3.4.3 Políticas para emprestimo
```sql
ALTER TABLE emprestimo ENABLE ROW LEVEL SECURITY;

-- Usuários veem seus próprios empréstimos
CREATE POLICY "Usuarios veem proprios emprestimos"
  ON emprestimo FOR SELECT
  USING (
    codigo_simade = auth.uid()::text
    OR EXISTS (
      SELECT 1 FROM usuario
      WHERE codigo_simade = auth.uid()::text
      AND tipo_usuario IN ('BIBLIOTECARIO', 'PROFESSOR')
    )
  );

-- Bibliotecários podem criar/modificar
CREATE POLICY "Bibliotecarios gerenciam emprestimos"
  ON emprestimo FOR ALL
  USING (
    EXISTS (
      SELECT 1 FROM usuario
      WHERE codigo_simade = auth.uid()::text
      AND tipo_usuario = 'BIBLIOTECARIO'
    )
  );
```

### 3.5 Views Úteis

#### 3.5.1 Empréstimos Ativos
```sql
CREATE OR REPLACE VIEW v_emprestimos_ativos AS
SELECT
  e.id_emprestimo,
  e.data_emprestimo,
  e.data_devolucao_prevista,
  CASE
    WHEN e.data_devolucao_prevista < NOW() THEN 'ATRASADO'
    ELSE 'ATIVO'
  END as status_calculado,
  l.titulo,
  l.autor,
  u.nome_completo,
  u.turma
FROM emprestimo e
JOIN livro l ON e.id_livro = l.id_livro
JOIN usuario u ON e.codigo_simade = u.codigo_simade
WHERE e.status = 'ATIVO';
```

#### 3.5.2 Livros Mais Emprestados
```sql
CREATE OR REPLACE VIEW v_livros_populares AS
SELECT
  l.id_livro,
  l.titulo,
  l.autor,
  COUNT(e.id_emprestimo) as total_emprestimos,
  COUNT(DISTINCT e.codigo_simade) as usuarios_distintos
FROM livro l
LEFT JOIN emprestimo e ON l.id_livro = e.id_livro
GROUP BY l.id_livro, l.titulo, l.autor
ORDER BY total_emprestimos DESC;
```

---

## 4. Funcionalidades por Perfil

### 4.1 ALUNO

#### Acesso ao Catálogo
- ✅ Visualizar catálogo completo de livros
- ✅ Buscar livros (título, autor, categoria)
- ✅ Ver detalhes do livro (sinopse, disponibilidade)
- ✅ Filtrar por categoria, autor, ano

#### Reservas
- ✅ Reservar livros disponíveis
- ✅ Cancelar reservas ativas
- ✅ Ver histórico de reservas

#### Empréstimos
- ✅ Ver empréstimos ativos
- ✅ Ver histórico de empréstimos
- ✅ Solicitar renovação (se permitido)

#### Perfil
- ✅ Ver dados pessoais
- ✅ Editar informações básicas (telefone, endereço)

### 4.2 PROFESSOR

#### Todas as funcionalidades de ALUNO +

#### Recomendações
- ✅ Recomendar livros para alunos específicos
- ✅ Recomendar livros para turmas
- ✅ Ver histórico de recomendações
- ✅ Acompanhar se alunos leram recomendações

### 4.3 BIBLIOTECÁRIO

#### Todas as funcionalidades anteriores +

#### Gestão de Livros
- ✅ Adicionar novos livros
- ✅ Editar informações de livros
- ✅ Remover livros (soft delete)
- ✅ Fazer upload de capas
- ✅ Marcar livros em manutenção

#### Gestão de Empréstimos
- ✅ Registrar empréstimo
- ✅ Registrar devolução
- ✅ Renovar empréstimo
- ✅ Marcar como perdido
- ✅ Calcular multas

#### Gestão de Usuários
- ✅ Cadastrar novos usuários
- ✅ Editar usuários
- ✅ Desativar usuários
- ✅ Ver histórico completo por usuário

#### Relatórios
- ✅ Relatório de empréstimos por período
- ✅ Relatório de livros mais emprestados
- ✅ Relatório de usuários com empréstimos atrasados
- ✅ Relatório de acervo completo
- ✅ Exportar relatórios (PDF/CSV)

#### Dashboard
- ✅ Estatísticas gerais (total de livros, empréstimos ativos, etc.)
- ✅ Alertas de livros atrasados
- ✅ Reservas pendentes

---

## 5. Roadmap - Sprints Adaptadas

### Sprint 0 - Kickoff + Base (22/set - 07/out) ✅ CONCLUÍDO

**Status:** Protótipo Next.js já existe

**Entregas:**
- ✅ Documento de requisitos revisado
- ✅ DER e dicionário de dados
- ✅ Protótipo Next.js funcional com mock data
- ✅ Componentes UI completos (shadcn/ui)
- ✅ Navegação entre páginas
- ✅ Design system estabelecido

### Sprint 1 - Infraestrutura + Database (07/out - 22/out) 🎯 PRIMEIRA SPRINT

**Objetivo:** Substituir mock data por Supabase real

#### Semana 1 (07/out - 14/out)
**Setup Infraestrutura**
- [ ] Criar `docker-compose.yml` com Supabase local
  - PostgreSQL
  - Supabase Studio
  - Kong (API Gateway)
  - GoTrue (Auth)
  - PostgREST
  - Realtime
- [ ] Documentar setup (README.md)
- [ ] Configurar variáveis de ambiente
- [ ] Testar acesso ao Studio (http://localhost:54323)

**Migração do Schema**
- [ ] Adaptar schema MySQL → PostgreSQL
- [ ] Criar migrations (`supabase/migrations/`)
  - 001_create_tables.sql
  - 002_create_triggers.sql
  - 003_create_views.sql
  - 004_enable_rls.sql
- [ ] Criar seed data (`supabase/seed.sql`)
- [ ] Testar migrations

#### Semana 2 (14/out - 22/out)
**Integração Next.js + Supabase**
- [ ] Instalar `@supabase/supabase-js` e `@supabase/auth-helpers-nextjs`
- [ ] Criar cliente Supabase (`lib/supabase/client.ts` e `server.ts`)
- [ ] Configurar middleware de autenticação
- [ ] Criar camada de serviços (services/)
  - livrosService.ts
  - emprestimosService.ts
  - reservasService.ts
  - usuariosService.ts

**CRUD de Livros (Completo)**
- [ ] Migrar componente de catálogo para dados reais
- [ ] Implementar busca full-text
- [ ] Implementar filtros
- [ ] Form de cadastro de livro integrado
- [ ] Upload de capa para Supabase Storage
- [ ] Edição e remoção de livros

**Entregas D30:**
- ✅ Docker Compose rodando Supabase local
- ✅ Migrations executadas com sucesso
- ✅ App Next.js conectado ao Supabase
- ✅ CRUD de livros funcionando
- ✅ Busca e filtros operacionais
- ✅ Trigger de disponibilidade testado

### Sprint 2 - Autenticação + Empréstimos (22/out - 06/nov)

#### Semana 1 (22/out - 29/out)
**Sistema de Autenticação**
- [ ] Implementar login com Supabase Auth
- [ ] Tela de login funcional
- [ ] Proteção de rotas por middleware
- [ ] Controle de acesso por perfil (RLS)
- [ ] Logout e gestão de sessão

**Gestão de Usuários**
- [ ] CRUD de usuários (apenas bibliotecário)
- [ ] Integração com código SIMADE
- [ ] Validações de formulário

#### Semana 2 (29/out - 06/nov)
**Fluxo de Empréstimos**
- [ ] Registrar empréstimo
- [ ] Registrar devolução
- [ ] Renovar empréstimo
- [ ] Cálculo de datas e prazos
- [ ] Validações de negócio (livro disponível, limite de empréstimos)
- [ ] Listagem de empréstimos ativos
- [ ] Histórico de empréstimos

**Fluxo de Reservas**
- [ ] Criar reserva
- [ ] Cancelar reserva
- [ ] Expiração automática (cron job)
- [ ] Notificação quando livro fica disponível

**Entregas D45:**
- ✅ Sistema de login operacional
- ✅ RLS policies funcionando
- ✅ Fluxo completo: emprestar → devolver
- ✅ Fluxo completo: reservar → cancelar
- ✅ Triggers atualizando disponibilidade
- ✅ Vídeo de demo ponta-a-ponta

### Sprint 3 - Relatórios + Qualidade (06/nov - 21/nov)

#### Semana 1 (06/nov - 13/nov)
**Sistema de Relatórios**
- [ ] Relatório de empréstimos por período
- [ ] Relatório de livros mais emprestados
- [ ] Relatório de usuários com atraso
- [ ] Relatório de acervo completo
- [ ] Exportação para PDF (usando jsPDF ou react-pdf)
- [ ] Exportação para CSV

**Dashboard do Bibliotecário**
- [ ] Cards com estatísticas
- [ ] Gráficos (Chart.js ou Recharts)
- [ ] Alertas de empréstimos atrasados
- [ ] Reservas pendentes

#### Semana 2 (13/nov - 21/nov)
**Recomendações de Professores**
- [ ] CRUD de recomendações
- [ ] Filtro individual vs turma
- [ ] Visualização para alunos

**Melhorias de UX**
- [ ] Loading states
- [ ] Error boundaries
- [ ] Validações de formulário aprimoradas
- [ ] Feedback visual (toasts, confirmações)
- [ ] Responsividade mobile
- [ ] Acessibilidade (ARIA labels)

**Testes**
- [ ] Testes de integração dos fluxos principais
- [ ] Documentação de casos de teste
- [ ] Checklist de QA

**Entregas D60:**
- ✅ 2+ relatórios funcionais e exportáveis
- ✅ Dashboard com estatísticas
- ✅ Sistema de recomendações completo
- ✅ UX polida e responsiva
- ✅ Relatório de testes

### Sprint 4 - Finalização (21/nov - 30/nov)

**Documentação**
- [ ] Manual do Usuário (com screenshots)
- [ ] Manual Técnico (arquitetura, setup, deploy)
- [ ] Documentação de API (se houver edge functions)
- [ ] Guia de contribuição

**Preparação para Deploy**
- [ ] Configurar Vercel para frontend
- [ ] Preparar migração para Supabase Cloud
- [ ] Variáveis de ambiente para produção
- [ ] Testes em ambiente de staging

**Apresentação Final**
- [ ] Slides de apresentação
- [ ] Vídeo de demonstração (3-5 min)
- [ ] Relatório final do projeto

**Entregas Finais:**
- ✅ Release Candidate (link Vercel + Supabase Cloud)
- ✅ Manuais completos (usuário + técnico)
- ✅ Relatório final
- ✅ Apresentação + demo

---

## 6. Requisitos Técnicos

### 6.1 Requisitos Funcionais

#### RF01 - Autenticação
- O sistema deve permitir login via email/senha
- O sistema deve controlar acesso por perfil (Aluno, Professor, Bibliotecário)
- O sistema deve manter sessão do usuário
- O sistema deve permitir logout

#### RF02 - Gestão de Livros
- O sistema deve permitir cadastro de livros (Bibliotecário)
- O sistema deve permitir edição de livros (Bibliotecário)
- O sistema deve permitir exclusão lógica de livros (Bibliotecário)
- O sistema deve permitir upload de capa de livros
- O sistema deve permitir busca full-text (título, autor, sinopse)
- O sistema deve permitir filtros (categoria, disponibilidade, ano)

#### RF03 - Gestão de Empréstimos
- O sistema deve permitir registro de empréstimo (Bibliotecário)
- O sistema deve validar disponibilidade antes de emprestar
- O sistema deve atualizar automaticamente a disponibilidade
- O sistema deve permitir registro de devolução (Bibliotecário)
- O sistema deve calcular automaticamente dias de atraso
- O sistema deve permitir renovação (se dentro do prazo)

#### RF04 - Gestão de Reservas
- O sistema deve permitir reserva de livros (qualquer usuário autenticado)
- O sistema deve cancelar reserva automaticamente após expiração
- O sistema deve permitir cancelamento manual
- O sistema deve notificar quando livro reservado fica disponível (futuro)

#### RF05 - Gestão de Usuários
- O sistema deve permitir cadastro de usuários (Bibliotecário)
- O sistema deve integrar com código SIMADE
- O sistema deve permitir edição de perfil (próprio usuário)
- O sistema deve permitir desativação de usuários (Bibliotecário)

#### RF06 - Recomendações
- O sistema deve permitir professores recomendarem livros
- O sistema deve suportar recomendação individual e por turma
- O sistema deve exibir recomendações para alunos

#### RF07 - Relatórios
- O sistema deve gerar relatório de empréstimos por período
- O sistema deve gerar relatório de livros mais emprestados
- O sistema deve gerar relatório de usuários com atraso
- O sistema deve gerar relatório de acervo
- O sistema deve permitir exportação em PDF e CSV

#### RF08 - Auditoria
- O sistema deve registrar todas as ações críticas em log
- O sistema deve armazenar quem, quando e o que foi alterado

### 6.2 Requisitos Não-Funcionais

#### RNF01 - Performance
- A busca de livros deve retornar resultados em < 1s
- O carregamento de páginas deve ser < 3s
- Queries devem usar índices apropriados

#### RNF02 - Segurança
- Senhas devem ser armazenadas com hash (Supabase Auth)
- Row Level Security deve proteger dados sensíveis
- Validação de entrada em todos os formulários
- Proteção contra SQL Injection (via prepared statements)
- HTTPS obrigatório em produção

#### RNF03 - Usabilidade
- Interface responsiva (mobile, tablet, desktop)
- Feedback visual para ações do usuário
- Mensagens de erro claras
- Navegação intuitiva

#### RNF04 - Manutenibilidade
- Código TypeScript com tipagem forte
- Componentização e reutilização
- Documentação inline
- Padrões de código (ESLint)

#### RNF05 - Disponibilidade
- Uptime de 99% (após deploy)
- Backup automático do banco (Supabase Cloud)
- Logs de erro para debugging

### 6.3 Regras de Negócio

#### RN01 - Empréstimos
- Prazo padrão: 14 dias
- Máximo de renovações: 2
- Não pode renovar se atrasado
- Aluno não pode ter mais de 3 empréstimos ativos
- Professor não pode ter mais de 5 empréstimos ativos

#### RN02 - Reservas
- Prazo de validade: 7 dias
- Usuário não pode reservar livro que já tem emprestado
- Máximo 2 reservas ativas por usuário

#### RN03 - Multas (futuro)
- R$ 1,00 por dia de atraso
- Usuário com multa não pode fazer novos empréstimos até pagar

#### RN04 - Disponibilidade
- Livro fica indisponível quando `quantidade_disponivel = 0`
- Atualização automática via trigger

---

## 7. Setup e Instalação

### 7.1 Pré-requisitos
- Node.js 18+ e npm/yarn/pnpm
- Docker e Docker Compose
- Git

### 7.2 Setup Inicial

```bash
# Clone o repositório
git clone <repo-url>
cd BiblioKopke/06_proto

# Instale dependências
npm install

# Suba o Supabase local
docker-compose up -d

# Aguarde ~30s para os serviços iniciarem

# Verifique se está rodando
docker-compose ps

# Acesse o Supabase Studio
# http://localhost:54323
```

### 7.3 Configuração

```bash
# Copie o arquivo de ambiente
cp .env.example .env.local

# Edite .env.local com as credenciais do Supabase local
# NEXT_PUBLIC_SUPABASE_URL=http://localhost:54321
# NEXT_PUBLIC_SUPABASE_ANON_KEY=<ver no docker-compose.yml>
# SUPABASE_SERVICE_ROLE_KEY=<ver no docker-compose.yml>
```

### 7.4 Execução

```bash
# Rode as migrations
npm run supabase:migrate

# Rode o seed (dados iniciais)
npm run supabase:seed

# Inicie o servidor de desenvolvimento
npm run dev

# Acesse http://localhost:3000
```

---

## 8. Estrutura de Arquivos (Proposta)

```
06_proto/
├── src/
│   ├── app/                           # Pages (App Router)
│   │   ├── (auth)/                    # Grupo de rotas autenticadas
│   │   │   ├── aluno/
│   │   │   ├── professor/
│   │   │   └── bibliotecario/
│   │   ├── login/
│   │   ├── api/
│   │   │   ├── books/                 # CRUD de livros
│   │   │   ├── loans/                 # Empréstimos
│   │   │   └── reservations/          # Reservas
│   │   ├── layout.tsx
│   │   └── page.tsx
│   ├── components/
│   │   ├── ui/                        # shadcn/ui components
│   │   ├── catalogo/
│   │   ├── emprestimos/
│   │   ├── reservas/
│   │   ├── relatorios/
│   │   └── layout/
│   ├── lib/
│   │   ├── supabase/
│   │   │   ├── client.ts              # Cliente browser
│   │   │   ├── server.ts              # Cliente server
│   │   │   └── middleware.ts          # Auth middleware
│   │   ├── utils.ts
│   │   └── validations.ts
│   ├── services/
│   │   ├── livrosService.ts
│   │   ├── emprestimosService.ts
│   │   ├── reservasService.ts
│   │   ├── usuariosService.ts
│   │   └── relatoriosService.ts
│   ├── hooks/
│   │   ├── useLivros.ts
│   │   ├── useEmprestimos.ts
│   │   ├── useAuth.ts
│   │   └── useSupabase.ts
│   ├── types/
│   │   ├── entities.ts                # Database types
│   │   ├── api.ts                     # API response types
│   │   └── forms.ts                   # Form types
│   └── contexts/
│       └── AuthContext.tsx
├── supabase/
│   ├── migrations/
│   │   ├── 20241107_001_create_tables.sql
│   │   ├── 20241107_002_create_triggers.sql
│   │   ├── 20241107_003_create_views.sql
│   │   └── 20241107_004_enable_rls.sql
│   ├── functions/                     # Edge Functions (se necessário)
│   ├── seed.sql
│   └── config.toml
├── public/
│   ├── covers/
│   └── bibliokopke.png
├── docker-compose.yml
├── .env.local
├── .env.example
├── next.config.ts
├── tsconfig.json
├── package.json
└── README.md
```

---

## 9. Critérios de Aceitação - Sprint 1

### Infraestrutura
- [ ] Docker Compose sobe sem erros
- [ ] Supabase Studio acessível em localhost:54323
- [ ] PostgreSQL aceita conexões
- [ ] Documentação de setup atualizada

### Database
- [ ] Todas as tabelas criadas via migrations
- [ ] Triggers funcionando (disponibilidade automática)
- [ ] RLS policies ativas
- [ ] Seed data carregado com sucesso
- [ ] Busca full-text operacional

### Integração Next.js
- [ ] Cliente Supabase configurado
- [ ] Middleware de autenticação básico
- [ ] Services layer implementada
- [ ] Types TypeScript gerados do schema

### CRUD Livros
- [ ] Listagem de livros vindo do banco
- [ ] Busca full-text funcionando
- [ ] Filtros aplicados (categoria, disponibilidade)
- [ ] Cadastro de livro salvando no banco
- [ ] Edição de livro funcionando
- [ ] Upload de capa para Supabase Storage
- [ ] Exclusão lógica (soft delete)

### Testes
- [ ] Trigger testado: emprestar livro → quantidade_disponivel diminui
- [ ] Trigger testado: devolver livro → quantidade_disponivel aumenta
- [ ] Busca full-text retorna resultados corretos
- [ ] RLS impede acesso não autorizado

### Documentação
- [ ] README.md com instruções de setup
- [ ] Evidências (screenshots, vídeo curto)
- [ ] Scripts SQL versionados no Git

---

## 10. Riscos e Mitigações

| Risco | Probabilidade | Impacto | Mitigação |
|-------|---------------|---------|-----------|
| Dificuldade em migrar MySQL → PostgreSQL | Média | Alto | Usar ferramentas como pgloader, revisar schema com cuidado |
| Docker não funciona no ambiente | Baixa | Alto | Documentar alternativas (Supabase CLI local sem Docker) |
| Performance ruim em queries | Média | Médio | Criar índices adequados, usar EXPLAIN ANALYZE |
| Complexidade do RLS | Alta | Médio | Documentar policies, criar helper functions |
| Prazo apertado | Alta | Alto | Priorizar features (MoSCoW), começar pelo MVP |
| Integração SIMADE complexa | Média | Baixo | Deixar para fase 2, usar código mockado |

---

## 11. Próximos Passos (Pós-MVP)

### Fase 2 - Recursos Avançados
- Notificações em tempo real (Supabase Realtime)
- Sistema de multas automatizado
- Integração real com API SIMADE
- Scanner de código de barras (ISBN)
- Aplicativo mobile (React Native + Expo)

### Fase 3 - Analytics e IA
- Dashboard analytics com métricas avançadas
- Recomendação de livros por IA (baseado em histórico)
- Previsão de demanda de livros
- Chatbot para FAQ

### Fase 4 - Comunidade
- Sistema de avaliações e comentários
- Clubes de leitura virtuais
- Gamificação (badges, rankings)
- Integração com redes sociais

---

## 12. Glossário

| Termo | Definição |
|-------|-----------|
| **SIMADE** | Sistema Mineiro de Administração Escolar (sistema do governo de MG) |
| **RLS** | Row Level Security - segurança a nível de linha no PostgreSQL |
| **Edge Functions** | Serverless functions do Supabase |
| **Supabase Studio** | Interface web de administração do Supabase |
| **Mock Data** | Dados fictícios para desenvolvimento |
| **Soft Delete** | Exclusão lógica (marcar como inativo ao invés de deletar) |
| **Full-text Search** | Busca por texto completo (mais inteligente que LIKE) |
| **Migration** | Script SQL versionado para mudanças no schema |
| **Seed** | Dados iniciais para popular o banco |

---

## 13. Referências

- [Documentação Next.js](https://nextjs.org/docs)
- [Documentação Supabase](https://supabase.com/docs)
- [PostgreSQL Triggers](https://www.postgresql.org/docs/current/triggers.html)
- [Row Level Security](https://supabase.com/docs/guides/auth/row-level-security)
- [shadcn/ui Components](https://ui.shadcn.com/)
- [Tailwind CSS](https://tailwindcss.com/docs)

---

**Documento gerado em:** Novembro 2025
**Versão:** 1.0
**Próxima revisão:** Após Sprint 1 (22/out)