# 📖 MANUAL DO USUÁRIO - BibliotecaJK v3.0

## Sumário
1. [Introdução](#introdução)
2. [Acesso ao Sistema](#acesso-ao-sistema)
3. [Dashboard Principal](#dashboard-principal)
4. [Cadastro de Alunos](#cadastro-de-alunos)
5. [Cadastro de Livros](#cadastro-de-livros)
6. [Empréstimos](#empréstimos)
7. [Devoluções](#devoluções)
8. [Reservas](#reservas)
9. [Relatórios](#relatórios)
10. [Dicas e Boas Práticas](#dicas-e-boas-práticas)

---

## Introdução

O **BibliotecaJK** é um sistema completo de gerenciamento de bibliotecas que permite:
- Controlar empréstimos e devoluções de livros
- Gerenciar cadastro de alunos e acervo
- Sistema de reservas com fila FIFO
- Cálculo automático de multas por atraso
- Geração de relatórios gerenciais

**Desenvolvido por:** Equipe BibliotecaJK (Pessoas 1-5)
**Versão:** 3.0
**Data:** 2025

---

## Acesso ao Sistema

### 1. Iniciando o Sistema

1. Execute o arquivo `BibliotecaJK.exe`
2. A tela de login será exibida

### 2. Fazendo Login

![Tela de Login]

**Campos:**
- **Login:** Digite seu nome de usuário
- **Senha:** Digite sua senha (caracteres ocultos com ●)

**Login Padrão (Administrador):**
```
Login: admin
Senha: admin123
```

**Ações:**
- **Botão Entrar:** Valida credenciais e acessa o sistema
- **Botão Cancelar:** Fecha o sistema
- **Tecla Enter:** Atalho para fazer login

**⚠️ Importante:** O sistema registra todas as tentativas de login (sucesso e falha) nos logs de auditoria.

---

## Dashboard Principal

Após o login bem-sucedido, você verá o **Dashboard** com estatísticas em tempo real:

### Informações Exibidas

**Barra Superior:**
- Nome do funcionário logado
- Perfil (Administrador/Bibliotecário)
- Login utilizado

**Cards de Estatísticas:**

1. **EMPRÉSTIMOS** (Verde)
   - Número de empréstimos ativos
   - Quantidade de empréstimos atrasados

2. **LIVROS** (Azul)
   - Total de livros no acervo
   - Exemplares disponíveis
   - Exemplares emprestados

3. **ALUNOS** (Roxo)
   - Total de alunos cadastrados
   - Alunos com empréstimos ativos
   - Alunos com atrasos

4. **MULTAS** (Vermelho)
   - Valor total acumulado de multas

### Menu Superior

**Cadastros**
- Alunos
- Livros

**Empréstimos**
- Novo Empréstimo
- Devoluções
- Consultar Empréstimos

**Reservas**
- Gerenciar Reservas

**Relatórios**
- Relatórios Gerenciais

**Sair**
- Encerra o sistema

---

## Cadastro de Alunos

### Acessando
`Menu → Cadastros → Alunos`

### Funcionalidades

#### 1. Cadastrar Novo Aluno

**Passo a passo:**
1. Clique no botão **Novo**
2. Preencha os campos obrigatórios:
   - **Nome Completo*** (obrigatório)
   - **CPF*** (obrigatório, com validação)
   - **Matrícula*** (obrigatório, único)
   - Turma (opcional)
   - Telefone (opcional)
   - E-mail (opcional, com validação)
3. Clique em **Salvar**

**Validações Automáticas:**
- ✅ CPF válido (com dígitos verificadores)
- ✅ Matrícula única (não pode duplicar)
- ✅ E-mail em formato válido
- ✅ Nome não pode estar vazio

**Formatação Automática:**
- CPF digitado `12345678901` → exibido `123.456.789-01`

#### 2. Editar Aluno

**Opção 1:** Duplo clique na linha do aluno
**Opção 2:** Selecione o aluno e clique em **Editar**

**Procedimento:**
1. Os dados serão carregados nos campos
2. Faça as alterações necessárias
3. Clique em **Salvar**
4. Ou clique em **Cancelar** para descartar

#### 3. Excluir Aluno

1. Selecione o aluno na lista
2. Clique em **Excluir**
3. Confirme a exclusão

**⚠️ Restrições:**
- Não é possível excluir alunos com empréstimos ativos
- O sistema exibirá mensagem informando o motivo

#### 4. Buscar Aluno

Use a busca em tempo real no campo de busca.

---

## Cadastro de Livros

### Acessando
`Menu → Cadastros → Livros`

### Funcionalidades

#### 1. Cadastrar Novo Livro

**Campos:**
- **Título*** (obrigatório)
- Autor
- ISBN (com validação ISBN-10 ou ISBN-13)
- Editora
- Ano de Publicação
- Categoria
- **Quantidade Total*** (obrigatório, mínimo 1)
- **Quantidade Disponível*** (obrigatório, ≤ Qtd. Total)

**Validações:**
- ✅ ISBN-10 ou ISBN-13 válido
- ✅ Quantidade disponível não pode ser maior que total
- ✅ ISBN único (não permite duplicatas)

#### 2. Editar Livro

Mesmo procedimento do cadastro de alunos.

**Importante:** Ao editar quantidades, considere os exemplares emprestados!

---

## Empréstimos

### Novo Empréstimo

#### Acessando
`Menu → Empréstimos → Novo Empréstimo`

#### Passo a Passo

**1. Selecionar Aluno**
- Use a busca por nome ou matrícula
- Clique no aluno desejado
- O sistema mostrará quantos empréstimos ativos o aluno possui

**2. Selecionar Livro**
- Use a busca por título ou autor
- Clique no livro desejado
- O sistema mostrará quantos exemplares estão disponíveis

**3. Registrar Empréstimo**
- Clique em **Registrar Empréstimo**
- O sistema aplicará as validações

#### Validações Automáticas

O sistema verifica:
1. ✅ **Aluno existe** no cadastro
2. ✅ **Livro existe** no acervo
3. ✅ **Livro disponível** (quantidade > 0)
4. ✅ **Aluno sem atrasos** (não pode ter empréstimos atrasados)
5. ✅ **Limite de empréstimos** (máximo 3 simultâneos por aluno)

#### Regras de Negócio

- **Prazo de devolução:** 7 dias
- **Máximo simultâneo:** 3 livros por aluno
- **Multa por atraso:** R$ 2,00 por dia
- **Renovações:** Máximo 2 vezes (7 dias cada)

**Mensagem de Sucesso:**
```
Empréstimo registrado com sucesso!

Prazo de devolução: 7 dias
Multa por atraso: R$ 2,00/dia
```

---

## Devoluções

### Acessando
`Menu → Empréstimos → Devoluções`

### Funcionalidades

#### 1. Buscar Empréstimo

**Filtros disponíveis:**
- Busca por nome do aluno
- ☑️ Apenas empréstimos atrasados

**Botão Atualizar:** Recarrega a lista

#### 2. Visualizar Detalhes

Ao selecionar um empréstimo, o sistema exibe:
- Data do Empréstimo
- Data Prevista de Devolução
- Dias de Atraso (se houver)
- **Multa Calculada Automaticamente**

**Indicador Visual:**
- 🟢 Verde: No prazo
- 🔴 Vermelho: Atrasado

#### 3. Registrar Devolução

1. Selecione o empréstimo
2. Revise os detalhes (especialmente a multa)
3. Clique em **Registrar Devolução**
4. Confirme a operação

**O que acontece:**
- ✅ Livro volta para o acervo (quantidade disponível +1)
- ✅ Multa é registrada (se houver atraso)
- ✅ Sistema processa fila de reservas automaticamente
- ✅ Ação é registrada nos logs

**Exemplo de Confirmação:**
```
Confirmar devolução?

Data do empréstimo: 01/01/2025
Data prevista: 08/01/2025
Dias de atraso: 5
Multa: R$ 10,00
```

---

## Reservas

### Acessando
`Menu → Reservas → Gerenciar Reservas`

### Sistema de Fila FIFO

O sistema de reservas funciona por **ordem de chegada** (First In, First Out):
- Primeiro a reservar é o primeiro a ser atendido
- Quando um livro é devolvido, a fila é processada automaticamente

### Aba 1: Nova Reserva

#### Como Criar uma Reserva

1. **Selecione o Aluno**
   - Use a busca
   - Clique no aluno

2. **Selecione o Livro**
   - **Importante:** Só aparecem livros **indisponíveis** (quantidade = 0)
   - Use a busca
   - Clique no livro

3. Clique em **Criar Reserva**

#### Validações

- ✅ Aluno não pode ter reserva duplicada para o mesmo livro
- ✅ Só permite reservar livros indisponíveis
- ✅ Reserva criada com status "Ativa"

### Aba 2: Reservas Ativas

Visualize todas as reservas ativas com:
- Nome do aluno
- Livro reservado
- Data e hora da reserva
- **Posição na fila** (1º, 2º, 3º...)

#### Cancelar Reserva

1. Selecione a reserva
2. Clique em **Cancelar Reserva**
3. Confirme

---

## Relatórios

### Acessando
`Menu → Relatórios → Relatórios Gerenciais`

### Tipos de Relatórios

#### 1. 📅 Empréstimos por Período
- Lista empréstimos dos últimos 30 dias
- Mostra: Data, Aluno, Livro, Status, Multa
- **Uso:** Acompanhar movimentação mensal

#### 2. 📚 Livros Mais Emprestados
- Top 20 livros mais populares
- Mostra: Posição, Título, Autor, Total de empréstimos
- **Uso:** Decidir novos exemplares para comprar

#### 3. 👥 Alunos Mais Ativos
- Top 20 alunos que mais pegam livros
- Mostra: Nome, Total, Ativos, Atrasados
- **Uso:** Identificar usuários frequentes

#### 4. 💰 Relatório de Multas
- Todas as multas geradas
- Diferencia: Pendente (não devolvido) vs Paga (devolvido)
- Total acumulado
- **Uso:** Controle financeiro

#### 5. ⚠️ Empréstimos Atrasados
- **Destaque em vermelho**
- Inclui: Telefone do aluno para contato
- Dias de atraso e multa acumulada
- **Uso:** Cobrar devoluções

#### 6. 🔖 Relatório de Reservas
- Todas as reservas ativas
- Dias de espera
- **Uso:** Gerenciar expectativas

#### 7. 📊 Estatísticas Gerais
- Resumo completo do sistema
- Todas as métricas em uma visão
- **Uso:** Relatório gerencial

### Exportar Relatórios

1. Gere o relatório desejado
2. Clique em **💾 Exportar para CSV**
3. Escolha o local e nome do arquivo
4. Formatos: `.csv` ou `.txt`

**O arquivo incluirá:**
- Todos os dados da tabela
- Data/hora de geração
- Nome do usuário que gerou
- Identificação do sistema

**Como abrir:**
- Excel/LibreOffice Calc: Arquivo → Abrir (use separador `;`)
- Bloco de Notas: Para visualização rápida

---

## Dicas e Boas Práticas

### Para Bibliotecários

#### Rotina Diária
1. ✅ Verificar **Dashboard** ao iniciar o dia
2. ✅ Consultar **Empréstimos Atrasados**
3. ✅ Entrar em contato com alunos em atraso
4. ✅ Processar devoluções assim que ocorrem
5. ✅ Verificar **Reservas Ativas** após devoluções

#### Rotina Semanal
1. ✅ Gerar **Relatório de Empréstimos** da semana
2. ✅ Revisar **Livros Mais Emprestados**
3. ✅ Verificar se há multas pendentes

#### Rotina Mensal
1. ✅ Gerar **todos os relatórios** para backup
2. ✅ Analisar **Estatísticas Gerais**
3. ✅ Avaliar necessidade de novos exemplares
4. ✅ Identificar alunos frequentes para reconhecimento

### Evitando Problemas

#### ❌ NÃO FAZER:
- Não alterar quantidade de livros sem conferir empréstimos
- Não excluir alunos com empréstimos ativos (sistema bloqueia)
- Não ignorar empréstimos atrasados (multa aumenta R$ 2/dia)

#### ✅ FAZER:
- Sempre atualizar o dashboard (botão 🔄 Atualizar)
- Usar as buscas para encontrar registros rapidamente
- Verificar validações em vermelho antes de salvar
- Exportar relatórios regularmente para backup

### Atalhos e Produtividade

- **Enter no campo Senha:** Faz login automaticamente
- **Duplo clique na lista:** Abre edição
- **Busca em tempo real:** Digite enquanto a lista filtra
- **Tecla Esc:** Fecha formulários (equivalente a Fechar)

### Mensagens do Sistema

#### Cores dos Avisos
- 🟢 **Sucesso (Verde):** Operação realizada
- 🟡 **Atenção (Amarelo):** Validação ou alerta
- 🔴 **Erro (Vermelho):** Falha na operação

#### Mensagens Comuns

**"Login ou senha incorretos"**
→ Verifique suas credenciais

**"Aluno possui empréstimos atrasados"**
→ Regularize devoluções antes de novo empréstimo

**"Aluno já possui 3 empréstimos ativos"**
→ Limite atingido, solicite devolução

**"Livro indisponível"**
→ Todos os exemplares emprestados, ofereça reserva

**"Não foi possível conectar ao banco de dados"**
→ Contate o administrador do sistema

---

## Suporte Técnico

### Problemas Comuns

**O sistema não abre**
→ Verifique se o MySQL está rodando

**Erro ao salvar**
→ Confira as validações em vermelho

**Relatório vazio**
→ Não há dados para o filtro selecionado

**Tela travada**
→ Aguarde ou feche e abra novamente

### Contato

Para suporte técnico:
- 📧 E-mail: suporte@bibliokopke.com
- 📞 Telefone: (XX) XXXX-XXXX
- 🌐 Sistema: BibliotecaJK v3.0

---

## Apêndices

### Glossário

- **Acervo:** Conjunto de todos os livros da biblioteca
- **Exemplar:** Cópia física de um livro (um livro pode ter vários exemplares)
- **FIFO:** First In, First Out (primeiro a entrar, primeiro a sair)
- **Multa:** Valor cobrado por devolução atrasada (R$ 2,00/dia)
- **Reserva Ativa:** Reserva aguardando disponibilidade do livro
- **Dashboard:** Painel inicial com estatísticas

### Regras de Negócio (Resumo)

| Regra | Valor |
|-------|-------|
| Prazo de devolução | 7 dias |
| Máximo de empréstimos simultâneos | 3 por aluno |
| Máximo de renovações | 2 vezes |
| Prazo de cada renovação | 7 dias |
| Multa por atraso | R$ 2,00/dia |
| Sistema de reservas | FIFO (fila) |

---

**Desenvolvido por:**
Pessoa 1: Banco de Dados
Pessoa 2: Camada DAL
Pessoa 3: Camada BLL
Pessoa 4: Interface WinForms
Pessoa 5: Relatórios e Documentação

**BibliotecaJK v3.0** - Sistema Completo de Gerenciamento de Bibliotecas
© 2025 - Todos os direitos reservados
