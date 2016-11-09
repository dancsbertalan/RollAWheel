using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _ujFelhasznaloKeszit : MonoBehaviour
{

    #region VÁLTOZÓK
    public InputField nevBeText;
    public InputField korBeText;
    public Button nevTorleseGomb;
    public Button korTorleseGomb;

    private string nev;
    private bool validNev = false;
    private int kor;
    private bool validKor = false;
    #endregion

    #region METÓDUSOK
    public void UjFelhasznaloKesz()
    {

        if (validKor == false && (nevBeText.text.Length <= 50 && nevBeText.text.Length > 3)) //csak akkor ha eddig még egyszer se néztük a kort 
        {
            validNev = true;
            nev = nevBeText.text;
        }
        if (validKor == false && int.TryParse(korBeText.text,out kor)) //csak akkor akarjon próbálkozni parsolni , ha a kor eddig nem volt valid // az az nem volt már egy próbálkozáss
        {
            validKor = true;
        }

        if (validKor == true && validNev == true) //ha mind a kettő valid
        {
            Debug.Log("Új felhasználó hozzá adva!");
            _adatbazisvezerlo adatbazis = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres);
            adatbazis.FelhasznaloHozzad(nev, kor);
            SceneManager.LoadScene(_konstansok.FELHASZNALOK);
        }
        else if (validKor == false && validNev == true)
        {
            Debug.Log("A kor nem csak számokat tartalmaz!");
            korTorleseGomb.gameObject.SetActive(true);
            korBeText.interactable = false;
        }
        else if (validKor == true && validNev == false)
        {
            Debug.Log("A név nincs benne az intervallumba!");
            nevBeText.interactable = false;
        }
        else
        {
            Debug.Log("A kor nem csak számokat tartalmaz és a név nincs benne az intervallumba!");
            nevTorleseGomb.gameObject.SetActive(true);
            korTorleseGomb.gameObject.SetActive(true);
            nevBeText.interactable = false;
            korBeText.interactable = false;
        }
    }

    public void KorTorleseGomb()
    {
        korBeText.text = "";
        korBeText.interactable = true;
        korTorleseGomb.gameObject.SetActive(false);
    }
    public void NevTorleseGomb()
    {
        nevBeText.text = "";
        nevBeText.interactable = true;
        nevTorleseGomb.gameObject.SetActive(false);
    }
    #endregion
}
