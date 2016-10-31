using UnityEngine;

using System.Collections;



public class _kameraKerek : MonoBehaviour
{
   GameObject Player;
    
    void Start()
    {
        Player = GameObject.Find(_konstansok.KEREK);
    }    
        
      
   
    void Update()
    {      
            transform.position = new Vector3(Player.transform.position.x , 0, -10);  
    }

}