using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite; //Az sqlite local adatbázis használatához szükséges névtér.


using System.Data;
using System.Text;
using System.IO;



public class _adatbazisvezerlo
{
    #region VÁLTOZÓK
    private bool adatbazisNyitottE = false;
    private IDataReader olvaso;
    private IDbCommand muvelet;
    private SqliteConnection adatbCsatlakozas;

    private static bool kellKeszites = false; // az az ,hogy az adatbázisban a táblákat el kell-e készíteni , hiszen nincsen még! - 
    #endregion

    #region SINGLETON
    private _adatbazisvezerlo()
    {
        string eleres = "";
        string csatlakozas;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        eleres = Application.streamingAssetsPath + "/_Adatbazis/" + _konstansok.ADATBAZIS_NEV; //ez itt jó
#else
        eleres = Application.persistentDataPath + "/_Adatbazis/" + _konstansok.ADATBAZIS_NEV;
        if (File.Exists(eleres) == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/_Adatbazis");
            File.Create(Application.persistentDataPath + "/_Adatbazis/" + _konstansok.ADATBAZIS_NEV);
            kellKeszites = true;
        }
#endif
        Debug.Log(string.Format("Jelenlegi fájl elérés: {0} - Létezik a fájl? - {1}", eleres, File.Exists(eleres).ToString()));

        if (File.Exists(eleres))
        {
            csatlakozas = "URI=file:" + eleres;
            adatbCsatlakozas = new SqliteConnection(csatlakozas);
            adatbCsatlakozas.Open();
            adatbazisNyitottE = true;
            if (kellKeszites == true)
            {
                TablakKeszitese();
                kellKeszites = false;
            }

        }
    }

    static _adatbazisvezerlo _peldany = null;

    /// <summary>
    /// Az adatbázis első elkészítésekor azt MEGNYITJUK! Bezárni azt egy külön metódussal kell. (Ha igényeljük)
    /// </summary>
    /// <returns></returns>
    public static _adatbazisvezerlo GetPeldany()
    {

        if (_peldany == null)
        {
            _peldany = new _adatbazisvezerlo();
        }

        return _peldany;
    }
    #endregion

    #region METÓDUSOK

    /// <summary>
    /// Ezzel a metódussal tudjuk az összes felhasználót lekérni MINDEN adatával!
    /// Visszatér egy "olvasó" az az readerrel - mellyet egy looppal feltudunk dolgozni.
    /// </summary>
    public IDataReader FelhasznalokLekerdezese()
    {
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = "select fh.Nev as 'Név' ,fh.Kor as 'Kor',ki.Nev as 'Aktív kinézet',ja.AktivSzint as 'Aktív szint',ja.Penz as 'Pénz'" +
            "from jatekadat ja inner join felhasznalo fh on ja.FelhasznaloID = fh.ID inner join kinezet ki on ja.AktivKinezetID = ki.ID";
        olvaso = muvelet.ExecuteReader();
        return olvaso;
    }

    /// <summary>
    /// Amennyiben egy konkrét felhasználónak az adatait szeretnénk lekérdezni DE oly módon ,hogy a Név, Aktív kinézet, Aktív szint oszlopoknál NEM a rá joinolt elemeket írja ki
    /// ez használandó.
    /// </summary>
    /// <param name="FelhasznaloID">Ez a paraméter adja meg ,hogy mely fehasználó érdekel minket.</param>
    /// <returns></returns>
    public IDataReader FelhasznaloAdatainakLekerdezese(int FelhasznaloID)
    {
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = string.Format("select ja.FelhasznaloID,fh.Kor,ja.AktivKinezetID,ja.AktivSzint,ja.Penz from jatekadat ja inner join" +
            " felhasznalo fh on ja.FelhasznaloID = fh.ID where ja.FelhasznaloID = {0}", FelhasznaloID);
        olvaso = muvelet.ExecuteReader();
        return olvaso;
    }

    /// <summary>
    /// Amennyiben egy adott ID alapján csak a felhasználó nevét szeretnénk lekérdezni.
    /// </summary>
    /// <param name="FelhasznaloID">Annak a felhasználónak a ID-je melynek a nevét szeretnénk megtudni.</param>
    /// <returns></returns>
    public IDataReader FelhasznaloNevLekerdezese(int FelhasznaloID)
    {
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = string.Format("select Nev from felhasznalo where id={0}", FelhasznaloID);
        olvaso = muvelet.ExecuteReader();
        return olvaso;
    }

    /// <summary>
    /// Amennyiben a tábla szerkezet még nem létezik , hiszen valamilyen úton módon a fájl sem létezett és azt a programnak kellett létrehoznia - jellemzően első indításnál androidos készülék esetén -
    /// akkor ezzel a metódussal fogjuk a program által elkészített .db fájlba elkészíteni a táblákat
    /// </summary>
    private void TablakKeszitese()
    {
        //kinézet tábla elkészítése
        string tabla = "CREATE TABLE `Kinezet` (`ID`	INTEGER NOT NULL,`Nev`	TEXT NOT NULL,PRIMARY KEY(`ID`))";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = tabla;
        muvelet.ExecuteNonQuery();

        //felhasználó tábla elkészítése
        tabla = "CREATE TABLE `Felhasznalo` (`ID`	INTEGER NOT NULL,`Kor`	INTEGER NOT NULL,`Nev`	TEXT NOT NULL,PRIMARY KEY(`ID`))";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = tabla;
        muvelet.ExecuteNonQuery();

        //játékadat tábla elkészítése
        tabla = "CREATE TABLE `Jatekadat` (`FelhasznaloID`	INTEGER,`Penz`	INTEGER,`AktivSzint`	INTEGER,`AktivKinezetID`	INTEGER,FOREIGN KEY(`FelhasznaloID`) REFERENCES `Felhasznalo`(`ID`),FOREIGN KEY(`AktivKinezetID`) REFERENCES `Kinezet`(`ID`))";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = tabla;
        muvelet.ExecuteNonQuery();

        //csak ameddig nem lehet végrehajtani a felhasználó készítést addig itt szúrom be a két teszt felhasználót
        string sor = "INSERT INTO `Kinezet` VALUES (1,'Alap')";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = sor;
        muvelet.ExecuteNonQuery();

        sor = "INSERT INTO `Jatekadat` VALUES (1,1250,1,1)";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = sor;
        muvelet.ExecuteNonQuery();

        sor = "INSERT INTO `Jatekadat` VALUES(2, 1300, 10, 1)";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = sor;
        muvelet.ExecuteNonQuery();

        sor = "INSERT INTO `Felhasznalo` VALUES (1,21,'iMager')";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = sor;
        muvelet.ExecuteNonQuery();

        sor = "INSERT INTO `Felhasznalo` VALUES (2,20,'AndorSebo')";
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = sor;
        muvelet.ExecuteNonQuery();
        //
    }

    private void JatekAdatNullazasa()
    {
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = "delete from jatekadat";
        muvelet.ExecuteNonQuery();
    }

    private void FelhasznaloNullazasa()
    {
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = "delete from felhasznalo";
        muvelet.ExecuteNonQuery();
    }

    /// <summary>
    /// Ha az adatbázis nyitott akkor bezárhatjuk manuálisan.
    /// </summary>
    public void AdatbazisKapcsolatZar()
    {
        if (AdatbazisNyitottE == true)
        {
            adatbCsatlakozas.Close();
            adatbazisNyitottE = false;
        }
    }

    /// <summary>
    /// Ha az adatbázis zárt , akkor kinyithatjuk manuálisan
    /// </summary>
    public void AdaatbazisKapcsolatNyitas()
    {
        if (AdatbazisNyitottE == false)
        {
            adatbCsatlakozas.Open();
            adatbazisNyitottE = true;
        }
    }
    #endregion

    #region PROPERTYK

    /// <summary>
    /// Azt adja vissza, hogy van-e felhasználó az adatbázisban
    /// Lehetséges visszatérési értékek:
    /// 0 - Az adatbázis zárt ezért nem tudta lekérdezni ,hogy van-e felhasználó.
    /// 1 - Az adatbázis nyitott és van felhasználó.
    /// 2 - Az adatbázis nyitott de nincs felhasználó.
    /// </summary>
    public int VanEFelhasznalo
    {
        get
        {
            int vanEFelhasznalo = 0;
            if (AdatbazisNyitottE == true)
            {
                IDbCommand tempMuvelet = adatbCsatlakozas.CreateCommand();
                //itt direkt nem a felhasználók lekérdezése metódust hívom meg , ha bár az is elég lenne - de ha az változik az ennek a kárára is mehet akár!//
                tempMuvelet.CommandText = "select * from felhasznalo";
                IDataReader tempOlvaso = tempMuvelet.ExecuteReader();

                IDbCommand tempMuvelet2 = adatbCsatlakozas.CreateCommand();
                tempMuvelet2.CommandText = "select * from jatekadat";
                IDataReader tempOlvaso2 = tempMuvelet2.ExecuteReader();
                if (tempOlvaso.Read() == true) //felhasználóban van valami
                {
                    tempOlvaso.Close();
                    if (tempOlvaso2.Read() == true) //játékadatban van valami
                    {
                        tempOlvaso2.Close();
                        vanEFelhasznalo = 1; //akkor
                    }
                    else //ha a felhasználó nem üres de a játék adat igen
                    {
                        tempOlvaso2.Close();
                        FelhasznaloNullazasa();
                        Debug.Log("Felhasználó nullázva!");
                        vanEFelhasznalo = 2;
                    }
                }
                else if (tempOlvaso2.Read() == true)
                {
                    tempOlvaso2.Close();
                    tempOlvaso.Close();
                    JatekAdatNullazasa();
                    Debug.Log("Játék adat nullázva!");
                    vanEFelhasznalo = 2;
                }
                else
                {
                    tempOlvaso2.Close();
                    tempOlvaso.Close();
                    vanEFelhasznalo = 2;
                    Debug.Log("Nincs felhasználó!");
                }
            }
            return vanEFelhasznalo;
        }
    }

    public bool AdatbazisNyitottE
    {
        get
        {
            return adatbazisNyitottE;
        }
    }
    #endregion
}

