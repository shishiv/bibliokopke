using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace BibliotecaJK.Helpers
{
    /// <summary>
    /// Armazena o estado de um formulário (posição, tamanho, estado da janela)
    /// </summary>
    public class FormState
    {
        public FormWindowState WindowState { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }

        public FormState()
        {
            WindowState = FormWindowState.Normal;
            Location = Point.Empty;
            Size = Size.Empty;
        }
    }

    /// <summary>
    /// Armazena o estado de um DataGridView (larguras de colunas, ordenação)
    /// </summary>
    public class DataGridState
    {
        public Dictionary<string, int> ColumnWidths { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }

        public DataGridState()
        {
            ColumnWidths = new Dictionary<string, int>();
            SortColumn = null;
            SortOrder = null;
        }
    }

    /// <summary>
    /// Gerenciador de estado de formulários
    /// Persiste e restaura posição, tamanho, estado de janela, estado de grids e filtros
    /// </summary>
    public static class FormStateManager
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Retorna o caminho base para armazenamento de estados de formulários
        /// </summary>
        /// <returns>Caminho completo do diretório de estados</returns>
        public static string GetConfigPath()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string configPath = Path.Combine(localAppData, "BibliotecaJK", "FormStates");

            try
            {
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao criar diretório de estados: {ex.Message}");
            }

            return configPath;
        }

        /// <summary>
        /// Valida se uma localização está visível em alguma tela
        /// </summary>
        /// <param name="location">Localização do formulário</param>
        /// <param name="size">Tamanho do formulário</param>
        /// <returns>True se a localização é válida</returns>
        public static bool ValidateLocation(Point location, Size size)
        {
            if (location.IsEmpty || size.IsEmpty)
                return false;

            // Cria um retângulo representando o formulário
            Rectangle formRect = new Rectangle(location, size);

            // Verifica se pelo menos 50% do formulário está visível em alguma tela
            foreach (Screen screen in Screen.AllScreens)
            {
                Rectangle intersection = Rectangle.Intersect(formRect, screen.WorkingArea);

                // Se a interseção tiver pelo menos 50% da área do formulário, considera válido
                if (intersection.Width * intersection.Height >= (size.Width * size.Height) / 2)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Salva o estado de um formulário (posição, tamanho, estado da janela)
        /// </summary>
        /// <param name="form">Formulário a ter o estado salvo</param>
        /// <returns>True se salvou com sucesso</returns>
        public static bool SaveFormState(Form form)
        {
            if (form == null)
                return false;

            try
            {
                string configPath = GetConfigPath();
                string fileName = Path.Combine(configPath, $"{form.Name}.json");

                FormState state = new FormState();

                // Salva o estado da janela
                state.WindowState = form.WindowState;

                // Se estiver minimizado ou maximizado, salva o RestoreBounds
                if (form.WindowState == FormWindowState.Normal)
                {
                    state.Location = form.Location;
                    state.Size = form.Size;
                }
                else
                {
                    state.Location = form.RestoreBounds.Location;
                    state.Size = form.RestoreBounds.Size;
                }

                string json = JsonSerializer.Serialize(state, JsonOptions);
                File.WriteAllText(fileName, json);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar estado do formulário {form.Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Restaura o estado de um formulário (posição, tamanho, estado da janela)
        /// </summary>
        /// <param name="form">Formulário a ter o estado restaurado</param>
        /// <returns>True se restaurou com sucesso</returns>
        public static bool RestoreFormState(Form form)
        {
            if (form == null)
                return false;

            try
            {
                string configPath = GetConfigPath();
                string fileName = Path.Combine(configPath, $"{form.Name}.json");

                if (!File.Exists(fileName))
                    return false;

                string json = File.ReadAllText(fileName);
                FormState? state = JsonSerializer.Deserialize<FormState>(json, JsonOptions);

                if (state == null)
                    return false;

                // Valida se a localização é válida antes de aplicar
                if (ValidateLocation(state.Location, state.Size))
                {
                    // Define StartPosition como Manual para usar Location customizado
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = state.Location;
                    form.Size = state.Size;
                }
                else
                {
                    // Se a localização não for válida, centraliza na tela
                    form.StartPosition = FormStartPosition.CenterScreen;
                }

                // Restaura o estado da janela (mas não minimizado)
                if (state.WindowState != FormWindowState.Minimized)
                {
                    form.WindowState = state.WindowState;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao restaurar estado do formulário {form.Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Salva o estado de um DataGridView (larguras de colunas, ordenação)
        /// </summary>
        /// <param name="dgv">DataGridView a ter o estado salvo</param>
        /// <param name="identifier">Identificador único para este grid</param>
        /// <returns>True se salvou com sucesso</returns>
        public static bool SaveDataGridState(DataGridView dgv, string identifier)
        {
            if (dgv == null || string.IsNullOrWhiteSpace(identifier))
                return false;

            try
            {
                string configPath = GetConfigPath();
                string fileName = Path.Combine(configPath, $"{identifier}_grid.json");

                DataGridState state = new DataGridState();

                // Salva larguras das colunas
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.Visible)
                    {
                        state.ColumnWidths[col.Name] = col.Width;
                    }
                }

                // Salva informações de ordenação
                if (dgv.SortedColumn != null)
                {
                    state.SortColumn = dgv.SortedColumn.Name;
                    state.SortOrder = dgv.SortOrder.ToString();
                }

                string json = JsonSerializer.Serialize(state, JsonOptions);
                File.WriteAllText(fileName, json);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar estado do grid {identifier}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Restaura o estado de um DataGridView (larguras de colunas, ordenação)
        /// </summary>
        /// <param name="dgv">DataGridView a ter o estado restaurado</param>
        /// <param name="identifier">Identificador único para este grid</param>
        /// <returns>True se restaurou com sucesso</returns>
        public static bool RestoreDataGridState(DataGridView dgv, string identifier)
        {
            if (dgv == null || string.IsNullOrWhiteSpace(identifier))
                return false;

            try
            {
                string configPath = GetConfigPath();
                string fileName = Path.Combine(configPath, $"{identifier}_grid.json");

                if (!File.Exists(fileName))
                    return false;

                string json = File.ReadAllText(fileName);
                DataGridState? state = JsonSerializer.Deserialize<DataGridState>(json, JsonOptions);

                if (state == null)
                    return false;

                // Restaura larguras das colunas
                foreach (var kvp in state.ColumnWidths)
                {
                    if (dgv.Columns.Contains(kvp.Key))
                    {
                        var column = dgv.Columns[kvp.Key];
                        if (column != null && column.Visible)
                        {
                            // Valida largura mínima e máxima
                            int width = kvp.Value;
                            if (width < column.MinimumWidth)
                                width = column.MinimumWidth;

                            column.Width = width;
                        }
                    }
                }

                // Restaura ordenação
                if (!string.IsNullOrEmpty(state.SortColumn) &&
                    !string.IsNullOrEmpty(state.SortOrder) &&
                    dgv.Columns.Contains(state.SortColumn))
                {
                    var sortColumn = dgv.Columns[state.SortColumn];
                    if (sortColumn != null && sortColumn.Visible)
                    {
                        ListSortDirection direction = state.SortOrder == "Ascending"
                            ? ListSortDirection.Ascending
                            : ListSortDirection.Descending;

                        dgv.Sort(sortColumn, direction);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao restaurar estado do grid {identifier}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Salva valores de filtros de um formulário
        /// </summary>
        /// <param name="formName">Nome do formulário</param>
        /// <param name="filters">Dicionário com pares chave-valor dos filtros</param>
        /// <returns>True se salvou com sucesso</returns>
        public static bool SaveFilterState(string formName, Dictionary<string, string> filters)
        {
            if (string.IsNullOrWhiteSpace(formName) || filters == null)
                return false;

            try
            {
                string configPath = GetConfigPath();
                string fileName = Path.Combine(configPath, $"{formName}_filters.json");

                string json = JsonSerializer.Serialize(filters, JsonOptions);
                File.WriteAllText(fileName, json);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar filtros do formulário {formName}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Restaura valores de filtros de um formulário
        /// </summary>
        /// <param name="formName">Nome do formulário</param>
        /// <returns>Dicionário com pares chave-valor dos filtros, ou null se não encontrado</returns>
        public static Dictionary<string, string>? RestoreFilterState(string formName)
        {
            if (string.IsNullOrWhiteSpace(formName))
                return null;

            try
            {
                string configPath = GetConfigPath();
                string fileName = Path.Combine(configPath, $"{formName}_filters.json");

                if (!File.Exists(fileName))
                    return null;

                string json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json, JsonOptions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao restaurar filtros do formulário {formName}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Limpa todos os estados salvos
        /// </summary>
        /// <returns>True se limpou com sucesso</returns>
        public static bool ClearAllStates()
        {
            try
            {
                string configPath = GetConfigPath();

                if (Directory.Exists(configPath))
                {
                    // Apaga todos os arquivos JSON no diretório
                    var files = Directory.GetFiles(configPath, "*.json");
                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Erro ao deletar arquivo {file}: {ex.Message}");
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao limpar estados: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Limpa o estado de um formulário específico
        /// </summary>
        /// <param name="formName">Nome do formulário</param>
        /// <returns>True se limpou com sucesso</returns>
        public static bool ClearFormState(string formName)
        {
            if (string.IsNullOrWhiteSpace(formName))
                return false;

            try
            {
                string configPath = GetConfigPath();

                // Remove arquivos relacionados ao formulário
                string[] patterns = new[]
                {
                    $"{formName}.json",           // Estado do formulário
                    $"{formName}_grid.json",      // Estado do grid
                    $"{formName}_filters.json"    // Estado dos filtros
                };

                bool anyDeleted = false;
                foreach (var pattern in patterns)
                {
                    string fileName = Path.Combine(configPath, pattern);
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                        anyDeleted = true;
                    }
                }

                return anyDeleted;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao limpar estado do formulário {formName}: {ex.Message}");
                return false;
            }
        }
    }
}
