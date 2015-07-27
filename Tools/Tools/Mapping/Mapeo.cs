using System;
using System.Runtime.InteropServices;

namespace CM.Tools.Mapping
{
    public class Mapeo
    {
        #region Declaraciones

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, Int32 dwFlags);
        [DllImport("mpr.dll")]
        static extern int WNetCancelConnection2(string lpName, Int32 dwFlags, bool bForce);

        // ReSharper disable NotAccessedField.Local
        // ReSharper disable UnusedMember.Local
        private struct NETRESOURCE
        {

            public int dwScope;

            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string lpLocalName;
            public string lpRemoteName;

            public string lpComment;
            public string lpProvider;
        }
        // ReSharper restore UnusedMember.Local
        // ReSharper restore NotAccessedField.Local

        // The following includes all the constants defined for NETRESOURCE,
        // not just the ones used in this example.
        public enum EnumNET_RESOURCE
        {
            CONNECT_UPDATE_PROFILE = 0x1,
            RESOURCETYPE_DISK = 0x1,
            RESOURCETYPE_PRINT = 0x2,
            RESOURCETYPE_ANY = 0x0,
            RESOURCE_CONNECTED = 0x1,
            RESOURCE_REMEMBERED = 0x3,
            RESOURCE_GLOBALNET = 0x2,
            RESOURCEDISPLAYTYPE_DOMAIN = 0x1,
            RESOURCEDISPLAYTYPE_GENERIC = 0x0,
            RESOURCEDISPLAYTYPE_SERVER = 0x2,
            RESOURCEDISPLAYTYPE_SHARE = 0x3,
            RESOURCEUSAGE_CONNECTABLE = 0x1,
            RESOURCEUSAGE_CONTAINER = 0x2
        }

        public enum EnumNET_ERROR
        {
            NO_ERROR = 0,
            ERROR_ACCESS_DENIED = 5,
            ERROR_ALREADY_ASSIGNED = 85,
            ERROR_BAD_DEV_TYPE = 66,
            ERROR_BAD_DEVICE = 1200,
            ERROR_BAD_NET_NAME = 67,
            ERROR_BAD_PROFILE = 1206,
            ERROR_BAD_PROVIDER = 1204,
            ERROR_BUSY = 170,
            ERROR_CANCELLED = 1223,
            ERROR_CANNOT_OPEN_PROFILE = 1205,
            ERROR_DEVICE_ALREADY_REMEMBERED = 1202,
            ERROR_EXTENDED_ERROR = 1208,
            ERROR_INVALID_PASSWORD = 86,
            ERROR_NO_NET_OR_BAD_PATH = 1203
        }

        #endregion

        #region Propiedades

        public string Unidad { get; set; }
        public string Carpeta { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }

        #endregion

        #region Metodos

        public Mapeo(string nUnidad, string nCarpeta)
        {
            Unidad = nUnidad;
            Carpeta = nCarpeta;
            Usuario = null;
            Contraseña = null;
        }
        public Mapeo(string nUnidad, string nCarpeta, string nUsuario, string nContraseña)
            : this(nUnidad, nCarpeta)
        {
            Usuario = nUsuario;
            Contraseña = nContraseña;
        }

        #endregion

        #region Funciones

        public EnumNET_ERROR Conectar()
        {
            var NetR = new NETRESOURCE();

            NetR.dwScope = (int)EnumNET_RESOURCE.RESOURCE_GLOBALNET;
            NetR.dwType = (int)EnumNET_RESOURCE.RESOURCETYPE_DISK;
            NetR.dwDisplayType = (int)EnumNET_RESOURCE.RESOURCEDISPLAYTYPE_SHARE;
            NetR.dwUsage = (int)EnumNET_RESOURCE.RESOURCEUSAGE_CONNECTABLE;
            NetR.lpLocalName = Unidad; // If undefined, Connect with no device
            NetR.lpRemoteName = Carpeta; // Your valid share
            //NetR.lpComment = "Optional Comment"
            //NetR.lpProvider =    ' Leave this undefined

            // If the UserName and Password arguments are NULL, the user context
            // for the process provides the default user name.
            return (EnumNET_ERROR)WNetAddConnection2(ref NetR, Contraseña, Usuario, (int)EnumNET_RESOURCE.CONNECT_UPDATE_PROFILE);
        }

        public EnumNET_ERROR Desconectar()
        {
            return (EnumNET_ERROR)WNetCancelConnection2(Unidad, (int)EnumNET_RESOURCE.CONNECT_UPDATE_PROFILE, false);
        }

        public string MensajeError(EnumNET_ERROR nError)
        {
            switch (nError)
            {
                case EnumNET_ERROR.NO_ERROR:
                    return "NO_ERROR";

                case EnumNET_ERROR.ERROR_ACCESS_DENIED:
                    return "ERROR_ACCESS_DENIED";

                case EnumNET_ERROR.ERROR_ALREADY_ASSIGNED:
                    return "ERROR ALREADY ASSIGNED";

                case EnumNET_ERROR.ERROR_BAD_DEV_TYPE:
                    return "ERROR BAD DEV TYPE";

                case EnumNET_ERROR.ERROR_BAD_DEVICE:
                    return "ERROR BAD DEVICE";

                case EnumNET_ERROR.ERROR_BAD_NET_NAME:
                    return "ERROR BAD NET NAME";

                case EnumNET_ERROR.ERROR_BAD_PROFILE:
                    return "ERROR BAD PROFILE";

                case EnumNET_ERROR.ERROR_BAD_PROVIDER:
                    return "ERROR BAD PROVIDER";

                case EnumNET_ERROR.ERROR_BUSY:
                    return "ERROR BUSY";

                case EnumNET_ERROR.ERROR_CANCELLED:
                    return "ERROR CANCELLED";

                case EnumNET_ERROR.ERROR_CANNOT_OPEN_PROFILE:
                    return "ERROR CANNOT OPEN PROFILE";

                case EnumNET_ERROR.ERROR_DEVICE_ALREADY_REMEMBERED:
                    return "ERROR DEVICE ALREADY REMEMBERED";

                case EnumNET_ERROR.ERROR_EXTENDED_ERROR:
                    return "ERROR EXTENDED ERROR";

                case EnumNET_ERROR.ERROR_INVALID_PASSWORD:
                    return "ERROR INVALID PASSWORD";

                case EnumNET_ERROR.ERROR_NO_NET_OR_BAD_PATH:
                    return "ERROR NO NET OR BAD PATH";

                default:
                    return "";

            }
        }

        #endregion
    }
}
