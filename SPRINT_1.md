# Sprint 1 - Infraestrutura + Database
**Período:** 07/out - 22/out (D15-D30)
**Objetivo:** Substituir mock data por Supabase real e implementar CRUD de livros

---

## Status Geral

### ✅ Concluído
- [x] PRD completo documentado
- [x] Docker Compose configurado
- [x] Estrutura de pastas Supabase criada
- [x] Variáveis de ambiente configuradas (.env.example)
- [x] README atualizado com instruções completas

### 🚧 Em Andamento
- [ ] Migrations do banco PostgreSQL
- [ ] Cliente Supabase configurado no Next.js
- [ ] CRUD de livros integrado
- [ ] Sistema de autenticação

---

## Checklist de Ações - Semana 1 (07/out - 14/out)

### 1️⃣ Setup da Infraestrutura

#### 1.1 Testar Supabase Local
```bash
# No diretório 06_proto/

# 1. Copiar variáveis de ambiente
cp .env.example .env.local

# 2. Subir containers
docker-compose up -d

# 3. Aguardar ~30s

# 4. Verificar status
docker-compose ps
# Todos devem estar "running" ou "healthy"

# 5. Acessar Supabase Studio
# http://localhost:54323

# 6. Verificar logs se algo falhar
docker-compose logs
```

**Critério de Aceitação:**
- ✅ Todos os containers rodando sem erros
- ✅ Supabase Studio acessível
- ✅ PostgreSQL aceitando conexões na porta 54322

#### 1.2 Testar Conexão ao PostgreSQL
```bash
# Teste via Docker
docker-compose exec db psql -U postgres -c "SELECT version();"

# Deve retornar a versão do PostgreSQL
```

**Critério de Aceitação:**
- ✅ Query retorna versão do PostgreSQL
- ✅ Sem erros de conexão

---

### 2️⃣ Criar Migrations do Banco

#### 2.1 Migração 001 - Criar Tabelas Principais

Criar arquivo: `supabase/migrations/20241107_001_create_tables.sql`

```sql
-- Habilitar extensão UUID
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Tabela de usuários
CREATE TABLE usuario (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  codigo_simade VARCHAR(50) UNIQUE NOT NULL,
  nome_completo VARCHAR(150) NOT NULL,
  email VARCHAR(100) UNIQUE NOT NULL,
  tipo_usuario VARCHAR(20) CHECK (tipo_usuario IN ('ALUNO', 'PROFESSOR', 'BIBLIOTECARIO')),
  data_nascimento DATE,
  telefone VARCHAR(20),
  endereco TEXT,
  turma VARCHAR(10),
  ativo BOOLEAN DEFAULT true,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Tabela de livros
CREATE TABLE livro (
  id_livro UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  isbn VARCHAR(20) UNIQUE,
  titulo VARCHAR(200) NOT NULL,
  autor VARCHAR(150),
  editora VARCHAR(100),
  ano_publicacao INTEGER,
  categoria VARCHAR(50),
  sinopse TEXT,
  quantidade_total INTEGER DEFAULT 1,
  quantidade_disponivel INTEGER DEFAULT 1,
  capa_url TEXT,
  status VARCHAR(20) CHECK (status IN ('DISPONIVEL', 'INDISPONIVEL', 'MANUTENCAO')) DEFAULT 'DISPONIVEL',
  titulo_autor_tsvector TSVECTOR,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Tabela de empréstimos
CREATE TABLE emprestimo (
  id_emprestimo UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  id_livro UUID REFERENCES livro(id_livro) ON DELETE CASCADE,
  codigo_simade VARCHAR(50) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
  data_emprestimo TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  data_devolucao_prevista TIMESTAMP WITH TIME ZONE NOT NULL,
  data_devolucao_real TIMESTAMP WITH TIME ZONE,
  renovacoes INTEGER DEFAULT 0,
  status VARCHAR(20) CHECK (status IN ('ATIVO', 'DEVOLVIDO', 'ATRASADO', 'PERDIDO')) DEFAULT 'ATIVO',
  observacoes TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Tabela de reservas
CREATE TABLE reserva (
  id_reserva UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  id_livro UUID REFERENCES livro(id_livro) ON DELETE CASCADE,
  codigo_simade VARCHAR(50) REFERENCES usuario(codigo_simade) ON DELETE CASCADE,
  data_reserva TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  data_expiracao TIMESTAMP WITH TIME ZONE NOT NULL,
  status VARCHAR(20) CHECK (status IN ('ATIVA', 'CANCELADA', 'EXPIRADA', 'CONCLUIDA')) DEFAULT 'ATIVA',
  motivo_cancelamento TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Tabela de histórico
CREATE TABLE historico_emprestimo (
  id_historico UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  id_emprestimo UUID REFERENCES emprestimo(id_emprestimo),
  id_livro UUID,
  codigo_simade VARCHAR(50),
  data_emprestimo TIMESTAMP WITH TIME ZONE,
  data_devolucao TIMESTAMP WITH TIME ZONE,
  dias_atraso INTEGER DEFAULT 0,
  multa DECIMAL(10,2) DEFAULT 0,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Tabela de recomendações
CREATE TABLE recomendacao (
  id_recomendacao UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  codigo_professor VARCHAR(50) REFERENCES usuario(codigo_simade),
  id_livro UUID REFERENCES livro(id_livro),
  codigo_aluno VARCHAR(50) REFERENCES usuario(codigo_simade),
  turma VARCHAR(10),
  tipo VARCHAR(20) CHECK (tipo IN ('INDIVIDUAL', 'TURMA')),
  justificativa TEXT,
  data_recomendacao TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Tabela de logs
CREATE TABLE log_sistema (
  id_log UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  codigo_usuario VARCHAR(50),
  acao VARCHAR(100) NOT NULL,
  tabela_afetada VARCHAR(50),
  registro_id UUID,
  dados_antes JSONB,
  dados_depois JSONB,
  ip_address INET,
  timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Índices para performance
CREATE INDEX idx_livro_titulo ON livro(titulo);
CREATE INDEX idx_livro_autor ON livro(autor);
CREATE INDEX idx_livro_categoria ON livro(categoria);
CREATE INDEX idx_emprestimo_status ON emprestimo(status);
CREATE INDEX idx_emprestimo_usuario ON emprestimo(codigo_simade);
CREATE INDEX idx_reserva_status ON reserva(status);
CREATE INDEX idx_reserva_usuario ON reserva(codigo_simade);
```

**Como executar:**

```bash
# Via psql
docker-compose exec db psql -U postgres -f /docker-entrypoint-initdb.d/migrations/20241107_001_create_tables.sql

# Ou via Studio: http://localhost:54323 > SQL Editor > Cole o SQL > Run
```

**Critério de Aceitação:**
- ✅ Todas as tabelas criadas
- ✅ Constraints funcionando
- ✅ Índices criados

#### 2.2 Migração 002 - Triggers e Functions

Criar arquivo: `supabase/migrations/20241107_002_create_triggers.sql`

```sql
-- Função para atualizar updated_at automaticamente
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Aplicar em todas as tabelas
CREATE TRIGGER update_usuario_updated_at
  BEFORE UPDATE ON usuario
  FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_livro_updated_at
  BEFORE UPDATE ON livro
  FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_emprestimo_updated_at
  BEFORE UPDATE ON emprestimo
  FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- Trigger para atualizar disponibilidade ao emprestar
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
  FOR EACH ROW EXECUTE FUNCTION atualizar_disponibilidade_emprestimo();

-- Trigger para atualizar disponibilidade ao devolver
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
  FOR EACH ROW EXECUTE FUNCTION atualizar_disponibilidade_devolucao();

-- Trigger para full-text search
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

CREATE TRIGGER tsvector_update
  BEFORE INSERT OR UPDATE ON livro
  FOR EACH ROW EXECUTE FUNCTION livro_search_trigger();

-- Índice GIN para busca full-text
CREATE INDEX livro_search_idx ON livro USING GIN(titulo_autor_tsvector);
```

**Critério de Aceitação:**
- ✅ Triggers criados sem erro
- ✅ Teste: INSERT em emprestimo diminui quantidade_disponivel
- ✅ Teste: UPDATE emprestimo para DEVOLVIDO aumenta quantidade_disponivel

#### 2.3 Migração 003 - Seed Data

Criar arquivo: `supabase/seed.sql`

```sql
-- Inserir usuários de teste
INSERT INTO usuario (codigo_simade, nome_completo, email, tipo_usuario, turma) VALUES
('ALU001', 'Maria Silva', 'maria.silva@aluno.kopke.edu.br', 'ALUNO', '3A'),
('ALU002', 'João Santos', 'joao.santos@aluno.kopke.edu.br', 'ALUNO', '3A'),
('PROF001', 'Ana Costa', 'ana.costa@kopke.edu.br', 'PROFESSOR', NULL),
('BIB001', 'Carlos Oliveira', 'carlos.oliveira@kopke.edu.br', 'BIBLIOTECARIO', NULL);

-- Inserir livros de exemplo (copiar alguns do mock atual)
INSERT INTO livro (isbn, titulo, autor, editora, ano_publicacao, categoria, sinopse, quantidade_total, quantidade_disponivel, capa_url) VALUES
('9788535902775', 'Dom Casmurro', 'Machado de Assis', 'Globo', 1899, 'Literatura Brasileira', 'Um dos maiores clássicos da literatura brasileira...', 3, 3, '/covers/dom-casmurro.jpg'),
('9788535911664', 'Grande Sertão: Veredas', 'Guimarães Rosa', 'Nova Fronteira', 1956, 'Literatura Brasileira', 'Obra-prima da literatura brasileira...', 2, 2, '/covers/grande-sertao.jpg'),
('9788525406552', 'O Cortiço', 'Aluísio Azevedo', 'Ática', 1890, 'Literatura Brasileira', 'Romance naturalista que retrata a vida...', 4, 4, '/covers/o-cortico.jpg');

-- Inserir um empréstimo de teste
INSERT INTO emprestimo (id_livro, codigo_simade, data_emprestimo, data_devolucao_prevista, status)
SELECT id_livro, 'ALU001', NOW(), NOW() + INTERVAL '14 days', 'ATIVO'
FROM livro WHERE isbn = '9788535902775' LIMIT 1;

-- Inserir uma reserva de teste
INSERT INTO reserva (id_livro, codigo_simade, data_reserva, data_expiracao, status)
SELECT id_livro, 'ALU002', NOW(), NOW() + INTERVAL '7 days', 'ATIVA'
FROM livro WHERE isbn = '9788535911664' LIMIT 1;
```

**Executar:**
```bash
docker-compose exec db psql -U postgres < supabase/seed.sql
```

**Critério de Aceitação:**
- ✅ 4 usuários criados
- ✅ 3+ livros criados
- ✅ 1 empréstimo ativo (quantidade_disponivel do livro deve ter diminuído!)
- ✅ 1 reserva ativa

---

## Checklist de Ações - Semana 2 (14/out - 22/out)

### 3️⃣ Integração Next.js + Supabase

#### 3.1 Instalar Dependências

```bash
cd 06_proto

npm install @supabase/supabase-js @supabase/auth-helpers-nextjs
```

#### 3.2 Criar Cliente Supabase

Criar arquivo: `src/lib/supabase/client.ts`

```typescript
import { createClientComponentClient } from '@supabase/auth-helpers-nextjs';
import { Database } from '@/types/database.types';

export const supabase = createClientComponentClient<Database>();
```

Criar arquivo: `src/lib/supabase/server.ts`

```typescript
import { createServerComponentClient } from '@supabase/auth-helpers-nextjs';
import { cookies } from 'next/headers';
import { Database } from '@/types/database.types';

export const createServerClient = () => {
  const cookieStore = cookies();
  return createServerComponentClient<Database>({ cookies: () => cookieStore });
};
```

#### 3.3 Gerar Types do Banco

```bash
# Instalar Supabase CLI
npm install -g supabase

# Gerar types (quando as migrations estiverem rodando)
npx supabase gen types typescript --project-id "local" > src/types/database.types.ts
```

**OU** criar manualmente baseado no schema.

#### 3.4 Criar Service de Livros

Criar arquivo: `src/services/livrosService.ts`

```typescript
import { supabase } from '@/lib/supabase/client';
import { Livro } from '@/types/entities';

export const livrosService = {
  async getAll(): Promise<Livro[]> {
    const { data, error } = await supabase
      .from('livro')
      .select('*')
      .order('titulo');

    if (error) {
      console.error('Erro ao buscar livros:', error);
      throw error;
    }

    return data || [];
  },

  async getById(id: string): Promise<Livro | null> {
    const { data, error } = await supabase
      .from('livro')
      .select('*')
      .eq('id_livro', id)
      .single();

    if (error) {
      console.error('Erro ao buscar livro:', error);
      return null;
    }

    return data;
  },

  async search(query: string): Promise<Livro[]> {
    const { data, error } = await supabase
      .from('livro')
      .select('*')
      .textSearch('titulo_autor_tsvector', query, {
        type: 'websearch',
        config: 'portuguese'
      });

    if (error) {
      console.error('Erro na busca:', error);
      throw error;
    }

    return data || [];
  },

  async create(livro: Omit<Livro, 'id_livro' | 'created_at' | 'updated_at'>): Promise<Livro> {
    const { data, error } = await supabase
      .from('livro')
      .insert([livro])
      .select()
      .single();

    if (error) {
      console.error('Erro ao criar livro:', error);
      throw error;
    }

    return data;
  },

  async update(id: string, livro: Partial<Livro>): Promise<Livro> {
    const { data, error } = await supabase
      .from('livro')
      .update(livro)
      .eq('id_livro', id)
      .select()
      .single();

    if (error) {
      console.error('Erro ao atualizar livro:', error);
      throw error;
    }

    return data;
  },

  async delete(id: string): Promise<void> {
    const { error } = await supabase
      .from('livro')
      .delete()
      .eq('id_livro', id);

    if (error) {
      console.error('Erro ao deletar livro:', error);
      throw error;
    }
  }
};
```

#### 3.5 Criar Hook Customizado

Criar arquivo: `src/hooks/useLivros.ts`

```typescript
'use client';

import { useState, useEffect } from 'react';
import { livrosService } from '@/services/livrosService';
import { Livro } from '@/types/entities';

export function useLivros() {
  const [livros, setLivros] = useState<Livro[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);

  useEffect(() => {
    loadLivros();
  }, []);

  const loadLivros = async () => {
    try {
      setLoading(true);
      const data = await livrosService.getAll();
      setLivros(data);
      setError(null);
    } catch (err) {
      setError(err as Error);
    } finally {
      setLoading(false);
    }
  };

  const searchLivros = async (query: string) => {
    try {
      setLoading(true);
      const data = await livrosService.search(query);
      setLivros(data);
      setError(null);
    } catch (err) {
      setError(err as Error);
    } finally {
      setLoading(false);
    }
  };

  return {
    livros,
    loading,
    error,
    reload: loadLivros,
    search: searchLivros
  };
}
```

#### 3.6 Migrar Componente de Catálogo

Atualizar: `src/components/catalogo/CatalogoLivros.tsx`

```typescript
'use client';

import { useLivros } from '@/hooks/useLivros';
import { useState } from 'react';
// ... imports

export default function CatalogoLivros() {
  const { livros, loading, error, search } = useLivros();
  const [searchQuery, setSearchQuery] = useState('');

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    if (searchQuery.trim()) {
      search(searchQuery);
    }
  };

  if (loading) {
    return <div>Carregando livros...</div>;
  }

  if (error) {
    return <div>Erro ao carregar livros: {error.message}</div>;
  }

  return (
    <div>
      <form onSubmit={handleSearch}>
        <input
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          placeholder="Buscar livros..."
        />
        <button type="submit">Buscar</button>
      </form>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {livros.map(livro => (
          <LivroCard key={livro.id_livro} livro={livro} />
        ))}
      </div>
    </div>
  );
}
```

**Critério de Aceitação:**
- ✅ Componente carrega livros do Supabase
- ✅ Loading state visível
- ✅ Erros tratados
- ✅ Busca funcional

---

### 4️⃣ Testes de Integração

#### 4.1 Teste Manual - CRUD Completo

1. **Create (Bibliotecário)**
   - Acessar /bibliotecario
   - Adicionar novo livro
   - Verificar no Studio se foi inserido

2. **Read (Todos)**
   - Acessar /aluno
   - Ver catálogo com livros do banco
   - Buscar por título

3. **Update (Bibliotecário)**
   - Editar um livro
   - Verificar atualização no Studio

4. **Delete (Bibliotecário)**
   - Remover um livro
   - Confirmar exclusão no Studio

#### 4.2 Teste de Triggers

```sql
-- No Supabase Studio > SQL Editor

-- 1. Inserir um livro com 2 unidades
INSERT INTO livro (titulo, autor, quantidade_total, quantidade_disponivel)
VALUES ('Livro Teste', 'Autor Teste', 2, 2);

-- 2. Criar empréstimo
INSERT INTO emprestimo (id_livro, codigo_simade, data_devolucao_prevista)
SELECT id_livro, 'ALU001', NOW() + INTERVAL '14 days'
FROM livro WHERE titulo = 'Livro Teste';

-- 3. Verificar se quantidade_disponivel diminuiu para 1
SELECT titulo, quantidade_disponivel FROM livro WHERE titulo = 'Livro Teste';
-- Deve retornar 1

-- 4. Devolver o livro
UPDATE emprestimo
SET status = 'DEVOLVIDO', data_devolucao_real = NOW()
WHERE id_livro = (SELECT id_livro FROM livro WHERE titulo = 'Livro Teste');

-- 5. Verificar se quantidade_disponivel voltou para 2
SELECT titulo, quantidade_disponivel FROM livro WHERE titulo = 'Livro Teste';
-- Deve retornar 2
```

**Critério de Aceitação:**
- ✅ Trigger diminui quantidade ao emprestar
- ✅ Trigger aumenta quantidade ao devolver
- ✅ Status do livro muda para INDISPONIVEL quando quantidade_disponivel = 0

---

## Entregas da Sprint 1 (D30 - 22/out)

### Documentação
- [x] PRD completo
- [x] README com instruções de setup
- [x] docker-compose.yml configurado
- [x] .env.example documentado

### Infraestrutura
- [ ] Supabase local rodando sem erros
- [ ] Migrations executadas com sucesso
- [ ] Seed data populado

### Código
- [ ] Cliente Supabase configurado
- [ ] Services layer implementada
- [ ] Hooks customizados criados
- [ ] Componente de catálogo integrado

### Testes
- [ ] CRUD de livros funcionando end-to-end
- [ ] Triggers testados e funcionais
- [ ] Busca full-text operacional
- [ ] Vídeo de demonstração (2-3 min)

---

## Evidências a Entregar

1. **Screenshots:**
   - Supabase Studio mostrando tabelas populadas
   - App Next.js mostrando livros do banco
   - Busca funcionando

2. **Vídeo (2-3 min):**
   - Mostrar docker-compose up
   - Acessar Studio
   - Acessar app
   - Fazer busca
   - Adicionar livro
   - Mostrar livro no Studio

3. **Scripts SQL:**
   - Migrations versionadas no Git
   - Seed data versionado

---

## Problemas Comuns e Soluções

### Containers não sobem
```bash
# Limpar tudo e recomeçar
docker-compose down -v
docker system prune -a
docker-compose up -d
```

### Migrations não executam
```bash
# Executar manualmente
docker-compose exec db psql -U postgres -f /caminho/do/arquivo.sql

# Ou via Studio SQL Editor
```

### Next.js não conecta ao Supabase
1. Verificar .env.local
2. Reiniciar dev server
3. Verificar se containers estão rodando

---

## Próximos Passos (Sprint 2)

- Autenticação com Supabase Auth
- Row Level Security (RLS) policies
- CRUD de empréstimos
- CRUD de reservas
- Gestão de usuários

---

**Última atualização:** 07/nov/2024
