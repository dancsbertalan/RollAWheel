using UnityEngine;
using System.Collections;

public class _kerekMozgas : MonoBehaviour
{
    bool jobb, bal = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || jobb)
            Jobb(GameObject.Find(_konstansok.KEREK));
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || bal)
            Bal(GameObject.Find(_konstansok.KEREK));
    }
    public void Jobb(GameObject Player)
    {
        jobb = true;
        Player.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(0.1f, 0));
    }
    public void Bal(GameObject Player)
    {
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
