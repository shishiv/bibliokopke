# 🎨 GUIA COMPLETO DE UX/UI - BibliotecaJK

## 📚 ÍNDICE

1. [ToastNotification - Notificações Não-Intrusivas](#1-toastnotification)
2. [LoadingPanel - Indicadores de Carregamento](#2-loadingpanel)
3. [ThemeManager - Modo Claro/Escuro](#3-thememanager)
4. [InputMaskHelper - Máscaras e Validação](#4-inputmaskhelper)
5. [KeyboardShortcutManager - Atalhos de Teclado](#5-keyboardshortcutmanager)
6. [Exemplos Práticos Completos](#6-exemplos-práticos)

---

## 1. ToastNotification

### 📖 O que é?
Notificações não-intrusivas estilo Android/Material Design que aparecem no canto superior direito e desaparecem automaticamente.

### 🎯 Quando usar?
- ✅ Confirmação de ações (salvar, excluir, atualizar)
- ✅ Avisos não-críticos
- ✅ Feedback de operações assíncronas
- ❌ **NÃO** para erros críticos que exigem atenção

### 💻 Como usar?

```csharp
using BibliotecaJK.Components;

// Método 1: Atalhos diretos (RECOMENDADO)
ToastNotification.Success("Aluno cadastrado com sucesso!");
ToastNotification.Error("Erro ao conectar ao banco de dados");
ToastNotification.Warning("CPF já cadastrado no sistema");
ToastNotification.Info("Sistema atualizado para versão 3.1");

// Método 2: Completo com duração customizada
ToastNotification.Show(
    "Operação concluída",
    ToastNotification.ToastType.Success,
    duracao: 5000 // 5 segundos
);
```

### 🎨 Tipos e Cores

| Tipo | Cor | Ícone | Uso |
|------|-----|-------|-----|
| Success | Verde (#4CAF50) | ✓ | Operações bem-sucedidas |
| Error | Vermelho (#F44336) | ✗ | Erros não-críticos |
| Warning | Laranja (#FF9800) | ⚠ | Avisos importantes |
| Info | Azul (#2196F3) | ℹ | Informações gerais |

### 📝 Exemplo Prático: Cadastro de Aluno

```csharp
private void BtnSalvar_Click(object? sender, EventArgs e)
{
    try
    {
        // Validações
        if (string.IsNullOrWhiteSpace(txtNome.Text))
        {
            ToastNotification.Warning("Por favor, preencha o nome do aluno");
            txtNome.Focus();
            return;
        }

        // Salvar
        var aluno = new Aluno { Nome = txtNome.Text, /* ... */ };
        var dal = new AlunoDAL();
        dal.Inserir(aluno);

        // Toast de sucesso
        ToastNotification.Success($"Aluno {aluno.Nome} cadastrado com sucesso!");

        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    catch (Exception ex)
    {
        ToastNotification.Error($"Erro: {ex.Message}");
    }
}
```

---

## 2. LoadingPanel

### 📖 O que é?
Overlay semi-transparente com spinner animado que bloqueia a interação durante operações longas.

### 🎯 Quando usar?
- ✅ Consultas ao banco de dados (> 1 segundo)
- ✅ Operações de rede
- ✅ Processamento de arquivos grandes
- ✅ Cálculos complexos

### 💻 Como usar?

```csharp
using BibliotecaJK.Components;

public class FormCadastroAluno : Form
{
    private LoadingPanel loadingPanel;

    private void InitializeComponent()
    {
        // ... outros controles ...

        // Criar LoadingPanel (SEMPRE por último, para ficar no topo)
        loadingPanel = new LoadingPanel();
        this.Controls.Add(loadingPanel);
    }

    // Método 1: Manual (controle total)
    private async void BtnBuscar_Click(object? sender, EventArgs e)
    {
        loadingPanel.Mensagem = "Buscando alunos...";
        loadingPanel.Show();

        try
        {
            await Task.Run(() => {
                // Operação pesada
                Thread.Sleep(2000);
                alunos = dal.Listar();
            });

            AtualizarGrid(alunos);
        }
        finally
        {
            loadingPanel.Hide();
        }
    }

    // Método 2: Automático (RECOMENDADO para operações síncronas)
    private void BtnCarregar_Click(object? sender, EventArgs e)
    {
        loadingPanel.ShowWhile(() => {
            // Código executado em background
            alunos = dal.Listar();

            // Atualizar UI deve ser no Invoke
            this.Invoke((MethodInvoker)delegate {
                AtualizarGrid(alunos);
            });
        }, "Carregando dados...");
    }
}
```

### ⚠️ IMPORTANTE: Thread Safety

```csharp
// ❌ ERRADO - Atualizar UI diretamente do background
loadingPanel.ShowWhile(() => {
    var dados = dal.Listar();
    dgvDados.DataSource = dados; // ERRO!
}, "Carregando...");

// ✅ CORRETO - Usar Invoke para atualizar UI
loadingPanel.ShowWhile(() => {
    var dados = dal.Listar();
    this.Invoke((MethodInvoker)delegate {
        dgvDados.DataSource = dados; // OK!
    });
}, "Carregando...");
```

---

## 3. ThemeManager

### 📖 O que é?
Sistema completo de temas claro/escuro com paletas Material Design.

### 💻 Como usar?

```csharp
using BibliotecaJK.Components;

// Em FormPrincipal (JÁ IMPLEMENTADO):
var btnModoEscuro = ThemeManager.CreateThemeToggleButton();
pnlSidebar.Controls.Add(btnModoEscuro);

// Em outros formulários:
public class MeuForm : Form
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // Aplicar tema atual
        ThemeManager.ApplyTheme(this, ThemeManager.IsDarkMode);
    }
}

// Usar cores do tema em código:
panel.BackColor = ThemeManager.GetColor(
    () => ThemeManager.Light.Surface,
    () => ThemeManager.Dark.Surface
);
```

### 🎨 Paleta de Cores

#### Modo Claro:
```csharp
Background:     #F5F5FA (245,245,250)
Surface:        #FFFFFF (Branco)
Primary:        #3F51B5 (63,81,181)   - Indigo
Secondary:      #2196F3 (33,150,243)  - Azul
Text:           #212121 (33,33,33)
TextSecondary:  Gray
```

#### Modo Escuro:
```csharp
Background:     #121212 (18,18,18)
Surface:        #1E1E1E (30,30,30)
Primary:        #6478DC (100,120,220) - Indigo claro
Secondary:      #50B4FA (80,180,250)  - Azul claro
Text:           #E6E6E6 (230,230,230)
TextSecondary:  #A0A0A0 (160,160,160)
```

---

## 4. InputMaskHelper

### 📖 O que é?
Coleção de máscaras de input prontas e validadores para dados brasileiros.

### 💻 Máscaras Prontas

```csharp
using BibliotecaJK.Components;

// CPF
var txtCPF = InputMaskHelper.CreateCPFTextBox();
txtCPF.Location = new Point(100, 50);
this.Controls.Add(txtCPF);

// Telefone
var txtTel = InputMaskHelper.CreateTelefoneTextBox();

// CEP
var txtCEP = InputMaskHelper.CreateCEPTextBox();

// ISBN-13
var txtISBN = InputMaskHelper.CreateISBNTextBox();

// Data
var txtData = InputMaskHelper.CreateDataTextBox();
```

### ✅ Validação com Feedback Visual

```csharp
var txtCPF = InputMaskHelper.CreateCPFTextBox();
this.Controls.Add(txtCPF);

// Adicionar validação visual (✓/✗)
InputMaskHelper.AddValidationFeedback(txtCPF, InputMaskHelper.ValidarCPF);

// Agora ao digitar:
// - Campo fica verde + ✓ se CPF válido
// - Campo fica vermelho + ✗ se CPF inválido
```

### 🔍 Search Box

```csharp
var searchPanel = InputMaskHelper.CreateSearchBox((s, e) => {
    var filtro = ((TextBox)searchPanel.Tag).Text.ToLower();
    var filtrados = todosAlunos.Where(a =>
        a.Nome.ToLower().Contains(filtro) ||
        a.Matricula.Contains(filtro)
    ).ToList();

    dgvAlunos.DataSource = filtrados;
});

searchPanel.Location = new Point(20, 20);
this.Controls.Add(searchPanel);
```

### 🛡️ Validadores

```csharp
// CPF
if (!InputMaskHelper.ValidarCPF(txtCPF.Text))
{
    ToastNotification.Warning("CPF inválido!");
    return;
}

// ISBN-13
if (!InputMaskHelper.ValidarISBN13(txtISBN.Text))
{
    ToastNotification.Warning("ISBN inválido!");
    return;
}
```

### 🔧 Extensions

```csharp
// Apenas números
var txtQuantidade = new TextBox();
txtQuantidade.AllowOnlyNumbers();

// Apenas letras
var txtNome = new TextBox();
txtNome.AllowOnlyLetters();
```

---

## 5. KeyboardShortcutManager

### 📖 O que é?
Gerenciador centralizado de atalhos de teclado com janela de ajuda integrada.

### 💻 Como usar?

```csharp
using BibliotecaJK.Components;

public class MeuForm : Form
{
    private KeyboardShortcutManager _shortcutManager;

    private void InitializeComponent()
    {
        // ... controles ...

        ConfigurarAtalhos();
    }

    private void ConfigurarAtalhos()
    {
        _shortcutManager = new KeyboardShortcutManager(this);

        // Registrar atalhos
        _shortcutManager.RegisterShortcut(
            Keys.Control | Keys.S,
            BtnSalvar_Click,
            "Salvar registro"
        );

        _shortcutManager.RegisterShortcut(
            Keys.Escape,
            () => this.Close(),
            "Cancelar e fechar"
        );

        _shortcutManager.RegisterShortcut(
            Keys.F1,
            () => _shortcutManager.ShowShortcutsHelp(),
            "Mostrar ajuda de atalhos"
        );

        _shortcutManager.RegisterShortcut(
            Keys.Control | Keys.F,
            () => txtBusca.Focus(),
            "Focar na busca"
        );
    }

    private void BtnSalvar_Click(object? sender = null, EventArgs? e = null)
    {
        // Código de salvar...
    }
}
```

### ⌨️ Atalhos Comuns (FormPrincipal)

```
F5              - Atualizar Dashboard
F1              - Ajuda de Atalhos
Ctrl+N          - Novo Empréstimo
Ctrl+D          - Devoluções
Ctrl+E          - Consultar Empréstimos
Ctrl+R          - Reservas
Ctrl+B          - Backup
Alt+1           - Cadastro Alunos
Alt+2           - Cadastro Livros
Ctrl+Shift+N    - Notificações
```

### 🚀 Atalhos Padrão para Formulários

```csharp
// Setup automático de Enter/Esc
KeyboardShortcutManager.CommonShortcuts.SetupFormShortcuts(
    this,
    btnSalvar,    // Ctrl+Enter para salvar
    btnCancelar   // Esc para cancelar
);

// Tab order com Enter
KeyboardShortcutManager.CommonShortcuts.SetupTabOrder(
    txtNome,
    txtCPF,
    txtEmail,
    btnSalvar
);
// Agora Enter navega entre campos
```

---

## 6. Exemplos Práticos

### 📝 Exemplo 1: Formulário de Cadastro Completo

```csharp
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.Components;
using BibliotecaJK.DAL;
using BibliotecaJK.Model;

namespace BibliotecaJK.Forms
{
    public class FormCadastroAlunoModerno : Form
    {
        private LoadingPanel loadingPanel;
        private KeyboardShortcutManager shortcutManager;
        private AlunoDAL dal = new AlunoDAL();

        private TextBox txtNome;
        private MaskedTextBox txtCPF;
        private MaskedTextBox txtTelefone;
        private TextBox txtEmail;
        private TextBox txtMatricula;
        private Button btnSalvar;
        private Button btnCancelar;
        private DataGridView dgvAlunos;
        private Panel searchPanel;

        public FormCadastroAlunoModerno()
        {
            InitializeComponent();
            ConfigurarAtalhos();
            CarregarAlunos();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(900, 600);
            this.Text = "Cadastro de Alunos - Moderno";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeManager.Light.Background;

            // === SEARCH BOX ===
            searchPanel = InputMaskHelper.CreateSearchBox(OnSearchTextChanged);
            searchPanel.Location = new Point(20, 20);
            this.Controls.Add(searchPanel);

            // === GRID DE ALUNOS ===
            dgvAlunos = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(860, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            dgvAlunos.SelectionChanged += DgvAlunos_SelectionChanged;
            this.Controls.Add(dgvAlunos);

            // === FORMULÁRIO ===
            var pnlForm = new Panel
            {
                Location = new Point(20, 390),
                Size = new Size(860, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Nome
            pnlForm.Controls.Add(new Label
            {
                Text = "Nome:",
                Location = new Point(20, 25),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            });
            txtNome = new TextBox
            {
                Location = new Point(110, 23),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10F)
            };
            txtNome.AllowOnlyLetters();
            pnlForm.Controls.Add(txtNome);

            // CPF com validação
            pnlForm.Controls.Add(new Label
            {
                Text = "CPF:",
                Location = new Point(430, 25),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            });
            txtCPF = InputMaskHelper.CreateCPFTextBox();
            txtCPF.Location = new Point(520, 23);
            txtCPF.Size = new Size(200, 25);
            pnlForm.Controls.Add(txtCPF);
            InputMaskHelper.AddValidationFeedback(txtCPF, InputMaskHelper.ValidarCPF);

            // Telefone
            pnlForm.Controls.Add(new Label
            {
                Text = "Telefone:",
                Location = new Point(20, 65),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            });
            txtTelefone = InputMaskHelper.CreateTelefoneTextBox();
            txtTelefone.Location = new Point(110, 63);
            txtTelefone.Size = new Size(200, 25);
            pnlForm.Controls.Add(txtTelefone);

            // Email
            pnlForm.Controls.Add(new Label
            {
                Text = "Email:",
                Location = new Point(330, 65),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            });
            txtEmail = new TextBox
            {
                Location = new Point(420, 63),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10F)
            };
            pnlForm.Controls.Add(txtEmail);

            // Matrícula
            pnlForm.Controls.Add(new Label
            {
                Text = "Matrícula:",
                Location = new Point(20, 105),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            });
            txtMatricula = new TextBox
            {
                Location = new Point(110, 103),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10F)
            };
            pnlForm.Controls.Add(txtMatricula);

            // Botões
            btnSalvar = new Button
            {
                Text = "💾 Salvar (Ctrl+S)",
                Location = new Point(560, 100),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Click += BtnSalvar_Click;
            pnlForm.Controls.Add(btnSalvar);

            btnCancelar = new Button
            {
                Text = "❌ Cancelar (Esc)",
                Location = new Point(710, 100),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.Click += (s, e) => LimparCampos();
            pnlForm.Controls.Add(btnCancelar);

            this.Controls.Add(pnlForm);

            // === LOADING PANEL (ÚLTIMO) ===
            loadingPanel = new LoadingPanel();
            this.Controls.Add(loadingPanel);

            this.ResumeLayout(false);
        }

        private void ConfigurarAtalhos()
        {
            shortcutManager = new KeyboardShortcutManager(this);
            shortcutManager.RegisterShortcut(Keys.Control | Keys.S, () => BtnSalvar_Click(null, null), "Salvar aluno");
            shortcutManager.RegisterShortcut(Keys.Escape, LimparCampos, "Limpar campos");
            shortcutManager.RegisterShortcut(Keys.F1, () => shortcutManager.ShowShortcutsHelp(), "Ajuda");
            shortcutManager.RegisterShortcut(Keys.Control | Keys.F, FocarBusca, "Buscar aluno");

            // Setup Enter/Esc padrão
            KeyboardShortcutManager.CommonShortcuts.SetupFormShortcuts(this, btnSalvar, btnCancelar);

            // Tab order com Enter
            KeyboardShortcutManager.CommonShortcuts.SetupTabOrder(
                txtNome, txtCPF, txtTelefone, txtEmail, txtMatricula, btnSalvar
            );
        }

        private void CarregarAlunos()
        {
            loadingPanel.ShowWhile(() =>
            {
                var alunos = dal.Listar();
                this.Invoke((MethodInvoker)delegate
                {
                    dgvAlunos.DataSource = alunos;
                    if (dgvAlunos.Columns.Count > 0)
                    {
                        dgvAlunos.Columns["Id"].HeaderText = "ID";
                        dgvAlunos.Columns["Nome"].HeaderText = "Nome";
                        dgvAlunos.Columns["CPF"].HeaderText = "CPF";
                        dgvAlunos.Columns["Matricula"].HeaderText = "Matrícula";
                    }
                });
            }, "Carregando alunos...");
        }

        private void OnSearchTextChanged(object? sender, EventArgs e)
        {
            var filtro = ((TextBox)searchPanel.Tag).Text.ToLower();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                CarregarAlunos();
                return;
            }

            var todosAlunos = dal.Listar();
            var filtrados = todosAlunos.Where(a =>
                a.Nome.ToLower().Contains(filtro) ||
                a.CPF.Contains(filtro) ||
                a.Matricula.Contains(filtro)
            ).ToList();

            dgvAlunos.DataSource = filtrados;
        }

        private void DgvAlunos_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvAlunos.SelectedRows.Count > 0)
            {
                var aluno = (Aluno)dgvAlunos.SelectedRows[0].DataBoundItem;
                txtNome.Text = aluno.Nome;
                txtCPF.Text = aluno.CPF;
                txtTelefone.Text = aluno.Telefone ?? "";
                txtEmail.Text = aluno.Email ?? "";
                txtMatricula.Text = aluno.Matricula;
            }
        }

        private void BtnSalvar_Click(object? sender, EventArgs? e)
        {
            try
            {
                // Validações
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    ToastNotification.Warning("Nome é obrigatório!");
                    txtNome.Focus();
                    return;
                }

                if (!InputMaskHelper.ValidarCPF(txtCPF.Text))
                {
                    ToastNotification.Warning("CPF inválido!");
                    txtCPF.Focus();
                    return;
                }

                // Salvar
                var aluno = new Aluno
                {
                    Nome = txtNome.Text.Trim(),
                    CPF = txtCPF.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text.Trim(),
                    Matricula = txtMatricula.Text.Trim()
                };

                loadingPanel.ShowWhile(() =>
                {
                    dal.Inserir(aluno);
                    this.Invoke((MethodInvoker)delegate
                    {
                        ToastNotification.Success($"Aluno {aluno.Nome} cadastrado!");
                        LimparCampos();
                        CarregarAlunos();
                    });
                }, "Salvando aluno...");
            }
            catch (Exception ex)
            {
                ToastNotification.Error($"Erro: {ex.Message}");
            }
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtCPF.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();
            txtMatricula.Clear();
            txtNome.Focus();
        }

        private void FocarBusca()
        {
            ((TextBox)searchPanel.Tag).Focus();
        }
    }
}
```

---

### 📝 Exemplo 2: Formulário com Operações Assíncronas

```csharp
private async void BtnGerarRelatorio_Click(object? sender, EventArgs e)
{
    loadingPanel.Mensagem = "Gerando relatório...";
    loadingPanel.Show();

    try
    {
        var relatorio = await Task.Run(() =>
        {
            // Operação pesada
            return emprestimoService.GerarRelatorioCompleto();
        });

        // Salvar arquivo
        using var saveDialog = new SaveFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf",
            FileName = $"relatorio_{DateTime.Now:yyyyMMdd}.pdf"
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            relatorio.SalvarPDF(saveDialog.FileName);
            ToastNotification.Success("Relatório gerado com sucesso!");
        }
    }
    catch (Exception ex)
    {
        ToastNotification.Error($"Erro: {ex.Message}");
    }
    finally
    {
        loadingPanel.Hide();
    }
}
```

---

## 📊 COMPARAÇÃO ANTES vs DEPOIS

### Antes (Sem Componentes):
```csharp
private void BtnSalvar_Click(object sender, EventArgs e)
{
    if (txtNome.Text == "")
    {
        MessageBox.Show("Preencha o nome"); // Intrusivo
        return;
    }

    dal.Inserir(aluno); // Sem feedback visual
    MessageBox.Show("Salvo com sucesso!"); // Intrusivo
    this.Close();
}
```

### Depois (Com Componentes):
```csharp
private void BtnSalvar_Click(object sender, EventArgs e)
{
    if (string.IsNullOrWhiteSpace(txtNome.Text))
    {
        ToastNotification.Warning("Preencha o nome"); // Não-intrusivo
        txtNome.Focus();
        return;
    }

    loadingPanel.ShowWhile(() => { // Feedback visual
        dal.Inserir(aluno);
        this.Invoke((MethodInvoker)delegate {
            ToastNotification.Success("Salvo!"); // Não-intrusivo
            this.Close();
        });
    }, "Salvando...");
}
```

---

## 🎯 MELHORES PRÁTICAS

### ✅ DO's

1. **Use ToastNotification para feedback não-crítico**
   ```csharp
   ToastNotification.Success("Operação concluída!");
   ```

2. **Use LoadingPanel para operações > 1 segundo**
   ```csharp
   loadingPanel.ShowWhile(() => { /* ... */ }, "Carregando...");
   ```

3. **Valide inputs com feedback visual**
   ```csharp
   InputMaskHelper.AddValidationFeedback(txtCPF, InputMaskHelper.ValidarCPF);
   ```

4. **Registre atalhos úteis**
   ```csharp
   _shortcutManager.RegisterShortcut(Keys.Control | Keys.S, Salvar, "Salvar");
   ```

5. **Ofereça modo escuro**
   ```csharp
   var btnTheme = ThemeManager.CreateThemeToggleButton();
   ```

### ❌ DON'Ts

1. **NÃO use MessageBox para tudo**
   ```csharp
   // ❌ Ruim
   MessageBox.Show("Salvo com sucesso!");

   // ✅ Bom
   ToastNotification.Success("Salvo com sucesso!");
   ```

2. **NÃO deixe operações longas sem feedback**
   ```csharp
   // ❌ Ruim
   var dados = dal.Listar(); // Usuário não sabe que está carregando

   // ✅ Bom
   loadingPanel.ShowWhile(() => dados = dal.Listar(), "Carregando...");
   ```

3. **NÃO ignore validação de CPF/ISBN**
   ```csharp
   // ❌ Ruim
   aluno.CPF = txtCPF.Text;

   // ✅ Bom
   if (!InputMaskHelper.ValidarCPF(txtCPF.Text)) return;
   aluno.CPF = txtCPF.Text;
   ```

---

## 🚀 CHECKLIST DE IMPLEMENTAÇÃO

Ao criar um novo formulário, siga este checklist:

- [ ] Adicionar `using BibliotecaJK.Components;`
- [ ] Criar LoadingPanel e adicionar por último
- [ ] Criar KeyboardShortcutManager e configurar atalhos
- [ ] Usar InputMaskHelper para CPF/Telefone/CEP/ISBN
- [ ] Usar ToastNotification em vez de MessageBox
- [ ] Adicionar search-as-you-type se tiver grid
- [ ] Configurar Enter/Esc com CommonShortcuts
- [ ] Aplicar ThemeManager se necessário
- [ ] Testar todos os atalhos de teclado
- [ ] Testar modo escuro

---

## 📞 SUPORTE

Dúvidas sobre algum componente? Verifique:
1. Este guia (README_UX_UI_GUIDE.md)
2. Código do FormPrincipal.cs (exemplo completo)
3. FormCadastroAlunoModerno.cs (exemplo prático)

---

**BibliotecaJK v3.2 - UX/UI Profissional** 🎨✨
