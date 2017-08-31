using DDDNet.Exceptions;

namespace DDDNet.Validations
{
    /// <summary>
    /// Représente une exception de validation.
    /// </summary>
    public class ValidationException : DomainException
    {
        /// <summary>
        /// Instantie une nouvelle ValidationException avec les exceptions de champs donnés
        /// </summary>
        /// <param name="fieldExceptions"></param>
        public ValidationException(params FieldException[] fieldExceptions) : base(nameof(ValidationException), "Validation failed", fieldExceptions)
        {

        }

        /// <summary>
        /// Instantie une nouvelle ValidationException pour une exception sur un champ.
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="field"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        public ValidationException(string resource, string field, string code, object data = null) : this(new FieldException(resource, field, code, data))
        {

        }
    }
}
