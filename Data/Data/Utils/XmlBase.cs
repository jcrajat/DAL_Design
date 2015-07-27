using System;
using System.Xml.Serialization;
using System.Collections;
using System.Data;

[Serializable]
public class XmlBase //: IXmlSerializable
{
    public XmlBase()
    {
    }

    public XmlBase(DataRow nDataRow)
    {
        FromDataRow(nDataRow);
    }

    //System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
    //{
    //    //var schema = new System.Xml.Schema.XmlSchema();
    //    //schema.na
    //    //schema.Namespaces = "cargomaster";
    //    //return schema;
    //    return null;
    //}

    //void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
    //{
    //    throw new NotImplementedException();
    //}

    //void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
    //{
    //    var fields = this.GetType().GetFields();

    //    foreach (var field in fields)
    //    {
    //        object fieldValue = field.GetValue(this);
    //        if (!(DBNulls.IsDbNull(fieldValue) || DBNulls.IsNull(fieldValue)))
    //        {
    //            writer.WriteStartElement(field.Name);
    //            writer.WriteString(fieldValue.ToString());
    //            writer.WriteEndElement();
    //        }

    //    }
    //}

    /// <summary>
    /// Exporta los datos como un arreglo de objetos
    /// </summary>
    /// <returns>Arreglo de datos</returns>
    public object[] ToArray()
    {
        var fields = this.GetType().GetFields();
        ArrayList array = new ArrayList();
        foreach (var field in fields)
        {
            object fieldValue = field.GetValue(this);
            array.Add(fieldValue);
        }
        return array.ToArray();
    }

    /// <summary>
    /// Exporta los datos contenidos en el objeto en un nuevo DataRow de acuerdo a la estructura de la tabla ingresada
    /// </summary>
    /// <param name="nDataTable">DataTable que contiene la estructura de la tabla</param>
    /// <returns>DataRow con los datos del objeto</returns>
    public DataRow ToDataRow(DataTable nDataTable)
    {
        var fields = this.GetType().GetFields();
        DataRow row = nDataTable.NewRow();
        foreach (var field in fields)
        {
            object fieldValue = field.GetValue(this);
            if (row.Table.Columns.Contains(field.Name))
                row[field.Name] = (fieldValue == null) ? DBNull.Value : DBNulls.GetValueFromNullable(fieldValue);
        }
        return row;
    }

    /// <summary>
    /// Establecer los valores del objeto a partir de los datos de un DataRow
    /// </summary>
    /// <param name="nDataRow">DataRow con los datos de origen</param>
    /// <param name="nIgnoreEmptyColumns">Indica si no se debe validar la existencia de la columna</param>
    public void FromDataRow(DataRow nDataRow, bool nIgnoreEmptyColumns)
    {
        var fields = this.GetType().GetFields();
        foreach (var field in fields)
        {
            if (!nIgnoreEmptyColumns || nDataRow.Table.Columns.Contains(field.Name))
            {
                try
                {
                    object fieldValue = nDataRow[field.Name];
                    field.SetValue(this, DBNulls.ConvertType(fieldValue, field.FieldType, field.Name));
                }
                catch (Exception ex)
                {
                    throw new Exception("No fue posible obtener el valor de " + field.Name + ", " + ex.Message, ex);
                }
            }
        }
    }

    /// <summary>
    /// Establecer los valores del objeto a partir de los datos de un DataRow
    /// </summary>
    /// <param name="nDataRow">DataRow con los datos de origen</param>
    public void FromDataRow(DataRow nDataRow)
    {
        FromDataRow(nDataRow, false);
    }
}