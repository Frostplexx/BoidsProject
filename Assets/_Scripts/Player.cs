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

public class Player : MonoBehaviour
{

    public float sens = 1f; 
    private float distance = 40f;

    public static float speed = 10f;
    public static int dashMultiplier = 2;

    private float dashspeed = speed * dashMultiplier;

    public bool canDash = true; 

    private float mouseX;
    private float mouseY;

    public Rigidbody player;
    Transform camTransform;
    Camera cam;

    public static int dashdur = 10; 
    private int dash = dashdur;

    // import sliders script functions
    public Sliders sliders;

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

    CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;

    public DepthTextureMode depthTextureMode { get; internal set; }

    public void Start()
    {
        // Damit der Cursor nicht sichtbar ist und sich nicht bewegt
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        camTransform = transform;
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

        // slider test
        if (Input.GetKey(KeyCode.K))
        {
            sliders.SetHealth(45);
        }
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
            player.AddForce(transform.forward.x * speed, 0, transform.forward.z * speed); 
        }
        if (Input.GetKey(KeyCode.S))
        {
            //player.transform.Translate(-transform.forward.x * speed, 0, -transform.forward.z * speed);
            player.AddForce(-transform.forward.x * speed, 0, -transform.forward.z * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //player.transform.Translate(-transform.forward.z * speed, 0, 0);
            player.AddForce(-transform.forward.z * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //player.transform.Translate(transform.forward.z * speed, 0, 0 );
            player.AddForce(transform.forward.z * speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //player.transform.Translate(0, transform.forward.y * speed, 0);
            player.AddForce(0, transform.forward.y * speed * 2, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //player.transform.Translate(0, -transform.forward.y * speed, 0);
            player.AddForce(0, -transform.forward.y * speed * 2, 0);
        }

        // Dash :)
        if (Input.GetKey(KeyCode.LeftShift) && canDash == true)
        {
            if (dash >= 0)
            {
                speed = dashspeed;
                dash--;
            }
            else
            {
                speed = 10f;
            }


        }
        else if (dash < dashdur && !Input.GetKey(KeyCode.LeftShift))
        {
            canDash = false;
            speed = 10f;
            dash = (int) Mathf.Floor(Time.time * 10) * 1;
                dash++;
        }
        else if (dash == dashdur) {
            canDash = true;
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

}