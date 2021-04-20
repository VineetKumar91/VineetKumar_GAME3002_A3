using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownZone_Script : MonoBehaviour
{
    [SerializeField] private float m_fSlowFactor;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            GameManager.DisplayMessage("In slow-zone. Be careful!!!");
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Keep on slowing the velocity of the character continuously
            collider.gameObject.GetComponent<Rigidbody>().velocity *= m_fSlowFactor;
        }
    }
}
