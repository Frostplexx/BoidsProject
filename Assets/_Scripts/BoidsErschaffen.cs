using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BoidsErschaffen : MonoBehaviour
{
    //variable, die das Boid-Modell beinhaltet
    public GameObject boid;
    //variable, die das Hai-Modell beinhaltet
    public GameObject hai;
    //Spawnradius 
    public static int raumGroesse = 100;
    //Meinge an Boids
    static int boidNummer = 500;

    public GameObject player;

    static int haiNummer = 1;
    //Menge an boids als array
    public static GameObject[] alleBoids = new GameObject[boidNummer];

    public static GameObject[] alleHaie = new GameObject[haiNummer];
    //Ziel für die boids
   // public static Vector3 ziel = new Vector3(Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse)); 
    public static Vector3 ziel = Vector3.zero; 
    void Start()
    {
    	//erzeugt vorgegebene menge an boids
        for (int i = 0; i < boidNummer; i++) {
            //boid wird zufällig innerhalb einer Kugel erschaffen
            Vector3 pos = new Vector3(Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse));
            alleBoids[i] = (GameObject)Instantiate(boid, pos, Quaternion.identity); 
        }
		//ezeugrt vorgegebene menge an haien 
         for (int i = 0; i < haiNummer; i++) {
            //boid wird zufällig innerhalb einer Kugel erschaffen
            Vector3 pos = new Vector3(Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse));
            alleHaie[i] = (GameObject)Instantiate(hai, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 2000) < 50) {

            ziel = new Vector3(Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse), Random.Range(-raumGroesse, raumGroesse));


        }
    }
}
