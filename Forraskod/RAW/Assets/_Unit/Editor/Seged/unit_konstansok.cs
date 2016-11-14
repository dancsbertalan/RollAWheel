using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Unit.Editor
{
    public static class unit_konstansok
    {
        public static string UnitAdatbazisEleres
        {
            get
            {
                return Application.dataPath + "/_Unit/Editor/_Adatbazis/" + "unit_" + _konstansok.ADATBAZIS_NEV;
            }
        }

        /// <summary>
        /// Felhasználók Lekérdezéséhez tartozó konstans adatbázis
        /// </summary>
        public static string FelhkLekKonstansUnitAdatbazisEleres
        {
            get
            {
                return Application.dataPath + "/_Unit/Editor/_Adatbazis/" + "felhkLek_konst_unit_" + _konstansok.ADATBAZIS_NEV;
            }
        }

        public static string TablaKeszitoUnitAdatbazisEleres
        {
            get
            {
                return Application.dataPath + "/_Unit/Editor/_Adatbazis/" + "tablaKeszito_unit_" + _konstansok.ADATBAZIS_NEV;
            }
        }

        public static string JatekadatFelhNullazoUnitAdatbazisEleres
        {
            get
            {
                return Application.dataPath + "/_Unit/Editor/_Adatbazis/" + "jatekAdatFelhNullazo_unit_" + _konstansok.ADATBAZIS_NEV;
            }
        }


    }
}
