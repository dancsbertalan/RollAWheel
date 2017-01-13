using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class _zaszlo : MonoBehaviour
{
    public GameObject felugroPanel;
    public Text gyujtottPenzTxt;
    public Text edigiPenzTxt;
    public Text osszPenzTxt;
    public GameObject felugroPanelIdoResze; //rejteni / jeleníteni kell az adatokkal attól függően ,hogy melyik módban vagyunk
    public Text mostElertIdo;
    public Text legjobbElertIdo;

    Timer idozito;
    public Text idozitoTxt;
    int idozitoSzamlalo = 0; //ms-ben (1000ms -> 1s)

    bool vegeVoltE = false;
    _adatbazisvezerlo adatbazis;
    void Start()
    {
        idozito = new Timer(_konstansok.TIMER_EGY_MASODPERC);
        string pp = PlayerPrefs.GetString(_konstansok.SZIMPLA_MOD);
        if (PlayerPrefs.GetString(_konstansok.SZIMPLA_MOD) == _konstansok.ERTEK_NEM)
        {
            idozitoTxt.gameObject.active = true;
        }
        adatbazis = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _konstansok.VEG_ZASZLO && vegeVoltE == false && PlayerPrefs.GetString(_konstansok.CSILLAG_MOD) == _konstansok.ERTEK_NEM)
        {
            idozito.Close();
            vegeVoltE = true;
            idozitoTxt.gameObject.active = false;
            _konstansok.MozoghatE = false;
            PanelMegjelenitese(PlayerPrefs.GetInt(_konstansok.TEMP_PENZ), PlayerPrefs.GetInt(_konstansok.PENZ), (PlayerPrefs.GetInt(_konstansok.TEMP_PENZ) + PlayerPrefs.GetInt(_konstansok.PENZ)));
            idozitoSzamlalo = 0;
            PlayerPrefs.SetInt(_konstansok.PENZ, (PlayerPrefs.GetInt(_konstansok.PENZ) + PlayerPrefs.GetInt(_konstansok.TEMP_PENZ)));
            PlayerPrefs.SetInt(_konstansok.TEMP_PENZ, 0);

            adatbazis.FelhasznaloPenzAtir(PlayerPrefs.GetInt(_konstansok.FELHASZNALOID), PlayerPrefs.GetInt(_konstansok.PENZ));
        }
        else if (other.tag == _konstansok.KEZD_ZASZLO && (PlayerPrefs.GetString(_konstansok.SZIMPLA_MOD) == _konstansok.ERTEK_NEM) && PlayerPrefs.GetString(_konstansok.CSILLAG_MOD) == _konstansok.ERTEK_NEM)
        {
            vegeVoltE = false;
            /*
                Timer mutatása!
            */
            if (idozito.Enabled == false)
            {
                idozito.Elapsed += Idozito_Elapsed;
                idozito.Start();
            }
        }
    }

    private void Idozito_Elapsed(object sender, ElapsedEventArgs e)
    {
        idozitoSzamlalo++;
    }

    void Update()
    {
        if (idozito.Enabled == true)
        {
            idozitoTxt.text = IdoFormaz(idozitoSzamlalo);
        }
    }

    private void PanelMegjelenitese(int gyujtott, int eddigi, int ossz)
    {
        felugroPanel.active = true;
        gyujtottPenzTxt.text = gyujtott.ToString();
        edigiPenzTxt.text = eddigi.ToString();
        osszPenzTxt.text = (eddigi + gyujtott).ToString();
        if (PlayerPrefs.GetString(_konstansok.SZIMPLA_MOD) == _konstansok.ERTEK_NEM)
        {
            int betoltottPalyaID = int.Parse(Application.loadedLevelName.Split('_')[1]);
            int legjobbIdo = -1;
            legjobbIdo = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres).FelhasznaloLegjobbIdoAdottPalyan(PlayerPrefs.GetInt(_konstansok.FELHASZNALOID), betoltottPalyaID);
            if (legjobbIdo <= -1) //ilyen esetben nem volt még ezen a pályán szal be kell szúrni (insert)
            {
                _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres).FelhasznaloLegjobbIdoAdottPalyaBeszur(PlayerPrefs.GetInt(_konstansok.FELHASZNALOID), betoltottPalyaID, idozitoSzamlalo);
                legjobbElertIdo.text = IdoFormaz(idozitoSzamlalo);
            }
            else
            {
                if (idozitoSzamlalo < legjobbIdo)
                {
                    _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres).FelhasznaloLegjobbIdoAdottPalyaFrissit(PlayerPrefs.GetInt(_konstansok.FELHASZNALOID), betoltottPalyaID, idozitoSzamlalo);

                    legjobbElertIdo.text = IdoFormaz(idozitoSzamlalo);
                }
                else
                {
                    legjobbElertIdo.text = IdoFormaz(legjobbIdo);
                }
            }
            //és utána írjuk ki
            mostElertIdo.text = IdoFormaz(idozitoSzamlalo);


            /*
             Kikérjük adatbázisból az itt (ezen a pályán , ezzel a felhasználóval) elért LEGJOBB időt. Majd végzünk egy össze hasonlítást a mostanival
             Ha a mostani nagyobb mint amit eddig tároltunk akkor frissítünk egyébként pedig nem történik semmi.
             */
            felugroPanelIdoResze.active = true;
        }
        /*
         Ha a időmódban vagyunk akkor a panel jelenítse meg (idő panel) - És mutassa meg az ITT adatbázisból kikért adatokat
         */
    }
    private string IdoFormaz(int formazandoIdo) {
        string formazottIdo = Mathf.Floor(formazandoIdo / 60).ToString("00") + ":" + Mathf.Floor(formazandoIdo % 60).ToString("00");
        return formazottIdo;
    }
}
