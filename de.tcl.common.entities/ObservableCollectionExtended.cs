using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace de.tcl.common.entities
{
    public class ObservableCollectionExtended<T> : ObservableCollection<T>
    {

        public ObservableCollectionExtended()
            : base()
        {

        }

        public ObservableCollectionExtended(IEnumerable<T> collection)
            : base(collection)
        {

        }

        #region =============== public & internal methods ==================

        public void AddRange(IEnumerable<T> items)
        {
            CheckReentrancy();
            foreach (T item in items)
            {
                Items.Add(item);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void InsertRange(IEnumerable<T> items)
        {
            CheckReentrancy();

            int currentIndex = 0;
            foreach (T item in items)
            {
                Items.Insert(currentIndex, item);
                currentIndex++;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            CheckReentrancy();

            bool hasRemovedAnyItems = false;
            foreach (T item in items)
            {
                if (Items.Remove(item))
                {
                    hasRemovedAnyItems = true;
                }
            }

            if (hasRemovedAnyItems)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        #endregion ============ public & internal methods ==================

    }
}
