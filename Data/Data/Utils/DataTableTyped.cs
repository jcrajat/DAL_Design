using System;
using System.Collections.Generic;
using System.Data;

namespace CMData.Utils
{
    public class DataTableTyped<T> : DataTable where T : XmlBase
    {
        public DataTableTyped()
            : base()
        {
            var fields = typeof(T).GetFields();
            foreach (var field in fields)
            {
                Columns.Add(field.Name, field.FieldType);
            }
        }
        public void Add(T nTypeRowBase)
        {
            Rows.Add(nTypeRowBase.ToDataRow(this));
        }
    }
}
