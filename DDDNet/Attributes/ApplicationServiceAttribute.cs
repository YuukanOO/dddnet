using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente un service applicatif.
    /// 
    /// Le rôle du service applicatif est de fournir un point d'entrée pour la couche de présentation et de fournir
    /// ainsi une passerelle pour attaquer le domaine. Il est commun de fournir aux opérations du service applicatif des
    /// commandes et sa responsabilité et de traduire ces commandes en actions du domaine.
    /// 
    /// C'est aussi à lui de sauvegarder les entités en passant par les dépôts.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ApplicationServiceAttribute : Attribute
    {

    }
}
