using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    
    private float sens = 250f;
    private float distance = 100f;

    private float mouseX;
    private float mouseY;

    public Transform player;
    public Transform camTransform;
    public Camera cam;


    void Start()
    {
        // Damit der Cursor nicht sichtbar ist und sich nicht bewegt
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
}