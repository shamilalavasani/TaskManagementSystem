using FluentValidation;

public static class ValidationExtensions
{
    public static async Task ValidateAsync<T>(this T model, IValidator<T> validator)
    {
        var result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });

            throw new ValidationException(result.Errors);
        }
    }
}