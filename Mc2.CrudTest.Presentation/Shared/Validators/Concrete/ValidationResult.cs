namespace Mc2.CrudTest.Presentation.Shared.Validators.Concrete;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
    public int ErrorCode { get; set; }
}

public interface IValidatable
{
    ValidationResult Validate();
}

public class Validator
{
    public static ValidationResult Validate(IValidatable valueObject)
    {
        return valueObject.Validate();
    }
}