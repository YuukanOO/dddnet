using System.Collections.Generic;

namespace DDDNet.Events
{
    /// <summary>
    /// Implémentation basique d'une source d'événements.
    /// 
    /// L'interface est ici implémentée explicitement de manière à ne pas poluer le domaine. Une méthode protégée permet
    /// néanmoins de lever des événements au sein de l'entité elle même.
    /// </summary>
    public abstract class EventSource : IEventSource
    {
        private List<IEvent> _pendingEvents;

        protected EventSource()
        {
            _pendingEvents = new List<IEvent>();
        }

        IEvent[] IEventSource.PopEvents()
        {
            var events = _pendingEvents.ToArray();

            _pendingEvents.Clear();

            return events;
        }

        void IEventSource.RaiseEvent(IEvent evt)
        {
            _pendingEvents.Add(evt);
        }

        /// <summary>
        /// Lève un événement du domaine
        /// </summary>
        /// <param name="evt"></param>
        protected void RaiseEvent(IEvent evt)
        {
            ((IEventSource)this).RaiseEvent(evt);
        }
    }
}
