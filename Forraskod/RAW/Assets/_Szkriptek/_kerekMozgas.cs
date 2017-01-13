using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class _kerekMozgas : MonoBehaviour
{
    bool jobb, bal = false;
    public Button Jobbra, Balra;
    public GameObject csillagok;
    public GameObject ermek;

    public GameObject penzKep;
    public GameObject penzSzoveg;

    public GameObject csillagJelzo0;
    public GameObject csillagJelzo1;
    public GameObject csillagJelzo2;
    public GameObject csillagJelzo3;
    public GameObject csillagJelzo4;
    public GameObject csillagJelzo5;



    void Start()
    {

        //illetve itt kell azt le kezelni ,hogy idő módban miket mutasson és miket nem ... stb // - mivel ez az a szkript ami minden pályánál ott lesz! - az , hogy melyik modban vagyunk ki van véve egy prefs-be
        //PlayerPrefs.GetString(_konstansok.SZIMPLA_MOD) .. értékei pedig az alábbi két konstans lehet _konstansok.SZIMPLA_MOD_ERTEK_IGEN/NEM
        KinezetAllit();
        _konstansok.MozoghatE = true;
        if (!(Application.platform == RuntimePlatform.Android))
        {
            Jobbra.enabled = false;
            Jobbra.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            Jobbra.GetComponentInChildren<Text>().color = Color.clear;
            Balra.enabled = false;
            Balra.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            Balra.GetComponentInChildren<Text>().color = Color.clear;
        }
        
        if (PlayerPrefs.GetString(_konstansok.CSILLAG_MOD) == _konstansok.ERTEK_IGEN)
        {
            csillagok.active = true;
            ermek.active = false;
            penzKep.active = false;
            penzKep.active = false;
            csillagJelzo0.active = true;
        }
        else
        {
            csillagok.active = false;
            ermek.active = true;
            penzKep.active = true;
            penzKep.active = true;
        }
    }

    

    /// <summary>
    /// Ez a metódus fogja beállítani a felhasználónak a kinézetét. Csak akkor történik változás ,hogy ha 
    /// </summary>
    private void KinezetAllit()
    {
        string adottFajLnev = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres).AdottKinezetFajLnev(PlayerPrefs.GetInt(_konstansok.AKTIVKINEZETID));
        Sprite ujKerek = Resources.Load("_Kerek/"+adottFajLnev, typeof(Sprite)) as Sprite;
        GameObject.Find(_konstansok.KEREK).GetComponent<SpriteRenderer>().sprite = ujKerek;
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || jobb) && _konstansok.MozoghatE == true)
            Jobb(GameObject.Find(_konstansok.KEREK));
        else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || bal) && _konstansok.MozoghatE == true)
            Bal(GameObject.Find(_konstansok.KEREK));
    }
    public void Jobb(GameObject Player)
    {
        if (_konstansok.MozoghatE == true)
        {
            if (Application.platform == RuntimePlatform.Android)
                jobb = true;
            Player.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(0.1f, 0));
        }
    }
    public void Bal(GameObject Player)
    {
        if (_konstansok.MozoghatE == true)
        {
            if (Application.platform == RuntimePlatform.Android)
                bal = true;
            Player.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(-0.1f, 0));
        }
    }
    public void felenged()
    {
        if (jobb)
            jobb = false;
        else
            bal = false;
    }
}
