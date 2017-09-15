using System;
using System.Threading.Tasks;

namespace DDDNet.Persistence
{
    /// <summary>
    /// Représente une unité de transaction
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface ITransactionUnit<TContext> : IDisposable
    {
        /// <summary>
        /// Contexte associé à cette unité de transaction
        /// </summary>
        TContext Context { get; }

        /// <summary>
        /// Commit les changements pour la transaction. C'est aussi ici qu'on aura pour habitude
        /// de dispatcher les événements du domaine une fois que la persistence aura été effectuée
        /// avec succès.
        /// </summary>
        Task Commit();
    }
}
