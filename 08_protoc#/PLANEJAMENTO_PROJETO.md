# Planejamento do Projeto BiblioKopke

## Sistema de Gestão de Biblioteca Escolar

**Período**: 01/out/2025 - 30/nov/2025 (60 dias)
**Equipe**: 5 pessoas
**Tecnologias**: C# (WinForms/WPF) + MySQL

---

## Divisao de Trabalho

### ?? Pessoa 1: Banco de Dados (MySQL)
**Responsabilidades:**
- [ ] Criar script DDL completo (tabelas, indices, constraints)
- [ ] Implementar modelo fisico a partir do DER
- [ ] Criar dados de teste (DML) para todas as entidades
- [ ] Desenvolver procedures e triggers (atualizacao automatica de disponibilidade)
- [ ] Documentar dicionario de dados

### ?? Pessoa 2: Backend - Camada de Dados (C#)
**Responsabilidades:**
- [ ] Configurar conexao C# -> MySQL (ADO.NET ou Entity Framework)
- [ ] Implementar classes de modelo (entidades do banco)
- [ ] Desenvolver camada de acesso a dados (Data Access Layer)
- [ ] Criar metodos CRUD para Livros, Alunos, Funcionarios
- [ ] Realizar testes de integracao com banco de dados

### ?? Pessoa 3: Backend - Logica de Negocio (C#)
**Responsabilidades:**
- [ ] Implementar regras de negocio para emprestimos
- [ ] Desenvolver logica de reservas
- [ ] Criar sistema de controle de devolucoes
- [ ] Implementar validacoes e tratamento de excecoes
- [ ] Desenvolver sistema de logs (quem fez o que e quando)

### ?? Pessoa 4: Frontend - Telas e Experiencia (C# WinForms/WPF)
**Passos concluidos (1 e 2):**
- [] Tela de login com validacao basica
- [] Estrutura de perfis e controle de acesso

**Responsabilidades pendentes:**
- [ ] Finalizar cadastros de Livros, Alunos e Funcionarios com validacoes
- [ ] Implementar fluxo de emprestimo, devolucao e reservas integrado ao backend
- [ ] Ajustar UX essencial (dashboard, pesquisa de acervo, feedback visual, atalhos principais)

### ?? Pessoa 5: Relatorios, Documentacao e Testes
**Responsabilidades:**
- [ ] Estruturar relatorios principais (emprestimos por periodo, acervo, alunos)
- [ ] Implementar exportacao (PDF/CSV)
- [ ] Elaborar manuais do usuario e tecnico
- [ ] Registrar evidencias de testes e checklist de acessibilidade
- [ ] Preparar materiais de apresentacao (slides e video)

## Cronograma de Entregas

### 📅 Semana 1-2: 01/out - 13/out (Fundação Crítica)
**Entrega: 13/out - Domingo**

#### Tarefas
- [ ] **P1**: Script DDL completo com todas as tabelas
- [ ] **P1**: Inserir dados de teste básicos
- [ ] **P2**: Configurar projeto C# com estrutura de camadas
- [ ] **P2**: Implementar conexão com MySQL
- [ ] **P2**: Criar classes de modelo (Livro, Aluno, Funcionário, Empréstimo, Reserva)
- [ ] **Todos**: Revisão final de requisitos e DER

#### Critérios de Aceitação
- [ ] Banco sobe do zero (DROP/CREATE) e popula dados de exemplo
- [ ] App C# conecta no banco e executa pelo menos 1 SELECT real
- [ ] Projeto compila sem erros

#### Evidências
- [ ] Script .sql versionado no repositório
- [ ] Vídeo curto mostrando app listando dados do banco
- [ ] Print da compilação bem-sucedida

#### Status: 🔴 Não iniciado

---

### 📅 Semana 3-4: 14/out - 27/out (Core do Sistema)
**Entrega: 27/out - Domingo**

#### Tarefas
- [ ] **P1**: Implementar triggers para disponibilidade automatica de livros
- [ ] **P1**: Criar procedures para regras de negocio
- [ ] **P2**: CRUD completo de Livros
- [ ] **P2**: CRUD completo de Alunos
- [ ] **P2**: CRUD completo de Funcionarios
- [ ] **P3**: Implementar logica de emprestimos
- [ ] **P3**: Implementar logica de reservas
- [ ] **P4**: Cadastros de Livros, Alunos e Funcionarios integrados ao backend
- [ ] **P4**: Dashboard inicial e pesquisa de acervo navegaveis
- [ ] **P4**: Tela de Login com validacao (passo 1 concluido)
- [ ] **P4**: Controle de acesso por perfil (passo 2 concluido)
- [ ] **P5**: Planejar estrutura e indicadores dos relatorios
#### Critérios de Aceitação
- [ ] Trigger/Procedure executa automaticamente regra crítica
- [ ] CRUDs com validação mínima (campos obrigatórios, formatos)
- [ ] Telas de cadastro salvam e listam dados corretamente

#### Evidências
- [ ] Scripts SQL das procedures/triggers
- [ ] Prints do app executando as ações CRUD
- [ ] Relatório de testes (happy path + 1 caso de erro)

#### Status: 🔴 Não iniciado

---

### 📅 Semana 5-6: 28/out - 10/nov (Fluxos Operacionais)
**Entrega: 10/nov - Domingo**

#### Tarefas
- [ ] **P3**: Sistema de devolucoes (normal e com atraso)
- [ ] **P3**: Implementar todas as validacoes de negocio
- [ ] **P3**: Sistema de logs (auditoria de acoes)
- [ ] **P4**: Fluxo de emprestimo ponta a ponta (tela + validacao)
- [ ] **P4**: Fluxo de devolucao com tratamento de atraso
- [ ] **P4**: Tela de reservas integrada e pesquisa de acervo refinada
- [ ] **P5**: Estrutura base para relatorios (queries e layout)
#### Criterios de Aceitacao
- [ ] Fluxo ponta-a-ponta executavel (Login -> Emprestimo -> Devolucao)
- [ ] Reservas e pesquisa refletem disponibilidade em tempo real
- [ ] Logs registram quem fez o que e quando
#### Evidencias
- [ ] Video de navegacao completa do fluxo principal
- [ ] Demonstracao de reservas e pesquisa integradas
- [ ] Log exportado comprovando auditoria de acoes
#### Status: 🔴 Não iniciado

---

### 📅 Semana 7-8: 11/nov - 24/nov (Relatórios + Qualidade + UX)
**Entrega: 24/nov - Domingo**

#### Tarefas
- [ ] **P4**: Aplicar mascaras de input e feedback visual nos formularios principais
- [ ] **P4**: Refinar navegacao e atalhos entre telas
- [ ] **P5**: Relatorio de emprestimos por periodo
- [ ] **P5**: Relatorio de acervo disponivel
- [ ] **P5**: Relatorio de livros mais emprestados
- [ ] **P5**: Relatorio de alunos com emprestimos ativos
- [ ] **P5**: Implementar exportacao PDF
- [ ] **P5**: Implementar exportacao CSV
- [ ] **P5**: Checklist de acessibilidade basica
- [ ] **Todos**: Testes integrados e correcao de bugs
#### Critérios de Aceitação
- [ ] Mínimo 2 relatórios exportáveis (PDF/CSV) com filtros funcionais
- [ ] Checklist de UX atendido (mensagens claras, campos com máscara/placeholder)
- [ ] Testes cobrem cenários críticos

#### Evidências
- [ ] PDFs/CSVs dos relatórios gerados
- [ ] Prints comparativos "antes/depois" das melhorias de UX
- [ ] Relatório de testes com cobertura de cenários

#### Status: 🔴 Não iniciado

---

### 📅 Semana 9: 25/nov - 30/nov (Finalização + Apresentação)
**Entrega: 30/nov - Sábado (FINAL)**

#### Tarefas
- [ ] **P2**: Build do executavel instalavel/portavel
- [ ] **P3**: Revisao final de codigo
- [ ] **P5**: Manual do Usuario (telas e fluxos)
- [ ] **P5**: Manual Tecnico (arquitetura, instalacao, scripts)
- [ ] **P5**: Relatorio Final (objetivos, decisoes, limitacoes, proximos passos)
- [ ] **P5**: Slides de apresentacao com demo guiada
- [ ] **P5**: Video de demonstracao (3-5 minutos)
- [ ] **Todos**: Ensaio da apresentacao (28/nov)
#### Critérios de Aceitação
- [ ] App inicia do zero (instalação simples)
- [ ] App conecta ao MySQL e executa os fluxos-chave
- [ ] Documentos completos e coerentes com implementação

#### Evidências Finais
- [ ] Pacote Release Candidate (RC)
- [ ] PDFs dos manuais (Usuário + Técnico)
- [ ] Relatório Final em PDF
- [ ] Slides da apresentação
- [ ] Vídeo de demonstração (3-5 min)

#### Status: 🔴 Não iniciado

---

## Funcionalidades do Sistema BiblioKopke

### Módulo de Cadastros
- [ ] Cadastro de Livros (título, autor, ISBN, editora, ano, quantidade, localização)
- [ ] Cadastro de Alunos (nome, CPF, matrícula, turma, contato)
- [ ] Cadastro de Funcionários (nome, CPF, cargo, login, senha, perfil)
- [ ] Pesquisa de acervo (por título, autor, ISBN, categoria)

### Módulo de Empréstimos
- [ ] Registrar empréstimo (aluno, livro, data empréstimo, prazo devolução)
- [ ] Registrar devolução (data real, multa se atrasado)
- [ ] Renovar empréstimo
- [ ] Verificar disponibilidade automática (trigger)
- [ ] Histórico de empréstimos por aluno

### Módulo de Reservas
- [ ] Registrar reserva de livro indisponível
- [ ] Notificar quando livro ficar disponível
- [ ] Cancelar reserva
- [ ] Fila de reservas por livro

### Módulo de Relatórios
- [ ] Empréstimos por período (dia/semana/mês)
- [ ] Livros mais emprestados
- [ ] Alunos com empréstimos ativos
- [ ] Alunos com empréstimos atrasados
- [ ] Acervo disponível vs. emprestado
- [ ] Exportação PDF/CSV

### Módulo de Controle de Acesso
- [ ] Login com usuário e senha
- [ ] Perfis: Administrador, Bibliotecário, Operador
- [ ] Logs de auditoria (ações dos usuários)

### Integração Futura
- [ ] Preparação para integração com SIMADE

---

## Pontos Críticos de Atenção

### 🔴 13/out - CRÍTICO
Se conexão com banco de dados não estiver funcionando, **todo o projeto atrasa**

### 🟡 27/out - IMPORTANTE
CRUDs devem estar **100% funcionais** para começar os fluxos operacionais

### 🟡 10/nov - IMPORTANTE
Fluxo completo de empréstimo/devolução deve **funcionar ponta-a-ponta**

### 🟢 24/nov - ATENÇÃO
**Última chance** para ajustes técnicos antes da finalização

### ⚪ 30/nov - APRESENTAÇÃO
Apenas apresentação, **sem desenvolvimento**

---

## Reuniões de Sincronização

### Reuniões Semanais
- **Quando**: Toda segunda-feira às 19h
- **Objetivo**: Status de cada pessoa + impedimentos + próximos passos
- **Duração**: 30-45 minutos

### Reuniões de Validação (Critical Path)
- **13/out**: Validação da fundação (BD + Conexão)
- **27/out**: Validação do core (CRUDs + Regras)
- **10/nov**: Validação dos fluxos operacionais

### Ensaio Final
- **28/nov**: Apresentação completa com todos os membros

---

## Estrutura de Entrega Final

```
BiblioKopke_Release/
├── Executável/
│   ├── BiblioKopke.exe
│   ├── BiblioKopke.dll
│   └── config/
│       └── appsettings.json
├── Database/
│   ├── 01_DDL_Create_Tables.sql
│   ├── 02_DML_Insert_Data.sql
│   ├── 03_Procedures.sql
│   └── 04_Triggers.sql
├── Documentacao/
│   ├── Manual_Usuario.pdf
│   ├── Manual_Tecnico.pdf
│   ├── Relatorio_Final.pdf
│   └── DER_Final.pdf
├── Apresentacao/
│   ├── Slides_Apresentacao.pptx
│   └── Video_Demonstracao.mp4
└── README.md
```