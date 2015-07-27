using System;
using System.Reflection;
using CM.Tools.Misellaneous;
using CMData.Utils;
using Tools;

/// <summary>
/// Representa un valor no existente para cada tipo de dato
/// </summary>
public class DBNulls
{
    public static cargomasterNullable<string> StringValue
    {
        get { return (cargomasterNullable<string>)DBNull.Value; }
    }

    public static cargomasterNullable<object> ObjectValue
    {
        get { return (cargomasterNullable<object>)DBNull.Value; }
    }

    public static cargomasterNullable<int> IntValue
    {
        get { return (cargomasterNullable<int>)DBNull.Value; }
    }

    public static cargomasterNullable<long> LongValue
    {
        get { return (cargomasterNullable<long>)DBNull.Value; }
    }

    public static cargomasterNullable<decimal> DecimalValue
    {
        get { return (cargomasterNullable<decimal>)DBNull.Value; }
    }

    public static cargomasterNullable<byte> ByteValue
    {
        get { return (cargomasterNullable<byte>)DBNull.Value; }
    }

    public static cargomasterNullable<short> ShortValue
    {
        get { return (cargomasterNullable<short>)DBNull.Value; }
    }

    public static cargomasterNullable<bool> BoolValue
    {
        get { return (cargomasterNullable<bool>)DBNull.Value; }
    }

    public static cargomasterNullable<DateTime> DateTimeValue
    {
        get { return (cargomasterNullable<DateTime>)DBNull.Value; }
    }

    public static cargomasterNullable<Guid> GuidValue
    {
        get { return (cargomasterNullable<Guid>)DBNull.Value; }
    }

    public static bool IsNull(object NullableValue)
    {
        if (NullableValue != null && NullableValue.GetType().Name == "cargomasterNullable`1")
        {
            PropertyInfo p = NullableValue.GetType().GetProperty("IsNull");
            if (p != null)
                return (bool)p.GetValue(NullableValue, null);
        }
        return (NullableValue == null);
    }

    public static bool IgnoreUpdate(object NullableValue)
    {
        if (NullableValue != null && NullableValue.GetType().Name == "cargomasterNullable`1")
        {
            PropertyInfo p = NullableValue.GetType().GetProperty("IgnoreUpdate");
            if (p != null)
                return (bool)p.GetValue(NullableValue, null);
        }
        return false;
    }

    public static bool IgnoreInsert(object NullableValue)
    {
        if (NullableValue != null && NullableValue.GetType().Name == "cargomasterNullable`1")
        {
            PropertyInfo p = NullableValue.GetType().GetProperty("IgnoreInsert");
            if (p != null)
                return (bool)p.GetValue(NullableValue, null);
        }
        return false;
    }

    public static bool IsDbNull(object NullableValue)
    {
        if (NullableValue != null && NullableValue.GetType().Name == "cargomasterNullable`1")
        {
            PropertyInfo p = NullableValue.GetType().GetProperty("IsDbNull");
            if (p != null)
                return (bool)p.GetValue(NullableValue, null);
        }
        return (NullableValue is DBNull);
    }

    public static bool IsEspecial(object NullableValue)
    {
        if (NullableValue != null && NullableValue.GetType().Name == "cargomasterNullable`1")
        {
            PropertyInfo p = NullableValue.GetType().GetProperty("IsEspecial");
            if (p != null)
                return (bool)p.GetValue(NullableValue, null);
        }
        return (NullableValue is EspecialescargomasterNullable);
    }

    public static EspecialescargomasterNullable GetEspecial(object NullableValue)
    {
        if (NullableValue is EspecialescargomasterNullable)
        {
            return (EspecialescargomasterNullable)NullableValue;
        }
        else if (NullableValue.GetType().Name == "cargomasterNullable`1")
        {
            PropertyInfo p = NullableValue.GetType().GetProperty("Especial");
            if (p != null)
                return (EspecialescargomasterNullable)p.GetValue(NullableValue, null);
            else
                throw new Exception("Valor no es un dato Especial");
        }
        else
        {
            throw new Exception("Valor no es un dato Especial");
        }
    }

    public static DateTime GetDateTime(object NullableValue)
    {
        if (NullableValue is DateTime)
            return (DateTime)NullableValue;
        else if (NullableValue.GetType().Name == "cargomasterNullable`1" && NullableValue is cargomasterNullable<DateTime>)
            return ((cargomasterNullable<DateTime>)NullableValue).Value;
        else
            throw new Exception("Valor no es un DateTime");
    }

    public static object ConvertType(object value, Type nType, string nPropertyName)
    {
        try
        {
            if (value == null || value is DBNull)
                return null;
            else if (nType.Name.ToLower() == "bool" || nType.Name.ToLower() == "boolean")
                return AppTypes.ToAppBoolean(value);
            else if (nType.Name == "cargomasterNullable`1")
                return ConvertToNullableValue(value, nType);
            else
                return Convert.ChangeType(value, nType);
        }
        catch (Exception ex)
        {
            throw new Exception("No se puede cargar el valor [" + AppTypes.GetStringValue(value) + "] de la propiedad " + nPropertyName + ", " + ex.Message, ex);
        }
    }

    public static Type GetTypeFromNullableType(Type nType)
    {
        PropertyInfo pValueType = nType.GetProperty("ValueType");
        if (pValueType == null)
            throw new Exception("El tipo " + nType.Name + " no contiene la propiedad ValueType");

        var assemb = Assembly.GetAssembly(nType);

        object NullableValue = assemb.CreateInstance(nType.FullName);//, true, BindingFlags.CreateInstance, null, new object[] { null }, null, null);


        return (Type)pValueType.GetValue(NullableValue, null);
    }

    public static object GetValueFromNullable(object NullableValue)
    {
        PropertyInfo pInValue = NullableValue.GetType().GetProperty("Value");
        if (pInValue == null)
            throw new Exception("El tipo " + NullableValue.GetType().Name + " no contiene la propiedad Value");

        return pInValue.GetValue(NullableValue, null);
    }

    public static Type GetTypeFromNullable(object NullableValue)
    {
        PropertyInfo pValueType = NullableValue.GetType().GetProperty("ValueType");
        if (pValueType == null)
            throw new Exception("El tipo " + NullableValue.GetType().Name + " no contiene la propiedad ValueType");

        return (Type)pValueType.GetValue(NullableValue, null);
    }

    public static object ConvertToNullableValue(object Value, Type NullableType)
    {
        Type inType = GetTypeFromNullableType(NullableType);
        object inValue = Convert.ChangeType(Value, inType);

        var assemb = Assembly.GetAssembly(NullableType);
        object NullableValue = assemb.CreateInstance(NullableType.FullName, true, BindingFlags.CreateInstance, null, new object[] { inValue }, null, null);
        return NullableValue;
    }

    public static object GetPrimitiveObjectValue(object NullableValue)
    {
        if (NullableValue == null)
        {
            return DBNull.Value;
        }
        else if (NullableValue.GetType().Name == "cargomasterNullable`1")
        {
            if (IsDbNull(NullableValue))
                return DBNull.Value;
            else if (IsNull(NullableValue))
                return null;

            var innerValue = GetValueFromNullable(NullableValue);

            if (innerValue.GetType().Name == "cargomasterNullable`1")
                return GetPrimitiveObjectValue(innerValue);

            return innerValue;
        }
        else
        {
            return NullableValue;
        }
    }
}
