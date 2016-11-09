using Mono.Data.Sqlite; //Az sqlite local adatbázis használatához szükséges névtér.
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;



public class _adatbazisvezerlo
{
    #region VÁLTOZÓK
    private bool adatbazisNyitottE = false;

    private SqliteConnection adatbCsatlakozas;

    private static bool kellKeszites = false; // az az ,hogy az adatbázisban a táblákat el kell-e készíteni , hiszen nincsen még! - 
    #endregion

    #region SINGLETON
    private _adatbazisvezerlo(string eleres)
    {
        string csatlakozas;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

#else //az az android esetén mindenképpen kell a változtatás , hiszen itt más módon érjük el. - ezt nem akarjuk az osztály felhasználójára át terhelni
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
    public static _adatbazisvezerlo GetPeldany(string eleres)
    {

        if (_peldany == null)
        {
            _peldany = new _adatbazisvezerlo(eleres);
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
        IDataReader olvaso;
        IDbCommand muvelet;
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = "select fh.Nev as 'Név' ,fh.Kor as 'Kor',ki.Nev as 'Aktív kinézet',ja.AktivSzint as 'Aktív szint',ja.Penz as 'Pénz',ja.FelhasznaloID as 'ID'" +
            "from jatekadat ja inner join felhasznalo fh on ja.FelhasznaloID = fh.ID inner join kinezet ki on ja.AktivKinezetID = ki.ID";
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
        IDataReader olvaso;
        IDbCommand muvelet;
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = string.Format("select Nev from felhasznalo where id={0}", FelhasznaloID);
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
        IDataReader olvaso;
        IDbCommand muvelet;
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = string.Format("select ja.FelhasznaloID,fh.Kor,ja.AktivKinezetID,ja.AktivSzint,ja.Penz,fh.Nev from jatekadat ja inner join" +
            " felhasznalo fh on ja.FelhasznaloID = fh.ID where ja.FelhasznaloID = {0}", FelhasznaloID);
        olvaso = muvelet.ExecuteReader();
        return olvaso;
    }

    /// <summary>
    /// Amennyiben a tábla szerkezet még nem létezik , hiszen valamilyen úton módon a fájl sem létezett és azt a programnak kellett létrehoznia - jellemzően első indításnál androidos készülék esetén -
    /// akkor ezzel a metódussal fogjuk a program által elkészített .db fájlba elkészíteni a táblákat
    /// </summary>
    private void TablakKeszitese()
    {
        IDbCommand muvelet;
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

#if UNITY_EDITOR
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
#endif
        //
    }

    private void JatekAdatNullazasa()
    {
        IDbCommand muvelet;
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = "delete from jatekadat";
        muvelet.ExecuteNonQuery();
    }

    private void FelhasznaloNullazasa()
    {
        IDbCommand muvelet;
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = "delete from felhasznalo";
        muvelet.ExecuteNonQuery();
    }

    private int LegnagyobbID
    {
        get
        {
            IDataReader olvaso;
            IDbCommand muvelet;
            int ID = 0;
            muvelet = adatbCsatlakozas.CreateCommand();
            muvelet.CommandText = string.Format("select ID from felhasznalo order by ID desc limit 1");
            olvaso = muvelet.ExecuteReader();
            ID = int.Parse(olvaso.GetValue(0).ToString());
            olvaso.Close();
            return ID;
        }
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
    public void AdatbazisKapcsolatNyitas()
    {
        if (AdatbazisNyitottE == false)
        {
            adatbCsatlakozas.Open();
            adatbazisNyitottE = true;
        }
    }

    public void FelhasznaloHozzad(string Nev, int Kor)
    {
        IDbCommand muvelet;
        string sor;
        int jelenFelhID = 1; //ha nincsen felhasználó akkor 1 - lesz az ID
        if (VanEFelhasznalo == 1) //ha van akkor a jelen felh id változzon
        {
            //ha van felhasználó, akkor a legnagyobb ID-jű +1 lesz a jelenFelhId értéke - 
            jelenFelhID = LegnagyobbID + 1;
        }
        if (VanEFelhasznalo != 0)
        {
            sor = string.Format("INSERT INTO `Felhasznalo`(Kor,Nev) VALUES ({0},'{1}')", Kor, Nev);
            muvelet = adatbCsatlakozas.CreateCommand();
            muvelet.CommandText = sor;
            muvelet.ExecuteNonQuery();

            sor = string.Format("INSERT INTO `Jatekadat` VALUES({0},{1},{2},{3})", jelenFelhID, _konstansok.ALAP_PENZ, _konstansok.ALAP_AKTIV_SZINT, _konstansok.ALAP_AKTIV_KIENZET);
            muvelet = adatbCsatlakozas.CreateCommand();
            muvelet.CommandText = sor;
            muvelet.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Felhasználó törlése az adatbázisból. (felhasználó és a játékadat táblából egyaránt)
    /// </summary>
    /// <param name="FelhasznaloID">Melyik felhasználót akarod törölni ? Szám -</param>
    public void FelhasznaloTorlese(int FelhasznaloID)
    {
        IDbCommand muvelet;
        string sor = string.Format("delete from felhasznalo where ID={0}", FelhasznaloID);
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = sor;
        muvelet.ExecuteNonQuery();

        sor = string.Format("delete from jatekadat where FelhasznaloID={0}", FelhasznaloID);
        muvelet = adatbCsatlakozas.CreateCommand();
        muvelet.CommandText = sor;
        muvelet.ExecuteNonQuery();
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

