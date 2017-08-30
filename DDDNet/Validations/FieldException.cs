using System;

namespace DDDNet.Validations
{
    /// <summary>
    /// Représente une exception de validation pour un champ en particulier.
    /// Ce type d'exception est destiné à être embarqué dans une ValidationException en tant qu'exception interne
    /// pour chacun des champs ne pouvant être validé.
    /// </summary>
    public class FieldException : Exception
    {
        /// <summary>
        /// Ressource possédant la propriété en erreur
        /// </summary>
        public string Resource { get; private set; }
        /// <summary>
        /// Propriété en erreur
        /// </summary>
        public string Field { get; private set; }
        /// <summary>
        /// Code d'erreur de validation
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Construit une nouvelle exception pour un champ donné
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="field"></param>
        /// <param name="code"></param>
        public FieldException(string resource, string field, string code) : base($"Field {resource}.{field} validation failed with error {code}")
        {
            Resource = resource;
            Field = field;
            Code = code;
        }
    }
}
