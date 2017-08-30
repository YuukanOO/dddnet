using DDDNet.Events;
using System;

namespace DDDNet.Attributes
{
    /// <summary>
    /// Attribut permettant de spécifier qu'une classe peut lever un événement du domaine particulier.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RaiseEventAttribute : Attribute
    {
        public Type EventType { get; private set; }

        public RaiseEventAttribute(Type eventType)
        {
            if (!typeof(IEvent).IsAssignableFrom(eventType))
            {
                throw new ArgumentException($"{nameof(EventType)} should implement the {nameof(IEvent)} interface!");
            }

            EventType = eventType;
        }
    }
}
