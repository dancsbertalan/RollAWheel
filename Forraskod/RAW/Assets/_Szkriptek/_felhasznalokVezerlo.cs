using UnityEngine;
using System.Data;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class _felhasznalokVezerlo : MonoBehaviour
{

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
        adatbazis = _adatbazisvezerlo.GetPeldany();
        IDataReader olvaso;
        int tempVaneFelhasznalo = adatbazis.VanEFelhasznalo;
        if (tempVaneFelhasznalo == 1) //tehát ha van felhasználó
        {
            olvaso = adatbazis.FelhasznalokLekerdezese();
            FeluletInicilizalas();
            int panelSorszam = 1; //az adatbázisbeli sorszámok szerint dolgozva
            while (olvaso.Read())
            {
                GameObject felhasznaloPanel = Instantiate(felhasznaloPrefab);
                Text[] felhasznaloSzovegek = felhasznaloPanel.GetComponentsInChildren<Text>();
                for (int i = 0; i <= _konstansok.FELHASZNALOK_OSZLOP_SZAM; i++)
                {
                    felhasznaloSzovegek[i].text = olvaso.GetValue(i).ToString();
                }
                felhasznaloPanel.name = panelSorszam.ToString();
                felhasznaloPanel.transform.SetParent(felhasznalokTrans);
                felhasznaloPanel.transform.localScale = new Vector3(1, 1, 1);

                EventTrigger.Entry belepo = new EventTrigger.Entry();
                belepo.eventID = EventTriggerType.PointerClick;
                belepo.callback = new EventTrigger.TriggerEvent();
                UnityEngine.Events.UnityAction<BaseEventData> visszahivas = new UnityEngine.Events.UnityAction<BaseEventData>(FelhasznaloPanelKlikk);
                belepo.callback.AddListener(visszahivas);
                EventTrigger esemenyKioldo = felhasznaloPanel.AddComponent<EventTrigger>();
                esemenyKioldo.triggers.Add(belepo);
                panelSorszam++;
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

    public void FelhasznaloPanelKlikk(UnityEngine.EventSystems.BaseEventData alapEsemeny)
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
            }
            olvaso.Close();
            //hozzá adjuk a nevet is és a jelenleg aktív kinézetet is a playerprefs-hez
            SceneManager.LoadScene(_konstansok.FOMENU);
        }
        else
        {

        }
    }

    public void NevErtekValtozas()
    {

    }
    #endregion
}
