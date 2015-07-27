using System;
using System.Text.RegularExpressions;
using Tools;

namespace CM.Tools.Misellaneous
{
    public static class DataConvert
    {
        public enum EnumDateFormat : byte
        {
            yyyyMMdd,
            ddMMyyyy,
            MMddyyyy,
            yyyyMMdd_hhmmss,
            ddMMyyyy_hhmmss,
            MMddyyyy_hhmmss,
        }

        public static bool ToBool(string nValue)
        {
            switch (nValue.ToUpper())
            {
                case "TRUE":
                case "YES":
                case "ON":
                case "S":
                case "Y":
                case "1":
                    return true;
                default:
                    return false;
            }
        }

        public static long Round(double nValue, int nSalto)
        {
            if ((nValue/nSalto) - Math.Floor(nValue/nSalto) > 0.5)
                return (long) ((Math.Floor(nValue/nSalto) + 1)*nSalto);

            return (long) (Math.Floor(nValue/nSalto)*nSalto);
        }

        public static long ToNumeric(string nValue)
        {
            if (nValue == "") return 0;

            long resultado;

            return long.TryParse(nValue, out resultado) ? resultado : 0;
        }

        public static decimal ToNumericDec(string nValue, string nPuntoFlotante)
        {
            if (nValue == "")
                return 0;

            var valor = nValue.Replace(nPuntoFlotante, GetPuntoFlotante());
            decimal resultado;

            return decimal.TryParse(valor, out resultado) ? resultado : 0;
        }

        public static double ToNumericD(string nValue, string nPuntoFlotante)
        {
            if (nValue == "")
                return 0;

            var valor = nValue.Replace(nPuntoFlotante, GetPuntoFlotante());
            double resultado;

            return double.TryParse(valor, out resultado) ? resultado : 0;
        }

        public static float ToNumericF(string nValue, string nPuntoFlotante)
        {
            if (nValue == "")
                return 0;

            var valor = nValue.Replace(nPuntoFlotante, GetPuntoFlotante());
            float resultado;

            return float.TryParse(valor, out resultado) ? resultado : 0;
        }

        public static bool IsNumeric(string nValue)
        {
            if (nValue == "")
                return false;

            long resultado1;
            double resultado2;

            return (long.TryParse(nValue, out resultado1) || double.TryParse(nValue, out resultado2));
        }

        public static bool IsDate(string nValue, EnumDateFormat nFormat, char nSeparator)
        {
            return (ToDate(nValue, nFormat, nSeparator) != null);
        }

        public static cargomasterNullable<DateTime> ToDate(string nValue, EnumDateFormat nFormat, char nSeparator)
        {
            try
            {
                var partes = nValue.Split(' ');
                var partesFecha = partes[0].Split(nSeparator);
                var partesHora = (partes.Length > 1) ? partes[1].Split(':') : new string[0];

                switch (nFormat)
                {
                    case EnumDateFormat.ddMMyyyy:
                        return new DateTime(int.Parse(partesFecha[2]), int.Parse(partesFecha[1]), int.Parse(partesFecha[0]));

                    case EnumDateFormat.ddMMyyyy_hhmmss:

                        if (partesHora.Length > 1)
                        {
                            return new DateTime(int.Parse(partesFecha[2]), int.Parse(partesFecha[1]), int.Parse(partesFecha[0]),
                                                int.Parse(partesHora[0]), int.Parse(partesHora[1]), int.Parse(partesHora[2]));
                        }

                        return new DateTime(int.Parse(partesFecha[2]), int.Parse(partesFecha[1]), int.Parse(partesFecha[0]));

                    case EnumDateFormat.yyyyMMdd:
                        return new DateTime(int.Parse(partesFecha[0]), int.Parse(partesFecha[1]), int.Parse(partesFecha[2]));

                    case EnumDateFormat.yyyyMMdd_hhmmss:
                        if (partesHora.Length > 1)
                        {
                            return new DateTime(int.Parse(partesFecha[0]), int.Parse(partesFecha[1]), int.Parse(partesFecha[2]),
                                                int.Parse(partesHora[0]), int.Parse(partesHora[1]), int.Parse(partesHora[2]));
                        }

                        return new DateTime(int.Parse(partesFecha[0]), int.Parse(partesFecha[1]), int.Parse(partesFecha[2]));

                    case EnumDateFormat.MMddyyyy:
                        return new DateTime(int.Parse(partesFecha[2]), int.Parse(partesFecha[0]), int.Parse(partesFecha[1]));

                    case EnumDateFormat.MMddyyyy_hhmmss:

                        if (partesHora.Length > 1)
                        {
                            return new DateTime(int.Parse(partesFecha[2]), int.Parse(partesFecha[0]), int.Parse(partesFecha[1]),
                                                int.Parse(partesHora[0]), int.Parse(partesHora[1]), int.Parse(partesHora[2]));
                        }

                        return new DateTime(int.Parse(partesFecha[2]), int.Parse(partesFecha[1]), int.Parse(partesFecha[0]));

                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static string ToString(DateTime nValue, EnumDateFormat nFormat, char nSeparator = '/')
        {
            var s = nSeparator;
            var year = nValue.Year.ToString();
            var month = ((nValue.Month < 10) ? "0" : "") + nValue.Month.ToString();
            var day = ((nValue.Day < 10) ? "0" : "") + nValue.Day.ToString();
            var hour = ((nValue.Hour < 10) ? "0" : "") + nValue.Hour.ToString();
            var minute = ((nValue.Minute < 10) ? "0" : "") + nValue.Minute.ToString();
            var second = ((nValue.Second < 10) ? "0" : "") + nValue.Second.ToString();

            switch (nFormat)
            {
                case EnumDateFormat.ddMMyyyy:
                    return day + s + month + s + year;

                case EnumDateFormat.ddMMyyyy_hhmmss:
                    return day + s + month + s + year + " " + hour + minute + second;

                case EnumDateFormat.yyyyMMdd:
                    return year + s + month + s + day;

                case EnumDateFormat.yyyyMMdd_hhmmss:
                    return year + s + month + s + day + " " + hour + minute + second;

                case EnumDateFormat.MMddyyyy:
                    return month + s + day + s + year;

                case EnumDateFormat.MMddyyyy_hhmmss:
                    return month + s + day + s + year + " " + hour + minute + second;

                default:
                    return null;
            }
        }

        public static string GetPuntoFlotante()
        {            
            try
            {
                return double.Parse("1.2") == 1.2 ? "." : ",";
            }
            catch
            {
                return ",";
            }
        }

        public static string GetNoPuntoFlotante()
        {
            try
            {
                return double.Parse("1.2") == 1.2 ? "," : ".";
            }
            catch
            {
                return ".";
            }
        }

        public static bool IsValidMail(string nMail)
        {
            var reg = new Regex(@"^(([^<;>;()[\]\\.,;:\s@\""]+(\.[^<;>;()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");

            return reg.IsMatch(nMail);
        }

        public static bool IsValidIP(string nIPAddress)
        {
            // create our match pattern
            const string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

            // create our Regular Expression object
            var check = new Regex(pattern);


            // check to make sure an ip address was provided
            return nIPAddress != "" && check.IsMatch(nIPAddress, 0);            
        }
    }
}
