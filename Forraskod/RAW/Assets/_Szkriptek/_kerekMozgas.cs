using UnityEngine;
using UnityEngine.UI;

public class _kerekMozgas : MonoBehaviour
{
    bool jobb, bal = false;
    public Button Jobbra, Balra;

    void Start()
    {
        if (!(Application.platform == RuntimePlatform.Android))
        {        
            Jobbra.enabled = false;
            Jobbra.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            Jobbra.GetComponentInChildren<Text>().color = Color.clear;
            Balra.enabled = false;
            Balra.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            Balra.GetComponentInChildren<Text>().color = Color.clear;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || jobb)
            Jobb(GameObject.Find(_konstansok.KEREK));
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || bal)
            Bal(GameObject.Find(_konstansok.KEREK));
    }
    public void Jobb(GameObject Player)
    {
        if (Application.platform == RuntimePlatform.Android)
            jobb = true;
        Player.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(0.1f, 0));
    }
    public void Bal(GameObject Player)
    {
        if (Application.platform == RuntimePlatform.Android)
            bal = true;
        Player.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(-0.1f, 0));
    }
    public void felenged()
    {
        if (jobb)
            jobb = false;
        else
            bal = false;
    }
}
