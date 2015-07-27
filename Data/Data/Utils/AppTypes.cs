using System;
using System.Collections.Specialized;
using System.Data;

namespace CMData.Utils
{
    public enum AppType
    {
        Text,
        Number,
        Date,
        Bool,
        BoolString,
        CustomList
    }

    public enum ValueOrigin
    {
        Interface,
        DefaultValue,
        NextId,
        CurrentUserId,
        CurrentUserLogin,
        Now,
        DbFuncion
    }

    public class AppTypes
    {
        public static AppType GetAppType(string mDataBaseType)
        {
            var mDbType = (DbType)Enum.Parse(typeof(DbType), mDataBaseType);
            switch (mDbType)
            {
                case DbType.Boolean:
                    return AppType.Bool;
                case DbType.String:
                case DbType.Guid:
                case DbType.Binary:
                    return AppType.Text;

                case DbType.DateTime:
                    return AppType.Date;

                case DbType.Int32:
                case DbType.Int64:
                case DbType.Decimal:
                case DbType.Int16:
                case DbType.Byte:
                case DbType.Single:
                    return AppType.Number;
            }
            return AppType.Text;
        }

        public static bool ToAppBoolean(object mDbBoolean)
        {
            StringCollection BooleanTrueValues = new StringCollection();
            BooleanTrueValues.AddRange(new string[] { "1", "S", "Y", "T", "V", "TRUE", "VERDADERO" });

            if (BooleanTrueValues.Contains(mDbBoolean.ToString().ToUpper()))
                return true;

            return false;
        }

        public static string GetDbDefaultBoolean(bool mAppBoolean)
        {
            if (mAppBoolean)
                return "1";
            else
                return "0";
        }

        public static string GetStringValue(object value)
        {
            try
            {
                return value.ToString();
            }
            catch
            {
                return "";
            }
        }
    }
}