# TERMO DE ACEITE DE PROJETO EXTENSIONISTA
## SEGUNDO SEMESTRE - DESENVOLVIMENTO E IMPLEMENTAÇÃO

**Escola Estadual João Kopke**
**Projeto:** BiblioKopke - Sistema de Gestão de Biblioteca Escolar
**Curso:** Sistemas de Informação - UEMG Frutal
**Disciplinas:** Projeto Interdisciplinar IV, Banco de Dados II, Programação II

---

## DECLARAÇÃO

Declaro, para os devidos fins, que a **Escola Estadual João Kopke** aceita dar continuidade à parceria no desenvolvimento do projeto extensionista intitulado **"BiblioKopke - Sistema de Gestão de Biblioteca Escolar"**, proposto por alunos do curso de Sistemas de Informação da Universidade do Estado de Minas Gerais - Unidade Frutal.

---

## CONTEXTO DO PROJETO

Este projeto dá continuidade ao trabalho iniciado no primeiro semestre (Projeto Interdisciplinar III), no qual foram realizados:

### Importante: Escopo Acadêmico vs. Projeto Extensionista Completo

É importante esclarecer que existem dois prazos distintos para este projeto:

**1. Projeto Acadêmico (Projeto Interdisciplinar IV - Este Semestre):**
- **Prazo:** 30/novembro/2025
- **Objetivo:** Desenvolver um MVP (Produto Mínimo Viável) funcional para fins de avaliação acadêmica
- **Escopo:** Implementação das funcionalidades essenciais do sistema
- **Entrega:** Protótipo funcional com documentação completa

**2. Projeto Extensionista Completo:**
- **Prazo:** Conforme acordado no primeiro semestre, **início do ano letivo de 2027**
- **Objetivo:** Sistema completo implantado e em produção na escola
- **Escopo:** Todas as funcionalidades, integração real com SIMADE, treinamento completo
- **Entrega:** Sistema em produção, com suporte e manutenção

O trabalho deste semestre é uma **etapa importante** do projeto extensionista maior, fornecendo uma base sólida que será expandida e refinada até a entrega final em 2027.

### Entregas do Primeiro Semestre (Concluídas)

✅ **1. Levantamento de Requisitos**
- Carta de apresentação e contato estabelecido com a direção da escola
- Termo de aceite formal assinado em 16/06/2025
- Identificação das necessidades do sistema de biblioteca

✅ **2. Histórias de Usuário**
- Documentação completa das funcionalidades para Alunos, Professores e Bibliotecários
- Definição de perfis de acesso e permissões
- Fluxos de uso documentados

✅ **3. Modelagem de Dados**
- Diagrama Entidade-Relacionamento (DER) completo
- Modelagem de dados normalizada (3FN)
- Estrutura preparada para integração com SIMADE
- Dicionário de dados completo

✅ **4. Modelagem UML**
- Diagrama de Casos de Uso
- Diagrama de Classes
- Diagrama de Sequência (fluxo de empréstimo)
- Diagrama de Atividades (fluxo de empréstimo)

✅ **5. Scripts SQL Iniciais**
- Script DDL completo (criação de tabelas, índices, constraints)
- Scripts DML básicos (inserção, atualização, exclusão)
- Triggers e procedures iniciais
- Views para consultas gerenciais

---

## OBJETIVO DO SEGUNDO SEMESTRE (MVP)

Nesta etapa, o foco será a **implementação prática de um MVP (Produto Mínimo Viável) em aplicação desktop C#**, integrada a um **banco de dados MySQL**, transformando toda a modelagem realizada no primeiro semestre em um **protótipo funcional** que demonstre as principais funcionalidades do sistema e sirva como base para o desenvolvimento completo até 2027.

---

## FUNCIONALIDADES A SEREM IMPLEMENTADAS

O sistema BiblioKopke será desenvolvido como uma aplicação desktop (Windows Forms ou WPF) com as seguintes funcionalidades:

### 📚 Módulo de Cadastros
- Cadastro de livros (título, autor, ISBN, editora, localização no acervo)
- Cadastro de alunos e professores (integrado ao SIMADE)
- Cadastro de funcionários/bibliotecários
- Pesquisa avançada de acervo

### 📖 Módulo de Empréstimos
- Registro de empréstimos com prazo de devolução
- Registro de devoluções (normais e atrasadas)
- Renovação de empréstimos
- Atualização automática de disponibilidade (trigger)
- Histórico completo de empréstimos por usuário

### 🔖 Módulo de Reservas
- Sistema de reservas para livros indisponíveis
- Notificação quando livro ficar disponível
- Fila de reservas por livro
- Cancelamento de reservas

### 📊 Módulo de Relatórios Gerenciais
- Empréstimos por período (diário, semanal, mensal)
- Livros mais emprestados
- Alunos com empréstimos ativos
- Alunos com empréstimos atrasados
- Acervo disponível vs. emprestado
- Exportação em PDF/CSV

### 🔐 Módulo de Controle de Acesso
- Login com usuário e senha
- Perfis de acesso: Administrador, Bibliotecário, Operador
- Logs de auditoria (registro de todas as ações)

### 🔄 Integração com SIMADE
- Utilização do código SIMADE como chave primária dos usuários
- Preparação para integração futura com base de dados do SIMADE

---

## CRONOGRAMA DE DESENVOLVIMENTO

O desenvolvimento será realizado entre **01/outubro/2025** e **30/novembro/2025**, com entregas quinzenais:

### 📅 Etapa 1: 01/out - 13/out (Fundação)
- Construção do banco de dados físico no MySQL
- Configuração da aplicação C# com conexão ao banco
- Implementação das classes de modelo

### 📅 Etapa 2: 14/out - 27/out (Core do Sistema)
- Implementação de procedures e triggers
- Desenvolvimento dos CRUDs principais (Livros, Usuários)
- Telas de cadastro funcionais

### 📅 Etapa 3: 28/out - 10/nov (Fluxos Operacionais)
- Implementação completa de empréstimos e devoluções
- Sistema de reservas
- Controle de acesso por perfil

### 📅 Etapa 4: 11/nov - 24/nov (Relatórios e Qualidade)
- Desenvolvimento dos relatórios gerenciais
- Melhorias de UX/UI
- Testes integrados e correções

### 📅 Etapa 5: 25/nov - 30/nov (Finalização)
- Documentação final (Manuais de Usuário e Técnico)
- Preparação da apresentação
- Entrega do executável instalável

---

## ENTREGÁVEIS FINAIS

Ao término do projeto, serão entregues à escola:

1. **Aplicação Desktop BiblioKopke**
   - Executável instalável para Windows
   - Código-fonte completo

2. **Banco de Dados MySQL**
   - Scripts SQL completos (DDL, DML, Procedures, Triggers)
   - Dados de teste pré-carregados
   - Backup inicial do banco

3. **Documentação Completa**
   - Manual do Usuário (com guias ilustrados)
   - Manual Técnico (arquitetura, instalação, manutenção)
   - Relatório Final do Projeto
   - Diagramas atualizados (DER, UML)

4. **Material de Apresentação**
   - Slides da apresentação final
   - Vídeo demonstrativo (3-5 minutos)

---

## COMPROMISSOS DA EQUIPE DESENVOLVEDORA

A equipe de desenvolvimento se compromete a:

- Manter comunicação constante com a direção e bibliotecários da escola
- Validar as funcionalidades periodicamente com os usuários finais
- Realizar ajustes conforme feedback recebido
- Entregar o sistema funcional dentro do prazo estabelecido
- Fornecer treinamento básico para os usuários do sistema
- Disponibilizar toda a documentação técnica necessária

---

## COMPROMISSOS DA ESCOLA

A escola se compromete a:

- Disponibilizar acesso aos bibliotecários para validação das funcionalidades
- Fornecer feedback sobre as entregas parciais
- Autorizar testes do sistema com dados reais (quando aplicável)
- Participar da apresentação final do projeto
- Avaliar a possibilidade de implantação efetiva do sistema

---

## VALIDAÇÃO E TESTES

Durante o desenvolvimento, serão realizadas:

- **Reuniões quinzenais** de validação com a escola (13/out, 27/out, 10/nov, 24/nov)
- **Testes de aceitação** com bibliotecários e usuários finais
- **Ajustes e melhorias** conforme necessidades identificadas
- **Apresentação final** com demonstração completa do sistema (30/nov)

---

## CONSIDERAÇÕES FINAIS

Este projeto representa a consolidação prática de todo o trabalho de análise e modelagem realizado no primeiro semestre. A aplicação desktop BiblioKopke será desenvolvida com foco na **usabilidade**, **segurança** e **eficiência**, atendendo às necessidades reais da biblioteca escolar.

Estamos cientes de que o sistema encontra-se em fase de desenvolvimento e que todas as funcionalidades serão validadas em conjunto com a escola para garantir sua adequação às necessidades operacionais.

A equipe está comprometida em entregar um **produto de qualidade** que possa efetivamente auxiliar na gestão da biblioteca e melhorar a experiência de alunos, professores e bibliotecários.

---

## ASSINATURAS

**Local e Data:** Frutal, _____ de outubro de 2025.

---

**Diretora Maria Auxiliadora Mendonça**
Escola Estadual João Kopke
(Assinatura digital)

---

**Equipe de Desenvolvimento**
Curso de Sistemas de Informação - UEMG Frutal

---

**Professor Orientador**
Universidade do Estado de Minas Gerais - Unidade Frutal

---

## ANEXOS

Este termo é acompanhado de documentação técnica completa e detalhada, totalizando **mais de 140 páginas** de especificações, diagramas e planejamento:

### 📋 ANEXO I: Cronograma Detalhado de Desenvolvimento (35+ páginas)
**Arquivo:** [anexos/ANEXO_I_Cronograma_Detalhado.md](anexos/ANEXO_I_Cronograma_Detalhado.md)

**Conteúdo completo:**
- Cronograma detalhado das 5 etapas (01/out - 30/nov)
- Divisão de trabalho por pessoa (6 desenvolvedores)
- Tarefas específicas para cada semana
- Critérios de aceitação para cada etapa
- Evidências requeridas (scripts, vídeos, prints)
- 9 reuniões de validação agendadas
- Pontos críticos de atenção (red flags)
- Indicadores de progresso (métricas)
- Análise de riscos e plano de contingência
- Estrutura de comunicação da equipe

**Destaques:**
- 5 etapas incrementais bem definidas
- 20+ tarefas detalhadas por pessoa
- Estrutura de entrega final completa
- Calendário de reuniões semanais

---

### 🗄️ ANEXO II: Diagrama Entidade-Relacionamento - DER Detalhado (20+ páginas)
**Arquivo:** [anexos/ANEXO_II_DER_Detalhado.md](anexos/ANEXO_II_DER_Detalhado.md)

**Conteúdo completo:**
- **Diagrama UML do banco de dados** (visual completo)
- **8 entidades detalhadas:**
  - USUARIO (20 campos) - Integração SIMADE
  - LIVRO (17 campos) - Acervo completo
  - EMPRESTIMO (10 campos) - Controle de empréstimos
  - RESERVA (8 campos) - Sistema de reservas
  - HISTORICO_EMPRESTIMO (10 campos) - Auditoria
  - LOG_SISTEMA (8 campos) - Rastreabilidade
  - RELATORIO (7 campos) - Relatórios gerados
  - RECOMENDACAO (6 campos) - Recomendações de professores
- **Relacionamentos completos** com multiplicidades
- **6 triggers implementados:**
  - Atualização automática de disponibilidade
  - Auditoria de usuários e livros
  - Inserção automática em histórico
- **4 procedures SQL:**
  - sp_realizar_emprestimo (com validações)
  - sp_realizar_devolucao (com cálculo de multa)
  - sp_renovar_emprestimo (com limite)
  - sp_criar_reserva (com fila)
- **5 views úteis** para consultas gerenciais
- **Índices e chaves estrangeiras** para performance
- **Regras de negócio** detalhadas do banco
- **Observações técnicas:** integridade, segurança, escalabilidade

**Imagem:** [04_diagramas/diagrama_uml_banco.png](../04_diagramas/diagrama_uml_banco.png)

---

### 📐 ANEXO III: Diagramas UML Completos (25+ páginas)
**Arquivo:** [anexos/ANEXO_III_Diagramas_UML.md](anexos/ANEXO_III_Diagramas_UML.md)

**Conteúdo completo:**

#### 1. Diagrama de Casos de Uso
- **28 casos de uso identificados**
- Funcionalidades por ator:
  - **Aluno:** 7 casos de uso
  - **Professor:** 6 casos de uso
  - **Bibliotecário:** 12 casos de uso
  - **Sistema SIMADE:** 3 casos de uso (automáticos)
- Relacionamentos extends e includes
- Imagem: [diagrama_casos_de_uso.png](../04_diagramas/diagrama_casos_de_uso.png)

#### 2. Diagrama de Classes
- **7 classes principais** completamente especificadas
- **8 enumerações (enums)** para tipos
- Atributos detalhados (80+ atributos no total)
- Métodos a serem implementados em C#
- Relacionamentos e multiplicidades
- Imagem: [diagrama_classes.png](../04_diagramas/diagrama_classes.png)

#### 3. Diagrama de Sequência - Fluxo de Empréstimo
- Interações completas entre 4 participantes
- Fluxo principal (cenário de sucesso)
- Fluxo alternativo (livro indisponível)
- Fluxo de exceção (validações)
- 10+ mensagens sequenciais
- Imagem: [diagrama_sequencia_emprestimo.png](../04_diagramas/diagrama_sequencia_emprestimo.png)

#### 4. Diagrama de Atividades - Fluxo de Empréstimo
- Fluxo de trabalho completo
- Pontos de decisão (disponibilidade, validações)
- Atividades automáticas vs. manuais
- Tratamento de erros
- Imagem: [diagrama_atividades_emprestimo.png](../04_diagramas/diagrama_atividades_emprestimo.png)

**Ferramentas:** Mermaid (código + imagens PNG de alta qualidade)

---

### 📝 ANEXO IV: Histórias de Usuário Detalhadas (30+ páginas)
**Arquivo:** [anexos/ANEXO_IV_Historias_Usuario.md](anexos/ANEXO_IV_Historias_Usuario.md)

**Conteúdo completo:**

#### 25 Histórias de Usuário Completas

**Aluno (7 histórias):**
- HU-A01: Visualizar Catálogo de Livros 🔴 MUST
- HU-A02: Pesquisar Livros 🔴 MUST
- HU-A03: Reservar Livro 🟡 SHOULD
- HU-A04: Acompanhar Empréstimos 🔴 MUST
- HU-A05: Renovar Empréstimo 🟡 SHOULD
- HU-A06: Consultar Histórico 🟢 COULD
- HU-A07: Cancelar Reserva 🟢 COULD

**Professor (6 histórias):**
- HU-P01: Visualizar Catálogo 🔴 MUST
- HU-P02: Solicitar Reserva para Aulas 🟡 SHOULD
- HU-P03: Ver Histórico 🟡 SHOULD
- HU-P04: Sugerir Aquisição 🟢 COULD
- HU-P05: Acessar Relatórios Básicos 🟢 COULD
- HU-P06: Recomendar Livros aos Alunos 🟢 COULD

**Bibliotecário (12 histórias):**
- HU-B01: Cadastrar Livro 🔴 MUST
- HU-B02: Editar Livro 🔴 MUST
- HU-B03: Remover Livro 🟡 SHOULD
- HU-B04: Registrar Empréstimo 🔴 MUST
- HU-B05: Registrar Devolução 🔴 MUST
- HU-B06: Gerenciar Reservas 🟡 SHOULD
- HU-B07: Gerar Relatórios Gerenciais 🔴 MUST
- HU-B08: Consultar Usuários 🟡 SHOULD
- HU-B09: Aplicar Multas 🟢 COULD
- HU-B10: Configurar Sistema 🟢 COULD
- HU-B11: Fazer Backup 🟢 COULD
- HU-B12: Importar Dados do SIMADE 🟢 COULD

**Para cada história:**
- Descrição no formato padrão (Como... Quero... Para...)
- Priorização MoSCoW (MUST/SHOULD/COULD/WON'T)
- 5-8 critérios de aceitação detalhados
- Tarefas técnicas de implementação
- Estimativas em Story Points

**Extras incluídos:**
- Definition of Done (DoD) completo
- Priorização: 9 MUST / 7 SHOULD / 9 COULD
- Tabela de estimativas (Story Points)
- Sugestão de distribuição por Sprint

**Referência:** Baseado em [historia de usuario.pdf](../03_requisitos/historia%20de%20usuario.pdf) (1º semestre) e expandido

---

### 📄 ANEXO V: Termo de Aceite do Primeiro Semestre (Referência)
**Arquivo:** [termo de aceite assinado.pdf](termo%20de%20aceite%20assinado.pdf)

**Conteúdo:**
- Aceite formal da Escola Estadual João Kopke
- Data: 16/junho/2025
- Assinatura digital da Diretora Maria Auxiliadora Mendonça
- Autorização para desenvolvimento do projeto
- Funcionalidades planejadas (versão inicial)

**Status:** ✅ Concluído no primeiro semestre

---

### 📚 Índice Geral dos Anexos
**Arquivo:** [anexos/README_ANEXOS.md](anexos/README_ANEXOS.md)

Documento de navegação completo com:
- Sumário de todos os anexos
- Referências cruzadas
- Estrutura de arquivos do projeto
- Guia de uso da documentação
- Checklist de documentação

---

## RESUMO DA DOCUMENTAÇÃO

**Total de páginas técnicas:** 140+ páginas
**Total de histórias de usuário:** 25 histórias completas
**Total de entidades no banco:** 8 entidades
**Total de diagramas UML:** 4 diagramas principais
**Total de procedures SQL:** 4 procedures
**Total de triggers SQL:** 6 triggers
**Total de views SQL:** 5 views
**Período de desenvolvimento:** 60 dias (01/out - 30/nov)

Toda a documentação está **completa, detalhada e pronta para implementação**, baseada nos entregáveis validados do primeiro semestre e expandida para atender às necessidades do desenvolvimento do segundo semestre.

---

**Projeto:** BiblioKopke - Sistema de Gestão de Biblioteca Escolar
**Período de Desenvolvimento:** 01/out/2025 - 30/nov/2025
**Apresentação Final:** 30/novembro/2025
**Documentação Técnica:** 140+ páginas de especificações completas
