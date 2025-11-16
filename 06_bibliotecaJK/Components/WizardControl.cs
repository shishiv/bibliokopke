using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace BibliotecaJK.Components
{
    /// <summary>
    /// WizardStep - Represents a single step in the wizard flow
    /// </summary>
    public class WizardStep
    {
        /// <summary>
        /// Gets or sets the step title displayed in the step indicator
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the step description/subtitle
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the control that will be displayed for this step
        /// </summary>
        public Control Content { get; set; }

        /// <summary>
        /// Optional validation function that returns true if the step is valid
        /// </summary>
        public Func<bool> ValidationFunction { get; set; }

        public WizardStep(string title, string description, Control content)
        {
            Title = title;
            Description = description;
            Content = content;
        }
    }

    /// <summary>
    /// WizardStepChangedEventArgs - Event arguments for step change events
    /// </summary>
    public class WizardStepChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the index of the previous step
        /// </summary>
        public int OldStepIndex { get; }

        /// <summary>
        /// Gets the index of the new current step
        /// </summary>
        public int NewStepIndex { get; }

        public WizardStepChangedEventArgs(int oldStepIndex, int newStepIndex)
        {
            OldStepIndex = oldStepIndex;
            NewStepIndex = newStepIndex;
        }
    }

    /// <summary>
    /// WizardControl - Multi-step wizard UserControl with Material Design styling
    /// Provides a guided flow through multiple steps with navigation and validation
    /// </summary>
    public class WizardControl : UserControl
    {
        private readonly List<WizardStep> _steps;
        private int _currentStepIndex;

        // UI Panels
        private Panel _topPanel;
        private Panel _contentPanel;
        private Panel _bottomPanel;

        // Navigation Buttons
        private ModernButton _btnPrevious;
        private ModernButton _btnNext;
        private ModernButton _btnFinish;

        // Step Indicator Panel
        private Panel _stepIndicatorPanel;

        /// <summary>
        /// Event raised when the current step changes
        /// </summary>
        public event EventHandler<WizardStepChangedEventArgs> StepChanged;

        /// <summary>
        /// Event raised when the wizard is completed (Finish button clicked)
        /// </summary>
        public event EventHandler WizardCompleted;

        /// <summary>
        /// Gets the current step index (0-based)
        /// </summary>
        public int CurrentStepIndex => _currentStepIndex;

        /// <summary>
        /// Gets the current step
        /// </summary>
        public WizardStep CurrentStep => _steps.Count > 0 && _currentStepIndex >= 0 && _currentStepIndex < _steps.Count
            ? _steps[_currentStepIndex]
            : null;

        /// <summary>
        /// Gets the total number of steps
        /// </summary>
        public int StepCount => _steps.Count;

        /// <summary>
        /// Gets or sets whether the Finish button should be enabled even if validation fails
        /// </summary>
        public bool AllowFinishWithoutValidation { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of WizardControl
        /// </summary>
        public WizardControl()
        {
            _steps = new List<WizardStep>();
            _currentStepIndex = 0;

            InitializeComponent();
            SetupLayout();
        }

        /// <summary>
        /// Initializes the wizard UI components
        /// </summary>
        private void InitializeComponent()
        {
            // Enable double buffering for smooth rendering
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            BackColor = ThemeManager.Background.LightDefault;
            MinimumSize = new Size(600, 400);

            // Create top panel for step indicators
            _topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = ThemeManager.Background.LightPaper,
                Padding = new Padding(ThemeManager.Spacing.LG)
            };

            // Create step indicator panel (will be populated dynamically)
            _stepIndicatorPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            _stepIndicatorPanel.Paint += StepIndicatorPanel_Paint;
            _topPanel.Controls.Add(_stepIndicatorPanel);

            // Create content panel for step content
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ThemeManager.Background.LightPaper,
                Padding = new Padding(ThemeManager.Spacing.XL)
            };

            // Create bottom panel for navigation buttons
            _bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = ThemeManager.Background.LightPaper,
                Padding = new Padding(ThemeManager.Spacing.LG)
            };

            // Create navigation buttons
            _btnPrevious = new ModernButton
            {
                Text = "Anterior",
                Size = new Size(120, 40),
                Location = new Point(ThemeManager.Spacing.LG, 15),
                Variant = ModernButton.ButtonVariant.Outlined,
                BackColor = ThemeManager.Colors.Neutral600,
                ForeColor = ThemeManager.Colors.Neutral600,
                Enabled = false
            };
            _btnPrevious.Click += BtnPrevious_Click;

            _btnNext = new ModernButton
            {
                Text = "Próximo",
                Size = new Size(120, 40),
                Variant = ModernButton.ButtonVariant.Contained,
                BackColor = ThemeManager.Colors.Primary500,
                ForeColor = Color.White
            };
            _btnNext.Click += BtnNext_Click;

            _btnFinish = new ModernButton
            {
                Text = "Concluir",
                Size = new Size(120, 40),
                Variant = ModernButton.ButtonVariant.Contained,
                BackColor = ThemeManager.Semantic.Success,
                ForeColor = Color.White,
                Visible = false
            };
            _btnFinish.Click += BtnFinish_Click;

            _bottomPanel.Controls.Add(_btnPrevious);
            _bottomPanel.Controls.Add(_btnNext);
            _bottomPanel.Controls.Add(_btnFinish);

            // Add panels to control
            Controls.Add(_contentPanel);
            Controls.Add(_topPanel);
            Controls.Add(_bottomPanel);
        }

        /// <summary>
        /// Sets up the layout and positioning of buttons
        /// </summary>
        private void SetupLayout()
        {
            Resize += (s, e) => PositionButtons();
            PositionButtons();
        }

        /// <summary>
        /// Positions the navigation buttons on the right side of the bottom panel
        /// </summary>
        private void PositionButtons()
        {
            if (_bottomPanel == null) return;

            int rightMargin = ThemeManager.Spacing.LG;
            int buttonSpacing = ThemeManager.Spacing.SM;
            int y = (_bottomPanel.Height - 40) / 2;

            // Position Finish button (rightmost)
            _btnFinish.Location = new Point(
                _bottomPanel.Width - _btnFinish.Width - rightMargin,
                y
            );

            // Position Next button (next to Finish)
            _btnNext.Location = new Point(
                _bottomPanel.Width - _btnNext.Width - rightMargin,
                y
            );

            // Previous button stays on the left
            _btnPrevious.Location = new Point(rightMargin, y);
        }

        /// <summary>
        /// Adds a new step to the wizard
        /// </summary>
        /// <param name="title">Step title</param>
        /// <param name="description">Step description</param>
        /// <param name="content">Control to display for this step</param>
        public void AddStep(string title, string description, Control content)
        {
            var step = new WizardStep(title, description, content);
            _steps.Add(step);

            // Show first step content if this is the first step
            if (_steps.Count == 1)
            {
                ShowCurrentStepContent();
            }

            // Update UI
            RenderStepIndicators();
            UpdateButtonStates();
        }

        /// <summary>
        /// Adds a new step to the wizard with custom validation
        /// </summary>
        public void AddStep(string title, string description, Control content, Func<bool> validationFunction)
        {
            var step = new WizardStep(title, description, content)
            {
                ValidationFunction = validationFunction
            };
            _steps.Add(step);

            if (_steps.Count == 1)
            {
                ShowCurrentStepContent();
            }

            RenderStepIndicators();
            UpdateButtonStates();
        }

        /// <summary>
        /// Navigates to the next step if validation passes
        /// </summary>
        /// <returns>True if navigation was successful</returns>
        public bool NextStep()
        {
            if (_currentStepIndex >= _steps.Count - 1)
                return false;

            // Validate current step before proceeding
            if (!ValidateCurrentStep())
            {
                MessageBox.Show(
                    "Por favor, preencha todos os campos obrigatórios antes de continuar.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            int oldIndex = _currentStepIndex;
            _currentStepIndex++;

            // Raise event
            OnStepChanged(new WizardStepChangedEventArgs(oldIndex, _currentStepIndex));

            ShowCurrentStepContent();
            UpdateButtonStates();
            RenderStepIndicators();

            return true;
        }

        /// <summary>
        /// Navigates to the previous step
        /// </summary>
        /// <returns>True if navigation was successful</returns>
        public bool PreviousStep()
        {
            if (_currentStepIndex <= 0)
                return false;

            int oldIndex = _currentStepIndex;
            _currentStepIndex--;

            // Raise event
            OnStepChanged(new WizardStepChangedEventArgs(oldIndex, _currentStepIndex));

            ShowCurrentStepContent();
            UpdateButtonStates();
            RenderStepIndicators();

            return true;
        }

        /// <summary>
        /// Validates the current step using the custom validation function if provided
        /// </summary>
        /// <returns>True if validation passes or no validation function is set</returns>
        public bool ValidateCurrentStep()
        {
            var currentStep = CurrentStep;
            if (currentStep == null)
                return false;

            // If no validation function is provided, consider the step valid
            if (currentStep.ValidationFunction == null)
                return true;

            // Execute custom validation
            return currentStep.ValidationFunction();
        }

        /// <summary>
        /// Renders the step indicators (breadcrumb/stepper UI) in the top panel
        /// </summary>
        private void RenderStepIndicators()
        {
            _stepIndicatorPanel.Invalidate();
        }

        /// <summary>
        /// Paints the step indicators
        /// </summary>
        private void StepIndicatorPanel_Paint(object sender, PaintEventArgs e)
        {
            if (_steps.Count == 0)
                return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int circleSize = 40;
            int lineHeight = 3;
            int totalWidth = _stepIndicatorPanel.Width - (ThemeManager.Spacing.XL * 2);
            int stepWidth = totalWidth / _steps.Count;
            int yCenter = _stepIndicatorPanel.Height / 2;

            for (int i = 0; i < _steps.Count; i++)
            {
                int xCenter = (ThemeManager.Spacing.XL) + (stepWidth * i) + (stepWidth / 2);

                // Determine step state
                bool isCompleted = i < _currentStepIndex;
                bool isCurrent = i == _currentStepIndex;
                bool isFuture = i > _currentStepIndex;

                // Draw connecting line (except for first step)
                if (i > 0)
                {
                    int lineStartX = xCenter - stepWidth / 2 - circleSize / 2;
                    int lineEndX = xCenter - circleSize / 2;
                    int lineY = yCenter - lineHeight / 2;

                    Color lineColor = isCompleted || isCurrent
                        ? ThemeManager.Colors.Primary500
                        : ThemeManager.Colors.Neutral300;

                    using (var brush = new SolidBrush(lineColor))
                    {
                        g.FillRectangle(brush, lineStartX, lineY, lineEndX - lineStartX, lineHeight);
                    }
                }

                // Draw circle
                Color circleColor;
                Color textColor;
                Font numberFont = ThemeManager.Typography.H6;

                if (isCompleted)
                {
                    circleColor = ThemeManager.Colors.Primary500;
                    textColor = Color.White;
                }
                else if (isCurrent)
                {
                    circleColor = ThemeManager.Colors.Primary500;
                    textColor = Color.White;
                }
                else
                {
                    circleColor = ThemeManager.Colors.Neutral300;
                    textColor = ThemeManager.Colors.Neutral600;
                }

                // Draw circle background
                using (var brush = new SolidBrush(circleColor))
                {
                    g.FillEllipse(brush, xCenter - circleSize / 2, yCenter - circleSize / 2, circleSize, circleSize);
                }

                // Draw circle border for current step
                if (isCurrent)
                {
                    using (var pen = new Pen(ThemeManager.Colors.Primary700, 2))
                    {
                        g.DrawEllipse(pen, xCenter - circleSize / 2, yCenter - circleSize / 2, circleSize, circleSize);
                    }
                }

                // Draw checkmark for completed steps
                if (isCompleted)
                {
                    DrawCheckmark(g, xCenter, yCenter, circleSize * 0.5f, Color.White);
                }
                else
                {
                    // Draw step number
                    string stepNumber = (i + 1).ToString();
                    var numberSize = g.MeasureString(stepNumber, numberFont);
                    using (var brush = new SolidBrush(textColor))
                    {
                        g.DrawString(
                            stepNumber,
                            numberFont,
                            brush,
                            xCenter - numberSize.Width / 2,
                            yCenter - numberSize.Height / 2
                        );
                    }
                }

                // Draw step title below circle
                string title = _steps[i].Title;
                Font titleFont = isCurrent ? ThemeManager.Typography.Body1 : ThemeManager.Typography.Body2;
                var titleSize = g.MeasureString(title, titleFont);
                Color titleColor = isCurrent
                    ? ThemeManager.Colors.Primary700
                    : ThemeManager.Colors.Neutral600;

                using (var brush = new SolidBrush(titleColor))
                {
                    g.DrawString(
                        title,
                        titleFont,
                        brush,
                        xCenter - titleSize.Width / 2,
                        yCenter + circleSize / 2 + ThemeManager.Spacing.SM
                    );
                }
            }
        }

        /// <summary>
        /// Draws a checkmark icon in a circle
        /// </summary>
        private void DrawCheckmark(Graphics g, float centerX, float centerY, float size, Color color)
        {
            using (var pen = new Pen(color, 3))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                // Checkmark path
                float scale = size / 20f;
                PointF[] points = new PointF[]
                {
                    new PointF(centerX - 6 * scale, centerY),
                    new PointF(centerX - 2 * scale, centerY + 4 * scale),
                    new PointF(centerX + 6 * scale, centerY - 4 * scale)
                };

                g.DrawLines(pen, points);
            }
        }

        /// <summary>
        /// Shows the current step's content in the content panel
        /// </summary>
        private void ShowCurrentStepContent()
        {
            _contentPanel.Controls.Clear();

            var currentStep = CurrentStep;
            if (currentStep?.Content != null)
            {
                currentStep.Content.Dock = DockStyle.Fill;
                _contentPanel.Controls.Add(currentStep.Content);
            }
        }

        /// <summary>
        /// Updates the enabled state and visibility of navigation buttons
        /// </summary>
        private void UpdateButtonStates()
        {
            // Previous button enabled if not on first step
            _btnPrevious.Enabled = _currentStepIndex > 0;

            // Show Next or Finish button based on current step
            bool isLastStep = _currentStepIndex >= _steps.Count - 1;
            _btnNext.Visible = !isLastStep;
            _btnFinish.Visible = isLastStep;

            // Enable Next/Finish based on validation
            bool isValid = AllowFinishWithoutValidation || ValidateCurrentStep();
            _btnNext.Enabled = isValid;
            _btnFinish.Enabled = isValid;
        }

        /// <summary>
        /// Handles the Previous button click
        /// </summary>
        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            PreviousStep();
        }

        /// <summary>
        /// Handles the Next button click
        /// </summary>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            NextStep();
        }

        /// <summary>
        /// Handles the Finish button click
        /// </summary>
        private void BtnFinish_Click(object sender, EventArgs e)
        {
            if (!AllowFinishWithoutValidation && !ValidateCurrentStep())
            {
                MessageBox.Show(
                    "Por favor, preencha todos os campos obrigatórios antes de concluir.",
                    "Validação",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            OnWizardCompleted(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the StepChanged event
        /// </summary>
        protected virtual void OnStepChanged(WizardStepChangedEventArgs e)
        {
            StepChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the WizardCompleted event
        /// </summary>
        protected virtual void OnWizardCompleted(EventArgs e)
        {
            WizardCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Navigates directly to a specific step index
        /// </summary>
        /// <param name="stepIndex">The 0-based index of the step to navigate to</param>
        /// <returns>True if navigation was successful</returns>
        public bool GoToStep(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= _steps.Count)
                return false;

            if (stepIndex == _currentStepIndex)
                return true;

            int oldIndex = _currentStepIndex;
            _currentStepIndex = stepIndex;

            OnStepChanged(new WizardStepChangedEventArgs(oldIndex, _currentStepIndex));

            ShowCurrentStepContent();
            UpdateButtonStates();
            RenderStepIndicators();

            return true;
        }

        /// <summary>
        /// Clears all steps from the wizard
        /// </summary>
        public void ClearSteps()
        {
            _steps.Clear();
            _currentStepIndex = 0;
            _contentPanel.Controls.Clear();
            RenderStepIndicators();
            UpdateButtonStates();
        }

        /// <summary>
        /// Gets a step by its index
        /// </summary>
        public WizardStep GetStep(int index)
        {
            if (index < 0 || index >= _steps.Count)
                return null;

            return _steps[index];
        }
    }
}
