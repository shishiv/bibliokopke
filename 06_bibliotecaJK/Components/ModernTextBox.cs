using System;
using System.Drawing;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// ModernTextBox - Material Design style text input with floating label
    /// </summary>
    public class ModernTextBox : UserControl
    {
        private Label _lblFloatingLabel = null!;
        private TextBox _textBox = null!;
        private Panel _linePanel = null!;
        private bool _isFocused = false;

        // Public properties
        public string FloatingLabel { get; set; } = "";
        public Color FocusedLineColor { get; set; } = Color.FromArgb(63, 81, 181); // ThemeManager.Light.Primary
        public Color UnfocusedLineColor { get; set; } = Color.FromArgb(224, 224, 224); // Gray300

        // TextBox properties delegation
        public override string? Text
        {
            get => _textBox?.Text ?? string.Empty;
            set
            {
                if (_textBox != null)
                {
                    _textBox.Text = value ?? string.Empty;
                    UpdateFloatingLabel();
                }
            }
        }

        public char PasswordChar
        {
            get => _textBox?.PasswordChar ?? '\0';
            set { if (_textBox != null) _textBox.PasswordChar = value; }
        }

        public bool Multiline
        {
            get => _textBox?.Multiline ?? false;
            set
            {
                if (_textBox != null)
                {
                    _textBox.Multiline = value;
                    if (value)
                    {
                        _textBox.Height = this.Height - 24; // Account for label and line
                    }
                }
            }
        }

        public int MaxLength
        {
            get => _textBox?.MaxLength ?? 32767;
            set { if (_textBox != null) _textBox.MaxLength = value; }
        }

        public bool ReadOnly
        {
            get => _textBox?.ReadOnly ?? false;
            set { if (_textBox != null) _textBox.ReadOnly = value; }
        }

        // Events
        public new event EventHandler? GotFocus;
        public new event EventHandler? LostFocus;
        public new event EventHandler? TextChanged;

        public ModernTextBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Set up the UserControl
            this.Size = new Size(250, 56);
            this.Padding = new Padding(0);
            this.BackColor = Color.Transparent;

            // Initialize TextBox
            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 16F), // ThemeManager.Typography.Body1
                Location = new Point(0, 24),
                Width = this.Width,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };

            // Initialize floating label
            _lblFloatingLabel = new Label
            {
                Font = new Font("Segoe UI", 12F), // ThemeManager.Typography.Caption
                ForeColor = Color.FromArgb(117, 117, 117), // Gray600
                AutoSize = true,
                Location = new Point(0, 0),
                Visible = false,
                BackColor = Color.Transparent
            };

            // Initialize bottom line
            _linePanel = new Panel
            {
                Height = 2,
                BackColor = UnfocusedLineColor,
                Location = new Point(0, this.Height - 2),
                Width = this.Width,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Wire up events
            _textBox.GotFocus += TextBox_GotFocus;
            _textBox.LostFocus += TextBox_LostFocus;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.KeyDown += (s, e) => OnKeyDown(e);
            _textBox.KeyPress += (s, e) => OnKeyPress(e);
            _textBox.KeyUp += (s, e) => OnKeyUp(e);

            // Add controls to UserControl
            this.Controls.Add(_lblFloatingLabel);
            this.Controls.Add(_textBox);
            this.Controls.Add(_linePanel);

            // Handle resize
            this.Resize += ModernTextBox_Resize;
        }

        private void ModernTextBox_Resize(object? sender, EventArgs e)
        {
            // Ensure controls stay properly positioned on resize
            if (_textBox != null && _linePanel != null)
            {
                _linePanel.Width = this.Width;
                _textBox.Width = this.Width;
            }
        }

        private void TextBox_GotFocus(object? sender, EventArgs e)
        {
            _isFocused = true;
            UpdateFloatingLabel();
            GotFocus?.Invoke(this, e);
        }

        private void TextBox_LostFocus(object? sender, EventArgs e)
        {
            _isFocused = false;
            UpdateFloatingLabel();
            LostFocus?.Invoke(this, e);
        }

        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            UpdateFloatingLabel();
            TextChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Updates the floating label visibility and line color based on focus and text state
        /// </summary>
        private void UpdateFloatingLabel()
        {
            if (_lblFloatingLabel == null || _linePanel == null || _textBox == null)
                return;

            if (_isFocused || !string.IsNullOrEmpty(_textBox.Text))
            {
                // Show floating label when focused or has text
                _lblFloatingLabel.Visible = true;
                _lblFloatingLabel.Text = FloatingLabel;
                _linePanel.BackColor = _isFocused ? FocusedLineColor : UnfocusedLineColor;
            }
            else
            {
                // Hide label when not focused and empty
                _lblFloatingLabel.Visible = false;
                _linePanel.BackColor = UnfocusedLineColor;
            }
        }

        /// <summary>
        /// Sets focus to the internal TextBox
        /// </summary>
        public new void Focus()
        {
            _textBox?.Focus();
        }

        /// <summary>
        /// Selects all text in the TextBox
        /// </summary>
        public void SelectAll()
        {
            _textBox?.SelectAll();
        }

        /// <summary>
        /// Clears the text
        /// </summary>
        public void Clear()
        {
            if (_textBox != null)
            {
                _textBox.Clear();
                UpdateFloatingLabel();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Apply theme colors if ThemeManager is available
            try
            {
                if (ThemeManager.IsDarkMode)
                {
                    _textBox.BackColor = ThemeManager.Dark.Surface;
                    _textBox.ForeColor = ThemeManager.Dark.Text;
                    this.BackColor = ThemeManager.Dark.Surface;
                }
                else
                {
                    _textBox.BackColor = Color.White;
                    _textBox.ForeColor = ThemeManager.Light.Text;
                    this.BackColor = Color.White;
                }

                // Update default colors from ThemeManager
                FocusedLineColor = ThemeManager.IsDarkMode
                    ? ThemeManager.Dark.Primary
                    : ThemeManager.Light.Primary;
            }
            catch
            {
                // If ThemeManager not available, use defaults
            }

            UpdateFloatingLabel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _textBox?.Dispose();
                _lblFloatingLabel?.Dispose();
                _linePanel?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
