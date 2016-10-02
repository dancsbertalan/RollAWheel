using UnityEngine;
using System.Collections;

public class _gombVezerlo : MonoBehaviour
{

    #region METÓDUSOK
    // use this for initialization
    void start()
    {
    }

    // update is called once per frame
    void update()
    {

    }

    public void Kilepes()
    {
        Application.Quit();
        Debug.Log("Kiléptél");
    }

    public void Vissza()
    {
        Application.LoadLevel(Elozo);
    }

    public void Jatek()
    {
        //betöltjük a játék mód választó jelenetet
        FoMenureBeallit();
        Application.LoadLevel("_modValaszto");
    }

    public void Beallitasok()
    {
        //betöltjük a beállítások jelenetét
        FoMenureBeallit();
        Application.LoadLevel("_beallitasok");
    }

    public void Kinezetek()
    {
        //betöltjük a kinézet választó jelenetet
        FoMenureBeallit();
        Application.LoadLevel("_kinezetek");
    }

    #region METÓDUSOK MELYEKET KIVÜL NEM HASZNÁLUNK
    private void FoMenureBeallit()
    {
        Elozo = "_foMenu";
    }
    #endregion

    #endregion

    #region PROPERTYK
    public static string Elozo
    {
        get { return _gombVezerlo._elozo; }
        private set { _gombVezerlo._elozo = value; }
    }
    #endregion

    #region MEZŐK
    private static string _elozo;
    #endregion



}
