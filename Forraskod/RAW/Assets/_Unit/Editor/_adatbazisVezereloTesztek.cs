using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Mono.Data.Sqlite;
using Assets._Unit.Editor;
using System.Data;
using System.Collections.Generic;
using System;

public class _adatbazisVezereloTesztek
{

    [Test]
    public void GetPeldanyTeszt()
    {

        //első ellenőrzés ,hogy nem nulla-e
        _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.UnitAdatbazisEleres);
        Assert.AreNotEqual(adatbazis, null);
        //második ellenőrzés hogy tényleg kinyitotta-e
        Assert.AreEqual(true, adatbazis.AdatbazisNyitottE);
    }

    [Test]
    public void FelhasznalokLekerdezeseTeszt()
    {
        //itt végig a felhKLek_unit_rawadatbazis.db-t fogjuk használni - 

        /*több módszer lehet a tesztelésre
          -1.- Felhasználók lekérdezésének egy olyan adatbázis elérést adunk meg mely DIREKT egy "konstans" adatbázis.
                Az az nem módosítottuk sehol, csak lekérdezünk belőle és 100%-ig tudjuk, hogy miket fogunk vissza kapni. 
                Ezeket egy Listában tároljuk és azokkal hasonlítjuk össze.
         -2.- Felhasználók lekérdezését itt is külön megírjuk egy teljesen új adatbázis kapcsolattal és azoknak az elemeit hasonlítjuk össze
                hiszen teljesmértékben előfordulhat az a hiba ,hogy nem magában a lekérdezésben van a gond, hanem a metódusban.
         ++++++ Mindegyik esetében lekérhetőek a field countok - az az hány oszlop van - és akár még a sorok is! - nyitva van-e,command text....stb
         -3.- Maga az olvasó példányok összehasonlíthatóak-e ???

         */
        #region ELSŐ MÓDSZER
        _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.FelhkLekKonstansUnitAdatbazisEleres);
        //azt már tudjuk ,hogy összesen 2 sorunk lesz mivel ennyit raktunk a KONSTANS adatbázisba
        //azt már tudjuk , hogy összesen az oszlopok száma -> 6 darab lesz mivel ennyit raktunk a KONSTANS adatbázisba a lekérdezésben 
        //sor,oszlop szerint indexeljük
        List<List<string>> elemek = new List<List<string>>() {
            new List<string>() {
                "iMager85",
                "21",
                "Alap",
                "10",
                "10000",
                "1"
            },
            new List<string>() {
                "AndorSebo",
                "20",
                "Alap",
                "15",
                "5000",
                "2"
            }
        };

        IDataReader olvasoAkt = adatbazis.FelhasznalokLekerdezese();
        //mielőtt még az elemeket vizsgálnánk megnézhetjük, hogy hány oszlop van ..
        for (int k = 0; k < 2; k++) //mivel két sor van és mind a kettőben illendő lenne ellenőrizni..
        {
            Assert.AreEqual(elemek[k].Count, olvasoAkt.FieldCount);
        }
        //
        int i = 0;
        while (olvasoAkt.Read())
        {
            for (int j = 0; j < 6; j++)
            {
                Assert.AreEqual(olvasoAkt.GetValue(j).ToString(), elemek[i][j]);
            }
            i++;
        }
        olvasoAkt.Close();


        #endregion

        #region MÁSODIK MÓDSZER
        SqliteConnection csat = new SqliteConnection("URI=file:" + unit_konstansok.FelhkLekKonstansUnitAdatbazisEleres);
        csat.Open();
        IDbCommand muv = csat.CreateCommand();
        muv.CommandText = "select fh.Nev as 'Név' ,fh.Kor as 'Kor',ki.Nev as 'Aktív kinézet',ja.AktivSzint as 'Aktív szint',ja.Penz as 'Pénz',ja.FelhasznaloID as 'ID'" +
            "from jatekadat ja inner join felhasznalo fh on ja.FelhasznaloID = fh.ID inner join kinezet ki on ja.AktivKinezetID = ki.ID";
        IDataReader olvasoElv = muv.ExecuteReader();
        IDataReader olvasoAkt2 = adatbazis.FelhasznalokLekerdezese();
        Assert.AreEqual(olvasoAkt2.FieldCount, olvasoElv.FieldCount);
        Assert.AreEqual(olvasoAkt2.Depth, olvasoElv.Depth);

        while (olvasoElv.Read()) //azért az elvárt readjével megyünk , mert a eztúl indexel , vagy alul indexel akkor tuti ,hogy hiba lesz a másikban
        {
            olvasoAkt2.Read(); //ezzel azért itt olvasunk , mert ez nem lehet a whileban feltétel! - hiszen ebben lehet hibaa! és ennek a hibáját keressük
            for (int k = 0; k < olvasoElv.FieldCount; k++) //ismét a fenti miatt használjuk ennek a fieldcountját
            {
                Assert.AreEqual(olvasoElv.GetValue(k).ToString(), olvasoAkt2.GetValue(k).ToString());
            }
        }

        olvasoAkt2.Close();
        olvasoElv.Close();
        #endregion
    }

    [Test]
    public void FelhasznloNevLekerdezeseTeszt()
    {
        /*
         Az előzőhöz hasonlóan több lehetőség van.
            1 - Konstansba tudjuk ,hogy lekérjük az első és a második felhasználó nevét és az Ő nevüket tudjuk ! Le ellenőrizzük ,hogy tényleg azt adja-e vissza !
            2 - Csinálunk egy saját lekérdezést egy saját adatbázissal melyet itt adunk meg !
         */

        #region ELSŐ MÓDSZER
        _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.FelhkLekKonstansUnitAdatbazisEleres);

        IDataReader olvasoElso = adatbazis.FelhasznaloNevLekerdezese(1);
        //tudjuk ,hogy ez most iMager85-t ad vissza , és egy oszloppal , hiszen csak a nevet kérjük le!
        Assert.AreEqual(olvasoElso.GetValue(0).ToString(), "iMager85");
        Assert.AreEqual(olvasoElso.FieldCount, 1);
        olvasoElso.Close();

        IDataReader olvasoMasodik = adatbazis.FelhasznaloNevLekerdezese(2);
        //tudjuk ,hogy ez most AndorSebo-t ad vissza , és egy oszloppal , hiszen csak a nevet kérjük le!
        Assert.AreEqual(olvasoMasodik.GetValue(0).ToString(), "AndorSebo");
        Assert.AreEqual(olvasoMasodik.FieldCount, 1);
        olvasoMasodik.Close();

        #endregion

        #region MÁSODIK MÓDSZER
        SqliteConnection csat = new SqliteConnection("URI=file:" + unit_konstansok.FelhkLekKonstansUnitAdatbazisEleres);
        csat.Open();
        IDbCommand muv = csat.CreateCommand();
        muv.CommandText = "select Nev from felhasznalo where id=1";
        IDataReader olvasoElvElso = muv.ExecuteReader();

        IDbCommand muv2 = csat.CreateCommand();
        muv2.CommandText = "select Nev from felhasznalo where id=2";
        IDataReader olvasoElvMasodik = muv2.ExecuteReader();

        Assert.AreEqual(olvasoElvElso.FieldCount, adatbazis.FelhasznaloNevLekerdezese(1).FieldCount);
        Assert.AreEqual(olvasoElvMasodik.FieldCount, adatbazis.FelhasznaloNevLekerdezese(2).FieldCount);

        Assert.AreEqual(olvasoElvElso.GetValue(0).ToString(), adatbazis.FelhasznaloNevLekerdezese(1).GetValue(0).ToString());
        Assert.AreEqual(olvasoElvMasodik.GetValue(0).ToString(), adatbazis.FelhasznaloNevLekerdezese(2).GetValue(0).ToString());
        #endregion
    }

}

