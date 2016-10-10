using UnityEngine;

using System.Collections;



public class _kameraKerek : MonoBehaviour
{
    public Transform Player;
    Vector3 Direction;
    void Start()
    {
        Direction = transform.position - Player.transform.position;
    }
    void Update()
    {      
            transform.position = Player.transform.position + Direction;
        
    }

}
