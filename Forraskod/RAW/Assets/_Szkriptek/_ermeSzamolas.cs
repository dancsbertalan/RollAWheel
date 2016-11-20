using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class _ermeSzamolas : MonoBehaviour
{
    int coin;
    public Text coinText;
    void Start()
    {
        //temp pénzt azért kell nullázni, mert ha ki crashelne a program vagy a felhasználó menet közben kilép akkor a következő menetben ehhez a temp pénzhez mérten "dolgozna"
        PlayerPrefs.SetInt(_konstansok.TEMP_PENZ, 0);
        coinText.text = PlayerPrefs.GetInt(_konstansok.TEMP_PENZ).ToString();
    }
       
    void OnTriggerEnter2D(Collider2D other)
    {
        coin = PlayerPrefs.GetInt(_konstansok.TEMP_PENZ);
        Destroy(gameObject);
        coin++;
        PlayerPrefs.SetInt(_konstansok.TEMP_PENZ, coin);
        coinText.text = PlayerPrefs.GetInt(_konstansok.TEMP_PENZ).ToString();
    }
}
