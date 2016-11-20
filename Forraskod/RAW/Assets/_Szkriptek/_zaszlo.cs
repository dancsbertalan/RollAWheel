using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _zaszlo : MonoBehaviour
{
    public GameObject felugroPanel;
    public Text gyujtottPenzTxt;
    public Text edigiPenzTxt;
    public Text osszPenzTxt;

    _adatbazisvezerlo adatbazis;
    void Start()
    {
        adatbazis = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _konstansok.MozoghatE = false;
        PanelMegjelenitese(PlayerPrefs.GetInt(_konstansok.TEMP_PENZ),PlayerPrefs.GetInt(_konstansok.PENZ),(PlayerPrefs.GetInt(_konstansok.TEMP_PENZ)+ PlayerPrefs.GetInt(_konstansok.PENZ)));
        PlayerPrefs.SetInt(_konstansok.PENZ, (PlayerPrefs.GetInt(_konstansok.PENZ) + PlayerPrefs.GetInt(_konstansok.TEMP_PENZ)));
        PlayerPrefs.SetInt(_konstansok.TEMP_PENZ, 0);

        adatbazis.FelhasznaloPenzAtir(PlayerPrefs.GetInt(_konstansok.FELHASZNALOID), PlayerPrefs.GetInt(_konstansok.PENZ));
        //SceneManager.LoadScene("_palyaValaszto");
    }

    private void PanelMegjelenitese(int gyujtott, int eddigi, int ossz) {
        felugroPanel.active = true;
        gyujtottPenzTxt.text = gyujtott.ToString();
        edigiPenzTxt.text = eddigi.ToString();
        osszPenzTxt.text = (eddigi + gyujtott).ToString();
    }
}
