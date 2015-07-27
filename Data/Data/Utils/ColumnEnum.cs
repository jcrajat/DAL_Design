using System;
using System.Collections.Generic;
using System.Text;

namespace CMData
{
    public abstract class ColumnEnum
    {
        private string _ColumnName;
        
        public string ColumnName
        {
            get { return _ColumnName; }
        }

        public bool Ascendente { get; set; }

        public ColumnEnum(string nColumnName)
        {
            this._ColumnName = nColumnName;
            this.Ascendente = true;
        }
    }
}
