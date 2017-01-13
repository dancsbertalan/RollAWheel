using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _csillagSzamolas : MonoBehaviour
{
    public GameObject csillagJelzo0;
    public GameObject csillagJelzo1;
    public GameObject csillagJelzo2;
    public GameObject csillagJelzo3;
    public GameObject csillagJelzo4;
    public GameObject csillagJelzo5;

    // Use this for initialization
    static int csillagDarabok;
    void Start()
    {
        csillagDarabok = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (csillagDarabok)
        {
            case 0:
                csillagJelzo0.active = false;
                csillagJelzo1.active = true;
                break;
            case 1:
                csillagJelzo1.active = false;
                csillagJelzo2.active = true;
                break;
            case 2:
                csillagJelzo2.active = false;
                csillagJelzo3.active = true;
                break;
            case 3:
                csillagJelzo3.active = false;
                csillagJelzo4.active = true;
                break;
            case 4:
                csillagJelzo4.active = false;
                csillagJelzo5.active = true;
                break;
        }
        csillagDarabok += 1;
        Destroy(gameObject);

    }
}