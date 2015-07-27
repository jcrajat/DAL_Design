using System.Reflection;

namespace Data.Mapping
{
    static class Program
    {
        #region Propiedades

        internal static string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                // if there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];

                    // if it is not an empty string, return it
                    if (titleAttribute.Title != "") return titleAttribute.Title;
                }

                // if there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        internal static string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        internal static string AssemblyName
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
        }

        internal static string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                // if there aren't any Description attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;

                // if there is a Description attribute, return its value
            }
        }

        internal static string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                // if there aren't any Product attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;

                // if there is a Product attribute, return its value
            }
        }

        internal static string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                // if there aren't any Copyright attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;

                // if there is a Copyright attribute, return its value
            }
        }

        internal static string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                // if there aren't any Company attributes, return an empty string
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;

                // if there is a Company attribute, return its value
            }
        }

        #endregion
    }
}