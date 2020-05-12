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
     float boidDist = 20.0f;
	//minimale boid distanz 
    float minDist = 3.0f;

    bool drehen = false; 

    public Vector3 ausweichen; 

    private void OnTriggerEnter(Collider other){
        if(!drehen){
            ausweichen = this.transform.position - other.gameObject.transform.position; 
        }
        drehen = true; 
    }
    private void OnTriggerExit(Collider other){
        drehen = false; 
    }
    void Update()
    {
        if(drehen){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ausweichen - transform.position), rotGeschw * Time.deltaTime);
            geschwindigkeit = Random.Range(15f,30f);
        } else {
    

        if(Vector3.Distance(transform.position, Vector3.zero) >= BoidsErschaffen.raumGroesse){
            //neuer vektor = entgegengesetzte richtug und engegensetzte rotation und zufällige Geschwindigkeit
            Vector3 richtung = Vector3.zero - transform.position; 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(richtung), rotGeschw * Time.deltaTime);
            geschwindigkeit = Random.Range(15f,30f);

        } else{ 
            //ansonsten werden die regeln angewandt
            //sie werden nur etwa 1 in 4 frames berechnet um die perfomance zu steigern
            if(Random.Range(0,4) < 1){
            Regeln();
            }
    
        }
          
            
        }
        //boids bewegen sich forwärts; die geschwindigkeit errechnet sich aus einem Basiswert * geschwindigkeit
        transform.Translate(0, 0, Time.deltaTime * geschwindigkeit);
    }


    void Regeln()
    {
        //boids array vom boidsErschaffen script geholt
        List<GameObject> boidGruppe;
        boidGruppe = BoidsErschaffen.alleBoids;

        List<GameObject> haiGruppe; 
        haiGruppe = BoidsErschaffen.alleHaie;

        //MItte der Gruppe
        Vector3 gruppeMitte = Vector3.zero;
        //wektor um gruppe zu vermeiden
        Vector3 gruppeVermeiden = Vector3.zero;
        //geschwindigkeit der Gruppe 
        float gruppeGeschw = 0f;

        //ziel vektor aus boidsErschaffen geholt
        Vector3 ziel = BoidsErschaffen.ziel;
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

            gruppeMitte = gruppeMitte / gruppeGroesse + (ziel - this.transform.position);

			geschwindigkeit = gruppeGeschw / gruppeGroesse;

            Vector3 richtung = (gruppeMitte + gruppeVermeiden) - transform.position;
            if (richtung != Vector3.zero)
            {

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(richtung), rotGeschw * Time.deltaTime);

            }

                if(Random.Range(0, 5) < 1){
              foreach(GameObject hai in haiGruppe){

                foreach(GameObject boid in boidGruppe){

                    if(Vector3.Distance(this.transform.position, hai.transform.position) <= 20f){
  
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-(hai.transform.position - transform.position)), rotGeschw * Time.deltaTime);
                    
                    }

                }

            }

        }


        }


    }
}
