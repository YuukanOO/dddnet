namespace DDDNet.Events
{
    /// <summary>
    /// Interface utilisée par les éléments pouvant lever des événements du domaine.
    /// 
    /// Les événements sont levés via la méthode RaiseEvent et seront récupéré par le dispatcher
    /// via la méthode PopEvents.
    /// </summary>
    public interface IEventSource
    {
        /// <summary>
        /// Récupère et vide la liste des événements levés et devant être traités par le dispatcher
        /// </summary>
        /// <returns></returns>
        IEvent[] PopEvents();

        /// <summary>
        /// Permet de lever un événement du domaine
        /// </summary>
        /// <param name="evt"></param>
        void RaiseEvent(IEvent evt);
    }
}
