using DDDNet.Events;
using System.Collections.Generic;

namespace DDDNet.Infrastructure.Events
{
    /// <summary>
    /// Implémentation basique d'une source d'événements
    /// </summary>
    public abstract class EventSource : IEventSource
    {
        private List<IEvent> _pendingEvents;

        protected EventSource()
        {
            _pendingEvents = new List<IEvent>();
        }

        public IEvent[] PopEvents()
        {
            var events = _pendingEvents.ToArray();

            _pendingEvents.Clear();

            return events;
        }

        public void RaiseEvent(IEvent evt)
        {
            _pendingEvents.Add(evt);
        }
    }
}
