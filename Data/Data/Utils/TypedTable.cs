using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace CMData.Utils
{
    public class TypedTable<T> : DataTable, IEnumerable<T>, IEnumerable where T : DataRow
    {
        #region Constructores

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
		public TypedTable()
			: base()
		{
		}

		[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal TypedTable(global::System.Data.DataTable table)
			: base()
		{
		}

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public TypedTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Implementacion IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {            
            foreach (T item in base.Rows)
            {
                yield return item;
            }
        }

        #endregion

        #region Implementacion IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return base.Rows.GetEnumerator();
        }

        #endregion
    }
}
