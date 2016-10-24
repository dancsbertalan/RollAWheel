using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class _ermeSzamolas : MonoBehaviour
{
    int coin;
    public Text coinText;
    void Start()
    {
        coinText.text = PlayerPrefs.GetInt("Coin").ToString();
    }
       
    void OnTriggerEnter2D(Collider2D other)
    {
        coin = PlayerPrefs.GetInt("Coin");
        Destroy(gameObject);
        coin++;
        PlayerPrefs.SetInt("Coin",coin);
        coinText.text = PlayerPrefs.GetInt("Coin").ToString();
    }
}
