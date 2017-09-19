using System;
using System.Runtime.Serialization;

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
        /// Données additionnelles permettant de préciser le code d'erreur
        /// </summary>
        public object CodeData { get; private set; }

        /// <summary>
        /// Construit une nouvelle exception pour un champ donné
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="field"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        public FieldException(string resource, string field, string code, object data = null)
            : base($"Field {resource}.{field} validation failed with error {code}")
        {
            Resource = resource;
            Field = field;
            Code = code;
            CodeData = data;
        }

        /// <summary>
        /// Surcharge de manière à modifier les informations remontées lors de la sérialisation de l'objet
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Resource), Resource);
            info.AddValue(nameof(Field), Field);
            info.AddValue(nameof(Code), Code);

            if (CodeData != null)
            {
                info.AddValue(nameof(CodeData), CodeData);
            }

            info.AddValue(nameof(Message), Message);
        }
    }
}
