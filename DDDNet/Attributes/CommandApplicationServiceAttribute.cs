using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente un service applicatif dont le but est de traiter des commandes de modification.
    /// Ce service aura pour but de venir modifier le domaine en interprétant les commandes en action du domaine.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandApplicationServiceAttribute : ApplicationServiceAttribute
    {

    }
}
