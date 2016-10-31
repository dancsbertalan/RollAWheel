using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class _ermeSzamolas : MonoBehaviour
{
    int coin;
    public Text coinText;
    void Start()
    {
        coinText.text = PlayerPrefs.GetInt(_konstansok.PENZ).ToString();
    }
       
    void OnTriggerEnter2D(Collider2D other)
    {
        coin = PlayerPrefs.GetInt(_konstansok.PENZ);
        Destroy(gameObject);
        coin++;
        PlayerPrefs.SetInt(_konstansok.PENZ, coin);
        coinText.text = PlayerPrefs.GetInt(_konstansok.PENZ).ToString();
        
    }
}
