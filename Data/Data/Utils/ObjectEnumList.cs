using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CMData.Manager
{
    public abstract class ObjectEnumList : ICollection
    {
        #region Declaraciones

        protected List<ObjectEnum> _Objects = new List<ObjectEnum>();

        #endregion

        #region Propiedades

        public ObjectEnum this[int index]
        {
            get { return this._Objects[index]; }
        }

        #endregion

        #region Constructores

        public ObjectEnumList()
        {
        }

        #endregion

        #region Metodos

        public void Add(ObjectEnum Item)
        {
            this._Objects.Add(Item);
        }

        public void Remove(ObjectEnum Item)
        {
            this._Objects.Remove(Item);
        }

        public void Clear()
        {
            this._Objects.Clear();
        }

        #endregion

        #region Funciones

        #endregion

        #region Miembros de ICollection

        public void CopyTo(Array array, int index)
        {
            this._Objects.CopyTo((ObjectEnum[])array, index);
        }

        public int Count
        {
            get { return this._Objects.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        #endregion

        #region Miembros de IEnumerable

        public IEnumerator GetEnumerator()
        {
            return this._Objects.GetEnumerator();
        }

        #endregion
    }
}