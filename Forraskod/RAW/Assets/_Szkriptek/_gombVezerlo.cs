using UnityEngine;
using System.Collections;

public class _gombVezerlo : MonoBehaviour
{
    #region METÓDUSOK
    public void Kilepes()
    {
        Application.Quit();
        Debug.Log("Kiléptél");
    }

    public void Vissza()
    {
        if (Application.loadedLevelName == "_beallitasok" || Application.loadedLevelName == "_kinezetek" || Application.loadedLevelName == "_modValaszto")
        {
            Application.LoadLevel("_foMenu");
        }
        else if (Application.loadedLevelName == "_palyaValaszto")
        {
            Application.LoadLevel("_modValaszto");
        }
        else if (Application.loadedLevelName.Split('_')[0] == "palya")
        {
            //map név szerint loadoljul vissza a betöltő menüt
            Application.LoadLevel("_modValaszto");
        }
    }

    public void Jatek()
    {
        //betöltjük a játék mód választó jelenetet
        Application.LoadLevel("_modValaszto");
    }

    public void Beallitasok()
    {
        //betöltjük a beállítások jelenetét
        Application.LoadLevel("_beallitasok");
    }

    public void Kinezetek()
    {
        //betöltjük a kinézet választó jelenetet
        Application.LoadLevel("_kinezetek");
    }
    
    public void SzimplaMod()
    {
        Application.LoadLevel("_palyaValaszto");
        //Application.LoadLevel("_tesztPalya");
    }

    public void IdoMod()
    {
        Application.LoadLevel("_palyaValaszto");
    }

    #endregion

    #region PROPERTYK
    
    #endregion

    #region MEZŐK
    #endregion
}
