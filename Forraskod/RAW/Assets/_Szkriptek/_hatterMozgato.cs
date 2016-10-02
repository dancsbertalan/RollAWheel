﻿using UnityEngine;
using System.Collections;

public class _hatterMozgato : MonoBehaviour
{

    private float _mozgasSebessege = 0.125f;

    void Update()
    {
        Vector2 eltolas = new Vector2(Time.time * _mozgasSebessege, 0);

        GetComponent<Renderer>().material.mainTextureOffset = eltolas;
    }
}