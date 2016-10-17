using UnityEngine;
using System.Collections;

public class _kerekMozgas : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))                    
                this.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(0.1f, 0));       
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))                   
                this.transform.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(-0.1f, 0));      
    }
}
