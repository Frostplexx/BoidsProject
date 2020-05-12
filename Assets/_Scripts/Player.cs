﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using TreeEditor;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    
   // private float sens = 250f;
    private float distance = 40f;

    public static float speed = 1f;
    public static int dashMultiplier = 2;

    private float dashspeed = speed * dashMultiplier;

    public bool canDash = true; 

    private float mouseX;
    private float mouseY;

    public Transform player;
    Transform camTransform;
    Camera cam;

    public static int dashdur = 10; 
    private int dash = dashdur; 

    CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;

    public DepthTextureMode depthTextureMode { get; internal set; }

    void Start()
    {
        // Damit der Cursor nicht sichtbar ist und sich nicht bewegt
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        camTransform = transform;
    }

    void Update()
    {
        // Mausposition definieren und Y Achse bei fast 90 Grad sperren um eine Komplette Umdrehung der Y Achse zu verhindern
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, -89, 89);
        
        // MouseScrollDelta ist scrollwheel. Aendert Distanz von Kamera zu player relativ zu scrollen. ScrollUP = naeher ScrollDOWN = weiter weg
        if(Input.mouseScrollDelta.y != 0){
            distance = distance - Input.mouseScrollDelta.y;
        }

        // Locks Camera between 40 and 500
        distance = Mathf.Clamp(distance, 40, 500);

        //player rotiert sich immer von der Kamera weg
        player.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z)), Quaternion.LookRotation(Vector3.zero), 0.1f); 




    }

    void LateUpdate()
    {
        // Camera hinter den Player (unsichtbares Objekt, dem die Boids folgen) setzen
        Vector3 dir = new Vector3(0, 0, -distance);
           Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
           camTransform.position = player.position + rotation * dir;

           // Camera den Player anvisieren lassen
           camTransform.LookAt(player.position);

    }

    private void FixedUpdate()
    {
        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(transform.forward.x * speed, 0, transform.forward.z * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(-transform.forward.x * speed, 0, -transform.forward.z * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(-transform.forward.z * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(transform.forward.z * speed, 0, 0 );
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            player.transform.Translate(0, transform.forward.y * speed, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            player.transform.Translate(0, -transform.forward.y * speed, 0);
        }

        //Dash :)

        if (Input.GetKey(KeyCode.LeftShift) && canDash == true)
        {
            if (dash >= 0)
            {
                speed = dashspeed;
                dash--;
            }
            else
            {

                speed = 1f;
            }


        }
        else if (dash < dashdur && !Input.GetKey(KeyCode.LeftShift))
        {
            canDash = false;
            speed = 1f;
            dash = (int) Mathf.Floor(Time.time * 10) * 1;
                dash++;
            

        }
        else if (dash == dashdur) {

            canDash = true;
        
        }


    }
}