using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente un repository.
    /// 
    /// Les dépôts permettent d'abstrairent la couche de persistence et définissent les opérations nécessaires pour
    /// la récupération des entités. Leur rôle consiste à recréer intégralement une entité et ses invariants depuis
    /// la couche de persistence.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class RepositoryAttribute : Attribute
    {

    }
}
