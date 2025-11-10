# BibliotecaJK - Sistema de Gerenciamento de Biblioteca

Sistema desktop para gestão completa de bibliotecas escolares, desenvolvido em C# com Windows Forms e PostgreSQL/Supabase.

## Características Principais

- **Gestão de Alunos**: Cadastro completo com validação de CPF e matrícula
- **Catálogo de Livros**: Controle de acervo com ISBN, categorias e localização
- **Empréstimos e Devoluções**: Sistema automatizado com cálculo de multas
- **Reservas**: Gestão de reservas com expiração automática
- **Notificações**: Alertas de atrasos e eventos importantes
- **Relatórios**: 7 tipos de relatórios gerenciais
- **Backup**: Sistema de backup integrado
- **Auditoria**: Log completo de todas as ações

## Tecnologias

- **.NET 8.0** - Framework principal
- **Windows Forms** - Interface desktop
- **PostgreSQL 16** - Banco de dados
- **Npgsql 8.0.8** - Driver PostgreSQL
- **BCrypt** - Hash seguro de senhas

## Início Rápido

### Pré-requisitos

1. Windows 10/11
2. .NET 8.0 Runtime ou SDK
3. PostgreSQL (local ou Supabase)

### Configuração com Supabase (Recomendado)

1. **Criar projeto no Supabase**:
   - Acesse https://supabase.com
   - Crie uma nova conta e projeto
   - Anote a connection string em Settings > Database

2. **Executar o schema**:
   - No Supabase, vá em SQL Editor
   - Cole o conteúdo completo do arquivo `schema-postgresql.sql`
   - Execute (Run)
   - Verifique se todas as tabelas foram criadas

3. **Configurar a aplicação**:
   - Execute o BibliotecaJK.exe
   - Na primeira execução, será solicitada a connection string:
     ```
     Host=db.xxxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=sua-senha
     ```
   - Teste a conexão
   - O sistema verificará automaticamente as tabelas

4. **Primeiro acesso**:
   - Login: `admin`
   - Senha: `admin123`
   - Você será obrigado a trocar a senha no primeiro login

### Configuração com PostgreSQL Local

1. **Instalar PostgreSQL 16**
2. **Criar banco de dados**:
   ```sql
   CREATE DATABASE bibliokopke;
   ```
3. **Executar schema**:
   ```bash
   psql -U postgres -d bibliokopke -f schema-postgresql.sql
   ```
4. **Connection string**:
   ```
   Host=localhost;Port=5432;Database=bibliokopke;Username=postgres;Password=sua-senha
   ```

## Arquitetura

O sistema segue arquitetura em 4 camadas:

```
Model (Entidades)
    ↓
DAL (Data Access Layer)
    ↓
BLL (Business Logic Layer)
    ↓
Forms (UI Layer)
```

### Estrutura de Pastas

```
08_c#/
├── Model/              # Entidades (Aluno, Livro, Emprestimo, etc)
├── DAL/                # Acesso a dados
├── BLL/                # Regras de negócio e validações
├── Forms/              # Formulários Windows Forms
├── Components/         # Componentes reutilizáveis (Toast, Theme, etc)
├── Conexao.cs          # Gerenciamento de conexão
├── Program.cs          # Ponto de entrada
└── schema-postgresql.sql  # Schema do banco de dados
```

### Entidades Principais

| Entidade | Descrição |
|----------|-----------|
| **Aluno** | Estudantes cadastrados (CPF, matrícula, turma) |
| **Funcionario** | Usuários do sistema (login, senha, perfil) |
| **Livro** | Acervo da biblioteca (ISBN, categoria, quantidade) |
| **Emprestimo** | Registro de empréstimos (datas, multa, status) |
| **Reserva** | Reservas de livros (status, expiração) |
| **Notificacao** | Alertas do sistema (atrasos, eventos) |
| **LogAcao** | Auditoria de todas as ações |

## Funcionalidades Detalhadas

### Empréstimos
- Prazo padrão: 7 dias
- Máximo simultâneo: 3 livros por aluno
- Multa: R$ 2,00 por dia de atraso
- Status automático: ATIVO → ATRASADO → DEVOLVIDO
- Atualização automática de estoque

### Reservas
- Validade: 7 dias
- Notificação quando livro ficar disponível
- Cancelamento automático após expiração

### Segurança
- Senhas com BCrypt (fator 11)
- Obrigatoriedade de troca no primeiro login
- Perfis de acesso: ADMIN, BIBLIOTECARIO, OPERADOR
- Validação de CPF, ISBN e Email
- Auditoria completa de ações

### Relatórios
1. Livros mais emprestados
2. Alunos com mais empréstimos
3. Empréstimos atrasados
4. Histórico de empréstimos por período
5. Livros disponíveis
6. Reservas ativas
7. Log de ações

## Desenvolvimento

### Compilar o projeto

```bash
cd 08_c#
dotnet build BibliotecaJK.csproj
```

### Executar em modo debug

```bash
dotnet run
```

### Criar release

```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

### Estrutura do Release

O executável gerado depende do .NET 8.0 Runtime instalado no Windows. Para criar um instalador completo, use o script `build-installer.bat`.

## Regras de Negócio

### Constantes do Sistema

Definidas em `BLL/EmprestimoService.cs`:
- `PRAZO_DIAS = 7` - Dias de prazo para devolução
- `MAX_EMPRESTIMOS_SIMULTANEOS = 3` - Máximo de livros por aluno
- `MULTA_POR_DIA = 2.00` - Valor da multa diária

### Validações

- CPF: Validação completa com dígitos verificadores
- ISBN: Formato ISBN-10 ou ISBN-13
- Email: Regex RFC 5322
- Matrícula: Alfanumérico único

## Troubleshooting

### Erro: "Arquivo schema-postgresql.sql não encontrado"
- Certifique-se de que o arquivo está na mesma pasta do executável
- Ou execute manualmente o schema no banco antes

### Erro: "Falha ao conectar ao banco de dados"
- Verifique a connection string
- Teste com psql ou ferramenta SQL
- Verifique firewall e portas (5432)

### Erro: "Função verificar_senha() não existe"
- Execute o schema completo novamente
- A extensão pgcrypto precisa estar habilitada

### Empréstimo não atualiza status automaticamente
- Execute manualmente: `SELECT atualizar_status_emprestimos();`
- Configure um cron job/scheduled task no servidor

## Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

## Licença

Este projeto é open source e está disponível para uso educacional.

## Suporte

Para questões e suporte:
- Consulte a documentação em `/docs`
- Verifique o `ARQUITETURA.md` para detalhes técnicos
- Veja `RELEASE_NOTES.md` para histórico de versões

## Roadmap

- [ ] Migração para .NET MAUI (multiplataforma)
- [ ] API REST para integração externa
- [ ] App mobile para consulta de livros
- [ ] Sistema de multas com pagamento online
- [ ] Integração com código de barras
- [ ] Dashboard em tempo real
- [ ] Exportação de relatórios em PDF

---

**Versão atual**: 3.1
**Última atualização**: Novembro 2024
