using System.Collections;
using UnityEngine;

public class _konstansok
{
    #region ADATBÁZISSAL KAPCSOLATOSAK
    public const string ADATBAZIS_NEV = "rawadatbazis.db";

    /// <summary>
    /// A felhsználók jelenetben használt oszlopok száma.
    /// 0 - Név | 1 - Kor | 2 - Aktív kinézet | 3 - Aktív szint | 4 - Pénz
    /// Minden olyan esetben amikor 5 darab oszlopot akarunk feldolgozni ez használható.
    /// </summary>
    public const int FELHASZNALOK_OSZLOP_SZAM = 4;

    /// <summary>
    /// Olyan esetben használjuk , amikor a felhasználókat az adataik nélkül kérjük le , csak ID-ket kapjunk vissza + kor-t de konkrét nevet , aktív kinézet nevet NEM!
    /// </summary>
    public const int FELHASZNALOK_ADATOKNELKUL_OSZLOP_SZAM = 5;

    #region PLAYER PREFSHEZ VAGY ADATBÁZISHOZ
    //Ezen konstansokban tároljuk el az oszlopok neveit KÖTELEZŐEN ezeket kell használni a player prefs BÁRMELY értékének lekérdezésénél is!
    public const string NEV = "Nev";
    public const string FELHASZNALOID = "FelhasznaloID";
    public const string KOR = "Kor";
    public const string AKTIVKINEZETID = "AktivKinezetID";
    public const string AKTIVSZINT = "AktivSzint";
    public const string PENZ = "Penz";
    #endregion
    #endregion

    #region JELENET NEVEK
    public const string BEALLITASOK = "_beallitasok";
    public const string FELHASZNALOK = "_felhasznalok";
    public const string FOMENU = "_foMenu";
    public const string KINEZETEK = "_kinezetek";
    public const string MODVALASZTO = "_modValaszto";
    public const string PALYAVALASZTO = "_palyaValaszto";
    public const string UJFELHASZNALO = "_ujFelhasznalo";

    #region PÁLYA JELENET NEVEK
    public const string PALYA_1 = "palya_1";
    public const string P1 = "P1";
    public const string PALYA_2 = "palya_2";
    public const string P2 = "P2";
    public const string PALYA_3 = "palya_3";
    public const string P3 = "P3";
    public const string PALYA_4 = "palya_4";
    public const string P4 = "P4";
    public const string PALYA_5 = "palya_5";
    public const string P5 = "P5";
    public const string PALYA_6 = "palya_6";
    public const string P6 = "P6";
    public const string PALYA_7 = "palya_7";
    public const string P7 = "P7";
    public const string PALYA_8 = "palya_8";
    public const string P8 = "P8";
    public const string PALYA_9 = "palya_9";
    public const string P9 = "P9";
    public const string PALYA_10 = "palya_10";
    public const string P10 = "P10";
    public const string PALYA_11 = "palya_11";
    public const string P11 = "P11";
    public const string PALYA_12 = "palya_12";
    public const string P12 = "P12";
    public const string PALYA_13 = "palya_13";
    public const string P13 = "P13";
    public const string PALYA_14 = "palya_14";
    public const string P14 = "P14";
    #endregion
    #endregion

    #region PREFAB NEVEK
    public const string KEREK = "Kerek";
    #endregion

}
