namespace CM.Tools.Misellaneous
{
    public class Item
    {
        #region Propiedades

        public object Value { get; set; }

        public string Display { get; set; }

        public string Id { get; set; }

        #endregion

        #region Constructores

        public Item()
            : this(null, "", "")
        {
        }

        public Item(object nValue, string nDisplay)
            : this(nValue, nDisplay, "")
        {
        }
        public Item(object nValue, string nDisplay, string nId)
        {
            Value = nValue;
            Display = nDisplay;
            Id = nId;
        }

        #endregion

        #region Funciones

        public override string ToString()
        {
            return (string)(this.Value) + " - " + this.Display;
        }

        #endregion
    }
}