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
        for (int i = 0; i < 100; i++)
        {
            //első ellenőrzés ,hogy nem nulla-e
            _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.UnitAdatbazisEleres);
            Assert.AreNotEqual(adatbazis, null);
            //második ellenőrzés hogy tényleg kinyitotta-e
            Assert.AreEqual(true, adatbazis.AdatbazisNyitottE);
        }
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
        for (int c = 0; c < 100; c++)
        {

            #region ELSŐ MÓDSZER
            _adatbazisvezerlo.PeldanyNullazasa();
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
    }

    [Test]
    public void FelhasznloNevLekerdezeseTeszt()
    {
        /*
         Az előzőhöz hasonlóan több lehetőség van.
            1 - Konstansba tudjuk ,hogy lekérjük az első és a második felhasználó nevét és az Ő nevüket tudjuk ! Le ellenőrizzük ,hogy tényleg azt adja-e vissza !
            2 - Csinálunk egy saját lekérdezést egy saját adatbázissal melyet itt adunk meg !
         */
        for (int c = 0; c < 100; c++)
        {
            #region ELSŐ MÓDSZER
            _adatbazisvezerlo.PeldanyNullazasa();
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

    [Test]
    public void FelhasznaloAdatainakLekerdezeseTeszt()
    {

        /*
         Az előzőhöz hasonlóan több lehetőség van.
            1 - Konstansba tudjuk ,hogy lekérjük az első és a második felhasználó nevét és az Ő nevüket tudjuk ! Le ellenőrizzük ,hogy tényleg azt adja-e vissza !
            2 - Csinálunk egy saját lekérdezést egy saját adatbázissal melyet itt adunk meg !
         */
        for (int c = 0; c < 100; c++)
        {
            #region ELSŐ MÓDSZER
            List<List<string>> elemek = new List<List<string>>() {
            new List<string>() {
                "1",
                "21",
                "1",
                "10",
                "10000",
                "iMager85"
            },
            new List<string>() {
                "2",
                "20",
                "1",
                "15",
                "5000",
                "AndorSebo"
            }
        };

            _adatbazisvezerlo.PeldanyNullazasa();
            _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.FelhkLekKonstansUnitAdatbazisEleres);
            IDataReader olvasoAkt = adatbazis.FelhasznaloAdatainakLekerdezese(1);

            //első esetben a is és a másodi kesetben is ismert a field count -> 6db mivel ennyi oszlopból kérünk le
            int i = 0;
            while (olvasoAkt.Read())
            {
                for (int j = 0; j < 6; j++)
                {
                    string elso = olvasoAkt.GetValue(j).ToString();
                    string masodik = elemek[i][j];
                    Assert.AreEqual(elso, masodik);
                }
                i++;
            }

            olvasoAkt = adatbazis.FelhasznaloAdatainakLekerdezese(2);
            i = 1;
            while (olvasoAkt.Read())
            {
                for (int j = 0; j < 6; j++)
                {
                    string elso = olvasoAkt.GetValue(j).ToString();
                    string masodik = elemek[i][j];
                    Assert.AreEqual(elso, masodik);
                }
                i++;
            }
            olvasoAkt.Close();
            #endregion

            #region MÁSODIK MÓDSZER
            SqliteConnection conn = new SqliteConnection("URI=file:" + unit_konstansok.FelhkLekKonstansUnitAdatbazisEleres);
            conn.Open();
            SqliteCommand comm = conn.CreateCommand();
            comm.CommandText = "select ja.FelhasznaloID,fh.Kor,ja.AktivKinezetID,ja.AktivSzint,ja.Penz,fh.Nev from jatekadat ja inner join" +
                " felhasznalo fh on ja.FelhasznaloID = fh.ID where ja.FelhasznaloID = 1";
            IDataReader olvasoElv2_1 = comm.ExecuteReader();
            IDataReader olvasoAkt2_1 = adatbazis.FelhasznaloAdatainakLekerdezese(1);
            //első felhasználó tesztje
            Assert.AreEqual(olvasoAkt2_1.FieldCount, olvasoElv2_1.FieldCount);
            while (olvasoElv2_1.Read())
            {
                olvasoAkt2_1.Read();
                for (int j = 0; j < 6; j++)
                {
                    string elso = olvasoAkt2_1.GetValue(j).ToString();
                    string masodik = olvasoElv2_1.GetValue(j).ToString();
                    Assert.AreEqual(elso, masodik);
                }
            }
            olvasoElv2_1.Close();
            olvasoAkt2_1.Close();


            SqliteCommand comm2 = conn.CreateCommand();
            comm2.CommandText = "select ja.FelhasznaloID,fh.Kor,ja.AktivKinezetID,ja.AktivSzint,ja.Penz,fh.Nev from jatekadat ja inner join" +
                " felhasznalo fh on ja.FelhasznaloID = fh.ID where ja.FelhasznaloID = 2";
            IDataReader olvasoElv2_2 = comm2.ExecuteReader();
            IDataReader olvasoAkt2_2 = adatbazis.FelhasznaloAdatainakLekerdezese(2);
            //második felhasználó tesztje
            Assert.AreEqual(olvasoAkt2_2.FieldCount, olvasoElv2_2.FieldCount);
            while (olvasoElv2_2.Read())
            {
                olvasoAkt2_2.Read();
                for (int j = 0; j < 6; j++)
                {
                    string elso = olvasoAkt2_2.GetValue(j).ToString();
                    string masodik = olvasoElv2_2.GetValue(j).ToString();
                    Assert.AreEqual(elso, masodik);
                }
            }
            olvasoAkt2_2.Close();
            olvasoElv2_2.Close();
            #endregion
        }

    }

    [Test]
    public void TablakKesziteseTeszt()
    {
        for (int c = 0; c < 100; c++)
        {
            #region ELSŐ MÓDSZER
            _adatbazisvezerlo.PeldanyNullazasa(); //erre akkor van szükség, ha már volt példány és újat akarunk más eléréssel csinálni
            _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.TablaKeszitoUnitAdatbazisEleres);
            try
            {
                adatbazis.TablakKeszitese();
            }
            catch (Exception e)
            {
                Debug.Log("Nem kellett táblákat létrehozni, mert már voltak!");
                Debug.Log(e.Message);
            }
            //ezzel elkészültek a táblák és azoknak oszlopaik
            SqliteConnection conn = new SqliteConnection("URI=file:" + unit_konstansok.TablaKeszitoUnitAdatbazisEleres);
            conn.Open();
            SqliteCommand commFelhTabl = conn.CreateCommand();
            commFelhTabl.CommandText = "select * from Felhasznalo";

            IDataReader olvasoFelhTabl = commFelhTabl.ExecuteReader();
            List<string> olvasoFelhTablKonst = new List<string>()
        {
            "ID",
            "Kor",
            "Nev"
        };


            for (int i = 0; i < olvasoFelhTablKonst.Count; i++) //azért a count-ig (3) kell menni mert az az elvárt elem szám
            {
                string elso = olvasoFelhTabl.GetName(i).ToString();
                string masodik = olvasoFelhTablKonst[i];
                Assert.AreEqual(elso, masodik);
            }
            olvasoFelhTabl.Close();

            SqliteCommand commJatekATabl = conn.CreateCommand();
            commJatekATabl.CommandText = "select * from Jatekadat";
            IDataReader olvasoJatekATabl = commJatekATabl.ExecuteReader();
            List<string> olvasoJatekATablKonst = new List<string>()
        {
            "FelhasznaloID",
            "Penz",
            "AktivSzint",
            "AktivKinezetID"
        };

            for (int i = 0; i < olvasoJatekATablKonst.Count; i++)
            {
                string elso = olvasoJatekATabl.GetName(i).ToString();
                string masodik = olvasoJatekATablKonst[i];
                Assert.AreEqual(elso, masodik);
            }
            olvasoJatekATabl.Close();

            SqliteCommand commKinTabl = conn.CreateCommand();
            commKinTabl.CommandText = "select * from Kinezet";
            IDataReader olvasoKinTabl = commKinTabl.ExecuteReader();
            List<string> olvasoKinTablKonst = new List<string>()
        {
            "ID",
            "Nev"
        };

            for (int i = 0; i < olvasoKinTablKonst.Count; i++)
            {
                string elso = olvasoKinTabl.GetName(i).ToString();
                string masodik = olvasoKinTablKonst[i];
                Assert.AreEqual(elso, masodik);
            }
            olvasoKinTabl.Close();


            //itt illendő lehetne az ,hogy a táblákat töröljük , hogy a következőnél is ténylegesen készítsen!

            #endregion
        }
    }

    [Test]
    public void JatekAdatNullazasaTeszt()
    {
        for (int c = 0; c < 100; c++)
        {
            _adatbazisvezerlo.PeldanyNullazasa();
            _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.JatekadatFelhNullazoUnitAdatbazisEleres);

            SqliteConnection conn = new SqliteConnection("URI=file:" + unit_konstansok.JatekadatFelhNullazoUnitAdatbazisEleres);
            conn.Open();
            SqliteCommand comm = conn.CreateCommand();

            for (int i = 0; i < 5; i++) //külső adatbázis kapcsolattal (az az nem az adatbázis vezérlő osztály egy példányával) hozzá adunk 5-t
            {
                comm.CommandText = string.Format("insert into Jatekadat(FelhasznaloID, Penz, AktivSzint, AktivKinezetID) values({0}, {1}, 1, 1)", i + 1, i + 2341);
                comm.ExecuteNonQuery();
            }
            //hogy legyen mit nullázni előtte hozzá kell adni pár elemet
            adatbazis.JatekAdatNullazasa();
            //majd valhogy beolvasunk a JA-ból- és ha tudott olvasni , akkor nem nullázta teljesen!
            //akkor nem jó

            comm.CommandText = "select * from Jatekadat";
            IDataReader olvasoJa = comm.ExecuteReader();
            bool olvasottE = false;
            while (olvasoJa.Read())
            {
                olvasottE = true;
                break;
            }
            olvasoJa.Close();
            Assert.AreEqual(olvasottE, false);
        }
    }

    [Test]
    public void FelhasznaloNullazasaTeszt()
    {
        for (int c = 0; c < 100; c++)
        {
            _adatbazisvezerlo.PeldanyNullazasa();
            _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(unit_konstansok.JatekadatFelhNullazoUnitAdatbazisEleres);

            SqliteConnection conn = new SqliteConnection("URI=file:" + unit_konstansok.JatekadatFelhNullazoUnitAdatbazisEleres);
            conn.Open();
            //insert into Felhasznalo(ID,Kor,Nev) values (1,21,"Teszt")
            SqliteCommand comm = conn.CreateCommand();

            for (int i = 0; i < 5; i++)
            {
                string nev = '"' + "Nev_" + i.ToString() + '"';
                comm.CommandText = string.Format("insert into Felhasznalo(ID,Kor,Nev) values ({0},{1},{2})", i + 1, i + 20, nev);
                comm.ExecuteNonQuery();
            }
            adatbazis.FelhasznaloNullazasa();

            comm.CommandText = "select * from Felhasznalo";
            IDataReader olvasoFh = comm.ExecuteReader();
            bool olvasottE = false;
            while (olvasoFh.Read())
            {
                olvasottE = true;
                break;
            }
            olvasoFh.Close();
            Assert.AreEqual(false, olvasottE);
        }

    }
}

