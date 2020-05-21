using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityStandardAssets.Effects;

public class KeyHider : MonoBehaviour
{
    public GameObject key;
    public static List<GameObject> alleKeys = new List<GameObject>();

    public GameObject[] hide;

    public Rigidbody player;

    int j = 0; 
    void Start()
    {
        //erzeugt vorgegebene menge an boids
        for (int i = 0; i < 5; i++)
        {
            alleKeys.Add(key);
            alleKeys[i] = (GameObject)Instantiate(key, Vector3.zero, Quaternion.identity);
        }

        while (j < 20)
        {
            foreach (GameObject key in alleKeys)
            {

                   key.transform.position = hide[Random.Range(0, 20)].transform.position;


                if (Vector3.Distance(this.transform.position, key.transform.position) < 2f) {

                    key.transform.position = hide[Random.Range(0, 20)].transform.position;

                }


            }
            j++;
        }

    }

    void Update()
    {

        foreach (GameObject key in alleKeys)
        {
            if (Vector3.Distance(key.transform.position, player.transform.position) <= 8f)
            {
                KeyHider.Destroy(alleKeys[alleKeys.IndexOf(key)]);
                alleKeys.RemoveAt(alleKeys.IndexOf(key));
                Score.scoreVal++;


            }
        }

        


    }
}
