# ✅ PLANO DE TESTES - BibliotecaJK v3.0

## Sumário
1. [Tipos de Testes](#tipos-de-testes)
2. [Casos de Teste por Módulo](#casos-de-teste-por-módulo)
3. [Cenários de Teste Integrados](#cenários-de-teste-integrados)
4. [Checklist de Validação](#checklist-de-validação)

---

## Tipos de Testes

### 1. Testes Funcionais
- Validar funcionalidades do sistema
- Verificar regras de negócio
- Confirmar cálculos (multas, prazos)

### 2. Testes de Interface
- Validar campos obrigatórios
- Testar navegação entre telas
- Verificar mensagens de erro/sucesso

### 3. Testes de Integração
- Banco de dados
- Fluxo completo (cadastro → empréstimo → devolução)

---

## Casos de Teste por Módulo

### 1. LOGIN (FormLogin)

#### TC-001: Login com credenciais válidas
**Pré-condição:** Banco de dados configurado
**Passos:**
1. Iniciar aplicação
2. Digitar `admin` no campo Login
3. Digitar `admin123` no campo Senha
4. Clicar em "Entrar"

**Resultado esperado:** ✅ Dashboard carrega com sucesso

#### TC-002: Login com senha incorreta
**Passos:**
1. Digitar `admin` no campo Login
2. Digitar `senhaerrada` no campo Senha
3. Clicar em "Entrar"

**Resultado esperado:** ❌ "Login ou senha incorretos"

#### TC-003: Campo vazio
**Passos:**
1. Deixar Login vazio
2. Clicar em "Entrar"

**Resultado esperado:** ❌ "Por favor, informe o login"

---

### 2. CADASTRO DE ALUNOS (FormCadastroAluno)

#### TC-010: Cadastrar aluno válido
**Passos:**
1. Menu → Cadastros → Alunos
2. Clicar em "Novo"
3. Preencher:
   - Nome: João Silva
   - CPF: 123.456.789-01
   - Matrícula: MAT2025001
4. Clicar em "Salvar"

**Resultado esperado:** ✅ "Aluno cadastrado com sucesso"

#### TC-011: Validar CPF inválido
**Passos:**
1. Preencher CPF: 111.111.111-11
2. Salvar

**Resultado esperado:** ❌ "CPF inválido"

#### TC-012: Matrícula duplicada
**Pré-condição:** Aluno com matrícula MAT001 já existe
**Passos:**
1. Tentar cadastrar novo aluno com matrícula MAT001
2. Salvar

**Resultado esperado:** ❌ "Matrícula já cadastrada"

#### TC-013: Editar aluno
**Passos:**
1. Duplo clique em um aluno
2. Alterar telefone
3. Salvar

**Resultado esperado:** ✅ "Aluno atualizado com sucesso"

#### TC-014: Excluir aluno sem empréstimos
**Passos:**
1. Selecionar aluno sem empréstimos
2. Clicar em "Excluir"
3. Confirmar

**Resultado esperado:** ✅ Aluno excluído

#### TC-015: Tentar excluir aluno com empréstimo ativo
**Pré-condição:** Aluno possui empréstimo ativo
**Passos:**
1. Selecionar aluno
2. Tentar excluir

**Resultado esperado:** ❌ "Aluno possui empréstimos ativos"

---

### 3. CADASTRO DE LIVROS (FormCadastroLivro)

#### TC-020: Cadastrar livro válido
**Passos:**
1. Menu → Cadastros → Livros
2. Preencher:
   - Título: Dom Casmurro
   - Autor: Machado de Assis
   - ISBN: 978-85-359-0277-1
   - Quantidade Total: 5
   - Quantidade Disponível: 5
3. Salvar

**Resultado esperado:** ✅ Livro cadastrado

#### TC-021: Validar ISBN inválido
**Passos:**
1. Preencher ISBN: 123-456
2. Salvar

**Resultado esperado:** ❌ "ISBN inválido"

#### TC-022: ISBN duplicado
**Pré-condição:** Livro com ISBN 978-85-359-0277-1 já existe
**Passos:**
1. Tentar cadastrar com mesmo ISBN

**Resultado esperado:** ❌ "ISBN já cadastrado"

---

### 4. EMPRÉSTIMOS (FormEmprestimo)

#### TC-030: Registrar empréstimo válido
**Pré-condição:**
- Aluno cadastrado sem empréstimos
- Livro disponível
**Passos:**
1. Menu → Empréstimos → Novo Empréstimo
2. Selecionar aluno
3. Selecionar livro
4. Clicar em "Registrar Empréstimo"

**Resultado esperado:**
✅ "Empréstimo registrado com sucesso"
✅ Prazo de devolução: 7 dias
✅ Quantidade disponível do livro -1

#### TC-031: Tentar emprestar livro indisponível
**Pré-condição:** Livro com quantidade disponível = 0
**Passos:**
1. Tentar registrar empréstimo

**Resultado esperado:** ❌ "Livro indisponível"

#### TC-032: Aluno com empréstimo atrasado
**Pré-condição:** Aluno possui empréstimo atrasado
**Passos:**
1. Tentar novo empréstimo

**Resultado esperado:** ❌ "Aluno possui empréstimos atrasados"

#### TC-033: Limite de 3 empréstimos simultâneos
**Pré-condição:** Aluno já possui 3 empréstimos ativos
**Passos:**
1. Tentar 4º empréstimo

**Resultado esperado:** ❌ "Limite de 3 empréstimos simultâneos atingido"

---

### 5. DEVOLUÇÕES (FormDevolucao)

#### TC-040: Devolução no prazo (sem multa)
**Pré-condição:** Empréstimo ativo, dentro do prazo
**Passos:**
1. Menu → Empréstimos → Devoluções
2. Selecionar empréstimo
3. Verificar detalhes (Dias de Atraso: 0)
4. Registrar devolução

**Resultado esperado:**
✅ Devolução registrada
✅ Multa: R$ 0,00
✅ Quantidade disponível do livro +1

#### TC-041: Devolução com atraso (com multa)
**Pré-condição:** Empréstimo atrasado 5 dias
**Passos:**
1. Selecionar empréstimo atrasado
2. Verificar: Dias de Atraso: 5, Multa: R$ 10,00
3. Registrar devolução

**Resultado esperado:**
✅ Devolução registrada
✅ Multa: R$ 10,00 (5 dias × R$ 2,00)

#### TC-042: Filtro "Apenas atrasados"
**Passos:**
1. Marcar checkbox "Apenas empréstimos atrasados"
2. Verificar lista

**Resultado esperado:** ✅ Apenas empréstimos atrasados aparecem (em vermelho)

---

### 6. RESERVAS (FormReserva)

#### TC-050: Criar reserva válida
**Pré-condição:** Livro indisponível (qtd = 0)
**Passos:**
1. Menu → Reservas → Gerenciar Reservas
2. Aba "Nova Reserva"
3. Selecionar aluno
4. Selecionar livro indisponível
5. Criar Reserva

**Resultado esperado:**
✅ Reserva criada
✅ Aparece na aba "Reservas Ativas"
✅ Posição na fila: 1º (se for o primeiro)

#### TC-051: Tentar reservar livro disponível
**Pré-condição:** Livro com qtd disponível > 0
**Passos:**
1. Tentar criar reserva

**Resultado esperado:** ❌ "Apenas livros indisponíveis podem ser reservados"

#### TC-052: Reserva duplicada
**Pré-condição:** Aluno já possui reserva para o livro
**Passos:**
1. Tentar criar segunda reserva

**Resultado esperado:** ❌ "Aluno já possui reserva para este livro"

#### TC-053: Sistema FIFO (Fila)
**Pré-condição:**
- Livro indisponível
- 3 reservas criadas nesta ordem: João, Maria, Pedro
**Passos:**
1. Registrar devolução do livro

**Resultado esperado:**
✅ Sistema notifica João (primeiro da fila)
✅ Reserva de João marcada como "Concluída"

#### TC-054: Cancelar reserva
**Passos:**
1. Aba "Reservas Ativas"
2. Selecionar reserva
3. Cancelar

**Resultado esperado:** ✅ Reserva removida da lista

---

### 7. RELATÓRIOS (FormRelatorios)

#### TC-060: Gerar relatório "Empréstimos por Período"
**Passos:**
1. Menu → Relatórios → Relatórios Gerenciais
2. Clicar em "📅 Empréstimos por Período"

**Resultado esperado:**
✅ Lista empréstimos dos últimos 30 dias
✅ Mostra: Data, Aluno, Livro, Status, Multa

#### TC-061: Gerar "Livros Mais Emprestados"
**Passos:**
1. Clicar em "📚 Livros Mais Emprestados"

**Resultado esperado:**
✅ Top 20 livros
✅ Ordenado por total de empréstimos (decrescente)

#### TC-062: Gerar "Empréstimos Atrasados"
**Passos:**
1. Clicar em "⚠️ Empréstimos Atrasados"

**Resultado esperado:**
✅ Lista apenas atrasados
✅ Linhas em vermelho
✅ Inclui telefone do aluno

#### TC-063: Exportar relatório para CSV
**Pré-condição:** Relatório gerado
**Passos:**
1. Clicar em "💾 Exportar para CSV"
2. Escolher local e nome
3. Salvar

**Resultado esperado:**
✅ Arquivo .csv criado
✅ Contém todos os dados da tabela
✅ Rodapé com data, usuário, sistema

#### TC-064: Abrir CSV no Excel
**Pré-condição:** CSV gerado
**Passos:**
1. Abrir arquivo no Excel
2. Usar separador `;`

**Resultado esperado:**
✅ Dados aparecem em colunas corretas
✅ Formatação legível

---

## Cenários de Teste Integrados

### CENÁRIO 1: Fluxo Completo de Empréstimo

**Objetivo:** Testar fluxo desde cadastro até devolução

**Passos:**
1. ✅ Cadastrar novo aluno (João Silva, MAT001)
2. ✅ Cadastrar novo livro (Dom Casmurro, 2 exemplares)
3. ✅ Registrar empréstimo (João → Dom Casmurro)
4. ✅ Verificar dashboard (empréstimos ativos +1)
5. ✅ Verificar livro (disponível = 1)
6. ✅ Avançar data do sistema 10 dias (simular atraso)
7. ✅ Registrar devolução
8. ✅ Verificar multa calculada (R$ 6,00 = 3 dias × R$ 2)
9. ✅ Verificar livro (disponível = 2 novamente)
10. ✅ Gerar relatório de multas (deve aparecer)

**Resultado esperado:** ✅ Fluxo completo sem erros

### CENÁRIO 2: Sistema de Reservas FIFO

**Objetivo:** Validar fila de reservas

**Passos:**
1. ✅ Cadastrar 1 livro com 1 exemplar
2. ✅ Registrar empréstimo (livro fica indisponível)
3. ✅ Criar reserva 1 (Aluno A)
4. ✅ Criar reserva 2 (Aluno B)
5. ✅ Criar reserva 3 (Aluno C)
6. ✅ Verificar posições (A=1º, B=2º, C=3º)
7. ✅ Registrar devolução do livro
8. ✅ Verificar que Aluno A foi notificado
9. ✅ Registrar novo empréstimo (Aluno A)
10. ✅ Verificar posições (B=1º, C=2º)

**Resultado esperado:** ✅ FIFO funciona corretamente

### CENÁRIO 3: Validações de Limite

**Objetivo:** Testar limite de 3 empréstimos

**Passos:**
1. ✅ Cadastrar 1 aluno
2. ✅ Cadastrar 4 livros disponíveis
3. ✅ Registrar empréstimo 1 → OK
4. ✅ Registrar empréstimo 2 → OK
5. ✅ Registrar empréstimo 3 → OK
6. ❌ Tentar empréstimo 4 → Bloqueado ("Limite atingido")
7. ✅ Devolver 1 livro
8. ✅ Registrar empréstimo 4 → OK (agora tem 3 ativos)

**Resultado esperado:** ✅ Limite respeitado

### CENÁRIO 4: Aluno com Atraso não pode Emprestar

**Objetivo:** Validar bloqueio por atraso

**Passos:**
1. ✅ Cadastrar aluno e livro
2. ✅ Registrar empréstimo
3. ✅ Avançar data 10 dias (empréstimo atrasado)
4. ❌ Tentar novo empréstimo → Bloqueado
5. ✅ Devolver livro atrasado
6. ✅ Tentar novo empréstimo → OK (agora sem atrasos)

**Resultado esperado:** ✅ Bloqueio por atraso funciona

---

## Checklist de Validação

### Funcionalidades Principais

- [ ] Login funciona (TC-001)
- [ ] Cadastro de alunos completo (TC-010 a TC-015)
- [ ] Cadastro de livros completo (TC-020 a TC-022)
- [ ] Empréstimos com todas validações (TC-030 a TC-033)
- [ ] Devoluções com cálculo de multa (TC-040 a TC-042)
- [ ] Reservas FIFO (TC-050 a TC-054)
- [ ] Relatórios gerando corretamente (TC-060 a TC-064)

### Validações de Dados

- [ ] CPF validado com dígitos verificadores
- [ ] ISBN-10 e ISBN-13 aceitos
- [ ] E-mail com formato válido
- [ ] Matrícula única
- [ ] ISBN único
- [ ] Campos obrigatórios não vazios

### Regras de Negócio

- [ ] Prazo: 7 dias
- [ ] Multa: R$ 2,00/dia
- [ ] Limite: 3 empréstimos simultâneos
- [ ] Máximo renovações: 2 vezes
- [ ] FIFO nas reservas
- [ ] Bloqueio por atraso
- [ ] Quantidade disponível atualizada

### Interface

- [ ] Todas as telas abrem sem erro
- [ ] Mensagens claras (sucesso/erro)
- [ ] Cores contextuais (vermelho para atrasados)
- [ ] Busca em tempo real funciona
- [ ] Duplo clique para editar
- [ ] Dashboard atualiza corretamente
- [ ] Exportação CSV funciona

### Banco de Dados

- [ ] Conexão estabelecida
- [ ] Todas as tabelas criadas
- [ ] Views funcionando
- [ ] Dados de teste inseridos
- [ ] Integridade referencial mantida

### Logs e Auditoria

- [ ] Login registrado nos logs
- [ ] Empréstimos registrados
- [ ] Devoluções registradas
- [ ] Exclusões registradas
- [ ] Logs consultáveis

---

## Ambiente de Teste

### Configuração Necessária

1. **Sistema Operacional:** Windows 10/11
2. **Banco de Dados:** MySQL 8.0 rodando
3. **Dados de Teste:** Schema.sql executado
4. **Aplicação:** BibliotecaJK v3.0 compilada

### Dados de Teste Padrão

**Funcionário:**
- Login: admin
- Senha: admin123

**Alunos:** 3 alunos de exemplo
**Livros:** 5 livros de exemplo
**Empréstimos:** 2 empréstimos de exemplo

---

## Relatório de Bugs

### Template para Reportar Bugs

```
ID: BUG-XXX
Severidade: [Crítico/Alto/Médio/Baixo]
Módulo: [Login/Cadastros/Empréstimos/etc]
Descrição: [Descrição clara do problema]
Passos para Reproduzir:
1. ...
2. ...
Resultado Esperado: ...
Resultado Obtido: ...
Screenshot: [se aplicável]
```

---

## Conclusão

Este plano de testes cobre:
- ✅ Todos os módulos principais
- ✅ Todas as regras de negócio
- ✅ Casos de sucesso e falha
- ✅ Fluxos integrados
- ✅ Validações de dados

**Critério de Aprovação:**
- 100% dos casos de teste principais (TC-001 a TC-064) passando
- Todos os cenários integrados funcionando
- Checklist de validação completo

---

**Desenvolvido por:**
Pessoa 1: Banco de Dados
Pessoa 2: Camada DAL
Pessoa 3: Camada BLL
Pessoa 4: Interface WinForms
Pessoa 5: Relatórios e Documentação

**BibliotecaJK v3.0** - Sistema Completo de Gerenciamento de Bibliotecas
© 2025 - Todos os direitos reservados
