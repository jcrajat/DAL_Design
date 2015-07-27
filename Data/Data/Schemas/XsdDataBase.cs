using System;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using CMData.DataBase;

namespace CMData.Schemas
{
    public partial class XsdDataBase
    {
        public List<Parameter> GetPrimaryKeys(XsdDataBase.TBL_ObjectRow table)
        {
            var param = new List<Parameter>();
            var fields = table.GetTBL_FieldRows();

            foreach (var field in fields)
            {
                //Si es una llave primaria y no es columna de una llave foranea
                if (field.PrimaryKey_Order != "")
                {
                    var direction = (ParameterDirection)(Enum.Parse(typeof(ParameterDirection), field.Direction));
                    var fType = (DbType)(Enum.Parse(typeof(DbType), field.Field_Type));

                    param.Add(new Parameter(field.Field_Name, fType, field.Specific_Type, null, field.Is_Nullable, field.Max_Length, field.Precision, field.Scale, direction));
                }
            }

            return param;
        }

        public List<Parameter> GetParameters(XsdDataBase.TBL_ObjectRow storedProcedure)
        {
            var param = new List<Parameter>();
            var fields = storedProcedure.GetTBL_FieldRows();

            foreach (var field in fields)
            {
                var direction = (ParameterDirection)(Enum.Parse(typeof(ParameterDirection), field.Direction));
                var fType = (DbType)(Enum.Parse(typeof(DbType), field.Field_Type));

                param.Add(new Parameter(field.Field_Name, fType, field.Specific_Type, null, field.Is_Nullable, field.Max_Length, field.Precision, field.Scale, direction));
            }

            return param;
        }

        public List<Parameter> GetForeignsOnPrimaryKeys(XsdDataBase.TBL_ObjectRow table)
        {
            var param = new List<Parameter>();
            var fields = table.GetTBL_FieldRows();

            foreach (var field in fields)
            {
                //Si es una llave primaria y no es columna de una llave foranea
                if (field.PrimaryKey_Order != "" && field.GetTBL_RelationRows().Length != 0)
                {
                    var direction = (ParameterDirection)(Enum.Parse(typeof(ParameterDirection), field.Direction));
                    var fType = (DbType)(Enum.Parse(typeof(DbType), field.Field_Type));

                    param.Add(new Parameter(field.Field_Name, fType, field.Specific_Type, "", field.Is_Nullable, field.Max_Length, field.Precision, field.Scale, direction));
                }
            }

            return param;
        }

        public List<Parameter> GetTableColumns(XsdDataBase.TBL_ObjectRow table)
        {
            var param = new List<Parameter>();
            var fields = table.GetTBL_FieldRows();

            foreach (var field in fields)
            {
                var direction = (ParameterDirection)(Enum.Parse(typeof(ParameterDirection), field.Direction));
                var fType = (DbType)(Enum.Parse(typeof(DbType), field.Field_Type));

                param.Add(new Parameter(field.Field_Name, fType, field.Specific_Type, "", field.Is_Nullable, field.Max_Length, field.Precision, field.Scale, direction));
            }

            return param;
        }

        public List<Parameter> GetIdOnPrimaryKey(XsdDataBase.TBL_ObjectRow table)
        {
            var fields = table.GetTBL_FieldRows();
            var KeyId = new List<Parameter>();

            foreach (var field in fields)
            {
                //Si es una llave primaria y no es columna de una llave foranea
                if (field.PrimaryKey_Order != "" && field.GetTBL_RelationRows().Length == 0)
                {
                    var direction = (ParameterDirection)(Enum.Parse(typeof(ParameterDirection), field.Direction));
                    var fType = (DbType)(Enum.Parse(typeof(DbType), field.Field_Type));

                    KeyId.Add(new Parameter(field.Field_Name, fType, field.Specific_Type, "", field.Is_Nullable, field.Max_Length, field.Precision, field.Scale, direction));
                }
            }

            return KeyId;
        }

        public List<Parameter> GetFilterColumns(XsdDataBase.TBL_FilterRow filtro, int id_Objeto)
        {
            var param = new List<Parameter>();
            var filterFields = filtro.GetTBL_Filter_FieldRows();

            foreach (var Flt in filterFields)
            {
                var field = (TBL_FieldRow)(TBL_Field.Select("fk_Object = " + id_Objeto + " AND Field_Name = '" + Flt.Field_Name + "'")[0]);

                var direction = (ParameterDirection)(Enum.Parse(typeof(ParameterDirection), field.Direction));
                var fType = (DbType)(Enum.Parse(typeof(DbType), field.Field_Type));

                param.Add(new Parameter(field.Field_Name, fType, "", field.Specific_Type, field.Is_Nullable, field.Max_Length, field.Precision, field.Scale, direction));
            }

            return param;
        }
    }

    [Serializable]
    public class Parameter
    {
        public string Name { get; set; }
        public DbType Type { get; set; }
        public object Value { get; set; }
        public bool IsNullable { get; set; }
        public int MaxLength { get; set; }
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public System.Data.ParameterDirection Direction { get; set; }
        public string SpecificType { get; set; }

        public FilterOption FilterOption { get; set; }
        public int FilterGroup { get; set; }

        //Futuras implementaciones
        //public ComparitionType ComparitionType { get; set; }

        public Parameter(string nName, object nValue)
        {
            Name = nName;
            Value = nValue;

            FilterOption = FilterOption.And;
            FilterGroup = 1;
        }

        public Parameter(string nName, object nValue, DbType nType)
        {
            Name = nName;
            Value = nValue;
            Type = nType;

            FilterOption = FilterOption.And;
            FilterGroup = 1;
        }

        public Parameter(string nName, object nValue, FilterOption nFilterOption, int nFilterGroup)
        {
            Name = nName;
            Value = nValue;

            FilterOption = nFilterOption;
            FilterGroup = nFilterGroup;
        }

        public Parameter(string nName, DbType nType, string nSpecificType, object nValue, bool nIsNullable, int nMaxLength, byte nPrecision, byte nScale, System.Data.ParameterDirection nDirection)
        {
            Name = nName;
            Type = nType;
            Value = nValue;
            IsNullable = nIsNullable;
            MaxLength = nMaxLength;
            Precision = nPrecision;
            Scale = nScale;
            Direction = nDirection;
            SpecificType = nSpecificType;

            FilterOption = FilterOption.And;
            FilterGroup = 1;

            //ComparitionType = ComparitionType.Equal;
        }

        public Parameter(string nName, DbType nType, string nSpecificType, object nValue, bool nIsNullable, int nMaxLength, byte nPrecision, byte nScale, System.Data.ParameterDirection nDirection, FilterOption nFilterOption, int nFilterGroup)
        {
            Name = nName;
            Type = nType;
            Value = nValue;
            IsNullable = nIsNullable;
            MaxLength = nMaxLength;
            Precision = nPrecision;
            Scale = nScale;
            Direction = nDirection;
            SpecificType = nSpecificType;

            FilterOption = nFilterOption;
            FilterGroup = nFilterGroup;

            //ComparitionType = ComparitionType.Equal;
        }

        public Parameter Clone()
        {
            return new Parameter(this.Name, this.Value)
            {
                Type = this.Type,
                IsNullable = this.IsNullable,
                MaxLength = this.MaxLength,
                Precision = this.Precision,
                Scale = this.Scale,
                Direction = this.Direction,
                SpecificType = this.SpecificType,
                FilterOption = this.FilterOption,
                FilterGroup = this.FilterGroup
                //ComparitionType = this.ComparitionType
            };
        }

        public static Parameter Find(List<Parameter> items, string nName)
        {
            foreach (var item in items)
            {
                if (item.Name == nName)
                {
                    return item;
                }
            }

            return null;
        }

    }

    public enum ReturnType
    {
        TablaGenerica,
        Escalar,
        TablaTipada,
        Nada
    }

    public enum ComparitionType
    {
        MajorThan,
        MinorThan,
        Equal
    }
}
