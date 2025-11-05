# ANEXOS DO TERMO DE ACEITE - SEGUNDO SEMESTRE

**Projeto:** BiblioKopke - Sistema de Gestão de Biblioteca Escolar
**Período:** 01/outubro/2025 - 30/novembro/2025

---

## Índice de Anexos

Este documento lista todos os anexos referenciados no **Termo de Aceite do Segundo Semestre** do projeto BiblioKopke.

---

## ANEXO I - Cronograma Detalhado de Desenvolvimento

**Arquivo:** [ANEXO_I_Cronograma_Detalhado.md](ANEXO_I_Cronograma_Detalhado.md)

**Conteúdo:**
- Cronograma completo das 5 etapas de desenvolvimento (01/out - 30/nov)
- Divisão de tarefas por pessoa e por semana
- Critérios de aceitação para cada etapa
- Evidências requeridas
- Reuniões de validação e datas críticas
- Pontos de atenção (red flags)
- Indicadores de progresso
- Riscos e plano de contingência

**Páginas:** 35+ páginas

**Destaques:**
- Etapa 1 (01/out - 13/out): Fundação Crítica
- Etapa 2 (14/out - 27/out): Core do Sistema
- Etapa 3 (28/out - 10/nov): Fluxos Operacionais
- Etapa 4 (11/nov - 24/nov): Relatórios + Qualidade
- Etapa 5 (25/nov - 30/nov): Finalização

---

## ANEXO II - Diagrama Entidade-Relacionamento (DER) Detalhado

**Arquivo:** [ANEXO_II_DER_Detalhado.md](ANEXO_II_DER_Detalhado.md)

**Conteúdo:**
- Diagrama UML do banco de dados completo
- Descrição detalhada de todas as 8 entidades:
  - USUARIO
  - LIVRO
  - EMPRESTIMO
  - RESERVA
  - HISTORICO_EMPRESTIMO
  - LOG_SISTEMA
  - RELATORIO
  - RECOMENDACAO
- Relacionamentos e multiplicidades
- Índices e chaves estrangeiras
- Triggers implementados (6 triggers)
- Procedures implementadas (4 procedures)
- Views úteis (5 views)
- Regras de negócio do banco
- Observações técnicas (integridade, performance, segurança)

**Páginas:** 20+ páginas

**Imagem do DER:** [diagrama_uml_banco.png](../../04_diagramas/diagrama_uml_banco.png)

---

## ANEXO III - Diagramas UML

**Arquivo:** [ANEXO_III_Diagramas_UML.md](ANEXO_III_Diagramas_UML.md)

**Conteúdo:**

### 1. Diagrama de Casos de Uso
- Funcionalidades por ator (Aluno, Professor, Bibliotecário, Sistema SIMADE)
- 28 casos de uso identificados
- Relacionamentos (extends, includes)
- **Imagem:** [diagrama_casos_de_uso.png](../../04_diagramas/diagrama_casos_de_uso.png)

### 2. Diagrama de Classes
- 7 classes principais
- Atributos detalhados
- Métodos a serem implementados
- 8 enumerações (enums)
- Relacionamentos e multiplicidades
- **Imagem:** [diagrama_classes.png](../../04_diagramas/diagrama_classes.png)

### 3. Diagrama de Sequência - Empréstimo
- Fluxo completo de empréstimo
- Interações entre Usuário, Bibliotecário, Sistema e Banco
- Fluxo principal, alternativo e de exceção
- **Imagem:** [diagrama_sequencia_emprestimo.png](../../04_diagramas/diagrama_sequencia_emprestimo.png)

### 4. Diagrama de Atividades - Empréstimo
- Fluxo de trabalho do empréstimo
- Pontos de decisão
- Atividades automáticas vs. manuais
- **Imagem:** [diagrama_atividades_emprestimo.png](../../04_diagramas/diagrama_atividades_emprestimo.png)

**Páginas:** 25+ páginas

**Ferramentas:** Mermaid (código + imagens PNG)

---

## ANEXO IV - Histórias de Usuário

**Arquivo:** [ANEXO_IV_Historias_Usuario.md](ANEXO_IV_Historias_Usuario.md)

**Conteúdo:**

### Histórias por Ator

#### Aluno (7 histórias)
- HU-A01: Visualizar Catálogo de Livros 🔴 MUST
- HU-A02: Pesquisar Livros 🔴 MUST
- HU-A03: Reservar Livro 🟡 SHOULD
- HU-A04: Acompanhar Empréstimos 🔴 MUST
- HU-A05: Renovar Empréstimo 🟡 SHOULD
- HU-A06: Consultar Histórico 🟢 COULD
- HU-A07: Cancelar Reserva 🟢 COULD

#### Professor (6 histórias)
- HU-P01: Visualizar Catálogo 🔴 MUST
- HU-P02: Solicitar Reserva para Aulas 🟡 SHOULD
- HU-P03: Ver Histórico 🟡 SHOULD
- HU-P04: Sugerir Aquisição 🟢 COULD
- HU-P05: Acessar Relatórios Básicos 🟢 COULD
- HU-P06: Recomendar Livros 🟢 COULD

#### Bibliotecário (12 histórias)
- HU-B01: Cadastrar Livro 🔴 MUST
- HU-B02: Editar Livro 🔴 MUST
- HU-B03: Remover Livro 🟡 SHOULD
- HU-B04: Registrar Empréstimo 🔴 MUST
- HU-B05: Registrar Devolução 🔴 MUST
- HU-B06: Gerenciar Reservas 🟡 SHOULD
- HU-B07: Gerar Relatórios 🔴 MUST
- HU-B08: Consultar Usuários 🟡 SHOULD
- HU-B09: Aplicar Multas 🟢 COULD
- HU-B10: Configurar Sistema 🟢 COULD
- HU-B11: Fazer Backup 🟢 COULD
- HU-B12: Importar SIMADE 🟢 COULD

### Priorização MoSCoW
- 🔴 MUST: 9 histórias (essenciais para MVP)
- 🟡 SHOULD: 7 histórias (importantes)
- 🟢 COULD: 9 histórias (desejáveis)
- ⚪ WON'T: 4 funcionalidades (versão futura)

### Extras
- Critérios de aceitação detalhados para cada história
- Definition of Done (DoD)
- Estimativas em Story Points
- Sugestão de distribuição por Sprint

**Páginas:** 30+ páginas

**Total de histórias:** 25 histórias de usuário

**Referência:** [historia de usuario.pdf](../../03_requisitos/historia%20de%20usuario.pdf) (documento original do 1º semestre)

---

## ANEXO V - Termo de Aceite do Primeiro Semestre (Referência)

**Arquivo:** [../termo de aceite assinado.pdf](../termo%20de%20aceite%20assinado.pdf)

**Data:** 16/junho/2025

**Conteúdo:**
- Aceite formal da Escola Estadual João Kopke
- Autorização para desenvolvimento do projeto
- Funcionalidades planejadas (versão web)
- Assinatura da Diretora Maria Auxiliadora Mendonça

**Status:** ✅ Concluído no primeiro semestre

**Observação:** Este termo serviu de base para o desenvolvimento do Termo de Aceite do Segundo Semestre, que foca na implementação prática (aplicação desktop C# + MySQL) ao invés da versão web inicialmente planejada.

---

## Estrutura de Arquivos

```
01_planejamento/
├── termo_aceite_segundo_semestre.md (Documento principal)
├── anexos/
│   ├── README_ANEXOS.md (Este arquivo)
│   ├── ANEXO_I_Cronograma_Detalhado.md
│   ├── ANEXO_II_DER_Detalhado.md
│   ├── ANEXO_III_Diagramas_UML.md
│   └── ANEXO_IV_Historias_Usuario.md
└── termo de aceite assinado.pdf (1º semestre)
```

---

## Referências Cruzadas

### Diagramas Originais
Todos os diagramas referenciados nos anexos estão localizados em:
- **Pasta:** `04_diagramas/`
- **Formatos:** `.md` (Markdown), `.mmd` (Mermaid), `.png` (Imagem)

### Scripts SQL
Scripts de banco de dados referenciados estão em:
- **Pasta:** `02_modelagem_banco/`
- **Arquivos principais:**
  - `banco_de_dados.sql` (DDL + DML completo)
  - `exemplos_consultas.sql` (Queries de exemplo)
  - `documentacao_banco.md` (Documentação técnica)

### Documentação de Requisitos
Documentos de requisitos originais estão em:
- **Pasta:** `03_requisitos/`
- **Arquivo:** `historia de usuario.pdf`

---

## Como Usar Este Material

### Para a Escola
1. Leia o [Termo de Aceite Principal](../termo_aceite_segundo_semestre.md)
2. Consulte os anexos para entender detalhes técnicos
3. Acompanhe o cronograma (Anexo I) para as validações quinzenais

### Para a Equipe de Desenvolvimento
1. Use o **Anexo I** como guia de trabalho semanal
2. Consulte **Anexo II** durante implementação do banco
3. Siga **Anexo III** para arquitetura e fluxos
4. Implemente **Anexo IV** história por história

### Para Apresentações
1. **Anexo III** (Diagramas UML) é ideal para explicar arquitetura
2. **Anexo II** (DER) mostra estrutura de dados
3. **Anexo IV** (Histórias) demonstra funcionalidades do ponto de vista do usuário

---

## Informações de Contato

### Escola Estadual João Kopke
- **Diretora:** Maria Auxiliadora Mendonça
- **Endereço:** [Endereço da escola]
- **Telefone:** [Telefone]
- **E-mail:** [E-mail]

### Equipe de Desenvolvimento
- **Instituição:** UEMG - Unidade Frutal
- **Curso:** Sistemas de Informação
- **Disciplinas:** Projeto Interdisciplinar IV, Banco de Dados II, Programação II

### Professor Orientador
- **Nome:** [Nome do professor]
- **E-mail:** [E-mail]

---

## Controle de Versão

| Versão | Data | Autor | Descrição |
|--------|------|-------|-----------|
| 1.0 | 01/out/2025 | Equipe BiblioKopke | Criação inicial dos anexos |

---

## Checklist de Documentação

### Anexos Criados
- [x] ANEXO I - Cronograma Detalhado
- [x] ANEXO II - DER Detalhado
- [x] ANEXO III - Diagramas UML
- [x] ANEXO IV - Histórias de Usuário
- [x] README_ANEXOS (Este arquivo)

### Documentos Relacionados
- [x] Termo de Aceite do Segundo Semestre
- [x] Termo de Aceite do Primeiro Semestre (referência)
- [x] Diagramas UML (imagens PNG)
- [x] Scripts SQL completos
- [x] Documentação do banco de dados

### Pendências
- [ ] Converter termo de aceite para PDF (após assinatura)
- [ ] Imprimir anexos para apresentação física
- [ ] Enviar cópia digital para a escola

---

## Observações Finais

Todos os anexos foram elaborados com base no trabalho realizado no primeiro semestre (Projeto Interdisciplinar III) e expandidos para atender às necessidades de implementação do segundo semestre (Projeto Interdisciplinar IV).

A documentação está completa e pronta para ser apresentada à escola para assinatura do termo de aceite.

**Data de elaboração:** 01/outubro/2025
**Próxima revisão:** 07/outubro/2025

---

**Projeto BiblioKopke - Sistema de Gestão de Biblioteca Escolar**
**UEMG Frutal - Sistemas de Informação - 2025**
