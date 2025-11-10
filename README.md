# 📚 BiblioKopke - Sistema de Gerenciamento de Biblioteca

Sistema completo de gerenciamento de bibliotecas escolares, desenvolvido como projeto interdisciplinar do curso de Análise e Desenvolvimento de Sistemas.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql)](https://www.postgresql.org/)
[![Supabase](https://img.shields.io/badge/Supabase-Ready-3ECF8E?logo=supabase)](https://supabase.com/)
[![License](https://img.shields.io/badge/License-Educational-green.svg)](LICENSE)

---

## 🎯 Sobre o Projeto

O **BiblioKopke** é um sistema desktop robusto para gestão completa de bibliotecas escolares, desenvolvido para facilitar o controle de acervo, empréstimos, devoluções, reservas e geração de relatórios gerenciais.

### Principais Características

- ✅ **Gestão Completa de Alunos** - Cadastro com validação de CPF e matrícula
- 📖 **Catálogo de Livros** - Controle de acervo com ISBN, categorias e localização
- 🔄 **Empréstimos e Devoluções** - Sistema automatizado com cálculo de multas
- 📅 **Reservas** - Gestão com expiração automática (7 dias)
- 🔔 **Notificações** - Alertas automáticos de atrasos e eventos
- 📊 **Relatórios Gerenciais** - 7 tipos de relatórios detalhados
- 💾 **Backup Integrado** - Sistema de backup do banco de dados
- 🔐 **Segurança** - BCrypt para senhas, auditoria completa
- 🎨 **Interface Moderna** - Windows Forms com suporte a temas claro/escuro

---

## 🚀 Início Rápido

### Pré-requisitos

- **Windows 10/11**
- **.NET 8.0 Runtime** ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **PostgreSQL 16** ou conta **Supabase** (recomendado para começar)

### Instalação Rápida (5 minutos)

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/shishiv/bibliokopke.git
   cd bibliokopke
   ```

2. **Configure o banco de dados**:

   **Opção A: Supabase (Recomendado)**
   - Crie uma conta gratuita em [supabase.com](https://supabase.com)
   - Crie um novo projeto
   - No SQL Editor, execute o conteúdo de `08_c#/schema-postgresql.sql`
   - Copie sua connection string em Settings → Database

   **Opção B: PostgreSQL Local**
   ```bash
   createdb bibliokopke
   psql -d bibliokopke -f 08_c#/schema-postgresql.sql
   ```

3. **Execute a aplicação**:
   ```bash
   cd 08_c#
   dotnet run
   ```

4. **Primeiro acesso**:
   - Login: `admin`
   - Senha: `admin123`
   - ⚠️ Você será obrigado a trocar a senha no primeiro login

---

## 📁 Estrutura do Repositório

```
bibliokopke/
├── 08_c#/                          # ⭐ Aplicação Principal (C# .NET 8)
│   ├── Model/                      # Entidades do domínio
│   ├── DAL/                        # Data Access Layer
│   ├── BLL/                        # Business Logic Layer
│   ├── Forms/                      # Interface Windows Forms
│   ├── Components/                 # Componentes reutilizáveis
│   ├── Constants.cs                # Constantes centralizadas
│   ├── Conexao.cs                  # Gerenciador de conexão
│   ├── Program.cs                  # Ponto de entrada
│   ├── schema-postgresql.sql       # Schema do banco de dados
│   ├── README.md                   # Documentação da aplicação
│   └── BibliotecaJK.csproj        # Arquivo do projeto
│
├── 01_planejamento/                # Documentação do projeto
│   ├── Projeto Interdisciplinar IV.pdf
│   ├── termo_aceite_segundo_semestre_COMPLETO.md
│   └── anexos/
│
├── 02_modelagem_banco/             # Modelagem do banco de dados
│   ├── banco_de_dados.sql         # Schema inicial (histórico)
│   └── exemplos_consultas.sql     # Queries de exemplo
│
├── 03_requisitos/                  # Requisitos e histórias de usuário
│   └── historia de usuario.pdf
│
├── 04_diagramas/                   # Diagramas UML do sistema
│   ├── diagrama_classes.png
│   ├── diagrama_casos_de_uso.png
│   ├── diagrama_sequencia_emprestimo.png
│   ├── diagrama_atividades_emprestimo.png
│   └── diagrama_uml_banco.png
│
├── 05_relatorios/                  # Relatórios finais e apresentações
│   ├── apresentacao_bibliokopke_foyer_final.pdf
│   └── relatório_bibliokopke_final.pdf
│
├── 07_foyer/                       # Protótipo HTML (histórico)
│   └── index.html
│
├── docs/                           # Documentação adicional
│   └── descricao_sistema.md
│
├── logo_jk.jpeg                    # Logo do projeto
└── README.md                       # 👈 Você está aqui
```

---

## 🏗️ Arquitetura

O sistema segue uma arquitetura em **4 camadas** bem definida:

```
┌─────────────────────────────────────┐
│  Forms (UI Layer)                   │  ← Interface Windows Forms
├─────────────────────────────────────┤
│  BLL (Business Logic Layer)         │  ← Regras de negócio e validações
├─────────────────────────────────────┤
│  DAL (Data Access Layer)            │  ← Acesso ao banco de dados
├─────────────────────────────────────┤
│  Model (Domain Entities)            │  ← Entidades do domínio
└─────────────────────────────────────┘
           ↓
    PostgreSQL / Supabase
```

### Tecnologias Utilizadas

| Camada | Tecnologia |
|--------|------------|
| **Frontend** | Windows Forms (.NET 8.0) |
| **Backend** | C# com arquitetura em camadas |
| **Banco de Dados** | PostgreSQL 16 / Supabase |
| **ORM** | Npgsql 8.0.8 (ADO.NET) |
| **Segurança** | BCrypt (hash de senhas) |
| **Diagramas** | Mermaid (UML) |

---

## 📊 Funcionalidades Detalhadas

### 1. Gestão de Empréstimos
- ⏱️ Prazo padrão: **7 dias**
- 📚 Máximo: **3 livros simultâneos** por aluno
- 💰 Multa: **R$ 2,00/dia** de atraso
- 🔄 Atualização automática de status (ATIVO → ATRASADO → DEVOLVIDO)
- 📦 Controle automático de estoque

### 2. Sistema de Reservas
- ⏳ Validade: **7 dias**
- 🔔 Notificação quando livro fica disponível
- ❌ Cancelamento automático após expiração

### 3. Segurança e Auditoria
- 🔐 Senhas com **BCrypt** (fator 11)
- 🔑 Troca obrigatória no primeiro login
- 👥 Perfis: ADMIN, BIBLIOTECARIO, OPERADOR
- ✅ Validação de CPF, ISBN, Email
- 📝 Log completo de todas as ações

### 4. Relatórios
1. 📊 Livros mais emprestados
2. 👥 Alunos com mais empréstimos
3. ⚠️ Empréstimos atrasados
4. 📅 Histórico por período
5. 📚 Livros disponíveis
6. 🔖 Reservas ativas
7. 📋 Log de ações do sistema

---

## 🛠️ Desenvolvimento

### Compilar o Projeto

```bash
cd 08_c#
dotnet build BibliotecaJK.csproj
```

### Executar em Modo Debug

```bash
dotnet run
```

### Criar Release

```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

A aplicação será gerada em `bin/Release/net8.0-windows/win-x64/publish/`

### Estrutura de Desenvolvimento

```bash
08_c#/
├── Model/              # POCOs (Aluno, Livro, Emprestimo, etc)
├── DAL/                # Classes de acesso a dados
├── BLL/                # Serviços e regras de negócio
├── Forms/              # 14 formulários Windows Forms
├── Components/         # Toast, ThemeManager, LoadingPanel, etc
└── Constants.cs        # Constantes e configurações
```

---

## 🎓 Projeto Acadêmico

Este projeto foi desenvolvido como **Projeto Interdisciplinar IV** do curso de **Análise e Desenvolvimento de Sistemas** da Faculdade Juscelino Kubitschek.

### Objetivos de Aprendizado

- ✅ Desenvolvimento de software desktop com C# e .NET
- ✅ Modelagem de banco de dados relacional
- ✅ Aplicação de padrões de arquitetura em camadas
- ✅ Implementação de CRUD completo
- ✅ Sistema de autenticação e autorização
- ✅ Geração de relatórios
- ✅ Controle de versão com Git
- ✅ Documentação técnica e diagramas UML

---

## 📖 Documentação

- 📘 **[README da Aplicação](08_c#/README.md)** - Guia completo da aplicação C#
- 🏛️ **[Arquitetura](08_c#/ARQUITETURA.md)** - Detalhes da arquitetura do sistema
- 📝 **[Release Notes](08_c#/RELEASE_NOTES.md)** - Histórico de versões
- 📊 **[Diagramas UML](04_diagramas/)** - Diagramas de classes, sequência, casos de uso
- 📋 **[Requisitos](03_requisitos/)** - Histórias de usuário

---

## 🐛 Troubleshooting

### Erro: "Conexão com banco de dados falhou"
- Verifique se o PostgreSQL está rodando
- Teste a connection string com `psql` ou ferramenta SQL
- Verifique firewall e porta 5432

### Erro: "Tabelas não encontradas"
- Execute o schema completo: `08_c#/schema-postgresql.sql`
- Verifique se todas as 7 tabelas foram criadas

### Erro: "Função verificar_senha não existe"
- A extensão `pgcrypto` precisa estar habilitada
- Execute: `CREATE EXTENSION IF NOT EXISTS pgcrypto;`

---

## 🤝 Contribuindo

Este é um projeto educacional, mas contribuições são bem-vindas!

1. Fork o projeto
2. Crie uma branch: `git checkout -b feature/NovaFuncionalidade`
3. Commit: `git commit -m 'feat: adiciona nova funcionalidade'`
4. Push: `git push origin feature/NovaFuncionalidade`
5. Abra um Pull Request

---

## 📝 Licença

Este projeto é de código aberto e está disponível para fins educacionais.

---

## 👥 Equipe

Desenvolvido com ❤️ por estudantes de Análise e Desenvolvimento de Sistemas da FAJK.

---

## 🗺️ Roadmap

- [ ] Migração para .NET MAUI (multiplataforma)
- [ ] API REST para integração externa
- [ ] App mobile para consulta de livros
- [ ] Sistema de pagamento de multas online
- [ ] Integração com leitor de código de barras
- [ ] Dashboard em tempo real
- [ ] Exportação de relatórios em PDF
- [ ] Sistema de recomendação de livros

---

## 📞 Suporte

- 📧 Email: contato@bibliokopke.com
- 🐛 Issues: [GitHub Issues](https://github.com/shishiv/bibliokopke/issues)
- 📚 Wiki: [Documentação Completa](https://github.com/shishiv/bibliokopke/wiki)

---

**Versão atual:** 3.1
**Última atualização:** Novembro 2024
**Status:** ✅ Em produção
