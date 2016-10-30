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
    #region 
    private IDataReader olvaso;
    private IDbCommand muvelet;
    private SqliteConnection adatbCsatlakozas;
    #endregion

    #region singleton
    private _adatbazisvezerlo()
    {
        string eleres = "";
        string csatlakozas;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        eleres = Application.dataPath + "/_Adatbazis/rawadatbazis.db";
#else
        eleres = Application.persistentDataPath + "/rawadatbazis.db";
#endif
        Debug.Log(string.Format("Jelenlegi fájl elérés: {0} - Létezik a fájl? - {1}", eleres, File.Exists(eleres).ToString()));

        if (File.Exists(eleres))
        {
            csatlakozas = "URI=file:" + eleres;
            adatbCsatlakozas = new SqliteConnection(csatlakozas);
            adatbCsatlakozas.Open();
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

    #endregion
}

