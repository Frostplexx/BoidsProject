using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
   public Rigidbody player; 



    void Start()
    {
       
    }

    bool wait; 
   
    void Update()
    {
        // Volcano damage
        if (Vector3.Distance(player.transform.position, transform.position) <= 100f)
        {
            if (!wait)
            {
                Player.takeDmg(2f);
                wait = true; 
            }
            Wait();

        }
    }

    IEnumerator Wait()
    {
        //yield on a new YieldInstruction that waits for 3 seconds.
        yield return new WaitForSeconds(3);
        wait = false;
    }
}
