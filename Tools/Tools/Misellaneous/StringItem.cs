namespace CM.Tools.Misellaneous
{
    public class StringItem
    {
        #region Propiedades

        public string Value { get; set; }

        public string Display { get; set; }        

        #endregion

        #region Constructores

        public StringItem()
        {
        }

        public StringItem(string nValue, string nDisplay)
        {
            Value = nValue;
            Display = nDisplay;
        }

        #endregion

        #region Funciones

        public override string ToString()
        {
            return this.Value + " - " + this.Display;
        }

        #endregion
    }
}
