using System.ComponentModel.DataAnnotations;

namespace MinimalAPIDemo.Models
{
    public class ValidationHelper
    {
        // TryValidate method performs validation on a generic model object.
        public static bool TryValidate<T>(T model, out List<ValidationResult> validationResults)
        {
            // Create a validation context of the model which contains the model type
            var validationContext = new ValidationContext(model, null, null);

            // Initialize the list to store validation results(errors, if any)
            validationResults = new List<ValidationResult>();

            // Perform the validation using the validation class , which uses reflection to find
            // and validate the properties of the model based on the data annotations.
            // The method returns true if the model passes on the validation; otherwise , false
            return Validator.TryValidateObject(model, validationContext, validationResults, true);
        }
    }
}
