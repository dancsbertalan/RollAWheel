using UnityEngine;
using System.Collections;

public class _kerekMozgas : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            Jobb(GameObject.Find(_konstansok.KEREK));
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            Bal(GameObject.Find(_konstansok.KEREK));     
    }    
    public void Jobb(GameObject Player)
    {
        Player.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(0.1f, 0));
    }
    public void Bal(GameObject Player)
    {
        Player.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(-0.1f, 0));
    }

}
