using DDDNet.Validations;

namespace DDDNet.Applications
{
    /// <summary>
    /// Interface représente une commande applicative. Cette commande sera traitée par un service applicatif.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Valide la commande et retourne le Validator associé. Le service applicatif sera en charge de traiter la présence
        /// ou non d'erreurs et pourra même en chainer d'avantage pour des règles plus complexes.
        /// </summary>
        /// <returns></returns>
        Validator Validate();
    }
}
