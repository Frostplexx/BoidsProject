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

public class Player : MonoBehaviour
{

    public float sens = 1f; 
    private float distance = 40f;

    public static float speed = 10f;

    public static int dashMultiplier = 2;
    private float dashspeed = speed * dashMultiplier;
    
    //dashdur ändern um die dash dauer zu verlängern
    public static int dashdur = 100;
    private int dash = dashdur;

    //cooldownTime ändern um den cooldown für den dash zu ändern
    public float cooldownTime = 5f; 

    public bool canDash = true;

    private bool waitActive; 

    private float mouseX;
    private float mouseY;

    public Rigidbody player;
    Transform camTransform;
    Camera cam;




    // sliders

    public UnityEngine.UI.Slider healthSlider;
    public UnityEngine.UI.Slider staminaSlider;
    public UnityEngine.UI.Slider hungerSlider;

    // health stuff here

    static int maxHealth = 100;
    int health;

    int dmgotTime;
    int dmgotDamage;


    // stamina stuff here

    static int maxStamina = 100;
    int stamina;


    // hunger stuff here

    static int maxHunger = 100;
    int hunger;
    int hungercounter;




    CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;

    public DepthTextureMode depthTextureMode { get; internal set; }


    Sliders sliders;

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

    }

    public void Update()
    {
        // Mausposition definieren und Y Achse bei fast 90 Grad sperren um eine Komplette Umdrehung der Y Achse zu verhindern
        mouseX += Input.GetAxis("Mouse X") * sens;
        mouseY += -Input.GetAxis("Mouse Y") * sens;
        mouseY = Mathf.Clamp(mouseY, -89, 89);
        
        // MouseScrollDelta ist scrollwheel. Aendert Distanz von Kamera zu player relativ zu scrollen. ScrollUP = naeher ScrollDOWN = weiter weg
        if(Input.mouseScrollDelta.y != 0){
            distance = distance - Input.mouseScrollDelta.y;
        }

        // Locks Camera between 40 and 500
        distance = Mathf.Clamp(distance, 40, 500);

        //player rotiert sich immer von der Kamera weg
        player.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z)), Quaternion.LookRotation(Vector3.zero), 0.1f);

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
            //player.transform.Translate(transform.forward.x * speed, 0, transform.forward.z * speed);
            player.AddForce(player.transform.forward.x * speed, 0, player.transform.forward.z * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //player.transform.Translate(-transform.forward.x * speed, 0, -transform.forward.z * speed);
            player.AddForce(-player.transform.forward.x * speed, 0, -player.transform.forward.z * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //player.transform.Translate(-transform.forward.z * speed, 0, 0);
            player.AddForce(-player.transform.forward.z * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //player.transform.Translate(transform.forward.z * speed, 0, 0 );
            player.AddForce(player.transform.forward.z * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //player.transform.Translate(0, transform.forward.y * speed, 0);
            player.AddForce(0, camTransform.transform.forward.y * speed * 2, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //player.transform.Translate(0, -transform.forward.y * speed, 0);
            player.AddForce(0, -camTransform.transform.forward.y * speed * 2, 0);
            player.AddRelativeTorque(new Vector3(0, 100, 0));
        }

        // Dash :)
        if (Input.GetKey(KeyCode.LeftShift) && canDash == true)
        {
            if (dash > 0)
            {
                speed = dashspeed;
                dash--;
                SetStamina(dash);
            }
            else
            {
                speed = 10f;
                StartCoroutine(Wait());
            }


        }
        else if (dash < dashdur && !Input.GetKey(KeyCode.LeftShift) && !waitActive)
        {
            canDash = false;
            speed = 10f;
            dash = dashdur;
            SetStamina(dash);
        }
        else if (dash == dashdur && !waitActive) {
            canDash = true;

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

        // Damage over time
        if(dmgotTime > 0)
        {
            health = health - dmgotDamage;
            dmgotTime--;
        }
    }

    void takeDmg(int damage)
    {
        health = health - damage;
    }


    IEnumerator Wait()
    {
        waitActive = true;
        canDash = false;
        yield return new WaitForSeconds(cooldownTime);
        canDash = true;
        waitActive = false;
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