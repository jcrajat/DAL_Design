namespace CM.Tools.Misellaneous
{
    public class ComboBoxListItem
    {
        #region Propiedades

        public string Name { get; set; }
        public object Value { get; set; }

        #endregion

        #region Constructores

        public ComboBoxListItem()
        {
        }

        public ComboBoxListItem(string nName, object nValue)
        {
            Name = nName;
            Value = nValue;
        }

        #endregion

        #region Funciones

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
