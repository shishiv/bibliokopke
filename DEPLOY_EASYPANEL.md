# Deploy no Easypanel - BiblioKopke

Guia completo para fazer deploy do BiblioKopke no Easypanel com Traefik para gerenciamento de domínios.

---

## Arquitetura de Deploy

```
┌─────────────────────────────────────────────┐
│           Internet                          │
└──────────────────┬──────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────┐
│   Traefik (Reverse Proxy)                   │
│   - SSL/TLS automático                      │
│   - Roteamento por domínio                  │
└──────────────────┬──────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────┐
│   Next.js (BiblioKopke)                     │
│   - Container Docker                        │
│   - Porta 3000                              │
│   - Variáveis de ambiente                   │
└──────────────────┬──────────────────────────┘
                   │
                   ▼
┌─────────────────────────────────────────────┐
│   Supabase Cloud                            │
│   - PostgreSQL                              │
│   - Auth                                    │
│   - Storage                                 │
└─────────────────────────────────────────────┘
```

---

## Pré-requisitos

### 1. Conta Supabase Cloud

1. Acesse https://supabase.com
2. Crie uma conta (gratuita)
3. Crie um novo projeto:
   - **Nome:** bibliokopke
   - **Database Password:** (anote em segurança)
   - **Region:** South America (São Paulo) ou mais próxima

4. Aguarde ~2 minutos para o projeto ser provisionado

5. Anote as credenciais (Settings > API):
   - **Project URL:** `https://xxxxx.supabase.co`
   - **anon public key:** `eyJhbG...`
   - **service_role key:** `eyJhbG...` (mantenha seguro!)

### 2. Servidor com Easypanel

- VPS com Docker instalado
- Easypanel configurado
- Traefik rodando (já vem com Easypanel)
- Domínio apontando para o servidor

---

## Passo 1: Configurar Banco de Dados no Supabase

### 1.1 Executar Migrations

Acesse seu projeto no Supabase:

1. Vá em **SQL Editor**
2. Clique em **New Query**
3. Cole o conteúdo de `database/migrations/001_create_tables.sql`
4. Clique em **Run** (ou `Ctrl+Enter`)
5. Repita para `002_create_triggers.sql` e `003_create_views.sql`

Ou use a Supabase CLI:

```bash
# Instalar CLI
npm install -g supabase

# Login
supabase login

# Link com seu projeto
supabase link --project-ref xxxxx

# Push migrations
supabase db push
```

### 1.2 Verificar Tabelas

No Supabase Studio:
- Vá em **Table Editor**
- Você deve ver: `usuario`, `livro`, `emprestimo`, `reserva`, etc.

### 1.3 Inserir Dados Iniciais

No **SQL Editor**, execute o conteúdo de `database/seeds/dev_data.sql` (se existir).

---

## Passo 2: Deploy no Easypanel

### 2.1 Criar Novo Serviço

1. Acesse seu Easypanel
2. Vá em **Projects** > **New Project**
3. Nome: `bibliokopke`
4. Clique em **Create**

### 2.2 Adicionar Aplicação Next.js

1. Dentro do projeto, clique em **Add Service**
2. Escolha **App (from Git)**
3. Configure:

#### Git Repository
- **Repository URL:** `https://github.com/seu-usuario/BiblioKopke`
- **Branch:** `main`
- **Path:** `06_proto` (se o Next.js está nessa pasta)

#### Build Settings
- **Build Command:** `npm run build`
- **Start Command:** `npm start`
- **Port:** `3000`

#### Environment Variables

Adicione as seguintes variáveis:

```bash
# Next.js
NODE_ENV=production
NEXT_PUBLIC_APP_URL=https://bibliokopke.suaescola.edu.br

# Supabase (copie do dashboard do Supabase)
NEXT_PUBLIC_SUPABASE_URL=https://xxxxx.supabase.co
NEXT_PUBLIC_SUPABASE_ANON_KEY=eyJhbG...

# Service Role Key (marque como SECRET!)
SUPABASE_SERVICE_ROLE_KEY=eyJhbG...

# App Config
EMPRESTIMO_PRAZO_DIAS=14
EMPRESTIMO_MAX_RENOVACOES=2
EMPRESTIMO_LIMITE_ALUNO=3
EMPRESTIMO_LIMITE_PROFESSOR=5
MULTA_VALOR_DIA=1.00
```

**IMPORTANTE:** Marque `SUPABASE_SERVICE_ROLE_KEY` como **SECRET** (não expor em logs).

### 2.3 Configurar Domínio com Traefik

1. No Easypanel, vá em **Domains**
2. Clique em **Add Domain**
3. Configure:
   - **Domain:** `bibliokopke.suaescola.edu.br`
   - **Port:** `3000`
   - **Enable HTTPS:** ✅
   - **Force HTTPS:** ✅
   - **Let's Encrypt:** ✅

4. Salve

### 2.4 Configurar DNS

No seu provedor de DNS (Cloudflare, Route53, etc.):

```
Tipo: A
Nome: bibliokopke
Valor: <IP do seu servidor>
TTL: 300 (ou automático)
```

Se usar subdomain:
```
Tipo: CNAME
Nome: bibliokopke
Valor: seu-servidor.com
TTL: 300
```

---

## Passo 3: Build e Deploy

### 3.1 Fazer Deploy

1. No Easypanel, clique em **Deploy**
2. Aguarde o build (pode levar 2-5 minutos na primeira vez)
3. Acompanhe os logs

### 3.2 Verificar Build

Verifique os logs de build:
```
✓ Creating an optimized production build
✓ Compiled successfully
✓ Collecting page data
✓ Generating static pages (X/X)
✓ Finalizing page optimization
```

### 3.3 Acessar Aplicação

Após deploy bem-sucedido:
- Acesse: `https://bibliokopke.suaescola.edu.br`
- Certificado SSL deve estar ativo (cadeado verde)

---

## Passo 4: Configurações Pós-Deploy

### 4.1 Configurar Auth Redirects no Supabase

No Supabase Dashboard:

1. Vá em **Authentication** > **URL Configuration**
2. Configure:
   - **Site URL:** `https://bibliokopke.suaescola.edu.br`
   - **Redirect URLs:**
     ```
     https://bibliokopke.suaescola.edu.br
     https://bibliokopke.suaescola.edu.br/**
     ```

### 4.2 Testar Autenticação

1. Tente fazer login
2. Verifique se os redirects funcionam
3. Teste criação de usuário (se habilitado)

### 4.3 Configurar Row Level Security (RLS)

No Supabase SQL Editor, execute:

```sql
-- Habilitar RLS em todas as tabelas
ALTER TABLE usuario ENABLE ROW LEVEL SECURITY;
ALTER TABLE livro ENABLE ROW LEVEL SECURITY;
ALTER TABLE emprestimo ENABLE ROW LEVEL SECURITY;
ALTER TABLE reserva ENABLE ROW LEVEL SECURITY;

-- Políticas básicas (exemplo para livro)
-- Todos podem ler livros
CREATE POLICY "Publico pode ver livros"
  ON livro FOR SELECT
  USING (true);

-- Apenas bibliotecários podem modificar
CREATE POLICY "Bibliotecarios podem modificar livros"
  ON livro FOR ALL
  USING (
    auth.jwt() ->> 'tipo_usuario' = 'BIBLIOTECARIO'
  );
```

Adapte as policies conforme necessário (veja PRD.md para exemplos completos).

---

## Passo 5: Monitoramento e Logs

### 5.1 Logs do Next.js

No Easypanel:
- Vá em **Logs**
- Acompanhe erros e requests

### 5.2 Logs do Supabase

No Supabase Dashboard:
- Vá em **Logs** > **Database**
- Veja queries executadas
- Identifique problemas de performance

### 5.3 Métricas

No Easypanel:
- CPU, RAM, Network
- Requests por segundo

No Supabase:
- Database size
- API requests (limite: 500k/mês no free tier)
- Storage usage

---

## Troubleshooting

### Build falha

**Erro:** `MODULE_NOT_FOUND`

```bash
# Certifique-se que package.json está correto
# Verifique se está usando npm/yarn/pnpm consistentemente

# No Easypanel, force rebuild
# Settings > Advanced > Clear Build Cache
```

**Erro:** `Out of memory`

```bash
# Aumente memória do container no Easypanel
# Settings > Resources > Memory Limit: 2GB
```

### Aplicação não carrega

**Erro 502 Bad Gateway**

```bash
# Verifique se a porta está correta (3000)
# Verifique os logs do container
# Certifique-se que o Next.js está rodando
```

**Erro:** Página em branco

```bash
# Verifique variáveis de ambiente
# Especialmente NEXT_PUBLIC_SUPABASE_URL e NEXT_PUBLIC_SUPABASE_ANON_KEY
# Elas devem estar visíveis no build
```

### SSL não funciona

```bash
# Verifique DNS (deve apontar para o servidor)
dig bibliokopke.suaescola.edu.br

# Verifique Traefik logs no Easypanel
# Certifique-se que porta 80 e 443 estão abertas no firewall
```

### Erro ao conectar no Supabase

**Erro:** `Invalid API key`

```bash
# Verifique se as chaves estão corretas
# NEXT_PUBLIC_SUPABASE_ANON_KEY deve ser a chave "anon public"
# SUPABASE_SERVICE_ROLE_KEY deve ser a chave "service_role"
```

**Erro:** `Row Level Security policy violation`

```bash
# RLS está bloqueando acesso
# Configure as policies corretas (veja Passo 4.3)
# Ou desabilite RLS temporariamente (NÃO recomendado em prod):
ALTER TABLE nome_tabela DISABLE ROW LEVEL SECURITY;
```

---

## Atualizações e CI/CD

### Deploy Manual

```bash
# No Easypanel
# 1. Vá em Deploy
# 2. Clique em "Redeploy"
# Ou use webhook (veja abaixo)
```

### GitHub Actions (Auto Deploy)

Crie `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Easypanel

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - name: Trigger Easypanel Deploy
        run: |
          curl -X POST ${{ secrets.EASYPANEL_WEBHOOK_URL }}
```

Configure o secret no GitHub:
- **EASYPANEL_WEBHOOK_URL:** (copie do Easypanel > Settings > Webhooks)

---

## Backup

### Banco de Dados

O Supabase faz backup automático. Para backup manual:

```bash
# Via Supabase CLI
supabase db dump -f backup.sql

# Ou via pg_dump direto
pg_dump -h db.xxxxx.supabase.co -U postgres -d postgres > backup.sql
```

### Storage (Capas de Livros)

No Supabase Dashboard:
- Vá em **Storage**
- Faça download dos buckets periodicamente
- Ou configure backup automático para S3

---

## Custos Estimados

### Supabase Cloud (Free Tier)
- **Database:** 500MB
- **Storage:** 1GB
- **Bandwidth:** 2GB
- **API Requests:** 500k/mês

**Upgrade para Pro ($25/mês):**
- Database: 8GB
- Storage: 100GB
- Bandwidth: 250GB
- API Requests: Ilimitado

### Servidor (Easypanel)
- Depende do seu VPS
- Recomendado: 2 CPU, 2GB RAM (~$10-20/mês)

---

## Segurança

### Checklist

- [ ] HTTPS habilitado (Let's Encrypt)
- [ ] Variáveis sensíveis marcadas como SECRET
- [ ] RLS habilitado em todas as tabelas
- [ ] Policies de acesso configuradas
- [ ] CORS configurado no Supabase
- [ ] Rate limiting habilitado (Traefik)
- [ ] Firewall configurado (apenas 80, 443, 22)
- [ ] Backups regulares configurados

---

## Recursos Úteis

- [Easypanel Docs](https://easypanel.io/docs)
- [Supabase Docs](https://supabase.com/docs)
- [Traefik Docs](https://doc.traefik.io/traefik/)
- [Next.js Deploy](https://nextjs.org/docs/deployment)

---

**Pronto!** Seu BiblioKopke está no ar! 🚀

Em caso de problemas, consulte os logs no Easypanel e Supabase Dashboard.
