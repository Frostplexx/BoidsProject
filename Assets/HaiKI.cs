using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaiKI : MonoBehaviour
{

	 float geschwindigkeit = 15f;

	 float rotGeschw = 1f;
    // Update is called once per frame
	 float chase = 0.03f;
    void Update()
    {
        //wenn die boids zu weit vom zentrum entfernt sind (raumGroesse)
    	if(Vector3.Distance(transform.position, Vector3.zero) >=BoidsErschaffen.raumGroesse){
    		//neuer vektor = entgegengesetzte richtug und engegensetzte rotation und zufällige Geschwindigkeit
    		Vector3 richtung = Vector3.zero - transform.position; 
    		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(richtung), rotGeschw * Time.deltaTime);
    		geschwindigkeit = Random.Range(30f,60f);

    	} else{ 
    		//ansonsten werden die regeln angewandt
    		//sie werden nur etwa 1 in 4 frames berechnet um die perfomance zu steigern
    		if(Random.Range(0,10) < 1){
    			HaiRegel();
        	}
        }
        //boids bewegen sich forwärts
        transform.Translate(0, 0, Time.deltaTime * geschwindigkeit);

    }

    void HaiRegel(){

        GameObject[] haiGruppe; 
        haiGruppe = BoidsErschaffen.alleHaie;

        GameObject[] boidGruppe;
        boidGruppe = BoidsErschaffen.alleBoids;

        foreach(GameObject boid in boidGruppe){

        	foreach(GameObject hai in haiGruppe){

        		if(Vector3.Distance(this.transform.position, boid.transform.position) <= 100f){
  
        			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(boid.transform.position - transform.position), chase * Time.deltaTime);
        			
        		}

        	}
        }
  



    }
}
