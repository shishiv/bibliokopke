# 🚀 Guia Rapido - Configurar BibliotecaJK com Supabase

Este guia mostra como configurar o BibliotecaJK para usar Supabase (PostgreSQL na nuvem) em vez de MySQL local.

---

## 🎯 Por que Supabase?

### Antes (MySQL local):
- ❌ Instalar MySQL (200+ MB)
- ❌ Configurar servidor MySQL
- ❌ Criar banco manualmente
- ❌ Configurar backup manual
- ⏱️ **Tempo: 15-30 minutos**

### Agora (Supabase):
- ✅ Banco na nuvem (ja configurado)
- ✅ Backups automaticos
- ✅ Acesso de qualquer lugar
- ✅ Gratis para sempre (500 MB)
- ⏱️ **Tempo: 2-5 minutos**

---

## 📋 Passo 1: Criar Conta no Supabase (2 minutos)

1. **Acesse**: https://supabase.com
2. **Clique em**: "Start your project"
3. **Faca login** com:
   - GitHub (recomendado)
   - Google
   - Email

---

## 📦 Passo 2: Criar Novo Projeto (1 minuto)

1. **Clique em**: "New Project"
2. **Preencha**:
   - **Name**: BibliotecaJK (ou qualquer nome)
   - **Database Password**: Escolha uma senha forte (anote!)
   - **Region**: Brazil (South America)
   - **Pricing Plan**: Free (500 MB, gratuito para sempre)
3. **Clique em**: "Create new project"
4. **Aguarde**: ~2 minutos (provisionamento do banco)

---

## 🔗 Passo 3: Obter Connection String (30 segundos)

1. **No painel do Supabase**, clique em:
   - **Settings** (engrenagem no canto inferior esquerdo)
   - **Database**
   - Role ate "Connection String"

2. **Selecione**: "URI" (nao "Connection Pooling")

3. **Copie** a connection string que parece com:
   ```
   postgresql://postgres:[SUA-SENHA]@db.xxxxxxxxxxxxx.supabase.co:5432/postgres
   ```

4. **IMPORTANTE**: Substitua `[SUA-SENHA]` pela senha que voce criou no Passo 2!

### Exemplo de Connection String Completa:
```
Host=db.xxxxxxxxxxxxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=sua_senha_aqui
```

Ou formato URI:
```
postgresql://postgres:sua_senha_aqui@db.xxxxxxxxxxxxx.supabase.co:5432/postgres
```

---

## 🗄️ Passo 4: Executar Schema SQL (1 minuto)

1. **No painel do Supabase**, clique em:
   - **SQL Editor** (icone </> na barra lateral)

2. **Clique em**: "+ New query"

3. **Cole todo o conteudo** do arquivo `schema-postgresql.sql`
   - Localizacao: `08_proto c#/schema-postgresql.sql`

4. **Clique em**: "Run" (ou pressione Ctrl+Enter)

5. **Verifique**: Deve aparecer "Success. No rows returned"

6. **Confirme** que as tabelas foram criadas:
   - Clique em "Table Editor"
   - Deve ver: Aluno, Funcionario, Livro, Emprestimo, Reserva, Log_Acao

---

## 🖥️ Passo 5: Configurar BibliotecaJK (30 segundos)

1. **Execute** BibliotecaJK.exe

2. **Na primeira execucao**, aparecera:
   - "Configuracao Inicial - Conexao com Banco de Dados"

3. **Cole a connection string** que copiou no Passo 3

4. **Clique em**: "Testar Conexao"
   - Deve aparecer: "Conexao testada com sucesso!" (verde)

5. **Clique em**: "Salvar e Continuar"

6. **Pronto!** Faca login com:
   - **Usuario**: admin
   - **Senha**: admin123

---

## ✅ Verificacao Rapida

Apos login, verifique:
- ✅ Dashboard carrega
- ✅ Pode cadastrar aluno de teste
- ✅ Pode cadastrar livro de teste
- ✅ Pode fazer emprestimo de teste

---

## 🔄 Alterar Connection String

Se precisar trocar o banco ou corrigir a senha:

### Opcao 1: Via Interface
1. Menu → Ferramentas → Configurar Conexao
2. Cole nova connection string
3. Teste e salve

### Opcao 2: Arquivo Manual
1. Feche o BibliotecaJK
2. Abra: `%LOCALAPPDATA%\BibliotecaJK\database.config`
3. Edite o JSON
4. Salve e reabra o programa

---

## 📊 Limites do Plano Gratuito

| Recurso | Limite Gratuito |
|---------|----------------|
| **Espaco** | 500 MB |
| **Bandwidth** | 5 GB/mes |
| **Conexoes simultaneas** | Ilimitadas* |
| **Backup automatico** | 7 dias |
| **Uptime** | 99.9% |

*Dentro do razoavel para uso educacional/pequeno.

**Para uma biblioteca escolar tipica**:
- 1.000 alunos
- 5.000 livros
- 10.000 emprestimos/ano
- **Uso estimado**: ~10-20 MB (muito dentro do limite!)

---

## 🔒 Seguranca

### O que o Supabase ja faz:
- ✅ Criptografia em transito (SSL/TLS)
- ✅ Criptografia em repouso (AES-256)
- ✅ Backups automaticos diarios
- ✅ Replicacao geografica
- ✅ Monitoramento 24/7

### O que VOCE deve fazer:
- ✅ Use senha forte (min 12 caracteres)
- ✅ Nao compartilhe a connection string
- ✅ Altere senha do admin apos primeiro login
- ✅ Crie usuarios separados (nao use sempre admin)

---

## 💡 Dicas Avancadas

### 1. Ver Dados em Tempo Real
- Painel Supabase → Table Editor
- Escolha uma tabela (ex: Aluno)
- Veja/edite dados diretamente

### 2. Executar Queries Customizadas
- SQL Editor → New query
- Execute SELECT, UPDATE, DELETE, etc.
- Exemplo:
  ```sql
  SELECT * FROM Aluno;
  SELECT * FROM Emprestimo WHERE data_devolucao IS NULL;
  ```

### 3. Monitorar Performance
- Database → Logs
- Veja queries lentas
- Monitore uso de recursos

### 4. Adicionar mais Usuarios Admin
```sql
-- Execute no SQL Editor do Supabase
INSERT INTO Funcionario (nome, cpf, cargo, login, senha_hash, perfil)
VALUES ('Seu Nome', '123.456.789-00', 'Bibliotecario', 'seunome',
        '$2a$11$hash_aqui', 'ADMIN');
```

---

## 🐛 Solucao de Problemas

### Erro: "Nao foi possivel conectar"
**Causa**: Connection string incorreta

**Solucao**:
1. Verifique se substituiu [SUA-SENHA]
2. Verifique se nao tem espacos extras
3. Teste no SQL Editor do Supabase primeiro

### Erro: "relation 'aluno' does not exist"
**Causa**: Schema nao foi executado

**Solucao**:
1. Va no SQL Editor
2. Execute `schema-postgresql.sql` completo
3. Verifique em Table Editor se as tabelas existem

### Erro: "password authentication failed"
**Causa**: Senha incorreta

**Solucao**:
1. Va em Settings → Database
2. Clique em "Reset database password"
3. Anote nova senha
4. Atualize connection string no BibliotecaJK

### Erro: "too many connections"
**Causa**: Muitas conexoes abertas (raro no uso normal)

**Solucao**:
1. Feche e reabra o BibliotecaJK
2. No Supabase: Database → Logs → veja conexoes ativas
3. Se persistir, contate suporte Supabase

---

## 📈 Migracao de MySQL Local para Supabase

Se voce ja tinha dados no MySQL local:

### Opcao 1: Exportar/Importar Manual
1. **Exportar do MySQL**:
   ```bash
   mysqldump -u root bibliokopke > backup.sql
   ```

2. **Converter para PostgreSQL**:
   - Use ferramenta: https://www.convert-in.com/mysql-to-postgres.htm
   - Ou edite manualmente (trocar AUTO_INCREMENT por SERIAL, etc.)

3. **Importar no Supabase**:
   - SQL Editor → Cole o SQL convertido → Run

### Opcao 2: CSV
1. Exporte cada tabela como CSV do MySQL
2. No Supabase: Table Editor → Import from CSV
3. Mapeie as colunas corretamente

---

## 🆘 Suporte

### Documentacao Oficial
- **Supabase Docs**: https://supabase.com/docs
- **PostgreSQL Docs**: https://www.postgresql.org/docs/

### Comunidade
- **Discord Supabase**: https://discord.supabase.com
- **Forum Supabase**: https://github.com/supabase/supabase/discussions

### BibliotecaJK
- **GitHub Issues**: https://github.com/shishiv/bibliokopke/issues
- **Manual do Usuario**: Veja `MANUAL_USUARIO.md`

---

## 🎓 Exemplo Completo (Passo a Passo Visual)

### 1. Criar Projeto no Supabase
```
https://supabase.com
→ Start your project
→ New Project
→ Name: BibliotecaJK
→ Password: SuaSenhaForte123!
→ Region: South America (Brazil)
→ Create new project
→ Aguardar ~2 minutos
```

### 2. Copiar Connection String
```
Settings (⚙️)
→ Database
→ Connection String
→ URI
→ Copiar: postgresql://postgres:SuaSenhaForte123!@db.abcdefgh.supabase.co:5432/postgres
```

### 3. Executar Schema
```
SQL Editor (</>)
→ + New query
→ Colar conteudo de schema-postgresql.sql
→ Run (Ctrl+Enter)
→ Verificar: "Success"
```

### 4. Configurar BibliotecaJK
```
Executar BibliotecaJK.exe
→ "Configuracao Inicial"
→ Colar connection string
→ Testar Conexao (✓ verde)
→ Salvar e Continuar
→ Login: admin / admin123
```

---

## 🎉 Pronto!

Agora voce tem:
- ✅ Banco de dados na nuvem
- ✅ Backups automaticos
- ✅ Acesso de qualquer lugar
- ✅ Sem instalacao de MySQL
- ✅ Gratis para sempre (500 MB)

**Instalacao total**: ~5 minutos vs 30 minutos (MySQL local)

---

**Desenvolvido com ❤️ pela BibliotecaJK Team**

**Versao**: 3.1 (Supabase Edition)

**Data**: 2025-11-05
