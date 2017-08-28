using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Représente un objet valeur.
    /// 
    /// Contrairement aux entités, les objets valeurs ne possède pas d'identité à proprement parler.
    /// En effet, seuls les attributs permettent de les distinguer entre eux. Ils sont invariants, c'est à dire 
    /// que leur état ne peut pas être modifié après la création, on préférera alors remplacer la référence tout simplement.
    /// 
    /// Les objets valeurs servent à regrouper des notions nécessitant plusieurs propriétés mais formant un tout. Une valeur de
    /// monnaie par exemple, possèdera à la fois un montant et une devise, l'un ne signifiant rien sans l'autre, il s'agit
    /// alors d'un objet valeur.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ValueObjectAttribute : Attribute
    {

    }
}
