using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente un service du domaine.
    /// 
    /// Les services du domaine sont utilisés lorsqu'il n'est pas possible de définir clairement quelle entité
    /// est responsable d'une opération mais que l'on souhaite garder le contrôle de cette opération dans le domaine.
    /// 
    /// Il peut s'agir d'une classe ou tout simplement d'une interface. Ils peuvent aussi être appelés depuis une entité.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class DomainServiceAttribute : Attribute
    {

    }
}
