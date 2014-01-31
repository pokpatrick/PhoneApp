using System;
using System.Windows.Media;

namespace PhoneApp.Model
{
    public class Tools
    {

        public static int requestTimeOut = 45000;

        /**
         * Ajoute un zero au debut d'un element si celui n'est compose que d'un chiffre pour l'avoir en nombre à 2 chiffres
         */
        public static String addZeroFormatStringDate(int elt)
        {
            if (elt < 10)
                return "0" + elt.ToString();
            else
                return elt.ToString();
        }

        /**
         * Convertit la date au format RFC3339
         * date: la date à convertir
         */
        public static String convertDateToRFC3339(DateTime date, Boolean dateOrDateTime)
        {
            String dateS;
            dateS = date.Year + "-" + addZeroFormatStringDate(date.Month) + "-" + addZeroFormatStringDate(date.Day);
            if (!dateOrDateTime)
                dateS += "T" + addZeroFormatStringDate(date.Hour) + ":" + addZeroFormatStringDate(date.Minute) + ":00Z";
            return dateS;
        }

        public static Color HexColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
