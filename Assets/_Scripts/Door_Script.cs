using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Script : MonoBehaviour
{
    // Light, Collider that blocks, Hinge that rotates and a Door Transform for rotation reset
    [SerializeField] private Light LightOnDoor;
    [SerializeField] private Collider DoorBlocker;
    [SerializeField] private Collider DoorTrigger;
    [SerializeField] private HingeJoint DoorHingeForOpening;
    [SerializeField] private Transform DoorTransform;

    // Update is called once per frame
    void Update()
    {
        // In case reset call is received, reset the door
        if (GameManager.m_bGameManagerResetCall)
        {
            Reset();
        }
    }

    // On trigger enter of the collider - do following
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Player has Key
            if (Player_Script.m_bKeyInPossession)
            {
                GameManager.DisplayMessage("Press E to open the door");
                LightOnDoor.color = Color.green;
            }
            else    // Player does not have Key
            {
                LightOnDoor.color = new Color(1f,0.42f,0.42f);
                GameManager.DisplayMessage("Activate key platform first");
            }
        }
    }

    // Player still present over there and trying to activate key
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Yes, pressing button and has key -> open the door
            if (Input.GetKey(KeyCode.E) && Player_Script.m_bKeyInPossession)
            {
                // Remove blocker, enable hinge motor and remove the present key from the player
                DoorBlocker.enabled = false;
                DoorHingeForOpening.useMotor = true;
                Player_Script.m_bKeyInPossession = false;
                DoorTrigger.enabled = false;
            }
        }
    }

    // On exiting, reset the color of light to white
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            LightOnDoor.color = new Color(1f, 1f, 1f);
        }
    }


    // Door resets when player dies
    private void Reset()
    {
        DoorTrigger.enabled = true;
        DoorBlocker.enabled = true;
        DoorHingeForOpening.useMotor = false;
        DoorTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
}
