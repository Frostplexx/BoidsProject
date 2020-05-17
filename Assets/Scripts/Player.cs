using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using TreeEditor;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;
using System.Security.AccessControl;

public class Player : MonoBehaviour
{

    public float sens = 1f; 
    private float distance = 40f;

    public static int standartSpeed = 10;
    public static int dashMultiplier = 10;
    private int dashSpeed = standartSpeed * dashMultiplier;
    public float speed; 

    //cooldownTime ändern um den cooldown für den dash zu ändern

    private bool waitActive; 

    private float mouseX;
    private float mouseY;

    public Rigidbody player;
    public static Rigidbody pl;
    Transform camTransform;
    Camera cam;

    // sliders

    public UnityEngine.UI.Slider healthSlider;
    public UnityEngine.UI.Slider staminaSlider;
    public UnityEngine.UI.Slider hungerSlider;

    // health stuff here

    static int maxHealth = 100;
    int health;

    // stamina stuff here

    static int maxStamina = 100;
    int stamina;
    int dashcooldown;
    

    // hunger stuff here

    static int maxHunger = 100;
    public static int hunger;
    int hungercounter;




    CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;

    public DepthTextureMode depthTextureMode { get; internal set; }


    public void Start()
    {
        // Damit der Cursor nicht sichtbar ist und sich nicht bewegt
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        camTransform = transform;

        // import sliders script functions
        //sl = GameObject.FindObjectsOfTypeAll(Function).GetComponent<Sliders>();

        health = 100; 
        stamina = 100;
        hunger = 100;

        SetStamina(100);
        SetHealth(100);
        SetHunger(100);

        pl = player; 

    }

    public void Update()
    {
        // Mausposition definieren und Y Achse bei fast 90 Grad sperren um eine Komplette Umdrehung der Y Achse zu verhindern
        mouseX += Input.GetAxis("Mouse X") * sens;
        mouseY += -Input.GetAxis("Mouse Y") * sens;
        mouseY = Mathf.Clamp(mouseY, -89.9f, 89.9f);
        
        // MouseScrollDelta ist scrollwheel. Aendert Distanz von Kamera zu player relativ zu scrollen. ScrollUP = naeher ScrollDOWN = weiter weg
        if(Input.mouseScrollDelta.y != 0){
            distance = distance - Input.mouseScrollDelta.y;
        }

        // Locks Camera between 40 and 500
        distance = Mathf.Clamp(distance, 40, 100);

        //player rotiert sich immer von der Kamera weg
        player.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z)), Quaternion.LookRotation(Vector3.zero), 0);




        //Update UI bars
        SetHealth(health);
        SetStamina(stamina);
        SetHunger(hunger);
        
       
    }

    public void LateUpdate()
    {
        // Camera hinter den Player setzen
        Vector3 dir = new Vector3(0, 0, -distance);
           Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
           camTransform.position = player.position + rotation * dir;

           // Camera den Player anvisieren lassen
           camTransform.LookAt(player.position);

    }

    public void FixedUpdate()
    {
        
    

        // Movement
        if (Input.GetKey(KeyCode.W))
        {
          
            player.AddRelativeForce(0, 0, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            
            player.AddRelativeForce(0, 0, -speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            
            player.AddRelativeForce(-speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            
            player.AddRelativeForce(speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {

            player.AddRelativeForce(0, -speed, 0);

        }
        if (Input.GetKey(KeyCode.Space))
        {
          player.AddRelativeForce(0, speed, 0);
         

        }

       // Dash
       if(dashcooldown <= 0 && stamina > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            speed = dashSpeed;
            stamina--;

        } else
        {
            speed = standartSpeed;

            if(dashcooldown > 0)
            {
                dashcooldown--;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dashcooldown = 250;
        }

        // Lose hunger, if not hunger lose health (2 values per second) 
        if (hungercounter >= 25 && hunger > 0)
        {
            hunger--;
            hungercounter = 0;
        } else
        {
            hungercounter++;
        }

        if (hunger <= 0 && hungercounter >= 25)
        {
            health--;
            hungercounter = 0;
        } else
        {
            hungercounter++;
        }


    }

    void takeDmg(int damage)
    {
        health = health - damage;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }

    public void SetStamina(int stamina)
    {
        staminaSlider.value = stamina;
    }

    public void SetHunger(int hunger)
    {
        hungerSlider.value = hunger;
    }
}