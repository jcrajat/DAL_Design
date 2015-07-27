namespace CM.Tools.Misellaneous
{
    public class GenericItem<T>
    {
        #region Propiedades

        public T Value { get; set; }

        public string Display { get; set; }

        public string Id { get; set; }

        #endregion

        #region Constructores

        public GenericItem()
        {
        }

        public GenericItem(T nValue, string nDisplay)
            : this(nValue, nDisplay, "")
        {
        }
        public GenericItem(T nValue, string nDisplay, string nId)
        {
            Value = nValue;
            Display = nDisplay;
            Id = nId;
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
