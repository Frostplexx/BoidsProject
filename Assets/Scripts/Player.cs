using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float sens = 1f;
    private float distance = 40f;

    public static int standartSpeed = 10;
    public static int dashMultiplier = 10;
    private int dashSpeed = standartSpeed * dashMultiplier;
    public float speed;

    // Death Stuff here
    public GameObject deathUI;

    //cooldownTime ändern um den cooldown für den dash zu ändern


    private float mouseX;
    private float mouseY;

    public Rigidbody player;
    public static Rigidbody pl;
    Transform camTransform;




    // Key stuff here

    public string keyText; 

    // sliders

    public UnityEngine.UI.Slider healthSlider;
    public UnityEngine.UI.Slider staminaSlider;
    public UnityEngine.UI.Slider hungerSlider;
    public UnityEngine.UI.Slider dashCooldownSlider;

    // health stuff here


    public float health = 100f;
    float lastHealth;
    int regenCounter; 

    // stamina stuff here

    float stamina = 100f;
    int dashcooldown;
   

    // hunger stuff here

    public static float hunger = 100f;

    public DepthTextureMode depthTextureMode { get; internal set; }


    public void Start()
    {
        // Damit der Cursor nicht sichtbar ist und sich nicht bewegt
        Time.timeScale = 1;
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        camTransform = transform;

        // You know what this does
        deathUI.SetActive(false);
        pl = player; 

    }

    public void Update()
    {
        // Mausposition definieren und Y Achse bei fast 90 Grad sperren um eine Komplette Umdrehung der Y Achse zu verhindern
        mouseX += Input.GetAxis("Mouse X") * sens;
        mouseY += -Input.GetAxis("Mouse Y") * sens;
        mouseY = Mathf.Clamp(mouseY, -89.9f, 89.9f);

        // MouseScrollDelta ist scrollwheel. Aendert Distanz von Kamera zu player relativ zu scrollen. ScrollUP = naeher ScrollDOWN = weiter weg
        if (Input.mouseScrollDelta.y != 0)
        {
            distance = distance - Input.mouseScrollDelta.y;
        }

        // Locks Camera between 40 and 500
        distance = Mathf.Clamp(distance, 40, 100);

        //player rotiert sich immer von der Kamera weg
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            player.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z)), Quaternion.LookRotation(Vector3.zero), 0);
        }

        SetHealth(health);
        SetStamina(stamina);
        SetHunger(hunger);

        if (hunger > 100)
        {

            hunger = 100;
        }

        // Set dashcooldown if 
        if (Input.GetKeyUp(KeyCode.LeftShift) && !(dashcooldown > 0))
        {
            dashcooldown = 250;
        }

        // Die! :)
        if(health <= 0)
        {
        //    Time.timeScale = 0;
        //    UnityEngine.Cursor.visible = true;
        //    UnityEngine.Cursor.lockState = CursorLockMode.None;
        //    deathUI.SetActive(true);

        }
    }

    public void LateUpdate()
    {
        // Camera hinter den Player setzen
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);
        camTransform.position = player.position + rotation * dir;

        camTransform.transform.LookAt(player.transform.position);


      //  Vector3 targetDelta = player.position - camTransform.transform.position;

        //get the angle between transform.forward and target delta
      //  float angleDiff = Vector3.Angle(player.transform.forward, targetDelta);

        // get its cross product, which is the axis of rotation to
        // get from one vector to the other
    //   Vector3 cross = Vector3.Cross(player.transform.forward, targetDelta);

        // apply torque along that axis according to the magnitude of the angle.
     //   player.AddTorque(cross * angleDiff * 1f);

    }

    public void FixedUpdate()
    {
        // Health regen
        if(health < lastHealth)
        {
            regenCounter = 100; 
        }
        
        if (regenCounter > 0)
        {
            regenCounter--;
        }


        lastHealth = health; 
        


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
        if (dashcooldown <= 0 && stamina > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            player.transform.rotation = Quaternion.Slerp(Quaternion.LookRotation(new Vector3(transform.forward.x, transform.forward.y, transform.forward.z)), Quaternion.LookRotation(Vector3.zero), 0);
            speed = dashSpeed;
            stamina--;
            

        }
        else
        {
            speed = standartSpeed;

            if (dashcooldown > 0)
            {
                dashcooldown--;
                SetDashCooldown(dashcooldown);
               
            }
            if (stamina < 100)
            {
                
                stamina += 0.1f;

            }
        }
        


        // Lose hunger, if not hunger lose health
        if(hunger > 0)
        {
            hunger -= 0.05f;
            if (health < 100) {

                health += 0.5f; 
            }
        } else if ( hunger <= 0)
        {
            health -= 0.1f; 
        }

       
    }

    public static void takeDmg(float damage)
    {
        damage = damage;
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }

    public void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }
    public void SetDashCooldown(int dashCooldown)
    {
        dashCooldownSlider.value = dashCooldown;
    }

    public void SetHunger(float hunger)
    {
        hungerSlider.value = hunger;
    }

 
    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
 

   
}