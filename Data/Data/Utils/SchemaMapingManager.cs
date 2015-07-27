using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using CM.Tools.Collections;

namespace CMData.Utils
{
    [Serializable]
    public class SchemaMapingManager
    {
        #region Propiedades

        [XmlElement]
        public SerializableDictionary<string, string> Schemas { get; set; }

        public string this[string SchemaAlias]
        {
            get
            {
                if (this.Schemas.ContainsKey(SchemaAlias))
                    return this.Schemas[SchemaAlias];
                else
                    throw new Exception("No se encontró un esquema para el alias: " + SchemaAlias);
            }
        }

        #endregion

        #region Constructores

        public SchemaMapingManager()
        {
            Schemas = new SerializableDictionary<string, string>();
        }

        #endregion

        #region Funciones

        public static void Serialize(SchemaMapingManager nObjectConfig, string nConfigFileName)
        {
            XmlSerializer ConfigXmlSerializer = new XmlSerializer(typeof(SchemaMapingManager));

            try
            {
                using (var ConfigTextWriter = new StreamWriter(nConfigFileName))
                {
                    ConfigXmlSerializer.Serialize(ConfigTextWriter, nObjectConfig);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo ecribir el archivo de configuración. " + ex.Message);
            }
        }

        public static SchemaMapingManager Deserialize(string nConfigFileName)
        {
            XmlSerializer ConfigXmlSerializer = new XmlSerializer(typeof(SchemaMapingManager));

            try
            {
                using (StreamReader ConfigTextReader = new StreamReader(nConfigFileName))
                {
                    return (SchemaMapingManager)ConfigXmlSerializer.Deserialize(ConfigTextReader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo leer el archivo de configuración. " + ex.Message);
            }
        }

        #endregion
    }
}
