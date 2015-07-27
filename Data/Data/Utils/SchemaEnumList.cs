using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CMData.Manager
{
    public abstract class SchemaEnumList : ICollection
    {
        #region Declaraciones

        protected List<SchemaEnum> _Schemas = new List<SchemaEnum>();

        #endregion

        #region Propiedades

        public SchemaEnum this[int index]
        {
            get { return this._Schemas[index]; }
        }

        #endregion

        #region Constructores

        public SchemaEnumList()
        {
        }

        #endregion

        #region Metodos

        public void Add(SchemaEnum Item)
        {
            this._Schemas.Add(Item);
        }

        public void Remove(SchemaEnum Item)
        {
            this._Schemas.Remove(Item);
        }

        public void Clear()
        {
            this._Schemas.Clear();
        }

        #endregion

        #region Funciones

        #endregion

        #region Miembros de ICollection

        public void CopyTo(Array array, int index)
        {
            this._Schemas.CopyTo((SchemaEnum[])array, index);
        }

        public int Count
        {
            get { return this._Schemas.Count; }
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
            return this._Schemas.GetEnumerator();
        }

        #endregion
    }
}