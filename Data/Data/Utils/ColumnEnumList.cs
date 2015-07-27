using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CMData.Manager
{
    public abstract class ColumnEnumList : ICollection
    {
        #region Declaraciones

        protected List<ColumnEnum> _Columns = new List<ColumnEnum>();

        #endregion

        #region Propiedades

        public ColumnEnum this[int index]
        {
            get { return this._Columns[index]; }
        }

        #endregion

        #region Constructores

        public ColumnEnumList()
        {
        }

        #endregion

        #region Metodos

        public void Add(ColumnEnum Item, bool Ascendente)
        {
            Item.Ascendente = Ascendente;
            this._Columns.Add(Item);
        }

        public void Remove(ColumnEnum Item)
        {
            this._Columns.Remove(Item);
        }

        public void Clear()
        {
            this._Columns.Clear();
        }

        #endregion

        #region Funciones

        public string getOrderByParams(DataBase.DataBase nDataBase)
        {
            var OrderByParams = new StringBuilder("");

            foreach (var Item in this._Columns)
            {
                if (OrderByParams.Length > 0)
                    OrderByParams.Append(", ");

                OrderByParams.Append(nDataBase.FormatToDatabaseColumnName(Item.ColumnName) + " " + (Item.Ascendente ? nDataBase.Identifier_OrderBy_ASC : nDataBase.Identifier_OrderBy_DESC));
            }

            return OrderByParams.ToString();
        }

        public string ToColumnsString(DataBase.DataBase nDataBase)
        {
            var columns = new StringBuilder("");

            foreach (var Item in this._Columns)
            {
                if (columns.Length > 0)
                    columns.Append(", ");

                if( nDataBase != null )
                    columns.Append(nDataBase.FormatToDatabaseColumnName(Item.ColumnName));
                else
                    columns.Append(Item.ColumnName);
            }

            return columns.ToString();
        }

        #endregion

        #region Miembros de ICollection

        public void CopyTo(Array array, int index)
        {
            this._Columns.CopyTo((ColumnEnum[])array, index);
        }

        public int Count
        {
            get { return this._Columns.Count; }
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
            return this._Columns.GetEnumerator();
        }

        #endregion
    }
}