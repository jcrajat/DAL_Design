using CMData;
using CMData.Schemas;

namespace CM.DataModel.Schemas
{
    public class XsdDataBaseDesign : XsdDataBase
    {
        #region Propiedades

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public XsdDefault.TBL_ConfigDataTable TBL_Config { get; set; }

        #endregion

        #region Constructores

        public XsdDataBaseDesign()
            : base()
        {
            InitClass2();
        }

        protected XsdDataBaseDesign(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            InitClass2();
        }

        #endregion

        #region Metodos

        public void InitClass2()
        {
            if (this.Tables["TBL_Config"] == null)
            {
                this.TBL_Config = new XsdDefault.TBL_ConfigDataTable();
                base.Tables.Add(this.TBL_Config);
            }
        }

        #endregion
    }
}
