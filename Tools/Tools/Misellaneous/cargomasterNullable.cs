using System;

namespace CM.Tools.Misellaneous
{
    public enum EspecialescargomasterNullable
    {
        None,
        SysDate,
        DBUser
    }

    [Serializable]
    public class cargomasterNullable<T>
    {
        #region Declaraciones

        private T value;
        private bool isNull;
        private bool isDbNull;
        private bool isEspecial;
        private bool ignoreUpdate;
        private bool ignoreInsert;
        private EspecialescargomasterNullable especial;

        #endregion

        #region Constructores

        public cargomasterNullable()
        {
            SetNull(true);
        }

        public cargomasterNullable(EspecialescargomasterNullable value)
        {
            SetEspecial(value != EspecialescargomasterNullable.None, value);
        }

// ReSharper disable UnusedParameter.Local
        public cargomasterNullable(DBNull value)
// ReSharper restore UnusedParameter.Local
        {
            SetDbNull(true);
        }

        public cargomasterNullable(T value)
        {
            SetValue(value);
        }

        #endregion

        #region Propiedades

        public bool HasValue { get { return !this.IsNull && !this.IsDbNull && !this.IsEspecial; } }

        public T Value
        {
            get { return this.value; }
            set { SetValue(value); }
        }

        public bool IsNull
        {
            get { return this.isNull; }
            set { SetNull(value); }
        }

        public bool IsDbNull
        {
            get { return this.isDbNull; }
            set { SetDbNull(value); }
        }

        public bool IsEspecial
        {
            get { return this.isEspecial; }
        }

        public bool IgnoreUpdate
        {
            get { return this.ignoreUpdate; }
            set { this.ignoreUpdate = value; }
        }

        public bool IgnoreInsert
        {
            get { return this.ignoreInsert; }
            set { this.ignoreInsert = value; }
        }

        public EspecialescargomasterNullable Especial
        {
            get { return this.especial; }
            set { SetEspecial(value != EspecialescargomasterNullable.None, value); }
        }

        public Type ValueType { get { return typeof(T); } }

        #endregion

        #region Operadores

        public static implicit operator cargomasterNullable<T>(DBNull value)
        {
            return new cargomasterNullable<T>(value);
        }

        public static implicit operator cargomasterNullable<T>(T value)
        {
            return new cargomasterNullable<T>(value);
        }

        public static implicit operator T(cargomasterNullable<T> value)
        {
            return value.Value;
        }

        #endregion

        #region Metodos

        private void SetValue(T nValue)
        {
// ReSharper disable ConvertConditionalTernaryToNullCoalescing
            this.value = (nValue == null) ? default(T) : nValue;
// ReSharper restore ConvertConditionalTernaryToNullCoalescing
            this.isNull = (nValue == null);
            this.isDbNull = (nValue is DBNull);

            if (nValue is EspecialescargomasterNullable)
            {
                this.isEspecial = true;
                this.especial = (EspecialescargomasterNullable)Enum.Parse(typeof(EspecialescargomasterNullable), nValue.ToString());
            }
            else
            {
                this.isEspecial = false;
                this.especial = EspecialescargomasterNullable.None;
            }
        }

        private void SetNull(bool nIsNull)
        {
            if (nIsNull)
            {
                this.value = default(T);
                this.isNull = true;
                this.isDbNull = false;
                this.isEspecial = false;
                this.especial = EspecialescargomasterNullable.None;
            }
            else
            {
                this.isNull = false;
            }
        }

        private void SetDbNull(bool nIsDbNull)
        {
            if (nIsDbNull)
            {
                this.value = default(T);
                this.isNull = false;
                this.isDbNull = true;
                this.isEspecial = false;
                this.especial = EspecialescargomasterNullable.None;
            }
            else
            {
                this.isDbNull = false;
            }
        }

        private void SetEspecial(bool isEspcial, EspecialescargomasterNullable nValue)
        {
            if (isEspcial)
            {
                this.value = default(T);
                this.isNull = false;
                this.isDbNull = false;
                this.isEspecial = true;
                this.especial = nValue;
            }
            else
            {
                this.isEspecial = false;
                this.especial = EspecialescargomasterNullable.None;
            }
        }

        #endregion

        #region Funciones

        public T GetValueOrDefault()
        {
            return this.HasValue ? this.value : default(T);
        }

        public T GetValueOrDefault(T defaultValue)
        {
            return this.HasValue ? this.value : defaultValue;
        }

        public override bool Equals(object other)
        {
            if (!this.HasValue)
                return (other == null);

            return other != null && this.value.Equals(other);
        }

        public override int GetHashCode()
        {
// ReSharper disable NonReadonlyFieldInGetHashCode
            return !this.HasValue ? 0 : this.value.GetHashCode();
// ReSharper restore NonReadonlyFieldInGetHashCode
        }

        public override string ToString()
        {
            return !this.HasValue ? "" : this.value.ToString();
        }

        #endregion
    }
}