using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;

    public class XmlList<T> : List<T>
    {
        public XmlList()
        {
        }

        public static List<T> ConvertToXmlList(DataTable nTableData)
        {
            List<T> xmlList = new List<T>();
            var TypeAssembly = Assembly.GetAssembly(typeof(T));

            foreach (DataRow row in nTableData.Rows)
                xmlList.Add(ConvertToXmlBasicType(row, TypeAssembly));

            return xmlList;
        }

        public static T ConvertToXmlBasicType(DataRow nDataRow)
        {
            var TypeAssembly = Assembly.GetAssembly(typeof(T));
            return ConvertToXmlBasicType(nDataRow, TypeAssembly);
        }

        public static T ConvertToXmlBasicType(DataRow nDataRow, bool nValidateColumns)
        {
            var TypeAssembly = Assembly.GetAssembly(typeof(T));
            return ConvertToXmlBasicType(nDataRow, TypeAssembly, nValidateColumns);
        }

        public static T ConvertToXmlBasicType(DataRow nDataRow, Assembly nAssemblyType)
        {
            return ConvertToXmlBasicType(nDataRow, nAssemblyType, false);
        }

        public static T ConvertToXmlBasicType(DataRow nDataRow, Assembly nAssemblyType, bool nValidateColumns)
        {
            Type nType = typeof(T);
            T xmlItem = (T)nAssemblyType.CreateInstance(nType.FullName);

            foreach (DataColumn col in nDataRow.Table.Columns)
            {
                //PropertyInfo p = nType.GetProperty(col.ColumnName);
                FieldInfo p = nType.GetField(col.ColumnName);
                if (p == null)
                {
                    if (nValidateColumns)
                    {
                        throw new Exception("El tipo " + nType.Name + " no contiene la propiedad " + col.ColumnName);
                    }
                }
                else
                {
                    object val = nDataRow[col.ColumnName];
                    p.SetValue(xmlItem, DBNulls.ConvertType(val, p.FieldType, col.ColumnName));//, null);
                }
            }

            return xmlItem;
        }        
    }

    public class XmlColArrayConverter
    {
        public static List<XmlColArray> ToXmlRowArray(DataTable tabla)
        {
            var listData = new List<XmlColArray>();
            Type nType = typeof(XmlColArray);

            foreach (DataRow row in tabla.Rows)
            {
                XmlColArray xmlItem = new XmlColArray();

                for (int i = 0; i < tabla.Columns.Count; i++)
                {
                    string colName = "Col" + i.ToString("00");
                    FieldInfo p = nType.GetField(colName);
                    if (p == null)
                    {
                        throw new Exception("El tipo " + nType.Name + " no contiene la propiedad " + colName);
                    }
                    object val = row[i];

                    if (val.GetType().Name == "Byte[]")
                    {
                        p.SetValue(xmlItem, "[Binary]");
                    }
                    else
                    {
                        p.SetValue(xmlItem, DBNulls.ConvertType(val, p.FieldType, colName));//, null);
                    }

                }
                listData.Add(xmlItem);
            }

            return listData;
        }

    }

    public class XmlColArray
    {
        public string Col00;
        public string Col01;
        public string Col02;
        public string Col03;
        public string Col04;
        public string Col05;
        public string Col06;
        public string Col07;
        public string Col08;
        public string Col09;
        public string Col10;
        public string Col11;
        public string Col12;
        public string Col13;
        public string Col14;
        public string Col15;
        public string Col16;
        public string Col17;
        public string Col18;
        public string Col19;
        public string Col20;
        public string Col21;
        public string Col22;
        public string Col23;
        public string Col24;
        public string Col25;
        public string Col26;
        public string Col27;
        public string Col28;
        public string Col29;
        public string Col30;
        public string Col31;
        public string Col32;
        public string Col33;
        public string Col34;
        public string Col35;
        public string Col36;
        public string Col37;
        public string Col38;
        public string Col39;
        public string Col40;
        public string Col41;
        public string Col42;
        public string Col43;
        public string Col44;
        public string Col45;
        public string Col46;
        public string Col47;
        public string Col48;
        public string Col49;
        public string Col50;
        public string Col51;
        public string Col52;
        public string Col53;
        public string Col54;
        public string Col55;
        public string Col56;
        public string Col57;
        public string Col58;
        public string Col59;
        public string Col60;
        public string Col61;
        public string Col62;
        public string Col63;
        public string Col64;
        public string Col65;
        public string Col66;
        public string Col67;
        public string Col68;
        public string Col69;
        public string Col70;
        public string Col71;
        public string Col72;
        public string Col73;
        public string Col74;
        public string Col75;
        public string Col76;
        public string Col77;
        public string Col78;
        public string Col79;
        public string Col80;
        public string Col81;
        public string Col82;
        public string Col83;
        public string Col84;
        public string Col85;
        public string Col86;
        public string Col87;
        public string Col88;
        public string Col89;
        public string Col90;
        public string Col91;
        public string Col92;
        public string Col93;
        public string Col94;
        public string Col95;
        public string Col96;
        public string Col97;
        public string Col98;
        public string Col99;
    }
