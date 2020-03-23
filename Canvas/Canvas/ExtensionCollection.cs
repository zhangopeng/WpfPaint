using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Canvas
{
    public class ExtensionCollection<T> : Collection<IExtension<T>> where T : IExtensible<T>
    {
        public ExtensionCollection(T owner)
        {
            this._owner = owner;
        }

        public ExtensionCollection([NotNull] IList<IExtension<T>> list, T owner) : base(list)
        {
            this._owner = owner;
        }

        protected override void InsertItem(int index, IExtension<T> item)
        {
            base.InsertItem(index, item);
            item.Attached(this._owner);
        }

        protected override void RemoveItem(int index)
        {
                    IExtension<T> extension = base.Items[index];
                    base.RemoveItem(index);
                    extension.Detached(this._owner);
        }

        protected override void ClearItems()
        {
            foreach(var item in Items)
            {
                item.Detached(this._owner);
            }
            base.ClearItems();
        }
        private readonly T _owner;
    }
    public interface IExtension<in T> where T : IExtensible<T>
    {
        // Token: 0x060003D7 RID: 983
        void Attached(T owner);

        // Token: 0x060003D8 RID: 984
        void Detached(T owner);
    }
}
