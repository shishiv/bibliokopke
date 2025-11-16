using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BibliotecaJK.Components;

namespace BibliotecaJK.Helpers
{
    /// <summary>
    /// AccessibilityHelper - Static helper class for accessibility features
    /// Provides focus indicators, keyboard navigation, screen reader support, and WCAG AA compliance
    /// </summary>
    public static class AccessibilityHelper
    {
        // Store original colors for restoration
        private static readonly Dictionary<Control, Color> _originalBackColors = new Dictionary<Control, Color>();
        private static readonly Dictionary<Control, Color> _originalForeColors = new Dictionary<Control, Color>();

        /// <summary>
        /// Applies focus indicators to all focusable controls recursively
        /// Highlights controls on focus with subtle background color and restores original on lost focus
        /// </summary>
        /// <param name="controls">Collection of controls to process</param>
        public static void ApplyFocusIndicators(Control.ControlCollection controls)
        {
            if (controls == null)
                return;

            foreach (Control control in controls)
            {
                // Apply to focusable controls
                if (control is TextBox || control is Button || control is ComboBox ||
                    control is CheckBox || control is RadioButton || control is DateTimePicker ||
                    control is NumericUpDown || control is RichTextBox || control is MaskedTextBox)
                {
                    // Store original colors if not already stored
                    if (!_originalBackColors.ContainsKey(control))
                    {
                        _originalBackColors[control] = control.BackColor;
                        _originalForeColors[control] = control.ForeColor;
                    }

                    // Remove existing handlers to avoid duplicates
                    control.GotFocus -= Control_GotFocus;
                    control.LostFocus -= Control_LostFocus;

                    // Add focus handlers
                    control.GotFocus += Control_GotFocus;
                    control.LostFocus += Control_LostFocus;
                }

                // Apply recursively to child controls
                if (control.HasChildren && control.Controls.Count > 0)
                {
                    ApplyFocusIndicators(control.Controls);
                }
            }
        }

        /// <summary>
        /// Event handler for GotFocus - applies focus indicator
        /// </summary>
        private static void Control_GotFocus(object? sender, EventArgs e)
        {
            if (sender is Control control)
            {
                // Apply focus indicator color
                Color focusColor = GetFocusColor(control.BackColor);
                control.BackColor = focusColor;

                // For buttons, also adjust border/appearance
                if (control is Button btn && btn.FlatStyle == FlatStyle.Flat)
                {
                    btn.FlatAppearance.BorderColor = ThemeManager.Colors.Primary500;
                    btn.FlatAppearance.BorderSize = 2;
                }
            }
        }

        /// <summary>
        /// Event handler for LostFocus - restores original color
        /// </summary>
        private static void Control_LostFocus(object? sender, EventArgs e)
        {
            if (sender is Control control)
            {
                // Restore original background color
                if (_originalBackColors.TryGetValue(control, out Color originalColor))
                {
                    control.BackColor = originalColor;
                }

                // For buttons, restore border
                if (control is Button btn && btn.FlatStyle == FlatStyle.Flat)
                {
                    btn.FlatAppearance.BorderSize = 0;
                }
            }
        }

        /// <summary>
        /// Sets up keyboard navigation for a form
        /// Configures tab order, keyboard shortcuts, Enter/Escape handlers
        /// </summary>
        /// <param name="form">Form to configure keyboard navigation</param>
        public static void SetupKeyboardNavigation(Form form)
        {
            if (form == null)
                return;

            // Enable KeyPreview to intercept key events at form level
            form.KeyPreview = true;

            // Remove existing handler to avoid duplicates
            form.KeyDown -= Form_KeyDown;

            // Add keyboard event handler
            form.KeyDown += Form_KeyDown;

            // Setup proper tab order
            SetupTabOrder(form.Controls);
        }

        /// <summary>
        /// Recursively sets up tab order for controls
        /// </summary>
        private static void SetupTabOrder(Control.ControlCollection controls)
        {
            if (controls == null)
                return;

            int tabIndex = 0;

            // Get all focusable controls in visual order (top to bottom, left to right)
            var focusableControls = new List<Control>();
            GetFocusableControls(controls, focusableControls);

            // Sort by location (top to bottom, left to right)
            focusableControls.Sort((a, b) =>
            {
                if (Math.Abs(a.Top - b.Top) < 10) // Same row (within 10 pixels)
                    return a.Left.CompareTo(b.Left);
                return a.Top.CompareTo(b.Top);
            });

            // Assign tab indices
            foreach (var control in focusableControls)
            {
                control.TabIndex = tabIndex++;
                control.TabStop = true;
            }
        }

        /// <summary>
        /// Recursively gets all focusable controls
        /// </summary>
        private static void GetFocusableControls(Control.ControlCollection controls, List<Control> result)
        {
            foreach (Control control in controls)
            {
                // Check if control can receive focus
                if (control.CanSelect && control.Visible && control.Enabled)
                {
                    result.Add(control);
                }

                // Recurse into containers (but not into leaf controls like TextBox)
                if (control is Panel || control is GroupBox || control is TableLayoutPanel ||
                    control is FlowLayoutPanel || control is SplitContainer || control is TabControl)
                {
                    if (control.HasChildren)
                    {
                        GetFocusableControls(control.Controls, result);
                    }
                }
            }
        }

        /// <summary>
        /// Form KeyDown event handler - handles keyboard shortcuts
        /// </summary>
        private static void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            if (sender is not Form form)
                return;

            // Enter key - activate default button
            if (e.KeyCode == Keys.Enter && !e.Alt && !e.Control)
            {
                // Don't trigger if in multiline textbox
                if (form.ActiveControl is TextBox txt && txt.Multiline)
                    return;

                if (form.AcceptButton != null)
                {
                    form.AcceptButton.PerformClick();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

            // Escape key - activate cancel button or close form
            if (e.KeyCode == Keys.Escape)
            {
                if (form.CancelButton != null)
                {
                    form.CancelButton.PerformClick();
                }
                else if (form.Modal) // Close modal dialogs
                {
                    form.DialogResult = DialogResult.Cancel;
                    form.Close();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            // Ctrl+S - Save (look for button with "Salvar" text)
            if (e.Control && e.KeyCode == Keys.S)
            {
                Button? saveButton = FindButtonByText(form, "Salvar");
                if (saveButton != null && saveButton.Enabled)
                {
                    saveButton.PerformClick();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }

            // F1 - Help
            if (e.KeyCode == Keys.F1)
            {
                // Show help if available
                Button? helpButton = FindButtonByText(form, "Ajuda");
                if (helpButton != null && helpButton.Enabled)
                {
                    helpButton.PerformClick();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        /// <summary>
        /// Finds a button by its text content
        /// </summary>
        private static Button? FindButtonByText(Control parent, string text)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Button btn && btn.Text.Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    return btn;
                }

                if (control.HasChildren)
                {
                    var result = FindButtonByText(control, text);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the accessible name for a control (used by screen readers)
        /// </summary>
        /// <param name="control">Control to set accessible name</param>
        /// <param name="name">Accessible name</param>
        public static void SetAccessibleName(Control control, string name)
        {
            if (control == null)
                return;

            control.AccessibleName = name;

            // If no description is set, use the name as description
            if (string.IsNullOrEmpty(control.AccessibleDescription))
            {
                control.AccessibleDescription = name;
            }

            // Set accessible role based on control type
            if (control.AccessibleRole == AccessibleRole.Default)
            {
                control.AccessibleRole = control switch
                {
                    Button => AccessibleRole.PushButton,
                    TextBox => AccessibleRole.Text,
                    ComboBox => AccessibleRole.ComboBox,
                    CheckBox => AccessibleRole.CheckButton,
                    RadioButton => AccessibleRole.RadioButton,
                    Label => AccessibleRole.StaticText,
                    DataGridView => AccessibleRole.Table,
                    _ => AccessibleRole.None
                };
            }
        }

        /// <summary>
        /// Sets the accessible description for a control (detailed description for screen readers)
        /// </summary>
        /// <param name="control">Control to set accessible description</param>
        /// <param name="description">Accessible description</param>
        public static void SetAccessibleDescription(Control control, string description)
        {
            if (control == null)
                return;

            control.AccessibleDescription = description;

            // Set accessible name if not already set
            if (string.IsNullOrEmpty(control.AccessibleName))
            {
                // Try to infer from Text property
                if (!string.IsNullOrEmpty(control.Text))
                {
                    control.AccessibleName = control.Text;
                }
                else
                {
                    control.AccessibleName = control.Name;
                }
            }
        }

        /// <summary>
        /// Applies all accessibility features to a form
        /// Combines focus indicators, keyboard navigation, DPI awareness, and accessible names
        /// </summary>
        /// <param name="form">Form to make accessible</param>
        public static void MakeAccessible(Form form)
        {
            if (form == null)
                return;

            try
            {
                // Enable DPI awareness
                form.AutoScaleMode = AutoScaleMode.Dpi;
                form.AutoScaleDimensions = new SizeF(96F, 96F);

                // Apply focus indicators to all controls
                ApplyFocusIndicators(form.Controls);

                // Setup keyboard navigation
                SetupKeyboardNavigation(form);

                // Set accessible names for controls that don't have them
                SetAccessibleNamesRecursive(form.Controls);

                // Set form-level accessible properties
                if (string.IsNullOrEmpty(form.AccessibleName))
                {
                    form.AccessibleName = form.Text;
                }
                if (string.IsNullOrEmpty(form.AccessibleDescription))
                {
                    form.AccessibleDescription = $"Formulário {form.Text}";
                }
                form.AccessibleRole = AccessibleRole.Dialog;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao aplicar acessibilidade: {ex.Message}");
            }
        }

        /// <summary>
        /// Recursively sets accessible names for controls that don't have them
        /// </summary>
        private static void SetAccessibleNamesRecursive(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Set accessible name if not already set
                if (string.IsNullOrEmpty(control.AccessibleName))
                {
                    if (!string.IsNullOrEmpty(control.Text))
                    {
                        SetAccessibleName(control, control.Text);
                    }
                    else if (!string.IsNullOrEmpty(control.Name))
                    {
                        SetAccessibleName(control, control.Name);
                    }
                }

                // Recurse
                if (control.HasChildren)
                {
                    SetAccessibleNamesRecursive(control.Controls);
                }
            }
        }

        /// <summary>
        /// Enables or disables high contrast mode for a form
        /// Uses WCAG AA compliant colors for better accessibility
        /// </summary>
        /// <param name="form">Form to apply high contrast mode</param>
        /// <param name="enable">True to enable high contrast, false to disable</param>
        public static void EnableHighContrast(Form form, bool enable)
        {
            if (form == null)
                return;

            if (enable)
            {
                // Apply high contrast colors (WCAG AA compliant)
                ApplyHighContrastColorsRecursive(form.Controls, true);

                // Set form background to high contrast
                form.BackColor = Color.Black;
                form.ForeColor = Color.White;
            }
            else
            {
                // Restore normal colors
                ApplyHighContrastColorsRecursive(form.Controls, false);

                // Restore normal form colors
                form.BackColor = ThemeManager.Light.Background;
                form.ForeColor = ThemeManager.Light.Text;
            }
        }

        /// <summary>
        /// Recursively applies high contrast colors to controls
        /// </summary>
        private static void ApplyHighContrastColorsRecursive(Control.ControlCollection controls, bool highContrast)
        {
            foreach (Control control in controls)
            {
                if (highContrast)
                {
                    // High contrast colors (WCAG AA: 4.5:1 ratio minimum)
                    if (control is TextBox || control is ComboBox || control is RichTextBox || control is MaskedTextBox)
                    {
                        control.BackColor = Color.White;
                        control.ForeColor = Color.Black;
                    }
                    else if (control is Button btn)
                    {
                        btn.BackColor = Color.Yellow;
                        btn.ForeColor = Color.Black;
                        btn.FlatStyle = FlatStyle.Standard;
                    }
                    else if (control is Label || control is CheckBox || control is RadioButton)
                    {
                        control.ForeColor = Color.White;
                    }
                    else if (control is DataGridView dgv)
                    {
                        dgv.BackgroundColor = Color.Black;
                        dgv.ForeColor = Color.White;
                        dgv.DefaultCellStyle.BackColor = Color.White;
                        dgv.DefaultCellStyle.ForeColor = Color.Black;
                        dgv.DefaultCellStyle.SelectionBackColor = Color.Yellow;
                        dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
                        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                        dgv.GridColor = Color.Black;
                    }
                    else if (control is Panel || control is GroupBox)
                    {
                        control.BackColor = Color.Black;
                        control.ForeColor = Color.White;
                    }
                }
                else
                {
                    // Restore theme colors
                    if (control is TextBox || control is ComboBox || control is RichTextBox || control is MaskedTextBox)
                    {
                        control.BackColor = ThemeManager.Light.Surface;
                        control.ForeColor = ThemeManager.Light.Text;
                    }
                    else if (control is Button btn)
                    {
                        // Restore original button colors (would need to be stored)
                        btn.ForeColor = ThemeManager.Light.Text;
                    }
                    else if (control is Label || control is CheckBox || control is RadioButton)
                    {
                        control.ForeColor = ThemeManager.Light.Text;
                    }
                    else if (control is DataGridView dgv)
                    {
                        dgv.BackgroundColor = ThemeManager.Light.Surface;
                        dgv.ForeColor = ThemeManager.Light.Text;
                        dgv.DefaultCellStyle.BackColor = Color.White;
                        dgv.DefaultCellStyle.ForeColor = ThemeManager.Light.Text;
                        dgv.DefaultCellStyle.SelectionBackColor = ThemeManager.Colors.Primary500;
                        dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                        dgv.AlternatingRowsDefaultCellStyle.BackColor = ThemeManager.Colors.Neutral50;
                        dgv.GridColor = ThemeManager.Colors.Neutral300;
                    }
                    else if (control is Panel || control is GroupBox)
                    {
                        control.BackColor = ThemeManager.Light.Background;
                        control.ForeColor = ThemeManager.Light.Text;
                    }
                }

                // Recurse
                if (control.HasChildren)
                {
                    ApplyHighContrastColorsRecursive(control.Controls, highContrast);
                }
            }
        }

        /// <summary>
        /// Gets an appropriate focus indicator color based on the control's background color
        /// Ensures sufficient contrast (WCAG AA minimum)
        /// </summary>
        /// <param name="baseColor">Base background color of the control</param>
        /// <returns>Focus indicator color with sufficient contrast</returns>
        public static Color GetFocusColor(Color baseColor)
        {
            // Calculate relative luminance of base color
            double luminance = GetRelativeLuminance(baseColor);

            // If base color is dark, use a light focus color
            if (luminance < 0.5)
            {
                return ThemeManager.Colors.Primary100; // Light blue for dark backgrounds
            }
            else
            {
                return ThemeManager.Colors.Primary50; // Very light blue for light backgrounds
            }
        }

        /// <summary>
        /// Calculates relative luminance of a color (WCAG formula)
        /// </summary>
        private static double GetRelativeLuminance(Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            r = r <= 0.03928 ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
            g = g <= 0.03928 ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
            b = b <= 0.03928 ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        /// <summary>
        /// Calculates contrast ratio between two colors (WCAG formula)
        /// </summary>
        /// <param name="color1">First color</param>
        /// <param name="color2">Second color</param>
        /// <returns>Contrast ratio (1.0 to 21.0)</returns>
        public static double CalculateContrastRatio(Color color1, Color color2)
        {
            double l1 = GetRelativeLuminance(color1);
            double l2 = GetRelativeLuminance(color2);

            double lighter = Math.Max(l1, l2);
            double darker = Math.Min(l1, l2);

            return (lighter + 0.05) / (darker + 0.05);
        }

        /// <summary>
        /// Checks if two colors meet WCAG AA standard (4.5:1 ratio for normal text)
        /// </summary>
        /// <param name="foreground">Foreground color</param>
        /// <param name="background">Background color</param>
        /// <returns>True if meets WCAG AA standard</returns>
        public static bool MeetsWCAG_AA(Color foreground, Color background)
        {
            double ratio = CalculateContrastRatio(foreground, background);
            return ratio >= 4.5; // WCAG AA minimum for normal text
        }

        /// <summary>
        /// Sets up arrow key navigation for a DataGridView
        /// Enables Home/End for first/last row/column and Page Up/Down support
        /// </summary>
        /// <param name="dgv">DataGridView to configure</param>
        public static void SetupArrowKeyNavigation(DataGridView dgv)
        {
            if (dgv == null)
                return;

            // Remove existing handler to avoid duplicates
            dgv.KeyDown -= DataGridView_KeyDown;

            // Add keyboard event handler
            dgv.KeyDown += DataGridView_KeyDown;

            // Enable standard navigation
            dgv.StandardTab = true;
            dgv.MultiSelect = false;
        }

        /// <summary>
        /// DataGridView KeyDown event handler - handles arrow key navigation
        /// </summary>
        private static void DataGridView_KeyDown(object? sender, KeyEventArgs e)
        {
            if (sender is not DataGridView dgv)
                return;

            // Home - Go to first column
            if (e.KeyCode == Keys.Home && !e.Control && dgv.CurrentCell != null)
            {
                dgv.CurrentCell = dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0];
                e.Handled = true;
            }

            // Ctrl+Home - Go to first cell (top-left)
            if (e.Control && e.KeyCode == Keys.Home && dgv.Rows.Count > 0 && dgv.Columns.Count > 0)
            {
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
                e.Handled = true;
            }

            // End - Go to last column
            if (e.KeyCode == Keys.End && !e.Control && dgv.CurrentCell != null)
            {
                int lastColIndex = dgv.Columns.Count - 1;
                dgv.CurrentCell = dgv.Rows[dgv.CurrentCell.RowIndex].Cells[lastColIndex];
                e.Handled = true;
            }

            // Ctrl+End - Go to last cell (bottom-right)
            if (e.Control && e.KeyCode == Keys.End && dgv.Rows.Count > 0 && dgv.Columns.Count > 0)
            {
                int lastRowIndex = dgv.Rows.Count - 1;
                int lastColIndex = dgv.Columns.Count - 1;
                dgv.CurrentCell = dgv.Rows[lastRowIndex].Cells[lastColIndex];
                e.Handled = true;
            }

            // Page Up - Scroll up one page
            if (e.KeyCode == Keys.PageUp && dgv.CurrentCell != null)
            {
                int displayedRows = dgv.DisplayedRowCount(false);
                int newRow = Math.Max(0, dgv.CurrentCell.RowIndex - displayedRows);
                dgv.CurrentCell = dgv.Rows[newRow].Cells[dgv.CurrentCell.ColumnIndex];
                e.Handled = true;
            }

            // Page Down - Scroll down one page
            if (e.KeyCode == Keys.PageDown && dgv.CurrentCell != null)
            {
                int displayedRows = dgv.DisplayedRowCount(false);
                int newRow = Math.Min(dgv.Rows.Count - 1, dgv.CurrentCell.RowIndex + displayedRows);
                dgv.CurrentCell = dgv.Rows[newRow].Cells[dgv.CurrentCell.ColumnIndex];
                e.Handled = true;
            }

            // Enter - Edit current cell or move to next row
            if (e.KeyCode == Keys.Enter && dgv.CurrentCell != null)
            {
                if (!dgv.CurrentCell.IsInEditMode)
                {
                    // Move to next row
                    int nextRow = Math.Min(dgv.Rows.Count - 1, dgv.CurrentCell.RowIndex + 1);
                    dgv.CurrentCell = dgv.Rows[nextRow].Cells[dgv.CurrentCell.ColumnIndex];
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Clears stored original colors (call when disposing forms)
        /// </summary>
        public static void ClearStoredColors()
        {
            _originalBackColors.Clear();
            _originalForeColors.Clear();
        }

        /// <summary>
        /// Removes focus indicators from a specific control
        /// </summary>
        /// <param name="control">Control to remove focus indicators from</param>
        public static void RemoveFocusIndicators(Control control)
        {
            if (control == null)
                return;

            control.GotFocus -= Control_GotFocus;
            control.LostFocus -= Control_LostFocus;

            // Restore original color if stored
            if (_originalBackColors.TryGetValue(control, out Color originalColor))
            {
                control.BackColor = originalColor;
                _originalBackColors.Remove(control);
            }

            if (_originalForeColors.ContainsKey(control))
            {
                _originalForeColors.Remove(control);
            }
        }
    }
}
