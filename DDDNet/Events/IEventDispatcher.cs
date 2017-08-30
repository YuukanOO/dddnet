using System;

namespace DDDNet.Events
{
    /// <summary>
    /// Représente une interface commune pour le dispatch des événements du domaine.
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Enregistre un handler pour le type d'événement renseigné
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        void Handle<T>(Action<T> action) where T : IEvent;

        /// <summary>
        /// Dispatche des événements à leurs handlers respectifs
        /// </summary>
        /// <param name="events"></param>
        void Dispatch(params IEvent[] events);
    }
}
