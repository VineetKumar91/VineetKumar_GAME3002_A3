using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icey_Script : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Just a display message to the user
            GameManager.DisplayMessage("Almost over! Let the character slide on this slope and you can sit back and relax.");
            Camera_Script.m_bIsFinale = true;
        }
    }
}
