using System;
using System.Collections.Generic;
using System.Text;

namespace CM.Tools.Cryptographic
{
    /// <summary>
    /// Clase que permite la generación de una contraseña. 
    /// La contraseña contiene un número de caracteres fijos y permite especificar el porcentaje
    /// de caracteres en mayúsculas y símbolos que se quieren obtener
    /// </summary>
    public class PasswordGenerator
    {
        /// <summary>
        /// Enumeración que permite conocer el tipo de juego de carácteres a emplear
        /// para cada carácter
        /// </summary>
        private enum TipoCaracterEnum
        {
            Minuscula,
            Mayuscula,
            Simbolo,
            Numero
        }

        #region Declaraciones

        private int porcentajeMayusculas;
        private int porcentajeSimbolos;
        private int porcentajeNumeros;
        Random semilla;

        // Caracteres que pueden emplearse en la contraseña
        private const string caracteres = "abcdefghijklmnopqrstuvwxyz";
        private const string numeros = "0123456789";
        private const string simbolos = "%$#@+-=&";

        // Cadena que contiene el password generado
        private StringBuilder password;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene o establece la longitud en carácteres de la contraseña a obtener
        /// </summary>
        public int LenghtPassword { get; set; }

        /// <summary>
        /// Obtiene o establece el porcentaje de carácteres en mayúsculas que 
        /// contendrá la contraseña
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un valor que no coincida con un porcentaje</exception>
        public int PorcentajeMayusculas
        {
            get { return porcentajeMayusculas; }
            set
            {
                if (value < 0 || value > 100)
// ReSharper disable NotResolvedInText
                    throw new ArgumentOutOfRangeException("El porcentaje es un número entre 0 y 100");
// ReSharper restore NotResolvedInText
                porcentajeMayusculas = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el porcentaje de símbolos que contendrá la contraseña
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un valor que no coincida con un porcentaje</exception>
        public int PorcentajeSimbolos
        {
            get { return porcentajeSimbolos; }
            set
            {
                if (value < 0 || value > 100)
// ReSharper disable NotResolvedInText
                    throw new ArgumentOutOfRangeException("El porcentaje es un número entre 0 y 100");
// ReSharper restore NotResolvedInText
                porcentajeSimbolos = value;
            }
        }

        /// <summary>
        /// Obtiene o establece el número de caracteres numéricos que contendrá la contraseña
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un valor que no coincida con un porcentaje</exception>
        public int PorcentajeNumeros
        {
            get { return porcentajeNumeros; }
            set
            {
                if (value < 0 || value > 100)
// ReSharper disable NotResolvedInText
                    throw new ArgumentOutOfRangeException("El porcentaje es un número entre 0 y 100");
// ReSharper restore NotResolvedInText
                porcentajeNumeros = value;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor. La contraseña tendrá 8 caracteres, incluyendo una letra mayúscula, 
        /// un número y un símbolo
        /// </summary>
        public PasswordGenerator()
            : this(8)
        { }

        /// <summary>
        /// Constructor. La contraseña tendrá un 20% de caracteres en mayúsculas y otro tanto de 
        /// símbolos
        /// </summary>
        /// <param name="longitudCaracteres">Lenght en carácteres de la contraseña a obtener</param>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un porcentaje de caracteres especiales mayor de 100</exception>
        public PasswordGenerator(int longitudCaracteres)
            : this(longitudCaracteres, 20, 20, 20)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="longitudCaracteres">Lenght en carácteres de la contraseña a obtener</param>
        /// <param name="porcentajeMayusculas">Porcentaje a aplicar de caracteres en mayúscula</param>
        /// <param name="porcentajeSimbolos">Porcenta a aplicar de símbolos</param>
        /// <param name="porcentajeNumeros">Porcentaje de caracteres numéricos</param>
        /// <exception cref="ArgumentOutOfRangeException">Se produce al intentar introducir
        /// un porcentaje de caracteres especiales mayor de 100</exception>
        public PasswordGenerator(int longitudCaracteres, int porcentajeMayusculas, int porcentajeSimbolos, int porcentajeNumeros)
        {
            LenghtPassword = longitudCaracteres;
            PorcentajeMayusculas = porcentajeMayusculas;
            PorcentajeSimbolos = porcentajeSimbolos;
            PorcentajeNumeros = porcentajeNumeros;

            if (PorcentajeMayusculas + porcentajeSimbolos + PorcentajeNumeros > 100)
                throw new ArgumentOutOfRangeException(
                "La suma de los porcentajes de caracteres especiales no puede superar el " +
                "100%, es decir, no puede ser superior a la longitud de la contraseña");
            semilla = new Random(DateTime.Now.Millisecond);
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Obtiene el password
        /// </summary>
        /// <returns></returns>
        public string GetNewPassword()
        {
            GeneraPassword();
            return password.ToString();
        }

        /// <summary>
        /// Permite establecer el número de caracteres especiales que se quieren obtener
        /// </summary>
        /// <param name="numeroCaracteresMayuscula">Número de caracteres en mayúscula</param>
        /// <param name="numeroCaracteresNumericos">Número de caracteres numéricos</param>
        /// <param name="numeroCaracteresSimbolos">Número de caracteres de símbolos</param>
        public void SetCaracteresEspeciales(int numeroCaracteresMayuscula, int numeroCaracteresNumericos, int numeroCaracteresSimbolos)
        {
            // Comprobación de errores
            if (numeroCaracteresMayuscula + numeroCaracteresNumericos + numeroCaracteresSimbolos > LenghtPassword)
// ReSharper disable NotResolvedInText
                throw new ArgumentOutOfRangeException("El número de caracteres especiales no puede superar la longitud del password");
// ReSharper restore NotResolvedInText

            PorcentajeMayusculas = numeroCaracteresMayuscula * 100 / LenghtPassword;
            PorcentajeNumeros = numeroCaracteresNumericos * 100 / LenghtPassword;
            PorcentajeSimbolos = numeroCaracteresSimbolos * 100 / LenghtPassword;
        }

        /// <summary>
        /// Constructor. La contraseña tendrá 8 caracteres, incluyendo una letra mayúscula, 
        /// un número y un símbolo
        /// </summary>
        public static string GetPassword()
        {
            // Se crea un método estático para facilitar el uso
            var gp = new PasswordGenerator();
            return gp.GetNewPassword();
        }

        #endregion

        #region Métodos de cálculo

        /// <summary>
        /// Método que genera el password. Primero crea una cadena de caracteres 
        /// en minúscula y va sustituyendo los caracteres especiales
        /// </summary>
        private void GeneraPassword()
        {
            // Se genera una cadena de caracteres en minúscula con la longitud del 
            // password seleccionado
            password = new StringBuilder(LenghtPassword);
            for (int i = 0; i < LenghtPassword; i++)
            {
                password.Append(GetCaracterAleatorio(TipoCaracterEnum.Minuscula));
            }

            // Se obtiene el número de caracteres especiales (Mayúsculas y caracteres) 
            var numMayusculas = (int)(LenghtPassword * (PorcentajeMayusculas / 100d));
            var numSimbolos = (int)(LenghtPassword * (PorcentajeSimbolos / 100d));
            var numNumeros = (int)(LenghtPassword * (PorcentajeNumeros / 100d));

            // Se obtienen las posiciones en las que irán los caracteres especiales
            int[] caracteresEspeciales =
                    GetPosicionesCaracteresEspeciales(numMayusculas + numSimbolos + numNumeros);
            int posicionInicial = 0;
            int posicionFinal = 0;

            // Se reemplazan las mayúsculas
            posicionFinal += numMayusculas;
            ReemplazaCaracteresEspeciales(caracteresEspeciales,
                 posicionInicial, posicionFinal, TipoCaracterEnum.Mayuscula);

            // Se reemplazan los símbolos
            posicionInicial = posicionFinal;
            posicionFinal += numSimbolos;
            ReemplazaCaracteresEspeciales(caracteresEspeciales,
                 posicionInicial, posicionFinal, TipoCaracterEnum.Simbolo);

            // Se reemplazan los Números
            posicionInicial = posicionFinal;
            posicionFinal += numNumeros;
            ReemplazaCaracteresEspeciales(caracteresEspeciales,
                 posicionInicial, posicionFinal, TipoCaracterEnum.Numero);
        }

        /// <summary>
        /// Reemplaza un caracter especial en la cadena Password
        /// </summary>
        private void ReemplazaCaracteresEspeciales(int[] posiciones, int posicionInicial, int posicionFinal, TipoCaracterEnum tipoCaracter)
        {
            for (int i = posicionInicial; i < posicionFinal; i++)
            {
                password[posiciones[i]] = GetCaracterAleatorio(tipoCaracter);
            }
        }

        /// <summary>
        /// Obtiene un array con las posiciones en las que deberán colocarse los caracteres
        /// especiales (Mayúsculas o Símbolos). Es importante que no se repitan los números
        /// de posición para poder mantener el porcentaje de dichos carácteres
        /// </summary>
        /// <param name="numeroPosiciones">Valor que representa el número de posiciones
        /// que deberán crearse sin repetir</param>
        private int[] GetPosicionesCaracteresEspeciales(int numeroPosiciones)
        {
            var lista = new List<int>();
            while (lista.Count < numeroPosiciones)
            {
                int posicion = semilla.Next(0, LenghtPassword);
                if (!lista.Contains(posicion))
                    lista.Add(posicion);
            }
            return lista.ToArray();
        }

        /// <summary>
        /// Obtiene un carácter aleatorio en base a la "matriz" del tipo de caracteres
        /// </summary>
        private char GetCaracterAleatorio(TipoCaracterEnum tipoCaracter)
        {
            string juegoCaracteres;
            switch (tipoCaracter)
            {
                case TipoCaracterEnum.Mayuscula:
                    juegoCaracteres = caracteres.ToUpper();
                    break;
                case TipoCaracterEnum.Minuscula:
                    juegoCaracteres = caracteres.ToLower();
                    break;
                case TipoCaracterEnum.Numero:
                    juegoCaracteres = numeros;
                    break;
                default:
                    juegoCaracteres = simbolos;
                    break;
            }

            // índice máximo de la matriz char de caracteres
            int longitudJuegoCaracteres = juegoCaracteres.Length;

            // Obtención de un número aletorio para obtener la posición del carácter
            int numeroAleatorio = semilla.Next(0, longitudJuegoCaracteres);

            // Se devuelve una posición obtenida aleatoriamente
            return juegoCaracteres[numeroAleatorio];
        }

        #endregion
    }
}