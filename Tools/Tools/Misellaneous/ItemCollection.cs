using System;
using System.Collections.Generic;

namespace CM.Tools.Misellaneous
{
    public class ItemCollection : List<Item>
    {
        public Item this[string nId]
        {
            get
            {
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].Id == nId)
                        return this[i];
                }
                throw new Exception("No se encontró un item con el Id [" + nId + "]");
            }
            set
            {
                Item it = null;
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].Id == nId)
                    {
                        it = this[i];
                        this[i] = value;
                        break;
                    }
                }

                if (it == null)
                    Add(value);
            }
        }

        public Item Find(string nId)
        {
            for (var i = 0; i < Count; i++)
            {
                if (this[i].Id == nId)
                    return this[i];
            }
            return null;
        }
    }
}
