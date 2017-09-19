using System;
using System.Runtime.Serialization;

namespace DDDNet.Exceptions
{
    /// <summary>
    /// Représente une exception du domaine. Pour faciliter les remontées d'erreur, on inclut un code d'erreur et des exceptions internes.
    /// Le domaine peut ainsi lever des exceptions facilement en cas d'opérations invalides.
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Code d'erreur
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// Erreurs internes permettant de préciser l'exception
        /// </summary>
        public Exception[] Errors { get; private set; }
        
        /// <summary>
        /// Instantie une nouvelle erreur du domaine
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="innerErrors"></param>
        public DomainException(string code, string message, params Exception[] innerErrors) : base(message)
        {
            Code = code;
            Errors = innerErrors;
        }

        /// <summary>
        /// Surcharge de manière à modifier les informations remontées lors de la sérialisation de l'objet
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Code), Code);
            info.AddValue(nameof(Message), Message);
            info.AddValue(nameof(Errors), Errors);
        }
    }
}
