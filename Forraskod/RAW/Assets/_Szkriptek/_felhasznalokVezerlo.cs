using UnityEngine;
using System.Data;
using UnityEngine.UI;

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
        FeluletInicilizalas();
        adatbazis = _adatbazisvezerlo.GetPeldany();
        IDataReader olvaso = adatbazis.FelhasznalokLekerdezese();
        while (olvaso.Read())
        {
            GameObject felhasznaloPanel = Instantiate(felhasznaloPrefab);
            Text[] felhasznaloSzovegek = felhasznaloPanel.GetComponentsInChildren<Text>();
            for (int i = 0; i <= _konstansok.FELHASZNALOK_OSZLOP_SZAM; i++)
            {
                felhasznaloSzovegek[i].text = olvaso.GetValue(i).ToString();
            }
            felhasznaloPanel.transform.SetParent(felhasznalokTrans);
            felhasznaloPanel.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ha a méret változik csak akkor kéne - lekezelendő!
        FeluletInicilizalas();
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
    #endregion
}
