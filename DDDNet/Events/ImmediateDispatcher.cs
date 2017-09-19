using System;
using System.Collections.Generic;

namespace DDDNet.Events
{
    /// <summary>
    /// Implémentation d'un dispatcher qui dispatche les événements le plus simplement du monde
    /// </summary>
    public class ImmediateDispatcher : IEventDispatcher
    {
        private Dictionary<Type, List<Action<IEvent>>> _handlers;

        public ImmediateDispatcher()
        {
            _handlers = new Dictionary<Type, List<Action<IEvent>>>();
        }

        public void Dispatch(params IEvent[] events)
        {
            foreach(var evt in events)
            {
                if(_handlers.TryGetValue(evt.GetType(), out List<Action<IEvent>> handlers))
                {
                    handlers.ForEach(h => h(evt));
                }
            }
        }

        public void Handle<T>(Action<T> action) where T : IEvent
        {
            var type = typeof(T);
            
            if (!_handlers.TryGetValue(type, out List<Action<IEvent>> handlers))
            {
                handlers = new List<Action<IEvent>>();
                _handlers.Add(type, handlers);
            }

            handlers.Add(o => action((T)o));
        }
    }
}
