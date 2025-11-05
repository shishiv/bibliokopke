# 🚀 GUIA DE INSTALAÇÃO - BibliotecaJK v3.0

## Sumário
1. [Requisitos do Sistema](#requisitos-do-sistema)
2. [Instalação Rápida](#instalação-rápida)
3. [Instalação Detalhada](#instalação-detalhada)
4. [Configuração do Banco de Dados](#configuração-do-banco-de-dados)
5. [Configuração da Aplicação](#configuração-da-aplicação)
6. [Primeiro Acesso](#primeiro-acesso)
7. [Solução de Problemas](#solução-de-problemas)
8. [Deploy em Produção](#deploy-em-produção)

---

## Requisitos do Sistema

### Requisitos Mínimos

**Hardware:**
- Processador: Intel Core i3 ou equivalente
- RAM: 4 GB
- Espaço em disco: 500 MB
- Resolução de tela: 1024x768

**Software:**
- Sistema Operacional: Windows 10 ou superior
- .NET Runtime 8.0 ou superior
- MySQL Server 5.7 ou superior

### Requisitos Recomendados

**Hardware:**
- Processador: Intel Core i5 ou equivalente
- RAM: 8 GB ou mais
- Espaço em disco: 1 GB
- Resolução de tela: 1920x1080 (Full HD)

**Software:**
- Sistema Operacional: Windows 11
- .NET Runtime 8.0
- MySQL Server 8.0

---

## Instalação Rápida

Para desenvolvedores familiarizados com .NET e MySQL:

```bash
# 1. Clonar repositório (se aplicável)
git clone https://github.com/seu-usuario/bibliokopke.git
cd bibliokopke/"08_proto c#"

# 2. Instalar banco de dados
mysql -u root -p < schema.sql

# 3. Ajustar connection string em Conexao.cs

# 4. Restaurar dependências
dotnet restore

# 5. Compilar
dotnet build

# 6. Executar
dotnet run
```

**Login padrão:** admin / admin123

---

## Instalação Detalhada

### Passo 1: Instalar .NET Runtime

#### Opção A: Instalação via Instalador (Recomendado)

1. Acesse: https://dotnet.microsoft.com/download/dotnet/8.0
2. Baixe **.NET Desktop Runtime 8.0 (Windows x64)**
3. Execute o instalador
4. Siga as instruções na tela
5. Reinicie o computador se solicitado

#### Opção B: Instalação via Winget (Windows 11)

```powershell
winget install Microsoft.DotNet.Runtime.8
```

#### Verificar Instalação

Abra o **Prompt de Comando** e execute:

```cmd
dotnet --version
```

Deve exibir algo como: `8.0.x`

### Passo 2: Instalar MySQL Server

#### Opção A: MySQL Installer (Recomendado para Iniciantes)

1. Acesse: https://dev.mysql.com/downloads/installer/
2. Baixe **MySQL Installer for Windows**
3. Execute o instalador
4. Escolha **Custom Installation**
5. Selecione:
   - MySQL Server 8.0.x
   - MySQL Workbench (opcional, mas recomendado)
6. Configure:
   - Tipo: Development Computer
   - Porta: 3306 (padrão)
   - Senha do root: **anote esta senha!**
7. Finalize a instalação

#### Opção B: Instalação Manual

1. Baixe o MySQL Community Server
2. Extraia para `C:\mysql`
3. Execute `mysqld --install`
4. Inicie o serviço: `net start MySQL`

#### Verificar Instalação

Abra o **Prompt de Comando** e execute:

```cmd
mysql --version
```

Deve exibir: `mysql  Ver 8.0.x`

### Passo 3: Obter os Arquivos do Sistema

#### Opção A: Download Direto

1. Baixe o arquivo ZIP do sistema
2. Extraia para `C:\BibliotecaJK`

#### Opção B: Clone do Repositório Git

```bash
git clone https://github.com/seu-usuario/bibliokopke.git
cd bibliokopke
```

**Estrutura esperada:**
```
C:\BibliotecaJK\
└── 08_proto c#\
    ├── BLL\
    ├── DAL\
    ├── Model\
    ├── Forms\
    ├── Program.cs
    ├── Conexao.cs
    ├── schema.sql
    └── BibliotecaJK.csproj
```

---

## Configuração do Banco de Dados

### Método 1: Via Linha de Comando (Mais Rápido)

1. Abra o **Prompt de Comando**
2. Navegue até a pasta do projeto:
   ```cmd
   cd "C:\BibliotecaJK\08_proto c#"
   ```
3. Execute o script SQL:
   ```cmd
   mysql -u root -p < schema.sql
   ```
4. Digite a senha do MySQL quando solicitado

**O que este comando faz:**
- ✅ Cria o banco de dados `bibliokopke`
- ✅ Cria todas as tabelas
- ✅ Cria views
- ✅ Insere dados de teste

### Método 2: Via MySQL Workbench (Visual)

1. Abra o **MySQL Workbench**
2. Conecte ao servidor MySQL local
3. Clique em **File → Open SQL Script**
4. Selecione o arquivo `schema.sql`
5. Clique no ícone ⚡ **Execute**
6. Verifique no painel de resultados se não há erros

### Método 3: Manual (Passo a Passo)

1. Abra o MySQL via linha de comando:
   ```cmd
   mysql -u root -p
   ```
2. Digite sua senha
3. Crie o banco de dados:
   ```sql
   CREATE DATABASE bibliokopke;
   USE bibliokopke;
   ```
4. Copie e execute o conteúdo do `schema.sql`

### Verificar Instalação do Banco

Execute no MySQL:

```sql
USE bibliokopke;
SHOW TABLES;
```

**Resultado esperado:**
```
+------------------------+
| Tables_in_bibliokopke  |
+------------------------+
| Aluno                  |
| Emprestimo             |
| Funcionario            |
| Livro                  |
| Log_Acao               |
| Reserva                |
+------------------------+
6 rows in set
```

### Dados de Teste

O `schema.sql` cria automaticamente:
- **1 Funcionário Administrador**
  - Login: admin
  - Senha: admin123
- **3 Alunos de exemplo**
- **5 Livros de exemplo**
- **Exemplos de empréstimos e reservas**

---

## Configuração da Aplicação

### 1. Ajustar Connection String

Abra o arquivo `Conexao.cs` em um editor de texto:

```csharp
private static string GetConnectionString()
{
    return "server=localhost;database=bibliokopke;uid=root;pwd=SUA_SENHA_AQUI;";
}
```

**Parâmetros a ajustar:**

| Parâmetro | Descrição | Valor Padrão |
|-----------|-----------|--------------|
| `server` | Endereço do servidor MySQL | `localhost` |
| `database` | Nome do banco de dados | `bibliokopke` |
| `uid` | Usuário do MySQL | `root` |
| `pwd` | Senha do MySQL | **(AJUSTE AQUI!)** |

**Exemplo:**
```csharp
return "server=localhost;database=bibliokopke;uid=root;pwd=minhasenha123;";
```

### 2. Compilar a Aplicação

#### Método A: Via Linha de Comando

```cmd
cd "C:\BibliotecaJK\08_proto c#"
dotnet build --configuration Release
```

**Resultado esperado:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

#### Método B: Via Visual Studio

1. Abra `BibliotecaJK.csproj` no Visual Studio
2. Menu: **Build → Build Solution** (Ctrl+Shift+B)
3. Aguarde a compilação

### 3. Executar a Aplicação

#### Método A: Via Linha de Comando

```cmd
dotnet run
```

#### Método B: Via Executável

```cmd
cd bin\Release\net8.0-windows
BibliotecaJK.exe
```

#### Método C: Via Visual Studio

Pressione **F5** ou clique no botão ▶ **Start**

---

## Primeiro Acesso

### 1. Tela de Login

Ao iniciar, você verá a tela de login.

**Use as credenciais padrão:**
```
Login: admin
Senha: admin123
```

### 2. Dashboard

Após o login bem-sucedido, você verá:
- Dashboard com estatísticas
- Menu superior com todas as opções

### 3. Teste Básico

Faça um teste simples para verificar se está tudo funcionando:

1. Menu → **Cadastros → Alunos**
   - Verifique se os alunos de teste aparecem
2. Menu → **Cadastros → Livros**
   - Verifique se os livros de teste aparecem
3. Menu → **Empréstimos → Novo Empréstimo**
   - Tente registrar um empréstimo de teste
4. Volte ao Dashboard
   - Clique em **🔄 Atualizar Dashboard**
   - Verifique se as estatísticas mudaram

**✅ Se tudo funcionou, a instalação está completa!**

---

## Solução de Problemas

### Problema 1: "Não foi possível conectar ao banco de dados"

**Causas possíveis:**
1. MySQL não está rodando
2. Senha incorreta no `Conexao.cs`
3. Banco de dados não foi criado

**Solução:**

1. Verificar se MySQL está rodando:
   ```cmd
   sc query MySQL80
   ```
   Se não estiver rodando:
   ```cmd
   net start MySQL80
   ```

2. Testar conexão manualmente:
   ```cmd
   mysql -u root -p
   ```
   Se não conectar, a senha está incorreta.

3. Verificar se banco existe:
   ```cmd
   mysql -u root -p -e "SHOW DATABASES LIKE 'bibliokopke';"
   ```

### Problema 2: "O sistema não inicia"

**Erro:** "This application requires the .NET Runtime"

**Solução:**
Instale o .NET Runtime 8.0 Desktop (ver Passo 1)

### Problema 3: "Login ou senha incorretos"

**Se você não alterou nada:**
Use: `admin` / `admin123`

**Se você alterou o banco:**
Verifique no MySQL:
```sql
USE bibliokopke;
SELECT login, senha_hash FROM Funcionario;
```

**Resetar senha do admin:**
```sql
UPDATE Funcionario
SET senha_hash = 'admin123'
WHERE login = 'admin';
```

### Problema 4: Tela de login aparece mas não mostra nada

**Causa:** Problema de renderização do Windows Forms

**Solução:**
1. Feche a aplicação
2. Execute como administrador
3. Verifique resolução de tela (mínimo 1024x768)

### Problema 5: "Package MySql.Data not found"

**Causa:** Dependências não foram restauradas

**Solução:**
```cmd
dotnet restore
dotnet build
```

### Problema 6: Erro ao exportar relatórios

**Erro:** "Access denied"

**Solução:**
Execute a aplicação como administrador ou salve em pasta com permissão de escrita (ex: Documentos)

---

## Deploy em Produção

### Preparação

#### 1. Backup do Banco de Dados de Desenvolvimento

```cmd
mysqldump -u root -p bibliokopke > backup_dev.sql
```

#### 2. Compilar para Release

```cmd
dotnet publish --configuration Release --output C:\Deploy\BibliotecaJK
```

### Instalação no Servidor/Estação de Produção

#### Passo 1: Preparar o Ambiente

1. Instale .NET Runtime 8.0 Desktop
2. Instale MySQL Server 8.0
3. Configure firewall (porta 3306 se necessário)

#### Passo 2: Criar Banco de Dados

```cmd
mysql -u root -p < schema.sql
```

**⚠️ IMPORTANTE:**
- Em produção, **não use o usuário root**
- Crie um usuário específico:

```sql
CREATE USER 'bibliotecajk'@'localhost' IDENTIFIED BY 'senha_forte_aqui';
GRANT ALL PRIVILEGES ON bibliokopke.* TO 'bibliotecajk'@'localhost';
FLUSH PRIVILEGES;
```

Ajuste o `Conexao.cs`:
```csharp
return "server=localhost;database=bibliokopke;uid=bibliotecajk;pwd=senha_forte_aqui;";
```

#### Passo 3: Copiar Arquivos

Copie a pasta `C:\Deploy\BibliotecaJK` para o servidor.

#### Passo 4: Criar Atalho

1. Botão direito em `BibliotecaJK.exe`
2. **Criar atalho**
3. Mova o atalho para a Área de Trabalho
4. Renomeie para "BibliotecaJK"

#### Passo 5: Segurança em Produção

**⚠️ ANTES DE IR PARA PRODUÇÃO:**

1. **Altere a senha do admin:**
   ```sql
   UPDATE Funcionario
   SET senha_hash = 'NOVA_SENHA_FORTE'
   WHERE login = 'admin';
   ```

2. **Implemente hash de senhas real:**
   - O protótipo armazena senhas em texto plano
   - Em produção, use BCrypt ou Argon2

3. **Configure backups automáticos:**
   ```cmd
   # Criar script backup_diario.bat
   mysqldump -u bibliotecajk -pSENHA bibliokopke > backup_%date:~-4,4%%date:~-7,2%%date:~-10,2%.sql
   ```

   Agende no **Agendador de Tarefas do Windows** para rodar diariamente.

4. **Restrinja permissões de arquivos:**
   - Apenas administradores podem modificar arquivos do sistema

### Configurações Avançadas

#### Múltiplas Estações

Se você terá múltiplas estações acessando o mesmo banco:

1. **Configure MySQL para aceitar conexões remotas:**

   Edite `my.ini` (geralmente em `C:\ProgramData\MySQL\MySQL Server 8.0\`):
   ```ini
   bind-address = 0.0.0.0
   ```

2. **Reinicie o MySQL:**
   ```cmd
   net stop MySQL80
   net start MySQL80
   ```

3. **Ajuste Connection String em cada estação:**
   ```csharp
   return "server=IP_DO_SERVIDOR;database=bibliokopke;uid=bibliotecajk;pwd=senha;";
   ```

4. **Configure firewall:**
   ```cmd
   netsh advfirewall firewall add rule name="MySQL" dir=in action=allow protocol=TCP localport=3306
   ```

#### Log de Erros

Adicione logging em `Program.cs`:

```csharp
catch (Exception ex)
{
    File.AppendAllText("erros.log",
        $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n\n");
}
```

---

## Manutenção

### Backup Recomendado

**Diário:**
- Backup do banco de dados

**Semanal:**
- Backup completo (banco + aplicação)

**Mensal:**
- Exportação de todos os relatórios
- Cópia offsite

### Atualizações

Para atualizar o sistema:

1. Faça backup completo
2. Compile nova versão
3. Substitua apenas os arquivos `.exe` e `.dll`
4. **Não substitua** `Conexao.cs` (contém configurações)
5. Execute migrations SQL se houver

### Monitoramento

Verifique regularmente:
- Espaço em disco (banco de dados cresce)
- Logs de erro
- Performance (se lento, pode precisar indexar tabelas)

---

## Checklist de Instalação

Use este checklist para garantir que nada foi esquecido:

### Pré-Instalação
- [ ] .NET Runtime 8.0 instalado
- [ ] MySQL Server instalado
- [ ] Senha do MySQL anotada
- [ ] Arquivos do sistema extraídos

### Banco de Dados
- [ ] Banco `bibliokopke` criado
- [ ] Tabelas criadas (6 tabelas)
- [ ] Dados de teste inseridos
- [ ] Conexão testada

### Aplicação
- [ ] `Conexao.cs` configurado com senha correta
- [ ] Compilação executada sem erros
- [ ] Aplicação inicia
- [ ] Login funciona (admin/admin123)
- [ ] Dashboard carrega
- [ ] Cadastros acessíveis
- [ ] Empréstimo de teste realizado
- [ ] Relatório gerado

### Produção (adicional)
- [ ] Usuário específico do MySQL criado
- [ ] Senha do admin alterada
- [ ] Backup configurado
- [ ] Atalho criado
- [ ] Documentação entregue aos usuários

---

## Suporte

Para dúvidas sobre instalação:

- 📧 E-mail: suporte@bibliokopke.com
- 📖 Documentação: Consulte `MANUAL_USUARIO.md`
- 🏗️ Arquitetura: Consulte `ARQUITETURA.md`

---

**Desenvolvido por:**
Pessoa 1: Banco de Dados
Pessoa 2: Camada DAL
Pessoa 3: Camada BLL
Pessoa 4: Interface WinForms
Pessoa 5: Relatórios e Documentação

**BibliotecaJK v3.0** - Sistema Completo de Gerenciamento de Bibliotecas
© 2025 - Todos os direitos reservados
