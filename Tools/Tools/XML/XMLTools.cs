using System;

namespace Tools.XML
{
    public static class XmlTools
    {
        public static string ParceXml(string nTexto)
        {
            nTexto = nTexto.Replace("'", "&apos;");
            nTexto = nTexto.Replace(Convert.ToChar(34).ToString(), "&quot");
            nTexto = nTexto.Replace(">", "&gt;");
            nTexto = nTexto.Replace("<", "&lt;");
            nTexto = nTexto.Replace("&", "&amp;");

            return nTexto;
        }
    }
}
