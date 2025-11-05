# Camada BLL (Business Logic Layer)

## Visão Geral

Esta camada contém toda a **lógica de negócio** do sistema BiblioKopke. Ela fica entre a camada de dados (DAL) e a interface do usuário (UI).

```
┌─────────────────┐
│   WinForms UI   │ ← Pessoa 4
└────────┬────────┘
         │
┌────────▼────────┐
│   BLL/Service   │ ← Pessoa 3 ✅ IMPLEMENTADO
└────────┬────────┘
         │
┌────────▼────────┐
│       DAL       │ ← Pessoa 2 ✅
└────────┬────────┘
         │
┌────────▼────────┐
│      MySQL      │ ← Pessoa 1 ✅
└─────────────────┘
```

---

## Arquivos Implementados

### 1. **ResultadoOperacao.cs**
Classe auxiliar para padronizar retornos de métodos.

**Uso**:
```csharp
public ResultadoOperacao MinhaOperacao()
{
    if (erro)
        return ResultadoOperacao.Erro("Mensagem de erro");

    return ResultadoOperacao.Ok("Operação bem-sucedida");
}
```

---

### 2. **Exceptions.cs**
Exceções personalizadas para diferentes cenários.

**Tipos**:
- `RegraDeNegocioException` - Violação de regra de negócio
- `EntidadeNaoEncontradaException` - Entidade não existe
- `ValidacaoException` - Falha na validação de dados

---

### 3. **Validadores.cs**
Métodos estáticos de validação.

**Validadores disponíveis**:
- `ValidarCPF(string cpf)` → bool
- `ValidarISBN(string isbn)` → bool (ISBN-10 e ISBN-13)
- `ValidarEmail(string email)` → bool
- `ValidarMatricula(string matricula)` → bool
- `CampoObrigatorio(string valor, string nomeCampo, out string mensagemErro)` → bool
- `NumeroPositivo(int valor, string nomeCampo, out string mensagemErro)` → bool

**Exemplo**:
```csharp
if (!Validadores.ValidarCPF(cpf))
{
    MessageBox.Show("CPF inválido!");
    return;
}
```

---

### 4. **LogService.cs**
Gerenciamento de logs do sistema.

**Métodos principais**:
- `Registrar(int? idFuncionario, string acao, string descricao)` → void
- `ObterPorFuncionario(int idFuncionario)` → List<LogAcao>
- `ObterPorPeriodo(DateTime inicio, DateTime fim)` → List<LogAcao>
- `ObterPorAcao(string acao)` → List<LogAcao>
- `ObterUltimos(int quantidade = 50)` → List<LogAcao>

**Exemplo**:
```csharp
var logService = new LogService();
logService.Registrar(idFuncionario, "LOGIN", "Funcionário João fez login");
```

---

### 5. **EmprestimoService.cs** ⭐
Serviço principal de empréstimos.

**Regras de Negócio Implementadas**:
- ✅ Prazo de devolução: **7 dias**
- ✅ Máximo de empréstimos simultâneos: **3 por aluno**
- ✅ Multa por atraso: **R$ 2,00 por dia**
- ✅ Bloqueio de empréstimo para alunos com atrasos
- ✅ Validação de disponibilidade de livros
- ✅ Atualização automática de quantidade disponível
- ✅ Renovação de empréstimos (máx 2 vezes)

**Métodos principais**:
```csharp
// Registrar empréstimo
ResultadoOperacao RegistrarEmprestimo(int idAluno, int idLivro, int? idFuncionario = null)

// Registrar devolução (calcula multa automaticamente)
ResultadoOperacao RegistrarDevolucao(int idEmprestimo, int? idFuncionario = null)

// Renovar empréstimo
ResultadoOperacao RenovarEmprestimo(int idEmprestimo, int? idFuncionario = null)

// Consultas
List<Emprestimo> ObterEmprestimosAtivos(int idAluno)
List<Emprestimo> ObterEmprestimosAtrasados(int idAluno)
List<Emprestimo> ObterTodosEmprestimosAtrasados()
List<Emprestimo> ObterHistoricoAluno(int idAluno)
List<Emprestimo> ObterHistoricoLivro(int idLivro)

// Estatísticas
(int Total, int Ativos, int Atrasados, decimal MultaTotal) ObterEstatisticas(DateTime? inicio, DateTime? fim)
```

**Exemplo de uso**:
```csharp
var emprestimoService = new EmprestimoService();

// Registrar empréstimo
var resultado = emprestimoService.RegistrarEmprestimo(idAluno: 1, idLivro: 5, idFuncionario: 1);
if (resultado.Sucesso)
{
    MessageBox.Show(resultado.Mensagem, "Sucesso");
}
else
{
    MessageBox.Show(resultado.Mensagem, "Erro");
}

// Registrar devolução
var resultadoDev = emprestimoService.RegistrarDevolucao(idEmprestimo: 10, idFuncionario: 1);
if (resultadoDev.Sucesso && resultadoDev.ValorMulta > 0)
{
    MessageBox.Show($"Multa: R$ {resultadoDev.ValorMulta:F2}", "Atenção");
}
```

---

### 6. **ReservaService.cs**
Gerenciamento de reservas de livros.

**Funcionalidades**:
- ✅ Criar reserva (apenas para livros indisponíveis)
- ✅ Cancelar reserva
- ✅ Sistema de fila FIFO (First In, First Out)
- ✅ Processamento automático da fila ao devolver livro
- ✅ Validação de reserva duplicada

**Métodos principais**:
```csharp
ResultadoOperacao CriarReserva(int idAluno, int idLivro, int? idFuncionario = null)
ResultadoOperacao CancelarReserva(int idReserva, int? idFuncionario = null)
(bool TemReserva, Reserva? ProximaReserva, Aluno? ProximoAluno) ProcessarFilaReservas(int idLivro)
int ObterPosicaoNaFila(int idAluno, int idLivro)
List<Reserva> ObterReservasAtivas(int idAluno)
List<Reserva> ObterFilaReservas(int idLivro)
```

**Exemplo de uso**:
```csharp
var reservaService = new ReservaService();

// Criar reserva
var resultado = reservaService.CriarReserva(idAluno: 2, idLivro: 3);
if (resultado.Sucesso)
{
    MessageBox.Show(resultado.Mensagem); // Mostra posição na fila
}

// Processar fila (chamar após devolução)
var (temReserva, reserva, aluno) = reservaService.ProcessarFilaReservas(idLivro: 3);
if (temReserva && aluno != null)
{
    MessageBox.Show($"Livro reservado para: {aluno.Nome}", "Notificação");
}
```

---

### 7. **LivroService.cs**
Gerenciamento de livros com regras de negócio.

**Funcionalidades**:
- ✅ Cadastro com validações (ISBN, campos obrigatórios)
- ✅ Validação de ISBN duplicado
- ✅ Busca por título, autor, ISBN
- ✅ Livros mais emprestados
- ✅ Estatísticas do acervo

**Métodos principais**:
```csharp
ResultadoOperacao CadastrarLivro(Livro livro, int? idFuncionario = null)
ResultadoOperacao AtualizarLivro(Livro livro, int? idFuncionario = null)
bool VerificarDisponibilidade(int idLivro)
List<Livro> BuscarPorTitulo(string termo)
List<Livro> BuscarPorAutor(string termo)
Livro? BuscarPorISBN(string isbn)
List<(Livro, int TotalEmprestimos)> ObterMaisEmprestados(int top = 10)
List<Livro> ObterDisponiveis()
List<Livro> ObterIndisponiveis()
(int TotalLivros, int TotalExemplares, int Disponiveis, int Emprestados) ObterEstatisticas()
```

---

### 8. **AlunoService.cs**
Gerenciamento de alunos com validações.

**Funcionalidades**:
- ✅ Cadastro com validações (CPF, matrícula, e-mail)
- ✅ Validação de CPF e matrícula duplicados
- ✅ Bloqueio de exclusão se tiver empréstimos ativos
- ✅ Consulta de alunos inadimplentes
- ✅ Verificação de aptidão para empréstimo

**Métodos principais**:
```csharp
ResultadoOperacao CadastrarAluno(Aluno aluno, int? idFuncionario = null)
ResultadoOperacao AtualizarAluno(Aluno aluno, int? idFuncionario = null)
ResultadoOperacao ExcluirAluno(int idAluno, int? idFuncionario = null)
List<Aluno> ObterAlunosComEmprestimosAtrasados()
List<Aluno> ObterAlunosComEmprestimosAtivos()
List<Aluno> BuscarPorNome(string termo)
Aluno? BuscarPorCPF(string cpf)
Aluno? BuscarPorMatricula(string matricula)
(bool Apto, string Mensagem) VerificarAptoParaEmprestimo(int idAluno)
(int Total, int ComEmprestimos, int ComAtrasos) ObterEstatisticas()
```

---

## Padrões de Uso

### 1. Sempre use ResultadoOperacao para retornos

**✅ BOM**:
```csharp
var resultado = emprestimoService.RegistrarEmprestimo(1, 2);
if (resultado.Sucesso)
{
    // Sucesso
}
else
{
    MessageBox.Show(resultado.Mensagem);
}
```

**❌ RUIM**:
```csharp
try
{
    emprestimoService.RegistrarEmprestimo(1, 2);
}
catch (Exception ex)
{
    MessageBox.Show(ex.Message);
}
```

---

### 2. Validações sempre na camada BLL

**✅ BOM**:
```csharp
// Na UI (WinForms):
var aluno = new Aluno { Nome = txtNome.Text, CPF = txtCPF.Text };
var resultado = alunoService.CadastrarAluno(aluno);
// Service faz todas as validações
```

**❌ RUIM**:
```csharp
// Na UI (WinForms):
if (string.IsNullOrEmpty(txtNome.Text)) return; // validação na UI
var aluno = new Aluno { Nome = txtNome.Text };
alunoDAL.Inserir(aluno); // chama DAL direto
```

---

### 3. Sempre passe idFuncionario para auditoria

```csharp
int idFuncionarioLogado = 1; // Do sistema de login

var resultado = emprestimoService.RegistrarEmprestimo(
    idAluno: 5,
    idLivro: 10,
    idFuncionario: idFuncionarioLogado  // ← IMPORTANTE para log
);
```

---

## Fluxos Implementados

### Fluxo de Empréstimo
```
1. UI chama EmprestimoService.RegistrarEmprestimo()
2. Service valida:
   - Aluno existe?
   - Livro existe?
   - Livro disponível?
   - Aluno tem atrasos?
   - Aluno atingiu limite de 3 empréstimos?
3. Se OK:
   - Cria empréstimo
   - Decrementa quantidade_disponivel
   - Registra log
4. Retorna ResultadoOperacao
```

### Fluxo de Devolução
```
1. UI chama EmprestimoService.RegistrarDevolucao()
2. Service:
   - Busca empréstimo
   - Calcula atraso (se houver)
   - Calcula multa (R$ 2,00/dia)
   - Atualiza empréstimo
   - Incrementa quantidade_disponivel
   - Registra log
3. Processa fila de reservas (ReservaService)
4. Retorna resultado com valor da multa
```

### Fluxo de Reserva
```
1. UI chama ReservaService.CriarReserva()
2. Service valida:
   - Livro realmente indisponível?
   - Aluno já tem reserva ativa para este livro?
3. Se OK:
   - Cria reserva com status ATIVA
   - Calcula posição na fila
   - Registra log
4. Quando livro devolvido:
   - ProcessarFilaReservas() é chamado
   - Primeira reserva ATIVA vira CONCLUIDA
   - Retorna dados do aluno para notificação
```

---

## Testes Manuais Realizados

### ✅ EmprestimoService
- [x] Registrar empréstimo válido
- [x] Bloquear empréstimo de livro indisponível
- [x] Bloquear empréstimo para aluno com atraso
- [x] Bloquear empréstimo quando aluno tem 3 ativos
- [x] Devolução no prazo (multa = 0)
- [x] Devolução com atraso (cálculo de multa)
- [x] Renovação de empréstimo
- [x] Bloquear renovação de empréstimo atrasado

### ✅ ReservaService
- [x] Criar reserva para livro indisponível
- [x] Bloquear reserva de livro disponível
- [x] Bloquear reserva duplicada
- [x] Processar fila (FIFO)
- [x] Cancelar reserva

### ✅ Validadores
- [x] CPF válido (com dígitos verificadores)
- [x] CPF inválido
- [x] ISBN-10 válido
- [x] ISBN-13 válido
- [x] E-mail válido/inválido

### ✅ LivroService
- [x] Cadastrar livro com validações
- [x] Bloquear ISBN duplicado
- [x] Buscar por título parcial
- [x] Livros mais emprestados

### ✅ AlunoService
- [x] Cadastrar aluno com validações
- [x] Bloquear CPF duplicado
- [x] Bloquear matrícula duplicada
- [x] Bloquear exclusão com empréstimos ativos
- [x] Verificar aptidão para empréstimo

---

## Próximos Passos (Para P4 - Frontend)

### Integração com WinForms

1. **Tela de Empréstimo**:
```csharp
private void btnRegistrarEmprestimo_Click(object sender, EventArgs e)
{
    var emprestimoService = new EmprestimoService();
    var resultado = emprestimoService.RegistrarEmprestimo(
        idAluno: Convert.ToInt32(cmbAluno.SelectedValue),
        idLivro: Convert.ToInt32(cmbLivro.SelectedValue),
        idFuncionario: SessaoAtual.IdFuncionario
    );

    if (resultado.Sucesso)
    {
        MessageBox.Show(resultado.Mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        LimparFormulario();
    }
    else
    {
        MessageBox.Show(resultado.Mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

2. **Tela de Devolução**:
```csharp
private void btnRegistrarDevolucao_Click(object sender, EventArgs e)
{
    var emprestimoService = new EmprestimoService();
    var resultado = emprestimoService.RegistrarDevolucao(
        idEmprestimo: Convert.ToInt32(dgvEmprestimos.SelectedRows[0].Cells["Id"].Value),
        idFuncionario: SessaoAtual.IdFuncionario
    );

    if (resultado.Sucesso)
    {
        MessageBox.Show(resultado.Mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

        if (resultado.ValorMulta > 0)
        {
            MessageBox.Show($"ATENÇÃO: Multa de R$ {resultado.ValorMulta:F2}",
                "Multa por Atraso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        CarregarEmprestimos();
    }
}
```

---

## Constantes de Regras de Negócio

Todas definidas em **EmprestimoService.cs**:

```csharp
private const int PRAZO_DIAS = 7;                    // Prazo de devolução
private const int MAX_EMPRESTIMOS_SIMULTANEOS = 3;   // Limite por aluno
private const int MAX_RENOVACOES = 2;                // Máximo de renovações
private const decimal MULTA_POR_DIA = 2.00m;         // Valor da multa
```

Para alterar essas regras, basta mudar as constantes e recompilar.

---

## Arquivos de Log

Todos os logs são salvos na tabela `Log_Acao` via `LogService`.

**Ações registradas**:
- `EMPRESTIMO_REGISTRADO`
- `EMPRESTIMO_DEVOLVIDO`
- `EMPRESTIMO_RENOVADO`
- `RESERVA_CRIADA`
- `RESERVA_CANCELADA`
- `RESERVA_ATENDIDA`
- `LIVRO_CADASTRADO`
- `ALUNO_CADASTRADO`
- `ERRO_*` (para todos os erros)

---

## Checklist de Entrega ✅

- [x] ResultadoOperacao.cs
- [x] Exceptions.cs
- [x] Validadores.cs (CPF, ISBN, Email, Matrícula)
- [x] LogService.cs
- [x] EmprestimoService.cs (completo)
- [x] ReservaService.cs (completo)
- [x] LivroService.cs (completo)
- [x] AlunoService.cs (completo)
- [x] Documentação (este arquivo)

---

**Camada BLL implementada por**: Pessoa 3
**Status**: ✅ CONCLUÍDA
**Data**: Novembro 2025
