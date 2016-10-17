using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class _palyaGombokKezeles : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GombokKigyujt();
	}
	
	// Update is called once per frame

	void Update () {
	}

    #region MEZŐK
    List<Button> _gombok = new List<Button>(); // a program elindításakor gyűjtjük ki ebbe a változóba a gombokat rendezetten p1,p2.... az az a számaik szerint.
    public Button _koviOldal;
    public Button _elozoOldal;
    int _oldalSzamlalo = 1;
    int _maxOldal = 3;
    int _minOldal = 1;
    bool _volteNagyobb = false;
    #endregion

    #region METÓDUSOK
    public void KoviOldal()
    {
        if (_oldalSzamlalo < _maxOldal)
        {
            foreach (Button gombElem in _gombok)
            {
                int tempGombSzam = int.Parse(gombElem.GetComponentInChildren<Text>().text.ToString().Split('P')[1]);
                tempGombSzam += 14;
                gombElem.GetComponentInChildren<Text>().text = "P" + tempGombSzam;
            }
            _oldalSzamlalo++;
            if (_oldalSzamlalo > 1 && _volteNagyobb == false)
            {
                _elozoOldal.gameObject.SetActive(true);
                _volteNagyobb = true;
            }
            if (_oldalSzamlalo == _maxOldal)
            {
                _koviOldal.gameObject.SetActive(false);
            }
        }
    }
    public void ElozoOldal()
    {
        if (_oldalSzamlalo > _minOldal)
        {
            foreach(Button gombElem in _gombok)
            {
                int tempGombSzam = int.Parse(gombElem.GetComponentInChildren<Text>().text.ToString().Split('P')[1]);
                tempGombSzam -= 14;
                gombElem.GetComponentInChildren<Text>().text = "P" + tempGombSzam;
            }
            _oldalSzamlalo--;
            if (_oldalSzamlalo > _minOldal && _oldalSzamlalo < _maxOldal)
            {
                _koviOldal.gameObject.SetActive(true);
            }
            if (_oldalSzamlalo == _minOldal)
            {
                _elozoOldal.gameObject.SetActive(false);
                _volteNagyobb = false;
            }
        }
    }

    private void GombokKigyujt()
    {
        Object[] _gK = Object.FindSceneObjectsOfType(typeof(Button));
        for (int i = 0; i < _gK.Length; i++)
        {
            if (_gK[i] is Button && _gombok.Contains(_gK[i] as Button) == false && (_gK[i] as Button).tag == "palyaGomb")
            {
                _gombok.Add(_gK[i] as Button);
            }
        }
        _gombok.Sort(
            delegate (Button btn1, Button btn2)
            {
                int btn1Int = int.Parse(btn1.name.Split(new string[] { "p", "Gomb" }, System.StringSplitOptions.None)[1]);
                int btn2Int = int.Parse(btn2.name.Split(new string[] { "p", "Gomb" }, System.StringSplitOptions.None)[1]);

                return btn1Int.CompareTo(btn2Int);
            });
    }

    public void Palya1Gomb()
    {
        if (_gombok[0].GetComponentInChildren<Text>().text.ToString() == "P1")
        {
            Application.LoadLevel("map1");
        }
    }
    public void Palya2Gomb()
    {
        if (_gombok[1].GetComponentInChildren<Text>().text.ToString() == "P2")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya3Gomb()
    {
        if (_gombok[2].GetComponentInChildren<Text>().text.ToString() == "P3")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya4Gomb()
    {
        if (_gombok[3].GetComponentInChildren<Text>().text.ToString() == "P4")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya5Gomb()
    {
        if (_gombok[4].GetComponentInChildren<Text>().text.ToString() == "P5")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya6Gomb()
    {
        if (_gombok[5].GetComponentInChildren<Text>().text.ToString() == "P6")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya7Gomb()
    {
        if (_gombok[6].GetComponentInChildren<Text>().text.ToString() == "P7")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya8Gomb()
    {
        if (_gombok[7].GetComponentInChildren<Text>().text.ToString() == "P8")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya9Gomb()
    {
        if (_gombok[8].GetComponentInChildren<Text>().text.ToString() == "P9")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya10Gomb()
    {
        if (_gombok[9].GetComponentInChildren<Text>().text.ToString() == "P10")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya11Gomb()
    {
        if (_gombok[10].GetComponentInChildren<Text>().text.ToString() == "P11")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya12Gomb()
    {
        if (_gombok[11].GetComponentInChildren<Text>().text.ToString() == "P12")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya13Gomb()
    {
        if (_gombok[12].GetComponentInChildren<Text>().text.ToString() == "P13")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    public void Palya14Gomb()
    {
        if (_gombok[13].GetComponentInChildren<Text>().text.ToString() == "P14")
        {
            Application.LoadLevel("_tesztPalya");
        }
    }
    #endregion
}
