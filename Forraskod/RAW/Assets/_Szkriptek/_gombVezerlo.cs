﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Data;
using System.IO;

public class _gombVezerlo : MonoBehaviour
{
    #region VÁLTOZÓK
    public Text profilText;
    private static bool elsoNyitasVoltE = false;
    #endregion

    #region METÓDUSOK
    public void Kilepes()
    {
        Application.Quit();
        Debug.Log("Kiléptél");
    }

    public void Jatek()
    {
        SceneManager.LoadScene(_konstansok.MODVALASZTO);
    }

    public void Beallitasok()
    {
        SceneManager.LoadScene(_konstansok.BEALLITASOK);
    }

    public void Kinezetek()
    {
        SceneManager.LoadScene(_konstansok.KINEZETEK);
    }

    public void Vissza()
    {
        switch (Application.loadedLevelName)
        {
            case _konstansok.MODVALASZTO:
                SceneManager.LoadScene(_konstansok.FOMENU);
                break;
                return;
            case _konstansok.PALYAVALASZTO:
                SceneManager.LoadScene(_konstansok.MODVALASZTO);
                break;
                return;
            case _konstansok.BEALLITASOK:
                SceneManager.LoadScene(_konstansok.FOMENU);
                break;
                return;
            case _konstansok.KINEZETEK:
                SceneManager.LoadScene(_konstansok.FOMENU);
                break;
                return;
        }
        if (Application.loadedLevelName.Split('_')[0] == "palya")
        {
            SceneManager.LoadScene(_konstansok.PALYAVALASZTO);
        }
    }

    public void SzimplaMod()
    {
        SceneManager.LoadScene(_konstansok.PALYAVALASZTO);
    }

    public void VersenyAzIdovel()
    {
        SceneManager.LoadScene(_konstansok.PALYAVALASZTO);
    }

    void Start()
    {
        if (Application.loadedLevelName == _konstansok.FOMENU)
        {
            Debug.Log(string.Format("Fő menu player prefs felhasználó id: {0} | Név :{1}", PlayerPrefs.GetInt(_konstansok.FELHASZNALOID), PlayerPrefs.GetString(_konstansok.NEV)));
            
            if (elsoNyitasVoltE == false)
            {
                IDataReader olvaso = _adatbazisvezerlo.GetPeldany(_konstansok.AdatbazisEleres).FelhasznaloNevLekerdezese(PlayerPrefs.GetInt(_konstansok.FELHASZNALOID));
                PlayerPrefs.SetString(_konstansok.NEV, olvaso.GetValue(0).ToString());
                elsoNyitasVoltE = true;
                profilText.text = "Profil: " + PlayerPrefs.GetString(_konstansok.NEV);
                olvaso.Close();
            }
            else
            {
                profilText.text = "Profil: " + PlayerPrefs.GetString(_konstansok.NEV);
            }
        }
    }

    public void FelhasznalokMegnyitasa()
    {
        SceneManager.LoadScene(_konstansok.FELHASZNALOK);
    }

    public void FelhasznaloKeszitesMegnyitasa()
    {
        SceneManager.LoadScene(_konstansok.UJFELHASZNALO);
    }
    #endregion
}
