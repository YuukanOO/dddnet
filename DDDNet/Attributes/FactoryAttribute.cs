using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente une fabrique.
    /// 
    /// Les fabriques sont utilisées lorsque la création d'un objet est trop complexe pour être porté par l'objet lui même ou
    /// que c'est juste impossible sans perdre trop de sens métier.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FactoryAttribute : Attribute
    {

    }
}
