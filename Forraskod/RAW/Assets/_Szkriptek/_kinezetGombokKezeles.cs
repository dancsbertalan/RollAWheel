using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _kinezetGombokKezeles : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        adatbazis = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres);
        GombokKigyujt();
        Debug.Log(string.Format("Összesen {0} darab kinézet gomb van.", _gombok.Count));
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region VÁLTOZÓK
    List<Button> _gombok = new List<Button>();
    _adatbazisvezerlo adatbazis;
    #endregion

    private void GombokKigyujt()
    {
        Object[] _gK = Object.FindSceneObjectsOfType(typeof(Button));
        for (int i = 0; i < _gK.Length; i++)
        {
            if (_gK[i] is Button && _gombok.Contains(_gK[i] as Button) == false && (_gK[i] as Button).tag == _konstansok.KINEZET_GOMB)
            {
                _gombok.Add(_gK[i] as Button);
            }
        }
        _gombok.Sort(
            delegate (Button btn1, Button btn2)
            {
                int btn1Int = int.Parse(btn1.name.Split(new string[] { "k", "Gomb" }, System.StringSplitOptions.None)[1]);
                int btn2Int = int.Parse(btn2.name.Split(new string[] { "k", "Gomb" }, System.StringSplitOptions.None)[1]);

                return btn1Int.CompareTo(btn2Int);
            });
    }
    private void KinezetSetHelper(int kinzetSzam) {
        adatbazis.KinezetModosult(kinzetSzam, PlayerPrefs.GetInt(_konstansok.FELHASZNALOID));
        PlayerPrefs.SetInt(_konstansok.AKTIVKINEZETID, kinzetSzam);
    }
    public void Kinezet1Gomb()
    {
        KinezetSetHelper(1);
        //az adatbázisba kiírjuk ,hogy melyik legyen az aktív kinézet
        //illetve a player prefset is átírjuk
        //ki

    }

    public void Kinezet2Gomb()
    {
        //ha van annyi pénzünk mint amennyibe ez kerül akkor lefuttathatjuk
        KinezetSetHelper(2);
    }

    public void Kinezet3Gomb()
    {
        KinezetSetHelper(3);
    }

    public void Kinezet4Gomb()
    {
        KinezetSetHelper(4);
    }

    /* public void Kinezet5Gomb()
     {

     }

     public void Kinezet6Gomb()
     {

     }

     public void Kinezet7Gomb()
     {

     }

     public void Kinezet8Gomb()
     {

     }

     public void Kinezet9Gomb()
     {

     }

     public void Kinezet10Gomb()
     {

     }

     public void Kinezet11Gomb()
     {

     }

     public void Kinezet12Gomb()
     {

     }

     public void Kinezet13Gomb()
     {

     }

     public void Kinezet14Gomb()
     {

     }*/
}
