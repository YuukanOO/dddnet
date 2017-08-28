using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente une entité du domaine.
    /// 
    /// Une entité possède une identité unique invariante qui lui est propre. Elle possède une durée de vie
    /// conséquente et son état change au cours du temps. Son identité lui permet d'être référencée
    /// par d'autres entités.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {

    }
}
