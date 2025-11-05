# ANEXO I - CRONOGRAMA DETALHADO DE DESENVOLVIMENTO

**Projeto:** BiblioKopke - Sistema de Gestão de Biblioteca Escolar
**Período:** 01/outubro/2025 - 30/novembro/2025 (60 dias)
**Metodologia:** Desenvolvimento incremental com entregas quinzenais

---

## Visão Geral do Cronograma

| Etapa | Período | Entrega | Status |
|-------|---------|---------|--------|
| Etapa 1 | 01/out - 13/out | Fundação Crítica | 🔴 Não iniciado |
| Etapa 2 | 14/out - 27/out | Core do Sistema | 🔴 Não iniciado |
| Etapa 3 | 28/out - 10/nov | Fluxos Operacionais | 🔴 Não iniciado |
| Etapa 4 | 11/nov - 24/nov | Relatórios + Qualidade | 🔴 Não iniciado |
| Etapa 5 | 25/nov - 30/nov | Finalização | 🔴 Não iniciado |

---

## ETAPA 1: Fundação Crítica (01/out - 13/out)

### 📅 Data de Entrega: 13/outubro/2025 (domingo)

### Responsáveis e Tarefas

#### Pessoa 1 - Banco de Dados (MySQL)
- [ ] Criar script DDL completo com todas as tabelas
  - Tabela `usuario` (integração SIMADE)
  - Tabela `livro` (acervo completo)
  - Tabela `emprestimo`
  - Tabela `reserva`
  - Tabela `historico_emprestimo`
  - Tabela `log_sistema`
  - Tabela `relatorio`
- [ ] Configurar índices e constraints
- [ ] Inserir dados de teste básicos (mínimo 20 livros, 10 usuários)
- [ ] Validar criação do banco em ambiente local

#### Pessoa 2 - Backend (Camada de Dados)
- [ ] Configurar projeto C# (WinForms ou WPF - decisão da equipe)
- [ ] Estruturar solução em camadas (UI / Domain / Data)
- [ ] Implementar conexão com MySQL (ADO.NET ou Entity Framework)
- [ ] Criar classes de modelo (entidades):
  - Classe `Usuario`
  - Classe `Livro`
  - Classe `Emprestimo`
  - Classe `Reserva`
  - Classe `HistoricoEmprestimo`
  - Classe `LogSistema`
- [ ] Testar conexão e executar SELECT básico

#### Todos
- [ ] Reunião de kickoff (02/out)
- [ ] Revisão final de requisitos e DER
- [ ] Definir padrões de código e convenções
- [ ] Configurar repositório Git com estrutura de branches

### Critérios de Aceitação
✅ Banco de dados sobe do zero (DROP/CREATE) sem erros
✅ Banco populado com dados de teste
✅ Aplicação C# compila sem erros
✅ Aplicação conecta ao banco e lista dados de pelo menos 1 tabela

### Evidências Requeridas
- Script `.sql` versionado no repositório
- Vídeo curto (2-3 min) mostrando:
  - Execução do script no MySQL
  - Aplicação C# conectando e listando dados
- Print da compilação bem-sucedida

### Reunião de Validação
**Data:** 13/outubro/2025 (domingo) às 19h
**Participantes:** Equipe completa + Professor orientador
**Objetivo:** Validar fundação do projeto

---

## ETAPA 2: Core do Sistema (14/out - 27/out)

### 📅 Data de Entrega: 27/outubro/2025 (domingo)

### Responsáveis e Tarefas

#### Pessoa 1 - Banco de Dados
- [ ] Implementar triggers:
  - Trigger para atualização automática de disponibilidade de livros
  - Trigger para inserção automática no histórico após devolução
  - Trigger de auditoria para tabela `usuario`
  - Trigger de auditoria para tabela `livro`
- [ ] Criar procedures:
  - Procedure para realizar empréstimo (com validações)
  - Procedure para realizar devolução (com cálculo de multa)
  - Procedure para renovar empréstimo
  - Procedure para criar reserva
- [ ] Testar todas as procedures e triggers
- [ ] Documentar parâmetros e retornos

#### Pessoa 2 - Backend (Camada de Dados)
- [ ] Implementar CRUD completo de Livros:
  - Método Create (inserir livro)
  - Método Read (buscar por ID, listar todos, buscar por ISBN)
  - Método Update (atualizar dados do livro)
  - Método Delete (remover livro - com validação)
- [ ] Implementar CRUD completo de Usuários (Alunos/Professores):
  - Método Create
  - Método Read (buscar por código SIMADE, listar por tipo)
  - Método Update
  - Método Delete/Inativar
- [ ] Implementar CRUD completo de Funcionários:
  - Método Create (com geração de hash de senha)
  - Método Read
  - Método Update
  - Método Autenticar (login)

#### Pessoa 3 - Backend (Lógica de Negócio)
- [ ] Implementar classe de serviço `EmprestimoService`:
  - Método RealizarEmprestimo (com validações)
  - Método ValidarDisponibilidadeLivro
  - Método ValidarLimiteEmprestimosUsuario
- [ ] Implementar classe de serviço `ReservaService`:
  - Método CriarReserva
  - Método ValidarReserva
  - Método CancelarReserva
- [ ] Implementar validações de negócio:
  - Limite máximo de empréstimos por usuário
  - Prazo de devolução (configurável)
  - Validação de usuário ativo

#### Pessoa 4 - Frontend (Telas Principais)
- [ ] Desenvolver tela de Login:
  - Interface de autenticação
  - Validação de credenciais
  - Tratamento de erros
- [ ] Desenvolver tela de cadastro de Livros:
  - Formulário completo
  - Validação de campos obrigatórios
  - Pesquisa de livros (grid)
  - Edição e exclusão
- [ ] Desenvolver tela de cadastro de Alunos:
  - Formulário de cadastro
  - Listagem em grid
  - Edição de dados

#### Pessoa 5 - Frontend (Telas Complementares)
- [ ] Desenvolver tela de cadastro de Funcionários:
  - Formulário com seleção de perfil
  - Geração de senha
  - Listagem de funcionários
- [ ] Estruturar navegação principal:
  - Menu lateral ou superior
  - Controle de acesso baseado em perfil
  - Dashboard inicial (estrutura básica)
- [ ] Implementar componentes reutilizáveis:
  - Componente de pesquisa
  - Componente de grid padrão
  - Componente de mensagens (sucesso/erro)

### Critérios de Aceitação
✅ Triggers executam automaticamente as regras de negócio
✅ Procedures funcionam corretamente com validações
✅ CRUDs salvam, listam, editam e excluem corretamente
✅ Telas de cadastro são funcionais e intuitivas
✅ Login funciona e restringe acesso

### Evidências Requeridas
- Scripts SQL das procedures e triggers
- Prints da aplicação executando os CRUDs
- Vídeo demonstrando:
  - Login
  - Cadastro de livro
  - Cadastro de usuário
  - Execução de trigger
- Relatório de testes (happy path + casos de erro)

### Reunião de Validação
**Data:** 27/outubro/2025 (domingo) às 19h
**Participantes:** Equipe + Bibliotecário(a) da escola
**Objetivo:** Validar CRUDs e funcionalidades básicas

---

## ETAPA 3: Fluxos Operacionais (28/out - 10/nov)

### 📅 Data de Entrega: 10/novembro/2025 (domingo)

### Responsáveis e Tarefas

#### Pessoa 3 - Backend (Lógica de Negócio)
- [ ] Implementar sistema de devoluções:
  - Método RealizarDevolucao
  - Cálculo automático de dias de atraso
  - Cálculo de multa (se aplicável)
  - Atualização de disponibilidade
- [ ] Implementar sistema de renovações:
  - Método RenovarEmprestimo
  - Validação de limite de renovações
  - Atualização de prazo
- [ ] Implementar sistema de logs:
  - Registrar todas as operações críticas
  - Incluir IP e timestamp
  - Dados antes e depois da alteração
- [ ] Implementar tratamento de exceções:
  - Exceções personalizadas de negócio
  - Log de erros
  - Mensagens amigáveis ao usuário

#### Pessoa 4 - Frontend (Telas Principais)
- [ ] Desenvolver tela de Empréstimo:
  - Busca de usuário (por código SIMADE ou nome)
  - Busca de livro (por ISBN, título ou autor)
  - Verificação de disponibilidade em tempo real
  - Confirmação de empréstimo
  - Impressão de comprovante
- [ ] Desenvolver tela de Devolução:
  - Busca de empréstimo ativo
  - Exibição de dados do empréstimo
  - Cálculo de multa (se houver)
  - Confirmação de devolução
  - Atualização automática de status
- [ ] Desenvolver tela de Pesquisa de Acervo:
  - Filtros múltiplos (título, autor, categoria, ISBN)
  - Exibição de disponibilidade
  - Detalhes do livro
  - Opção de reserva (se indisponível)

#### Pessoa 5 - Frontend (Telas Complementares)
- [ ] Desenvolver tela de Reservas:
  - Formulário de criação de reserva
  - Listagem de reservas ativas
  - Cancelamento de reserva
  - Notificação de disponibilidade
- [ ] Implementar controle de acesso por perfil:
  - Perfil Administrador (acesso total)
  - Perfil Bibliotecário (operações da biblioteca)
  - Perfil Operador (operações básicas)
  - Ocultar/desabilitar funcionalidades conforme perfil
- [ ] Desenvolver Dashboard principal:
  - Indicadores: livros disponíveis, empréstimos ativos, reservas
  - Empréstimos próximos do vencimento
  - Empréstimos atrasados
  - Gráficos básicos

#### Pessoa 6 - Relatórios e Documentação
- [ ] Estruturar módulo de relatórios:
  - Interface base de relatórios
  - Filtros de período
  - Preview antes de exportar
- [ ] Iniciar documentação técnica:
  - Arquitetura da aplicação
  - Fluxo de dados
  - Diagramas de componentes

### Critérios de Aceitação
✅ Fluxo completo de empréstimo funciona ponta-a-ponta
✅ Fluxo completo de devolução funciona com cálculo de multa
✅ Sistema de reservas funciona corretamente
✅ Perfis restringem telas/ações conforme definido
✅ Logs registram todas as operações críticas
✅ Dashboard exibe informações em tempo real

### Evidências Requeridas
- Vídeo completo de navegação (5-7 min):
  - Login como diferentes perfis
  - Realizar empréstimo completo
  - Realizar devolução (normal e com atraso)
  - Criar reserva
  - Visualizar dashboard
- Tabela documentada de perfis e permissões
- Relatório de testes de integração

### Reunião de Validação com a Escola
**Data:** 10/novembro/2025 (domingo) às 14h
**Local:** Biblioteca da Escola Estadual João Kopke
**Participantes:** Equipe + Diretora + Bibliotecário(a)
**Objetivo:** Validar fluxos operacionais com usuários finais
**Formato:** Demonstração prática + coleta de feedback

---

## ETAPA 4: Relatórios, Qualidade e UX (11/nov - 24/nov)

### 📅 Data de Entrega: 24/novembro/2025 (domingo)

### Responsáveis e Tarefas

#### Pessoa 5 - Frontend (Melhorias de UX)
- [ ] Implementar máscaras de input:
  - Máscara de CPF
  - Máscara de telefone
  - Máscara de data
  - Máscara de ISBN
- [ ] Implementar feedback visual:
  - Mensagens de sucesso (toast/snackbar)
  - Mensagens de erro (toast/snackbar)
  - Loading indicators em operações longas
  - Confirmação antes de exclusões
- [ ] Implementar atalhos de teclado:
  - F2: Novo registro
  - F3: Pesquisar
  - F5: Atualizar
  - ESC: Cancelar/Voltar
  - CTRL+S: Salvar
- [ ] Melhorar navegação:
  - Breadcrumbs
  - Navegação por Tab otimizada
  - Foco automático em campos principais
  - Validação em tempo real

#### Pessoa 6 - Relatórios e Documentação
- [ ] Implementar Relatório 1: Empréstimos por Período
  - Filtros: data inicial, data final, tipo de usuário
  - Exibição: tabela com usuário, livro, data, status
  - Totalizadores: total de empréstimos, devoluções, em aberto
  - Exportação: PDF e CSV
- [ ] Implementar Relatório 2: Livros Mais Emprestados
  - Filtros: período, categoria
  - Exibição: ranking com título, autor, quantidade de empréstimos
  - Gráfico de barras
  - Exportação: PDF e CSV
- [ ] Implementar Relatório 3: Situação de Empréstimos
  - Empréstimos ativos
  - Empréstimos atrasados (com cálculo de multa)
  - Empréstimos a vencer (próximos 7 dias)
  - Exportação: PDF e CSV
- [ ] Implementar Relatório 4: Status do Acervo
  - Total de livros cadastrados
  - Livros disponíveis vs. emprestados
  - Livros com reservas
  - Livros por categoria
  - Gráficos: pizza e barras
  - Exportação: PDF e CSV
- [ ] Implementar funcionalidade de exportação:
  - Biblioteca para geração de PDF
  - Exportação CSV
  - Formatação adequada
  - Logotipo da escola no PDF

#### Todos - Testes e Qualidade
- [ ] **Pessoa 1**: Otimizar consultas SQL lentas
- [ ] **Pessoa 2**: Revisar camada de dados (performance)
- [ ] **Pessoa 3**: Testar regras de negócio (casos extremos)
- [ ] **Pessoa 4**: Testar todas as telas (validações)
- [ ] **Pessoa 5**: Checklist de acessibilidade:
  - Contraste de cores adequado
  - Fonte legível (tamanho mínimo)
  - Elementos clicáveis com tamanho adequado
  - Navegação por teclado funcionando
- [ ] **Todos**: Executar testes integrados:
  - Cenários de uso completos
  - Teste de carga (múltiplos empréstimos simultâneos)
  - Teste de concorrência (mesmo livro, múltiplas reservas)
  - Teste de recuperação de erros
- [ ] **Todos**: Correção de bugs identificados
- [ ] **Todos**: Code review mútuo

### Critérios de Aceitação
✅ Mínimo 4 relatórios funcionais e exportáveis
✅ Relatórios possuem filtros e gráficos
✅ Exportação PDF e CSV funciona corretamente
✅ UX melhorada com máscaras e feedback visual
✅ Atalhos de teclado implementados
✅ Checklist de acessibilidade atendido (mínimo 80%)
✅ Todos os bugs críticos corrigidos
✅ Sistema estável e performático

### Evidências Requeridas
- PDFs e CSVs de exemplo dos relatórios gerados
- Prints comparativos "antes/depois" das melhorias de UX
- Relatório de testes com:
  - Casos de teste executados
  - Cobertura de funcionalidades
  - Bugs encontrados e corrigidos
  - Métricas de performance
- Checklist de acessibilidade preenchido
- Vídeo demonstrando:
  - Geração de relatórios
  - Exportações
  - Melhorias de UX

### Reunião de Validação
**Data:** 24/novembro/2025 (domingo) às 19h
**Participantes:** Equipe completa
**Objetivo:** Validar qualidade geral e preparar finalização

---

## ETAPA 5: Finalização e Apresentação (25/nov - 30/nov)

### 📅 Data de Entrega Final: 30/novembro/2025 (sábado)

### Responsáveis e Tarefas

#### Pessoa 1 e 2 - Build e Empacotamento
- [ ] Criar script de instalação do banco:
  - Script único DROP/CREATE
  - Dados iniciais (usuário admin padrão)
  - Instruções de instalação
- [ ] Gerar build de produção da aplicação:
  - Configuração de Release
  - Arquivo de configuração (connection string)
  - Testar em máquina limpa
- [ ] Criar instalador (se possível):
  - Setup.exe ou pacote portável
  - Incluir dependências (.NET Framework/Core)
  - README de instalação

#### Pessoa 3 - Revisão Final de Código
- [ ] Revisar todo o código-fonte:
  - Remover códigos comentados desnecessários
  - Remover logs de debug
  - Verificar tratamento de exceções
  - Padronizar nomenclatura
- [ ] Adicionar comentários XML:
  - Documentar classes públicas
  - Documentar métodos públicos
  - Documentar parâmetros complexos
- [ ] Gerar documentação do código (se aplicável)

#### Pessoa 6 - Documentação Final
- [ ] Criar Manual do Usuário (PDF, 15-25 páginas):
  - Introdução ao sistema
  - Tela de login
  - Como cadastrar livros (com prints)
  - Como cadastrar usuários (com prints)
  - Como realizar empréstimo (com prints)
  - Como realizar devolução (com prints)
  - Como criar reserva (com prints)
  - Como gerar relatórios (com prints)
  - Perguntas frequentes (FAQ)
  - Resolução de problemas comuns
- [ ] Criar Manual Técnico (PDF, 20-30 páginas):
  - Arquitetura do sistema (diagrama de camadas)
  - Tecnologias utilizadas
  - Estrutura do banco de dados (DER)
  - Dicionário de dados
  - Procedures e triggers (documentação)
  - Instalação do ambiente de desenvolvimento
  - Instalação do sistema em produção
  - Backup e restauração do banco
  - Manutenção e troubleshooting
  - Roadmap de melhorias futuras
- [ ] Criar Relatório Final do Projeto (PDF, 10-15 páginas):
  - Resumo executivo
  - Objetivos alcançados
  - Funcionalidades implementadas
  - Tecnologias utilizadas
  - Decisões técnicas tomadas
  - Desafios enfrentados e soluções
  - Limitações conhecidas
  - Melhorias futuras sugeridas
  - Conclusão
  - Referências bibliográficas
- [ ] Organizar todos os diagramas atualizados:
  - DER atualizado
  - Diagramas UML atualizados
  - Diagrama de arquitetura
  - Diagrama de implantação

#### Todos - Apresentação
- [ ] Criar slides de apresentação (PowerPoint/Google Slides):
  - Capa com identificação do projeto
  - Contexto e motivação
  - Objetivos
  - Metodologia
  - Tecnologias utilizadas
  - Arquitetura do sistema
  - Funcionalidades principais (com prints)
  - Demonstração ao vivo (preparar roteiro)
  - Resultados e métricas
  - Desafios e aprendizados
  - Próximos passos
  - Agradecimentos
  - Contatos da equipe
- [ ] Gravar vídeo demonstrativo (3-5 minutos):
  - Apresentação do sistema
  - Login
  - Cadastro de livro
  - Realizar empréstimo
  - Realizar devolução
  - Gerar relatório
  - Conclusão
- [ ] Preparar roteiro de demonstração ao vivo:
  - Banco populado com dados consistentes
  - Cenários de uso preparados
  - Backup de segurança do banco
- [ ] Ensaio geral da apresentação:
  - **Data:** 28/novembro/2025 às 19h
  - Todos os membros apresentam sua parte
  - Cronometrar tempo (máximo 30 minutos)
  - Ajustar conforme necessário

### Critérios de Aceitação
✅ Aplicação inicia do zero em máquina limpa
✅ Instalação é simples e bem documentada
✅ Todos os manuais estão completos e coerentes
✅ Relatório final apresenta visão completa do projeto
✅ Slides de apresentação estão profissionais
✅ Vídeo demonstrativo está claro e objetivo
✅ Equipe está preparada para apresentação

### Estrutura de Entrega Final

```
BiblioKopke_Release_v1.0/
├── 1_Executavel/
│   ├── BiblioKopke.exe
│   ├── BiblioKopke.dll
│   ├── appsettings.json
│   ├── README_Instalacao.txt
│   └── dependencias/
├── 2_Database/
│   ├── 01_DDL_Create_Database.sql
│   ├── 02_DDL_Create_Tables.sql
│   ├── 03_DML_Insert_Initial_Data.sql
│   ├── 04_Procedures.sql
│   ├── 05_Triggers.sql
│   ├── 06_Views.sql
│   └── README_Banco.txt
├── 3_Codigo_Fonte/
│   ├── BiblioKopke.sln
│   ├── src/
│   └── README_Desenvolvimento.txt
├── 4_Documentacao/
│   ├── Manual_Usuario_BiblioKopke.pdf
│   ├── Manual_Tecnico_BiblioKopke.pdf
│   ├── Relatorio_Final_Projeto.pdf
│   ├── DER_Final.pdf
│   └── Diagramas_UML/
│       ├── Casos_de_Uso.pdf
│       ├── Diagrama_Classes.pdf
│       ├── Diagrama_Sequencia.pdf
│       └── Diagrama_Atividades.pdf
├── 5_Apresentacao/
│   ├── Slides_Apresentacao_BiblioKopke.pptx
│   ├── Slides_Apresentacao_BiblioKopke.pdf
│   └── Video_Demonstracao_BiblioKopke.mp4
└── README_PRINCIPAL.txt
```

### Apresentação Final
**Data:** 30/novembro/2025 (sábado)
**Local:** A definir (Universidade ou Escola)
**Horário:** A definir
**Duração:** 30 minutos (20 min apresentação + 10 min perguntas)
**Participantes:**
- Equipe de desenvolvimento (6 pessoas)
- Professor orientador
- Diretora da escola
- Bibliotecário(a)
- Convidados

**Formato:**
1. Introdução do projeto (5 min) - Pessoa responsável
2. Demonstração ao vivo do sistema (10 min) - Revezamento
3. Arquitetura técnica (3 min) - Pessoa 1 e 2
4. Resultados e impactos (2 min) - Pessoa 6
5. Perguntas e respostas (10 min) - Todos

---

## Reuniões Semanais de Acompanhamento

**Periodicidade:** Todas as segundas-feiras às 19h
**Duração:** 30-45 minutos
**Formato:** Online (Google Meet ou similar)

### Agenda Padrão
1. Status de cada membro (5 min cada)
2. Impedimentos e bloqueios (10 min)
3. Decisões técnicas necessárias (10 min)
4. Planejamento da próxima semana (10 min)

### Datas das Reuniões Semanais
- ✅ 02/out - Kickoff
- 📅 07/out - Acompanhamento semana 1
- 📅 14/out - Acompanhamento semana 2 + Início Etapa 2
- 📅 21/out - Acompanhamento semana 3
- 📅 28/out - Acompanhamento semana 4 + Início Etapa 3
- 📅 04/nov - Acompanhamento semana 5
- 📅 11/nov - Acompanhamento semana 6 + Início Etapa 4
- 📅 18/nov - Acompanhamento semana 7
- 📅 25/nov - Acompanhamento semana 8 + Início Etapa 5

---

## Reuniões de Validação (Critical Path)

### Validação 1: Fundação
- **Data:** 13/outubro/2025 às 19h
- **Objetivo:** Validar banco de dados e conexão
- **Critério de Sucesso:** App lista dados do banco

### Validação 2: Core do Sistema
- **Data:** 27/outubro/2025 às 19h
- **Objetivo:** Validar CRUDs e regras de negócio
- **Critério de Sucesso:** Cadastros funcionam completamente
- **Participantes Externos:** Bibliotecário(a) da escola

### Validação 3: Fluxos Operacionais
- **Data:** 10/novembro/2025 às 14h (NA ESCOLA)
- **Objetivo:** Validar fluxos com usuários finais
- **Critério de Sucesso:** Empréstimo e devolução funcionam perfeitamente
- **Participantes Externos:** Diretora + Bibliotecário(a)

### Validação 4: Qualidade e Relatórios
- **Data:** 24/novembro/2025 às 19h
- **Objetivo:** Validar qualidade geral antes da finalização
- **Critério de Sucesso:** Sistema estável e relatórios funcionais

### Ensaio Geral
- **Data:** 28/novembro/2025 às 19h
- **Objetivo:** Ensaiar apresentação final
- **Duração:** 90 minutos

### Apresentação Final
- **Data:** 30/novembro/2025
- **Horário:** A definir
- **Local:** A definir

---

## Pontos Críticos de Atenção (Red Flags)

### 🔴 13/out - CRÍTICO
**Se a conexão com o banco NÃO estiver funcionando:**
- **Impacto:** TODO o projeto atrasa
- **Ação:** Priorizar absolutamente a resolução
- **Responsáveis:** Pessoa 1 e 2 devem trabalhar juntas até resolver

### 🟡 27/out - IMPORTANTE
**Se os CRUDs NÃO estiverem 100% funcionais:**
- **Impacto:** Fluxos operacionais não podem ser implementados
- **Ação:** Não iniciar Etapa 3 até CRUDs estarem completos
- **Responsáveis:** Pessoa 2, 3 e 4 focam em finalizar

### 🟡 10/nov - IMPORTANTE
**Se o fluxo de empréstimo/devolução NÃO funcionar ponta-a-ponta:**
- **Impacto:** Core business do sistema comprometido
- **Ação:** Priorizar empréstimo/devolução sobre outras features
- **Responsáveis:** Pessoa 3 e 4 focam exclusivamente nisso

### 🟢 24/nov - ATENÇÃO
**Última chance para ajustes técnicos:**
- **Impacto:** Após esta data, apenas documentação e apresentação
- **Ação:** Congelar funcionalidades, focar em estabilidade
- **Responsáveis:** Todos revisam e testam

### ⚪ 30/nov - APRESENTAÇÃO
**Sem desenvolvimento nesta data:**
- Apenas apresentação e demonstração
- Sistema deve estar 100% pronto no dia 29/nov

---

## Indicadores de Progresso

| Métrica | Meta | Responsável |
|---------|------|-------------|
| Tabelas criadas | 7/7 | Pessoa 1 |
| Triggers implementados | 4/4 | Pessoa 1 |
| Procedures implementados | 4/4 | Pessoa 1 |
| CRUDs completos | 3/3 | Pessoa 2 |
| Telas principais | 6/6 | Pessoa 4 e 5 |
| Relatórios | 4/4 | Pessoa 6 |
| Bugs críticos | 0 | Todos |
| Cobertura de testes | >80% | Todos |
| Documentação | 100% | Pessoa 6 |

---

## Riscos e Plano de Contingência

| Risco | Probabilidade | Impacto | Mitigação | Contingência |
|-------|---------------|---------|-----------|--------------|
| Atraso na conexão BD | Média | Alto | Validação antecipada | Pair programming P1+P2 |
| Mudança de escopo | Baixa | Alto | Congelar requisitos após kickoff | Priorizar MVP |
| Doença de membro | Média | Médio | Documentar tudo | Redistribuir tarefas |
| Problemas técnicos | Média | Médio | Testes constantes | Ter plano B de tecnologia |
| Falta de tempo | Alta | Alto | Cronograma realista | Reduzir escopo se necessário |

---

## Comunicação da Equipe

### Canais
- **WhatsApp:** Comunicação rápida diária
- **Google Meet:** Reuniões online
- **GitHub:** Versionamento de código e documentação
- **Google Drive:** Documentos compartilhados

### Horários de Disponibilidade
- **Dias de semana:** 19h - 22h
- **Finais de semana:** 14h - 18h
- **Reuniões fixas:** Segundas 19h

### Regras de Comunicação
1. Responder mensagens em até 24h
2. Reportar impedimentos imediatamente
3. Commits no Git com mensagens claras
4. Pull requests com revisão de pelo menos 1 pessoa
5. Documentar todas as decisões técnicas

---

**Documento elaborado em:** 01/outubro/2025
**Última atualização:** 01/outubro/2025
**Próxima revisão:** 07/outubro/2025
