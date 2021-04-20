using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SpeedupZone_Script : MonoBehaviour
{
    [SerializeField] private Player_Script player_Script;
    [SerializeField] private float m_fBoostFactor;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            GameManager.DisplayMessage("Speed-up zone boosts your speed. Beware! Too much speed will kill you.");
            player_Script = collider.gameObject.GetComponent<Player_Script>();

            player_Script.MaxVelocity = 25;
            // Get current Velocity of player
            Vector3 currentVelocity = collider.gameObject.GetComponent<Rigidbody>().velocity;

            // Set any velocity other than x to 0
            currentVelocity = new Vector3(currentVelocity.x, 0f, 0f);

            // Assign the velocity to the player's velocity
            collider.gameObject.GetComponent<Rigidbody>().velocity = currentVelocity;

            // Give player that jolt of force - impulse
            collider.gameObject.GetComponent<Rigidbody>().AddForce(currentVelocity * m_fBoostFactor,ForceMode.Impulse);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            // Reset the max velocity back to original value
            player_Script.MaxVelocity = 10f;
        }
    }


}
