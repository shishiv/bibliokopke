# ANEXO IV - HISTÓRIAS DE USUÁRIO

**Projeto:** BiblioKopke - Sistema de Gestão de Biblioteca Escolar
**Metodologia:** User Stories (Histórias de Usuário)
**Framework:** Scrum / Agile

---

## Índice

1. [Introdução](#introdução)
2. [Histórias de Usuário - Aluno](#histórias-de-usuário---aluno)
3. [Histórias de Usuário - Professor](#histórias-de-usuário---professor)
4. [Histórias de Usuário - Bibliotecário](#histórias-de-usuário---bibliotecário)
5. [Priorização (MoSCoW)](#priorização-moscow)
6. [Critérios de Aceitação Detalhados](#critérios-de-aceitação-detalhados)

---

## Introdução

As histórias de usuário foram elaboradas seguindo o formato padrão:

**"Como [tipo de usuário], quero [realizar ação], para [alcançar objetivo]."**

Cada história foi validada com potenciais usuários finais (alunos, professores e bibliotecários) da Escola Estadual João Kopke durante a fase de levantamento de requisitos no primeiro semestre.

---

## Histórias de Usuário - Aluno

### 📖 HU-A01: Visualizar Catálogo de Livros

**Como** aluno,
**Quero** visualizar o catálogo de livros online,
**Para** escolher o que desejo ler.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] O sistema exibe lista de todos os livros do acervo
- [ ] Cada livro mostra: título, autor, categoria, disponibilidade
- [ ] É possível visualizar detalhes do livro (sinopse, editora, ano, localização)
- [ ] A interface é clara e de fácil navegação
- [ ] Livros indisponíveis são visualmente diferenciados
- [ ] É possível ver quantos exemplares estão disponíveis

**Tarefas Técnicas:**
- Tela de catálogo (ListView/DataGrid)
- Consulta SQL para listar livros
- Tela de detalhes do livro
- Lógica de disponibilidade

---

### 🔍 HU-A02: Pesquisar Livros

**Como** aluno,
**Quero** pesquisar livros por título, autor, categoria ou ISBN,
**Para** encontrar rapidamente o que procuro.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] Campo de pesquisa aceita texto livre
- [ ] Pesquisa busca em: título, autor, categoria, ISBN
- [ ] Resultados aparecem enquanto digito (busca incremental)
- [ ] Resultados são filtrados em tempo real
- [ ] Sistema informa quando não há resultados
- [ ] É possível limpar a pesquisa facilmente

**Tarefas Técnicas:**
- Campo de busca com autocomplete
- Query SQL com LIKE / FULLTEXT
- Filtros dinâmicos
- Performance otimizada (índices)

---

### 🔖 HU-A03: Reservar Livro

**Como** aluno,
**Quero** reservar um livro quando está indisponível,
**Para** garantir meu empréstimo quando ele for devolvido.

**Prioridade:** 🟡 SHOULD (Importante)

**Critérios de Aceitação:**
- [ ] Só é possível reservar livros indisponíveis
- [ ] Sistema informa posição na fila de reservas
- [ ] Aluno pode ter no máximo 2 reservas ativas
- [ ] Reserva tem validade de 3 dias após disponibilidade
- [ ] Sistema notifica quando livro fica disponível (via e-mail se configurado)
- [ ] É possível cancelar reserva

**Tarefas Técnicas:**
- Tela de criação de reserva
- Procedure sp_criar_reserva
- Sistema de fila (FIFO)
- Notificações (opcional)
- Expiração automática (trigger ou job)

---

### 📚 HU-A04: Acompanhar Empréstimos

**Como** aluno,
**Quero** acompanhar meus empréstimos ativos e devoluções,
**Para** evitar atrasos e multas.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] Exibe lista de empréstimos ativos do aluno
- [ ] Mostra data de empréstimo e data de devolução prevista
- [ ] Destaca empréstimos próximos do vencimento (48h)
- [ ] Destaca empréstimos atrasados em vermelho
- [ ] Mostra valor de multa para empréstimos atrasados
- [ ] É possível ver histórico de empréstimos passados

**Tarefas Técnicas:**
- Tela "Meus Empréstimos"
- Query de empréstimos por usuário
- Cálculo de dias restantes
- Indicadores visuais (cores)
- Acesso ao histórico

---

### 🔄 HU-A05: Renovar Empréstimo

**Como** aluno,
**Quero** renovar meu empréstimo online,
**Para** ter mais tempo para ler o livro.

**Prioridade:** 🟡 SHOULD (Importante)

**Critérios de Aceitação:**
- [ ] Aluno pode renovar até 2 vezes
- [ ] Não pode renovar se estiver atrasado
- [ ] Não pode renovar se houver reserva para o livro
- [ ] Renovação estende prazo em +7 dias
- [ ] Sistema confirma renovação com nova data
- [ ] Histórico registra número de renovações

**Tarefas Técnicas:**
- Botão "Renovar" na tela de empréstimos
- Procedure sp_renovar_emprestimo
- Validações de regras de negócio
- Atualização de prazo
- Log de operação

---

### 📜 HU-A06: Consultar Histórico

**Como** aluno,
**Quero** ver o histórico completo dos meus empréstimos,
**Para** saber quais livros já li e quando.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Lista todos os empréstimos passados
- [ ] Mostra: livro, data empréstimo, data devolução
- [ ] Indica se houve atraso e multa
- [ ] É possível filtrar por período
- [ ] É possível filtrar por categoria
- [ ] Ordenação por data (mais recente primeiro)

**Tarefas Técnicas:**
- Tela de histórico
- Query em historico_emprestimo
- Filtros de período e categoria
- Paginação (se muitos registros)

---

### ❌ HU-A07: Cancelar Reserva

**Como** aluno,
**Quero** cancelar uma reserva que fiz,
**Para** liberar a vaga para outro aluno quando não quiser mais o livro.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Só pode cancelar reservas com status ATIVA
- [ ] Sistema confirma cancelamento
- [ ] Próximo da fila é notificado automaticamente
- [ ] Histórico registra cancelamento
- [ ] É possível informar motivo (opcional)

**Tarefas Técnicas:**
- Botão "Cancelar" na lista de reservas
- Atualização de status
- Notificação do próximo da fila
- Log de operação

---

## Histórias de Usuário - Professor

### 📖 HU-P01: Visualizar Catálogo

**Como** professor,
**Quero** visualizar o catálogo de livros,
**Para** recomendar leituras aos meus alunos.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] Mesmos critérios da HU-A01
- [ ] Possibilidade de filtrar por faixa etária/série
- [ ] Identificação de livros didáticos

---

### 📚 HU-P02: Solicitar Reserva para Aulas

**Como** professor,
**Quero** solicitar reserva de múltiplos exemplares do mesmo livro,
**Para** usar em atividades em sala de aula.

**Prioridade:** 🟡 SHOULD (Importante)

**Critérios de Aceitação:**
- [ ] Pode reservar múltiplos exemplares simultaneamente
- [ ] Sistema verifica disponibilidade total
- [ ] Prazo de reserva é maior (até 14 dias)
- [ ] Bibliotecário precisa aprovar a reserva
- [ ] Professor pode especificar data/período necessário

**Tarefas Técnicas:**
- Tela específica para reserva de múltiplos exemplares
- Workflow de aprovação
- Validação de quantidade disponível
- Notificação ao bibliotecário

---

### 📊 HU-P03: Ver Histórico de Empréstimos

**Como** professor,
**Quero** ver o histórico dos meus empréstimos,
**Para** fins de controle pessoal e planejamento de aulas.

**Prioridade:** 🟡 SHOULD (Importante)

**Critérios de Aceitação:**
- [ ] Mesmos critérios da HU-A06
- [ ] Limite maior de empréstimos simultâneos (5)
- [ ] Prazo de devolução pode ser maior

---

### 💡 HU-P04: Sugerir Aquisição de Livros

**Como** professor,
**Quero** sugerir livros para o acervo,
**Para** enriquecer o material disponível aos alunos.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Formulário com: título, autor, editora, justificativa
- [ ] Sugestões são enviadas ao bibliotecário
- [ ] Professor pode acompanhar status da sugestão
- [ ] Bibliotecário pode aprovar/rejeitar com feedback

**Tarefas Técnicas:**
- Tela de sugestão
- Tabela sugestoes_livro (nova)
- Workflow de aprovação
- Notificações

---

### 📈 HU-P05: Acessar Relatórios Básicos

**Como** professor,
**Quero** ver estatísticas de leitura dos meus alunos,
**Para** acompanhar o engajamento com leitura.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Relatório de empréstimos por aluno (da turma)
- [ ] Ranking de livros mais lidos pela turma
- [ ] Filtro por período e turma
- [ ] Exportação em PDF

**Tarefas Técnicas:**
- Tela de relatório específica
- Queries agregadas
- Exportação PDF
- Filtros de turma

---

### 📝 HU-P06: Recomendar Livros aos Alunos

**Como** professor,
**Quero** recomendar livros específicos para alunos,
**Para** incentivar leituras direcionadas.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Professor seleciona livro e aluno(s)
- [ ] Aluno vê recomendação em seu painel
- [ ] Professor pode adicionar comentário
- [ ] Aluno pode marcar como "lido"

**Tarefas Técnicas:**
- Tabela recomendacao (já existe no banco)
- Tela de recomendação
- Painel do aluno com recomendações
- Notificações

---

## Histórias de Usuário - Bibliotecário

### 📚 HU-B01: Cadastrar Livro

**Como** bibliotecário,
**Quero** cadastrar livros com título, autor, editora, ISBN, categoria e localização,
**Para** manter o acervo atualizado e organizado.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] Formulário com todos os campos necessários
- [ ] Validação de ISBN (formato e unicidade)
- [ ] Campos obrigatórios: título, autor, quantidade
- [ ] Possibilidade de adicionar múltiplos exemplares
- [ ] Upload de imagem da capa (opcional)
- [ ] Sistema gera código de localização automático
- [ ] Confirmação visual de cadastro bem-sucedido

**Tarefas Técnicas:**
- Tela de cadastro de livro
- Validações de formulário
- INSERT no banco
- Upload de imagem (se implementado)
- Trigger de log

---

### ✏️ HU-B02: Editar Informações do Livro

**Como** bibliotecário,
**Quero** editar as informações de um livro cadastrado,
**Para** corrigir erros ou atualizar dados.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] Busca de livro por título, autor ou ISBN
- [ ] Todos os campos são editáveis (exceto ID)
- [ ] Sistema mantém log de alterações
- [ ] Confirmação antes de salvar
- [ ] Validações mantidas (ISBN único, etc.)

**Tarefas Técnicas:**
- Tela de edição (pode ser mesma do cadastro)
- UPDATE no banco
- Trigger de auditoria

---

### 🗑️ HU-B03: Remover Livro do Acervo

**Como** bibliotecário,
**Quero** remover ou inativar um livro do acervo,
**Para** manter apenas livros em boas condições.

**Prioridade:** 🟡 SHOULD (Importante)

**Critérios de Aceitação:**
- [ ] Não pode remover livro com empréstimo ativo
- [ ] Não pode remover livro com reserva ativa
- [ ] Sistema pede confirmação (ação irreversível)
- [ ] Opção de "Inativar" ao invés de excluir
- [ ] Motivo da remoção é registrado
- [ ] Histórico é preservado

**Tarefas Técnicas:**
- Botão "Remover/Inativar"
- Validações de empréstimos/reservas ativas
- Soft delete (inativar) ou hard delete
- Log da operação

---

### ➕ HU-B04: Registrar Empréstimo

**Como** bibliotecário,
**Quero** registrar empréstimos de forma prática,
**Para** controlar a saída de livros da biblioteca.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] Busca rápida de usuário (código SIMADE, nome ou CPF)
- [ ] Busca rápida de livro (ISBN, título ou código)
- [ ] Validações automáticas:
  - Livro disponível
  - Usuário ativo
  - Limite de empréstimos não excedido
  - Sem empréstimos atrasados
- [ ] Define prazo automaticamente (7 dias aluno, configurável)
- [ ] Gera comprovante imprimível
- [ ] Atualização automática de disponibilidade

**Tarefas Técnicas:**
- Tela de empréstimo (wizard ou formulário)
- Campos com autocomplete
- Procedure sp_realizar_emprestimo
- Validações em tempo real
- Impressão de comprovante

---

### ✅ HU-B05: Registrar Devolução

**Como** bibliotecário,
**Quero** registrar devoluções de forma prática,
**Para** controlar o retorno de livros e cobrar multas quando necessário.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] Busca de empréstimo por código, livro ou usuário
- [ ] Sistema calcula automaticamente:
  - Dias de atraso (se houver)
  - Valor da multa (R$ 1,00/dia - configurável)
- [ ] Exibe informações do empréstimo claramente
- [ ] Confirmação da devolução
- [ ] Registro automático no histórico
- [ ] Atualização automática de disponibilidade
- [ ] Gera comprovante de devolução

**Tarefas Técnicas:**
- Tela de devolução
- Procedure sp_realizar_devolucao
- Cálculo de multa
- Trigger para histórico
- Impressão de comprovante

---

### 🎫 HU-B06: Gerenciar Reservas

**Como** bibliotecário,
**Quero** gerenciar todas as reservas do sistema,
**Para** atender os usuários quando livros ficarem disponíveis.

**Prioridade:** 🟡 SHOULD (Importante)

**Critérios de Aceitação:**
- [ ] Lista de todas as reservas ativas
- [ ] Ordenação por data (mais antigas primeiro)
- [ ] Filtros: status, livro, usuário
- [ ] Ações: atender, cancelar, estender prazo
- [ ] Notificação automática ao usuário
- [ ] Controle de expiração de reservas

**Tarefas Técnicas:**
- Tela de gerenciamento de reservas
- Grid com filtros
- Botões de ação
- Notificações
- Job/trigger de expiração

---

### 📊 HU-B07: Gerar Relatórios Gerenciais

**Como** bibliotecário,
**Quero** gerar relatórios de empréstimos, livros mais lidos e situação do acervo,
**Para** fins administrativos e tomada de decisão.

**Prioridade:** 🔴 MUST (Essencial)

**Critérios de Aceitação:**
- [ ] **Relatório 1:** Empréstimos por período
  - Filtros: data inicial, data final, tipo de usuário
  - Totalizadores
  - Exportação PDF/CSV
- [ ] **Relatório 2:** Livros mais emprestados
  - Ranking com gráfico
  - Filtro de período
  - Exportação PDF/CSV
- [ ] **Relatório 3:** Situação atual
  - Empréstimos ativos
  - Empréstimos atrasados
  - Multas a receber
  - Exportação PDF/CSV
- [ ] **Relatório 4:** Status do acervo
  - Total de livros
  - Disponíveis vs. emprestados
  - Livros por categoria
  - Gráficos
  - Exportação PDF/CSV

**Tarefas Técnicas:**
- Módulo de relatórios
- Queries agregadas
- Biblioteca de geração de PDF
- Exportação CSV
- Gráficos (Chart control)

---

### 👥 HU-B08: Consultar Usuários

**Como** bibliotecário,
**Quero** consultar informações dos usuários (alunos, professores),
**Para** verificar situação de empréstimos e contato.

**Prioridade:** 🟡 SHOULD (Importante)

**Critérios de Aceitação:**
- [ ] Busca por código SIMADE, nome ou CPF
- [ ] Exibe: dados pessoais, empréstimos ativos, histórico
- [ ] Indica se tem empréstimos atrasados
- [ ] Mostra total de multas pendentes
- [ ] Possibilidade de editar contato (e-mail, telefone)

**Tarefas Técnicas:**
- Tela de consulta de usuário
- Query de informações completas
- Indicadores visuais

---

### 💰 HU-B09: Aplicar Multas

**Como** bibliotecário,
**Quero** registrar pagamento de multas,
**Para** controlar recebimentos.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Lista de multas pendentes
- [ ] Registra forma de pagamento
- [ ] Atualiza status do empréstimo
- [ ] Gera recibo
- [ ] Histórico de pagamentos

**Tarefas Técnicas:**
- Tela de multas
- Tabela pagamentos_multa (nova)
- Atualização de status
- Impressão de recibo

---

### ⚙️ HU-B10: Configurar Sistema

**Como** bibliotecário (administrador),
**Quero** configurar parâmetros do sistema,
**Para** adaptar às regras da biblioteca.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Configurações disponíveis:
  - Prazo padrão de empréstimo
  - Limite de empréstimos por tipo de usuário
  - Valor da multa por dia
  - Limite de renovações
  - Validade de reserva
- [ ] Apenas administrador tem acesso
- [ ] Log de alterações de configuração

**Tarefas Técnicas:**
- Tela de configurações
- Tabela configuracoes (nova)
- Controle de acesso
- Log de alterações

---

### 💾 HU-B11: Fazer Backup

**Como** bibliotecário (administrador),
**Quero** fazer backup do banco de dados,
**Para** garantir segurança dos dados.

**Prioridade:** 🟢 COULD (Desejável)

**Critérios de Aceitação:**
- [ ] Botão para backup manual
- [ ] Backup automático semanal (opcional)
- [ ] Local de armazenamento configurável
- [ ] Histórico de backups realizados
- [ ] Possibilidade de restaurar

**Tarefas Técnicas:**
- Comando mysqldump via C#
- Agendamento (se automático)
- Interface de backup/restore

---

### 📥 HU-B12: Importar Dados do SIMADE

**Como** bibliotecário,
**Quero** importar dados de alunos e professores do SIMADE,
**Para** manter cadastro atualizado automaticamente.

**Prioridade:** 🟢 COULD (Desejável - Futura)

**Critérios de Aceitação:**
- [ ] Importação via arquivo CSV/Excel
- [ ] Validação de formato
- [ ] Não duplica registros existentes
- [ ] Atualiza dados desatualizados
- [ ] Relatório de importação (sucesso/erros)

**Tarefas Técnicas:**
- Parser de CSV/Excel
- Validações de dados
- Upsert (insert or update)
- Log de importação

---

## Priorização (MoSCoW)

### 🔴 MUST (Deve ter - Essencial para MVP)
- HU-A01: Visualizar Catálogo
- HU-A02: Pesquisar Livros
- HU-A04: Acompanhar Empréstimos
- HU-P01: Visualizar Catálogo
- HU-B01: Cadastrar Livro
- HU-B02: Editar Livro
- HU-B04: Registrar Empréstimo
- HU-B05: Registrar Devolução
- HU-B07: Gerar Relatórios

**Total MUST: 9 histórias**

### 🟡 SHOULD (Deveria ter - Importante)
- HU-A03: Reservar Livro
- HU-A05: Renovar Empréstimo
- HU-P02: Solicitar Reserva para Aulas
- HU-P03: Ver Histórico
- HU-B03: Remover Livro
- HU-B06: Gerenciar Reservas
- HU-B08: Consultar Usuários

**Total SHOULD: 7 histórias**

### 🟢 COULD (Poderia ter - Desejável)
- HU-A06: Consultar Histórico
- HU-A07: Cancelar Reserva
- HU-P04: Sugerir Aquisição
- HU-P05: Acessar Relatórios Básicos
- HU-P06: Recomendar Livros
- HU-B09: Aplicar Multas
- HU-B10: Configurar Sistema
- HU-B11: Fazer Backup
- HU-B12: Importar SIMADE

**Total COULD: 9 histórias**

### ⚪ WON'T (Não terá - Versão futura)
- Integração automática com SIMADE via API
- Notificações via SMS
- Aplicativo mobile
- Leitura de código de barras/QR Code

---

## Critérios de Aceitação Detalhados

### Definição de Pronto (Definition of Done)

Uma história de usuário é considerada "pronta" quando:

✅ **Desenvolvimento:**
- [ ] Código implementado conforme critérios de aceitação
- [ ] Código revisado por pelo menos 1 membro da equipe
- [ ] Padrões de código seguidos
- [ ] Sem warnings ou erros de compilação

✅ **Testes:**
- [ ] Testes unitários escritos e passando (se aplicável)
- [ ] Testes de integração com banco passando
- [ ] Teste manual realizado (happy path + casos de erro)
- [ ] Validações de formulário testadas

✅ **Banco de Dados:**
- [ ] Scripts SQL versionados
- [ ] Triggers/procedures testados
- [ ] Índices criados para performance

✅ **Documentação:**
- [ ] Comentários no código (onde necessário)
- [ ] Manual do usuário atualizado (se aplicável)
- [ ] Prints de tela capturados

✅ **UX:**
- [ ] Interface intuitiva e consistente
- [ ] Feedback visual adequado (sucesso/erro)
- [ ] Navegação por teclado funciona
- [ ] Mensagens de erro são claras

✅ **Demonstração:**
- [ ] Funcionalidade demonstrável para o cliente
- [ ] Aceite do cliente (bibliotecário/diretor)

---

## Estimativas (Story Points)

Usando Fibonacci: 1, 2, 3, 5, 8, 13

### Estimativas por História

| ID | História | Story Points | Sprint Sugerido |
|----|----------|--------------|-----------------|
| HU-A01 | Visualizar Catálogo | 3 | 1 |
| HU-A02 | Pesquisar Livros | 3 | 1 |
| HU-A03 | Reservar Livro | 5 | 2 |
| HU-A04 | Acompanhar Empréstimos | 3 | 2 |
| HU-A05 | Renovar Empréstimo | 3 | 3 |
| HU-A06 | Consultar Histórico | 2 | 3 |
| HU-A07 | Cancelar Reserva | 2 | 3 |
| HU-P02 | Reserva para Aulas | 5 | 4 |
| HU-P04 | Sugerir Aquisição | 3 | 5 |
| HU-P06 | Recomendar Livros | 3 | 5 |
| HU-B01 | Cadastrar Livro | 5 | 1 |
| HU-B02 | Editar Livro | 3 | 1 |
| HU-B03 | Remover Livro | 3 | 2 |
| HU-B04 | Registrar Empréstimo | 8 | 2 |
| HU-B05 | Registrar Devolução | 8 | 3 |
| HU-B06 | Gerenciar Reservas | 5 | 3 |
| HU-B07 | Gerar Relatórios | 13 | 4 |
| HU-B08 | Consultar Usuários | 3 | 2 |
| HU-B09 | Aplicar Multas | 5 | 5 |
| HU-B10 | Configurar Sistema | 5 | 5 |

**Total estimado:** ~90 story points

---

## Referência ao Documento Original

**Arquivo PDF:** [historia de usuario.pdf](../../03_requisitos/historia%20de%20usuario.pdf)

---

**Documento elaborado em:** 01/outubro/2025
**Última atualização:** 01/outubro/2025
**Versão:** 2.0 (expandida do original)
