using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PRDownloader.Validators;

public sealed class DirectoryMustExistAttribute : ValidationAttribute
{
    public DirectoryMustExistAttribute()
    {
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not string path)
        {
            return new($"Expected validating strings, got {value.GetType().Name}");
        }

        return Directory.Exists(path) ? ValidationResult.Success! : new ValidationResult("Directory does not exist.");
    }
}
