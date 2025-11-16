# Helpers Directory

This directory contains utility classes that provide helper functions for common tasks throughout the BiblioKopke application.

## Available Helpers

### ValidationHelper.cs

A comprehensive validation utility that complements the existing `Validadores` class with modern, functional validation patterns.

**Key Features:**
- Returns validator functions compatible with `ValidatedTextBox`
- Composable validators - combine multiple validation rules
- Integration with existing `Validadores` class (CPF, ISBN, Email)
- Form-level validation utilities
- Support for both `ValidatedTextBox` and standard controls

**Validator Categories:**

1. **Basic Validators**
   - `Required()` - Non-empty validation
   - `MinLength()` - Minimum length check
   - `MaxLength()` - Maximum length check
   - `LengthRange()` - Length within range

2. **Pattern Validators**
   - `Pattern()` - Regex pattern matching

3. **Domain-Specific Validators** (integrates with `Validadores.cs`)
   - `Email()` - Email format validation
   - `CPF()` - CPF validation with check digits
   - `ISBN()` - ISBN-10 and ISBN-13 validation
   - `Matricula()` - Student enrollment number validation

4. **Numeric Validators**
   - `Numeric()` - Checks if value is numeric
   - `IntegerRange()` - Integer within range
   - `DecimalRange()` - Decimal within range

5. **Date Validators**
   - `Date()` - Valid date check
   - `DateRange()` - Date within range

6. **Custom & Composite**
   - `Custom()` - Custom validation logic
   - `Composite()` - Combines multiple validators

7. **Form Utilities**
   - `ValidateForm()` - Validates multiple controls
   - `ShowValidationErrors()` - Displays validation errors
   - `ClearValidationErrors()` - Clears error indicators
   - `HighlightInvalidControl()` - Highlights invalid controls

**Quick Start:**

```csharp
using BibliotecaJK.Helpers;
using BibliotecaJK.Components;

// Simple required field
var txtNome = new ValidatedTextBox
{
    FloatingLabel = "Nome",
    Validator = ValidationHelper.Required("Nome"),
    ValidationEnabled = true
};

// Composite validation (multiple rules)
var txtCPF = new ValidatedTextBox
{
    FloatingLabel = "CPF",
    Validator = ValidationHelper.Composite(
        ValidationHelper.Required("CPF"),
        ValidationHelper.CPF()
    ),
    ValidationEnabled = true
};

// Form-level validation
private void BtnSalvar_Click(object sender, EventArgs e)
{
    var errors = ValidationHelper.ValidateForm(
        (txtNome, ValidationHelper.Required("Nome")),
        (txtEmail, ValidationHelper.Email()),
        (txtCPF, ValidationHelper.CPF())
    );

    if (errors.Count > 0)
    {
        ValidationHelper.ShowValidationErrors(this, errors);
        return;
    }

    // Proceed with save...
}
```

**See Also:**
- `ValidationHelper.Examples.cs` - Comprehensive examples of all validator types
- `06_bibliotecaJK/BLL/Validadores.cs` - Core validation logic
- `06_bibliotecaJK/Components/ValidatedTextBox.cs` - Component that uses validators

---

### AsyncOperationHelper.cs

Helper for managing asynchronous operations in Windows Forms with proper UI thread synchronization and error handling.

**Features:**
- Safe async/await patterns for WinForms
- Automatic UI thread marshalling
- Progress reporting
- Cancellation support
- Error handling

---

### FormStateManager.cs

Helper for managing form state, including loading states, error states, and data persistence.

**Features:**
- Form state tracking
- Loading indicators
- Error state management
- Form data persistence

---

## Best Practices

### Validation Best Practices

1. **Use Composite Validators**
   - Combine `Required()` with format validators
   - Example: `Composite(Required("CPF"), CPF())`

2. **Validate on Appropriate Events**
   - Use `ValidateOnBlurOnly = true` for long text inputs
   - Use real-time validation for short, formatted inputs

3. **Provide Clear Error Messages**
   - Use custom messages for business-specific rules
   - Keep messages concise and actionable

4. **Reuse Common Validators**
   - Create validator factory classes for repeated patterns
   - Share validators across forms

5. **Validate at Form Level**
   - Use `ValidateForm()` before submitting data
   - Provide consolidated error feedback

### Integration with Existing Code

The ValidationHelper integrates seamlessly with:

- **Validadores.cs** - Uses existing validation logic internally
- **ValidatedTextBox** - Compatible validator function signature
- **BLL Services** - Complement service-level validation
- **ResultadoOperacao** - Can be used with existing result patterns

---

## Contributing

When adding new helpers:

1. Follow the existing naming conventions
2. Provide comprehensive XML documentation
3. Create example usage in a `.Examples.cs` file
4. Update this README with the new helper
5. Ensure compatibility with existing architecture

---

## Related Documentation

- [/06_bibliotecaJK/BLL/README_BLL.md](../BLL/README_BLL.md) - Business Logic Layer documentation
- [/06_bibliotecaJK/Components/README.md](../Components/README.md) - UI Components documentation
- [/CLAUDE.md](/CLAUDE.md) - Project architecture overview
