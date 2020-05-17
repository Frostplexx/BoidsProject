using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidKI : MonoBehaviour
{
    //Variable um die Geschwindigkeit zu verändern (muss mindesens 1 betragen)
   float geschwindigkeit = 30f;
    //geschwindigkeit um sich zu drehen
   float rotGeschw = 7.0f;
    //variable zur bestimmung der entferung welcher boid noch als nachar gilt
     float boidDist = 30f;
	//minimale boid distanz 
    float minDist = 3f;

    Rigidbody player; 
   //ziel vektor aus boidsErschaffen geholt
    Vector3 ziel = BoidsErschaffen.ziel;

    int raumGroesse = BoidsErschaffen.raumGroesse; 

    bool drehen = false;

    bool want = true;

    public Vector3 ausweichen; 

    private void OnTriggerEnter(Collider other){
       
        ausweichen = this.transform.position - other.gameObject.transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ausweichen), 14f * Time.deltaTime);      
    }
    private void OnTriggerExit(Collider other){
     
       ziel = BoidsErschaffen.ziel; 
       want = true;

    }


    void Update()
    {
        if (drehen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ausweichen - transform.position), rotGeschw * Time.deltaTime);
            geschwindigkeit = Random.Range(15f, 30f);
        }
        else
        {


            if (Vector3.Distance(transform.position, Vector3.zero) >= BoidsErschaffen.raumGroesse)
            {
                //neuer vektor = entgegengesetzte richtug und engegensetzte rotation und zufällige Geschwindigkeit
                Vector3 richtung = Vector3.zero - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(richtung), rotGeschw * Time.deltaTime);
                geschwindigkeit = Random.Range(15f, 30f);

            }
            else 
            {
                //ansonsten werden die regeln angewandt
                //sie werden nur etwa 1 in 4 frames berechnet um die perfomance zu steigern
                if (Random.Range(0, 3) < 1)
                {
                    Regeln();
                }

            }


        }
        //boids bewegen sich forwärts; die geschwindigkeit errechnet sich aus einem Basiswert * geschwindigkeit
        transform.Translate(0, 0, Time.deltaTime * geschwindigkeit);

       
    }

    private void Start()
    {
        player = BoidsErschaffen.pl;
    }

    void Regeln()
    {
        //boids array vom boidsErschaffen script geholt
        List<GameObject> boidGruppe;
        boidGruppe = BoidsErschaffen.alleBoids;

        List<GameObject> haiGruppe; 
        haiGruppe = BoidsErschaffen.alleHaie;

        //Mitte der Gruppe
        Vector3 gruppeMitte = Vector3.zero;
        //wektor um gruppe zu vermeiden
        Vector3 gruppeVermeiden = Vector3.zero;
        //geschwindigkeit der Gruppe 
        float gruppeGeschw = 0f;


        //distantz variable
        float distanz;
        //groesse der Gruppe
        int gruppeGroesse = 0;

        //für jeden einzelnen boid
        foreach (GameObject boid in boidGruppe)
        {
        	//distanz = distanz zwischen nachbar-boid und dir 
        	distanz = Vector3.Distance(boid.transform.position, this.transform.position);
        	//wenn es nicht du selber ist und die distanz kleiner ist als boidBist
             if (boid != this.gameObject &&  distanz <= boidDist && distanz >= minDist){
             	 //gruppenmitte neu festgelegt
                    gruppeMitte += boid.transform.position;
                    //du wirst zur gruppenGröße gezählt
                    gruppeGroesse++;
                    BoidKI gruppe = boid.GetComponent<BoidKI>();
                    gruppeGeschw = gruppeGeschw + gruppe.geschwindigkeit;
                //wenn sie sich zu nahe kommen
            	} else if(boid != this.gameObject && distanz < minDist){

            		//neuer vektor entgegengesetzt zum zu nahen boid
                        gruppeVermeiden += (this.transform.position - boid.transform.position);
            	}
        }
  
        //wenn in der Gruppe mehr als 0 mitglieder sind
        if (gruppeGroesse > 0)
        {
            //+ (ziel - this.transform.position)
            gruppeMitte = gruppeMitte / gruppeGroesse + (ziel - this.transform.position);
			geschwindigkeit = gruppeGeschw / gruppeGroesse;

            Vector3 richtung = (gruppeMitte + gruppeVermeiden) - transform.position;
            if (richtung != Vector3.zero)
            {

              transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(richtung), rotGeschw * Time.deltaTime);

            }
            //Hai AI           
            if (Vector3.Distance(this.transform.position, player.transform.position) <= 50f)
            {

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-(player.transform.position - transform.position)), rotGeschw * Time.deltaTime);
                geschwindigkeit = 60f;
            }
            else {

                geschwindigkeit = 30f; 
            
            }

            if (Vector3.Distance(this.transform.position, player.transform.position) <= 4f){

                        Player.hunger += Random.Range(10,20);
                        BoidsErschaffen.remBoids = 1;  
                    
                  
            }
            if (Vector3.Distance(ziel, player.transform.position) <= 150f) { 
                
                
               ziel = new Vector3(Random.Range(-raumGroesse, raumGroesse), Random.Range(10, 100), Random.Range(-raumGroesse, raumGroesse));


            }

               

           

        }


        


    }
}
