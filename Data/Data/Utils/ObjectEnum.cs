using System;
using System.Collections.Generic;
using System.Text;

namespace CMData
{
    public abstract class ObjectEnum
    {
        private string _ObjectName;
        
        public string ObjectName
        {
            get { return _ObjectName; }
        }

        public ObjectEnum(string nObjectName)
        {
            this._ObjectName = nObjectName;
        }
    }
}
