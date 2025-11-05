# 🎯 PLANO DE AÇÃO - PESSOA 3
## Backend - Lógica de Negócio (Business Logic Layer)

**Tecnologia**: C# + WinForms
**Status**: P1 ✅ Entregou | P2 ✅ Entregou | **P3 🔄 EM ANDAMENTO**

---

## 📊 SITUAÇÃO ATUAL

### ✅ O QUE JÁ FOI ENTREGUE (P1 + P2)

#### Pessoa 1 - Banco de Dados ✅
- [x] Script DDL completo (schema.sql)
- [x] Tabelas: Aluno, Funcionario, Livro, Emprestimo, Reserva, Log_Acao
- [x] Índices e constraints
- [x] Views úteis
- [x] Dados de teste

#### Pessoa 2 - Camada de Dados ✅
- [x] Conexão MySQL (Conexao.cs)
- [x] Classes Model (Pessoa, Aluno, Funcionario, Livro, Emprestimo, Reserva, LogAcao)
- [x] CRUD de Alunos (AlunoDAL.cs)
- [x] CRUD de Funcionários (FuncionarioDAL.cs)
- [x] CRUD de Livros (LivroDAL.cs)
- [x] CRUD de Empréstimos (EmprestimoDAL.cs)
- [x] CRUD de Reservas (ReservaDAL.cs)
- [x] CRUD de Logs (LogAcaoDAL.cs)
- [x] Menu interativo de testes (Program.cs)

---

## 🎯 SUAS RESPONSABILIDADES (PESSOA 3)

### Camada de Lógica de Negócio (BLL - Business Logic Layer)

Você precisa criar uma camada intermediária entre:
- **DAL** (Data Access Layer) ← já existe
- **UI** (WinForms) ← será feito por P4

```
┌─────────────────┐
│   WinForms UI   │ ← Pessoa 4
└────────┬────────┘
         │
┌────────▼────────┐
│   BLL/Service   │ ← VOCÊ (Pessoa 3)
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

## 📋 TAREFAS DETALHADAS

### 🔴 SPRINT 1: Estrutura + Empréstimos (SEMANA 3-4)
**Prazo**: 2 semanas
**Prioridade**: CRÍTICA

#### Tarefa 1.1: Criar estrutura da camada BLL
**Tempo estimado**: 2 horas

```csharp
// Criar pasta: 08_proto c#/BLL/

08_proto c#/
├── BLL/
│   ├── EmprestimoService.cs
│   ├── ReservaService.cs
│   ├── LivroService.cs
│   ├── AlunoService.cs
│   └── LogService.cs
```

**Entregável**:
- [ ] Pasta BLL/ criada
- [ ] Classe base ServiceBase.cs (opcional)

---

#### Tarefa 1.2: Implementar EmprestimoService
**Tempo estimado**: 8 horas

**Regras de Negócio para Implementar**:

1. **Registrar Empréstimo**
   - [ ] Validar se aluno existe
   - [ ] Validar se livro existe
   - [ ] Validar se livro está disponível (quantidade_disponivel > 0)
   - [ ] Validar se aluno não tem empréstimos atrasados
   - [ ] Validar limite de empréstimos simultâneos por aluno (ex: máx 3)
   - [ ] Calcular data de devolução prevista (ex: +7 dias)
   - [ ] Decrementar quantidade_disponivel do livro
   - [ ] Registrar empréstimo no banco
   - [ ] Registrar log da ação

2. **Registrar Devolução**
   - [ ] Validar se empréstimo existe
   - [ ] Validar se empréstimo ainda está ativo (data_devolucao = null)
   - [ ] Calcular se há atraso
   - [ ] Calcular multa se atrasado (ex: R$ 2,00 por dia)
   - [ ] Atualizar data_devolucao
   - [ ] Incrementar quantidade_disponivel do livro
   - [ ] Registrar log da ação
   - [ ] Retornar valor da multa (se houver)

3. **Renovar Empréstimo**
   - [ ] Validar se empréstimo existe e está ativo
   - [ ] Validar se não está atrasado
   - [ ] Validar limite de renovações (ex: máx 2 vezes)
   - [ ] Estender data prevista (ex: +7 dias)
   - [ ] Registrar log da ação

4. **Consultar Empréstimos Ativos de um Aluno**
   - [ ] Buscar empréstimos com data_devolucao = null
   - [ ] Calcular dias restantes até devolução
   - [ ] Marcar quais estão atrasados

**Exemplo de código**:

```csharp
using BibliotecaJK.Model;
using BibliotecaJK.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaJK.BLL
{
    public class EmprestimoService
    {
        private readonly EmprestimoDAL _emprestimoDAL;
        private readonly LivroDAL _livroDAL;
        private readonly AlunoDAL _alunoDAL;
        private readonly LogService _logService;

        // Constantes de regras de negócio
        private const int PRAZO_DIAS = 7;
        private const int MAX_EMPRESTIMOS_SIMULTANEOS = 3;
        private const int MAX_RENOVACOES = 2;
        private const decimal MULTA_POR_DIA = 2.00m;

        public EmprestimoService()
        {
            _emprestimoDAL = new EmprestimoDAL();
            _livroDAL = new LivroDAL();
            _alunoDAL = new AlunoDAL();
            _logService = new LogService();
        }

        public ResultadoOperacao RegistrarEmprestimo(int idAluno, int idLivro, int? idFuncionario)
        {
            try
            {
                // 1. Validar se aluno existe
                var aluno = _alunoDAL.ObterPorId(idAluno);
                if (aluno == null)
                    return ResultadoOperacao.Erro("Aluno não encontrado.");

                // 2. Validar se livro existe
                var livro = _livroDAL.ObterPorId(idLivro);
                if (livro == null)
                    return ResultadoOperacao.Erro("Livro não encontrado.");

                // 3. Validar disponibilidade
                if (livro.QuantidadeDisponivel <= 0)
                    return ResultadoOperacao.Erro("Livro indisponível no momento.");

                // 4. Validar empréstimos atrasados
                var emprestimosAtrasados = ObterEmprestimosAtrasados(idAluno);
                if (emprestimosAtrasados.Any())
                    return ResultadoOperacao.Erro($"Aluno possui {emprestimosAtrasados.Count} empréstimo(s) atrasado(s).");

                // 5. Validar limite de empréstimos simultâneos
                var emprestimosAtivos = ObterEmprestimosAtivos(idAluno);
                if (emprestimosAtivos.Count >= MAX_EMPRESTIMOS_SIMULTANEOS)
                    return ResultadoOperacao.Erro($"Aluno já possui o máximo de {MAX_EMPRESTIMOS_SIMULTANEOS} empréstimos ativos.");

                // 6. Criar empréstimo
                var emprestimo = new Emprestimo
                {
                    IdAluno = idAluno,
                    IdLivro = idLivro,
                    DataEmprestimo = DateTime.Now,
                    DataPrevista = DateTime.Now.AddDays(PRAZO_DIAS),
                    Multa = 0
                };

                // 7. Decrementar quantidade disponível
                livro.QuantidadeDisponivel--;
                _livroDAL.Atualizar(livro);

                // 8. Salvar empréstimo
                _emprestimoDAL.Inserir(emprestimo);

                // 9. Registrar log
                _logService.Registrar(idFuncionario, "EMPRESTIMO_REGISTRADO",
                    $"Aluno: {aluno.Nome} | Livro: {livro.Titulo}");

                return ResultadoOperacao.Sucesso("Empréstimo registrado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao registrar empréstimo: {ex.Message}");
            }
        }

        public ResultadoOperacao RegistrarDevolucao(int idEmprestimo, int? idFuncionario)
        {
            try
            {
                // 1. Buscar empréstimo
                var emprestimo = _emprestimoDAL.ObterPorId(idEmprestimo);
                if (emprestimo == null)
                    return ResultadoOperacao.Erro("Empréstimo não encontrado.");

                // 2. Validar se ainda está ativo
                if (emprestimo.DataDevolucao != null)
                    return ResultadoOperacao.Erro("Empréstimo já foi devolvido.");

                // 3. Calcular atraso e multa
                var diasAtraso = (DateTime.Now.Date - emprestimo.DataPrevista.Date).Days;
                decimal multa = 0;

                if (diasAtraso > 0)
                {
                    multa = diasAtraso * MULTA_POR_DIA;
                }

                // 4. Atualizar empréstimo
                emprestimo.DataDevolucao = DateTime.Now;
                emprestimo.Multa = multa;
                _emprestimoDAL.Atualizar(emprestimo);

                // 5. Incrementar quantidade disponível
                var livro = _livroDAL.ObterPorId(emprestimo.IdLivro);
                if (livro != null)
                {
                    livro.QuantidadeDisponivel++;
                    _livroDAL.Atualizar(livro);
                }

                // 6. Registrar log
                var mensagem = multa > 0
                    ? $"Devolução com atraso de {diasAtraso} dia(s). Multa: R$ {multa:F2}"
                    : "Devolução no prazo";
                _logService.Registrar(idFuncionario, "EMPRESTIMO_DEVOLVIDO", mensagem);

                var resultado = multa > 0
                    ? $"Devolução registrada. ATENÇÃO: Multa de R$ {multa:F2} ({diasAtraso} dia(s) de atraso)"
                    : "Devolução registrada com sucesso!";

                return ResultadoOperacao.Sucesso(resultado, multa);
            }
            catch (Exception ex)
            {
                return ResultadoOperacao.Erro($"Erro ao registrar devolução: {ex.Message}");
            }
        }

        public List<Emprestimo> ObterEmprestimosAtivos(int idAluno)
        {
            return _emprestimoDAL.Listar()
                .Where(e => e.IdAluno == idAluno && e.DataDevolucao == null)
                .ToList();
        }

        public List<Emprestimo> ObterEmprestimosAtrasados(int idAluno)
        {
            return _emprestimoDAL.Listar()
                .Where(e => e.IdAluno == idAluno &&
                           e.DataDevolucao == null &&
                           e.DataPrevista.Date < DateTime.Now.Date)
                .ToList();
        }
    }

    // Classe auxiliar para retorno padronizado
    public class ResultadoOperacao
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public decimal ValorMulta { get; set; }

        public static ResultadoOperacao Sucesso(string mensagem, decimal multa = 0)
        {
            return new ResultadoOperacao { Sucesso = true, Mensagem = mensagem, ValorMulta = multa };
        }

        public static ResultadoOperacao Erro(string mensagem)
        {
            return new ResultadoOperacao { Sucesso = false, Mensagem = mensagem };
        }
    }
}
```

**Entregáveis**:
- [ ] EmprestimoService.cs implementado
- [ ] ResultadoOperacao.cs (classe auxiliar)
- [ ] Todas as validações implementadas
- [ ] Testes manuais documentados

---

#### Tarefa 1.3: Implementar LogService
**Tempo estimado**: 2 horas

```csharp
namespace BibliotecaJK.BLL
{
    public class LogService
    {
        private readonly LogAcaoDAL _logDAL;

        public LogService()
        {
            _logDAL = new LogAcaoDAL();
        }

        public void Registrar(int? idFuncionario, string acao, string descricao)
        {
            try
            {
                var log = new LogAcao
                {
                    IdFuncionario = idFuncionario,
                    Acao = acao,
                    Descricao = descricao,
                    DataHora = DateTime.Now
                };

                _logDAL.Inserir(log);
            }
            catch (Exception ex)
            {
                // Não deve lançar exceção para não quebrar fluxo principal
                Console.WriteLine($"Erro ao registrar log: {ex.Message}");
            }
        }

        public List<LogAcao> ObterPorFuncionario(int idFuncionario)
        {
            return _logDAL.Listar()
                .Where(l => l.IdFuncionario == idFuncionario)
                .OrderByDescending(l => l.DataHora)
                .ToList();
        }

        public List<LogAcao> ObterPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            return _logDAL.Listar()
                .Where(l => l.DataHora >= dataInicio && l.DataHora <= dataFim)
                .OrderByDescending(l => l.DataHora)
                .ToList();
        }
    }
}
```

**Entregáveis**:
- [ ] LogService.cs implementado
- [ ] Métodos de consulta por funcionário e período

---

### 🟡 SPRINT 2: Reservas + Validações (SEMANA 5-6)
**Prazo**: 2 semanas
**Prioridade**: ALTA

#### Tarefa 2.1: Implementar ReservaService
**Tempo estimado**: 6 horas

**Regras de Negócio**:

1. **Criar Reserva**
   - [ ] Validar se livro está realmente indisponível
   - [ ] Validar se aluno já não tem reserva ativa para o mesmo livro
   - [ ] Criar reserva com status "ATIVA"
   - [ ] Registrar log

2. **Cancelar Reserva**
   - [ ] Validar se reserva existe e está ativa
   - [ ] Atualizar status para "CANCELADA"
   - [ ] Registrar log

3. **Processar Fila de Reservas** (quando livro é devolvido)
   - [ ] Buscar reservas ativas para o livro
   - [ ] Ordenar por data de reserva (FIFO)
   - [ ] Marcar primeira reserva como "NOTIFICADA"
   - [ ] Retornar dados do aluno para notificação

4. **Consultar Reservas de um Aluno**
   - [ ] Listar reservas ativas
   - [ ] Mostrar posição na fila

**Entregáveis**:
- [ ] ReservaService.cs implementado
- [ ] Integração com EmprestimoService (notificar ao devolver)

---

#### Tarefa 2.2: Implementar Validações Centralizadas
**Tempo estimado**: 4 horas

```csharp
namespace BibliotecaJK.BLL
{
    public static class Validadores
    {
        public static bool ValidarCPF(string cpf)
        {
            // Implementar algoritmo de validação de CPF
            // Remover pontos e traços
            // Validar dígitos verificadores
        }

        public static bool ValidarISBN(string isbn)
        {
            // Validar ISBN-10 ou ISBN-13
        }

        public static bool ValidarEmail(string email)
        {
            // Validar formato de e-mail
        }

        public static bool ValidarMatricula(string matricula)
        {
            // Validar formato de matrícula da escola
        }
    }
}
```

**Entregáveis**:
- [ ] Classe Validadores.cs
- [ ] Validação de CPF
- [ ] Validação de ISBN
- [ ] Validação de Email
- [ ] Testes de cada validador

---

#### Tarefa 2.3: Tratamento de Exceções
**Tempo estimado**: 3 horas

```csharp
namespace BibliotecaJK.BLL
{
    // Exceções personalizadas
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException(string mensagem) : base(mensagem) { }
    }

    public class EntidadeNaoEncontradaException : Exception
    {
        public EntidadeNaoEncontradaException(string entidade, int id)
            : base($"{entidade} com ID {id} não encontrado(a).") { }
    }

    public class ValidacaoException : Exception
    {
        public ValidacaoException(string mensagem) : base(mensagem) { }
    }
}
```

**Entregáveis**:
- [ ] Exceções personalizadas criadas
- [ ] Services atualizados para usar exceções
- [ ] Documentação de quando lançar cada tipo

---

### 🟢 SPRINT 3: Refinamentos (SEMANA 7-8)
**Prazo**: 2 semanas
**Prioridade**: MÉDIA

#### Tarefa 3.1: Implementar LivroService (regras extras)
**Tempo estimado**: 3 horas

```csharp
public class LivroService
{
    public bool VerificarDisponibilidade(int idLivro)
    {
        var livro = _livroDAL.ObterPorId(idLivro);
        return livro != null && livro.QuantidadeDisponivel > 0;
    }

    public List<Livro> BuscarPorTitulo(string termo)
    {
        // Busca parcial (LIKE)
    }

    public List<Livro> ObterMaisEmprestados(int top = 10)
    {
        // Consultar tabela de empréstimos
        // Agrupar por livro
        // Ordenar por quantidade
    }
}
```

**Entregáveis**:
- [ ] LivroService.cs
- [ ] Métodos de busca e consulta

---

#### Tarefa 3.2: Implementar AlunoService (regras extras)
**Tempo estimado**: 2 horas

```csharp
public class AlunoService
{
    public ResultadoOperacao CadastrarAluno(Aluno aluno)
    {
        // Validar CPF
        // Validar matrícula única
        // Validar campos obrigatórios
    }

    public List<Aluno> ObterComEmprestimosAtrasados()
    {
        // JOIN com emprestimos
        // Filtrar atrasados
    }
}
```

---

#### Tarefa 3.3: Testes Integrados
**Tempo estimado**: 4 horas

**Cenários para Testar**:

1. **Fluxo Feliz - Empréstimo**
   - [ ] Cadastrar aluno
   - [ ] Cadastrar livro
   - [ ] Registrar empréstimo
   - [ ] Verificar quantidade_disponivel decrementou
   - [ ] Registrar devolução no prazo
   - [ ] Verificar multa = 0

2. **Fluxo com Atraso**
   - [ ] Registrar empréstimo
   - [ ] Simular atraso (ajustar data_prevista manualmente no BD)
   - [ ] Registrar devolução
   - [ ] Verificar cálculo de multa correto

3. **Fluxo de Reserva**
   - [ ] Emprestar todos exemplares de um livro
   - [ ] Criar reserva para o livro
   - [ ] Devolver um exemplar
   - [ ] Verificar se reserva foi notificada

4. **Validações**
   - [ ] Tentar emprestar com livro indisponível (deve falhar)
   - [ ] Tentar emprestar com aluno inadimplente (deve falhar)
   - [ ] Tentar devolver empréstimo já devolvido (deve falhar)

**Entregáveis**:
- [ ] Documento de testes (PDF ou MD)
- [ ] Screenshots das execuções
- [ ] Lista de bugs encontrados (se houver)

---

## 📅 CRONOGRAMA PESSOAL (PESSOA 3)

### Semana 3-4: 🔴 CRÍTICO
- **Seg-Ter**: Criar estrutura BLL + EmprestimoService base
- **Qua-Qui**: Implementar validações de empréstimo
- **Sex-Sab**: Implementar lógica de devolução + multa
- **Dom**: LogService + testes iniciais
- **ENTREGA**: EmprestimoService funcionando + logs

### Semana 5-6: 🟡 IMPORTANTE
- **Seg-Ter**: ReservaService completo
- **Qua-Qui**: Validadores (CPF, ISBN, Email)
- **Sex-Sab**: Exceções personalizadas
- **Dom**: Integração Reserva ↔ Empréstimo
- **ENTREGA**: Sistema de reservas + validações

### Semana 7-8: 🟢 REFINAMENTO
- **Seg-Ter**: LivroService + AlunoService
- **Qua-Qui**: Testes integrados (todos os cenários)
- **Sex-Sab**: Correção de bugs + ajustes
- **Dom**: Documentação do código (comentários XML)
- **ENTREGA**: BLL completa + testes documentados

---

## ✅ CHECKLIST DE ENTREGA

### Código
- [ ] Pasta BLL/ criada com todas as classes
- [ ] EmprestimoService.cs (completo)
- [ ] ReservaService.cs (completo)
- [ ] LogService.cs (completo)
- [ ] LivroService.cs (completo)
- [ ] AlunoService.cs (completo)
- [ ] Validadores.cs (completo)
- [ ] Exceções personalizadas
- [ ] ResultadoOperacao.cs (classe auxiliar)

### Regras de Negócio Implementadas
- [ ] Validação de disponibilidade de livros
- [ ] Limite de empréstimos simultâneos (3)
- [ ] Cálculo de prazo de devolução (7 dias)
- [ ] Cálculo de multa por atraso (R$ 2,00/dia)
- [ ] Bloqueio de empréstimo para inadimplentes
- [ ] Sistema de fila de reservas (FIFO)
- [ ] Renovação de empréstimo (máx 2 vezes)
- [ ] Validação de CPF
- [ ] Validação de ISBN
- [ ] Logs de todas as ações críticas

### Testes
- [ ] Teste: Empréstimo no prazo
- [ ] Teste: Empréstimo com atraso
- [ ] Teste: Empréstimo bloqueado (inadimplente)
- [ ] Teste: Empréstimo bloqueado (limite atingido)
- [ ] Teste: Reserva criada
- [ ] Teste: Fila de reservas processada
- [ ] Teste: Validadores (CPF válido/inválido)
- [ ] Documento de testes em PDF/MD

### Documentação
- [ ] Comentários XML em métodos públicos
- [ ] README da camada BLL
- [ ] Diagrama de fluxo (empréstimo/devolução)
- [ ] Constantes de regras documentadas

---

## 🎯 CRITÉRIOS DE ACEITAÇÃO

### Semana 4 (Checkpoint)
- [ ] P4 (Frontend) consegue chamar `EmprestimoService.RegistrarEmprestimo()`
- [ ] Validações estão funcionando (livro indisponível bloqueia)
- [ ] Multa é calculada corretamente
- [ ] Logs são gravados no banco

### Semana 6 (Checkpoint)
- [ ] Sistema de reservas funcionando ponta-a-ponta
- [ ] Validadores retornam true/false corretamente
- [ ] Exceções personalizadas são lançadas nos cenários certos

### Semana 8 (Entrega Final)
- [ ] TODOS os testes passando
- [ ] Código compila sem warnings
- [ ] Documentação completa
- [ ] P4 consegue integrar facilmente com WinForms

---

## 🚨 PONTOS DE ATENÇÃO

### Dependências
- ⚠️ **P4 (Frontend)** depende do seu trabalho para começar os fluxos
- ⚠️ Definir **interface clara** dos Services na Semana 3
- ⚠️ Comunicação constante com P4 sobre assinaturas de métodos

### Riscos
- 🔴 **ALTO**: Se validações não funcionarem, todo fluxo quebra
- 🟡 **MÉDIO**: Cálculo de multa errado afeta relatórios
- 🟢 **BAIXO**: Logs não críticos para MVP

---

## 📞 COMUNICAÇÃO COM A EQUIPE

### Com P2 (DAL)
- ✅ DAL está pronto, mas você pode solicitar métodos adicionais se precisar
- Exemplo: "Preciso de um método `ObterEmprestimosAtivosPorAluno(int id)`"

### Com P4 (Frontend)
- 🤝 **CRÍTICO**: Alinhar assinaturas de métodos na Semana 3
- 🤝 Informar quando cada Service estiver pronto para integração
- 🤝 Fornecer exemplos de uso

### Com P5 (Relatórios)
- ℹ️ Seus Services serão usados nos relatórios
- ℹ️ Garantir que métodos de consulta sejam eficientes

---

## 🎓 RECURSOS ÚTEIS

### C# + Regras de Negócio
- LINQ para consultas em memória
- Exception handling best practices
- Service Layer Pattern

### Validações
- Algoritmo de CPF: https://www.devmedia.com.br/validando-o-cpf-em-c/3760
- ISBN validation
- Regex para email

---

## 📊 ESTIMATIVA DE TEMPO

| Sprint | Horas | Dias (4h/dia) |
|--------|-------|---------------|
| Sprint 1 | 12h | 3 dias |
| Sprint 2 | 13h | 3.25 dias |
| Sprint 3 | 9h | 2.25 dias |
| **TOTAL** | **34h** | **~8.5 dias** |

**Prazo real**: 6 semanas (42 dias corridos)
**Carga**: ~1 hora/dia ou 4-5 horas nos fins de semana

---

## 🎯 PRÓXIMOS PASSOS IMEDIATOS

### HOJE (Esta Semana)
1. ✅ Ler este documento completamente
2. [ ] Criar pasta `08_proto c#/BLL/`
3. [ ] Criar classe `ResultadoOperacao.cs`
4. [ ] Começar `EmprestimoService.cs`

### SEMANA QUE VEM
5. [ ] Implementar `RegistrarEmprestimo()`
6. [ ] Implementar `RegistrarDevolucao()`
7. [ ] Criar `LogService.cs`
8. [ ] Fazer primeiro teste manual

---

**Boa sorte! Você é a peça-chave entre o banco de dados e a interface! 🚀**
