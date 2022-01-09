using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace TradeHub.Core.Model
{
    /// <summary>
    /// A list of ticks along with computed values.
    /// </summary>
    public class TickList
    {
        /// <summary>
        /// The list of ticks.
        /// </summary>
        public ObservableCollection<Tick> Ticks { get; init; }

        /// <summary>
        /// The lowest low of the ticks collection.
        /// </summary>
        public decimal Min { get; private set; }

        /// <summary>
        /// The highest high of the ticks collection.
        /// </summary>
        public decimal Max { get; private set; }

        public TickList(List<Tick> ticks)
        {
            Ticks = new ObservableCollection<Tick>(ticks);
            Ticks.CollectionChanged += OnCollectionChanged;
            ComputeValues();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ComputeValues();
        }

        private void ComputeValues()
        {
            if (Ticks == null || Ticks.Count == 0)
            {
                Min = 0;
                Max = 0;
                return;
            }

            Min = Ticks.Min(s => s.Low);
            Max = Ticks.Max(s => s.High);
        }
    }
}
