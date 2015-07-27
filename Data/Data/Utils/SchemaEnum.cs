using System;
using System.Collections.Generic;
using System.Text;

namespace CMData
{
    public abstract class SchemaEnum
    {
        private string _SchemaName;
        
        public string SchemaName
        {
            get { return _SchemaName; }
        }

        public SchemaEnum(string nSchemaName)
        {
            this._SchemaName = nSchemaName;
        }
    }
}
