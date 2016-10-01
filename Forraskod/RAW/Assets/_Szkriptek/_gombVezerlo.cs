using UnityEngine;
using System.Collections;

public class _gombVezerlo : MonoBehaviour
{

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

    public void Jatek()
    { 
        //betöltjük a játék mód választó jelenetet
    }

    public void Beallitasok()
    { 
        //betöltjük a beállítások jelenetét
    }

    public void Kinezetek()
    { 
        //betöltjük a kinézet választó jelenetet
    }


}
