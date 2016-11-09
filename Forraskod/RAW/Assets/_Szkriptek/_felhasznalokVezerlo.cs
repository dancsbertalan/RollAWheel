using UnityEngine;
using System.Data;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Collections.Generic;



public class _felhasznalokVezerlo : MonoBehaviour
{
    #region DELEGATEK
    /// <summary>
    /// Ezzel a delegattel tudjuk átadni a végrehajtandó metódust a klikk esemény hozzárendelése metódushoz
    /// </summary>
    /// <param name="alapEsemeny"></param>
    private delegate void KlikkDelegate(UnityEngine.EventSystems.BaseEventData alapEsemeny);
    #endregion

    #region VÁLTOZÓK
    private _adatbazisvezerlo adatbazis;


    public GameObject felhasznalokFoPanel;
    public GameObject felhasznalokPanel;
    public GameObject felhasznaloPrefab;
    public Transform felhasznalokTrans;
    #endregion

    #region METÓDUSOK
    // Use this for initialization
    void Start()
    {
        adatbazis = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres);
        FelhasznaloKi();

    }

    // Update is called once per frame
    void Update()
    {        //ha a méret változik csak akkor kéne - lekezelendő!
        if (Application.loadedLevelName == _konstansok.FELHASZNALOK)
        {
            FeluletInicilizalas();
        }
    }


    /// <summary>
    /// Arra használom ezt a metódusot ,hogy a felhasznalokPanel Grid Layout Groupjának a Cell Size X -ét belőjjem 
    /// akkorára amekkora az egész felhasznalokFoPanel szélessége
    /// </summary>
    private void FeluletInicilizalas()
    {
        //REGERENCIA!!!
        GridLayoutGroup glg = felhasznalokPanel.GetComponent<GridLayoutGroup>();
        glg.cellSize = new Vector2(felhasznalokFoPanel.GetComponent<RectTransform>().rect.width,
            felhasznalokPanel.GetComponent<GridLayoutGroup>().cellSize.y);
    }

    private void FelhasznaloKi()
    {
        IDataReader olvaso;
        int tempVaneFelhasznalo = adatbazis.VanEFelhasznalo;
        if (tempVaneFelhasznalo == 1) //tehát ha van felhasználó
        {
            olvaso = adatbazis.FelhasznalokLekerdezese();
            FeluletInicilizalas();

            //az adatbázisbeli ID-k szerint dolgozik , mert ez lesz a törlés gombok és a panelek neve - ígyí tudjuk azonosítani ,hogy melyiket nyomtam meg és azt kell törölni az adatbázisbol
            while (olvaso.Read())
            {
                //ha konstansok oszlop számossal oldjuk meg akkor oszlopszam + 1 (5) mert az 5. oszlop lesz az id
                int panelSorszam = int.Parse(olvaso.GetValue(olvaso.FieldCount - 1).ToString()); //-1 azért kell mert nekünk a 6ot kell lekérni de az 5. az id
                GameObject felhasznaloPanel = Instantiate(felhasznaloPrefab);
                Text[] felhasznaloSzovegek = felhasznaloPanel.GetComponentsInChildren<Text>();

                #region GOMB KÉSZÍTÉSE
                Button felhasznaloTorleseGomb = felhasznaloPanel.GetComponentInChildren<Button>();
                felhasznaloTorleseGomb.name = panelSorszam.ToString();
                #endregion

                #region GOMBHOZ ESEMÉNY RENDELÉS
                ElemKlikkHozzaadas(felhasznaloTorleseGomb.gameObject, FelhasznaloTorlese);
                #endregion


                //ha field countal oldjuk meg akkor kell a < mint és kell a -1 hogy 5 nél kisebb lehessen az az a látható (az az a kiírt)
                //5 oszlopnál több ne legyen , ne legyen túl indexelés
                //vagy ha a konstansok oszlop számossal oldjuk meg akkor a <= kell ,hogy az indexeknél legyen 4. is és úgy adva 5-öt
                for (int i = 0; i < olvaso.FieldCount - 1; i++) //
                {
                    felhasznaloSzovegek[i].text = olvaso.GetValue(i).ToString();
                }
                felhasznaloPanel.name = panelSorszam.ToString();
                felhasznaloPanel.transform.SetParent(felhasznalokTrans);
                felhasznaloPanel.transform.localScale = new Vector3(1, 1, 1);

                #region FELHASZNALO GOMBHOZ ESEMÉNY RENDELÉS
                ElemKlikkHozzaadas(felhasznaloPanel, FelhasznaloPanelKlikk);
                #endregion
            }
            olvaso.Close();
        }
        else if (tempVaneFelhasznalo == 2) //egyébként ha nincs felhasználó
        {
            SceneManager.LoadScene(_konstansok.UJFELHASZNALO);
        }
        else //ha nem tudtuk megnézni mert zárt az adatbázis
        {

        }
    }

    private void FelhasznaloPanelKlikk(UnityEngine.EventSystems.BaseEventData alapEsemeny)
    {
        if (alapEsemeny is PointerEventData)
        {
            PointerEventData ped;
            ped = (alapEsemeny as PointerEventData);
            //pointer press adja meg, hogy melyiket nyomtuk meg
            GameObject kivalasztott = ped.pointerPress;

            //lekérjük a kiválasztott felhasználó adatait de nem inner joinolunk - hiszen most az ID-kre van szükség.
            IDataReader olvaso = adatbazis.FelhasznaloAdatainakLekerdezese(int.Parse(kivalasztott.name));
            if (olvaso != null && olvaso.FieldCount == _konstansok.FELHASZNALOK_ADATOKNELKUL_OSZLOP_SZAM)
            {
                //FelhasznaloID
                Debug.Log(olvaso.GetValue(0));
                PlayerPrefs.SetInt(_konstansok.FELHASZNALOID, int.Parse(olvaso.GetValue(0).ToString()));
                //Kor
                Debug.Log(int.Parse(olvaso.GetValue(1).ToString()));
                PlayerPrefs.SetInt(_konstansok.KOR, int.Parse(olvaso.GetValue(1).ToString()));
                //AktivKinezetID
                Debug.Log(int.Parse(olvaso.GetValue(2).ToString()));
                PlayerPrefs.SetInt(_konstansok.AKTIVKINEZETID, int.Parse(olvaso.GetValue(2).ToString()));
                //AktivSzint
                Debug.Log(int.Parse(olvaso.GetValue(3).ToString()));
                PlayerPrefs.SetInt(_konstansok.AKTIVSZINT, int.Parse(olvaso.GetValue(3).ToString()));
                //Penz
                Debug.Log(int.Parse(olvaso.GetValue(4).ToString()));
                PlayerPrefs.SetInt(_konstansok.PENZ, int.Parse(olvaso.GetValue(4).ToString()));
                //FelhasznaloNev
                Debug.Log(olvaso.GetValue(5));
                PlayerPrefs.SetString(_konstansok.NEV, olvaso.GetValue(5).ToString());
            }
            olvaso.Close();
            //hozzá adjuk a nevet is és a jelenleg aktív kinézetet is a playerprefs-hez
            SceneManager.LoadScene(_konstansok.FOMENU);
        }
    }

    /// <summary>
    /// Ezt a metódust rendelem hozzá a felhasználó törlése gombhoz.
    /// </summary>
    private void FelhasznaloTorlese(UnityEngine.EventSystems.BaseEventData alapEsemeny)
    {
        if (alapEsemeny is PointerEventData)
        {
            PointerEventData ped;
            ped = (alapEsemeny as PointerEventData);
            int felhasznaloID = 0;
            int.TryParse(ped.pointerPress.name, out felhasznaloID);
            adatbazis.FelhasznaloTorlese(felhasznaloID);
            SceneManager.LoadScene(_konstansok.FELHASZNALOK);
        }
    }
    
    /// <summary>
    /// Ezzel a metódussal tudok hozzá rendelni egy gameobject elemhez klikket. (Csak klikk esemény)
    /// </summary>
    /// <param name="elem">Az az elem emihez hozzá szeretnénk ezt rendelni.</param>
    private void ElemKlikkHozzaadas(GameObject elem,KlikkDelegate vegrehajtandoMetodus) {
        EventTrigger.Entry belepo = new EventTrigger.Entry();
        belepo.eventID = EventTriggerType.PointerClick;
        belepo.callback = new EventTrigger.TriggerEvent();
        UnityEngine.Events.UnityAction<BaseEventData> visszahivas = new UnityEngine.Events.UnityAction<BaseEventData>(vegrehajtandoMetodus);
        belepo.callback.AddListener(visszahivas);
        EventTrigger esemenyKioldo = elem.gameObject.AddComponent<EventTrigger>();
        esemenyKioldo.triggers.Add(belepo);
    }
    #endregion
}
