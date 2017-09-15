using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente un service applicatif dont le but est la lecture seule d'informations. 
    /// Il n'est pas autorisé à modifier le domaine. Il permet en revanche d'aggréger des données provenants de différents contextes de manière à le passer à la 
    /// couche de présentation au travers de POCO.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryApplicationServiceAttribute : ApplicationServiceAttribute
    {

    }
}
