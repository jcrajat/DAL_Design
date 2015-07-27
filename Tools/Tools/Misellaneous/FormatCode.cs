using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace CM.Tools.Misellaneous
{
    public class FormatCode
    {
        #region Declaraciones

        private static StringCollection _InvalidCharacters;

        #endregion

        #region Propiedades

        public static StringCollection InvalidCharacters
        {
            get
            {
                if (_InvalidCharacters == null)
                {
                    _InvalidCharacters = new StringCollection();
                    _InvalidCharacters.AddRange(new[] {"@", ".", "$", " "});
                }
                return _InvalidCharacters;
            }
        }

        #endregion

        #region Funciones

        public static string ToIdentifier(object name)
        {
            var formatName = name.ToString();

            foreach (var ichar in InvalidCharacters)
            {
                formatName = formatName.Replace(ichar, "_");
            }

            return formatName;
        }

        public static bool IsValidIdentifier(string nIdentifier)
        {
            // create our match pattern
            const string pattern = @"[_a-zA-Z][_a-zA-Z0-9]*";

            // create our Regular Expression object
            var check = new Regex(pattern);

            // check to make sure an ip address was provided
            return nIdentifier != "" && check.IsMatch(nIdentifier, 0);   
        }

        public static String Tabs(int tabsCount)
        {
            var strTabs = "";
            for (var i = 0; i < tabsCount; i++)
            {
                strTabs += ControlChars.Tab;
            }

            return strTabs;
        }

        #endregion
    }
}
