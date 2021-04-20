using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class TorqueTrap_Script : MonoBehaviour
{
    // Torque formula
    // T = F cross product (p - x)
    // Where F is the force applied.
    // Where p is the point where force is applied.
    // Where x is the center of mass of the object.
    [Header("Torque Formulae")]
    [SerializeField] private Vector3 m_vForce = Vector3.zero;
    [SerializeField] private Vector3 m_vForcePoint = Vector3.zero;
    [SerializeField] private Vector3 m_vCenterOfMass = Vector3.zero;

    [Header("Rigidbody Values")] 
    [SerializeField] private float m_fMaxAngularVelocity = 5f;

    // Resultant torque
    private Vector3 m_vTorque = Vector3.zero;

    private Rigidbody m_rb = null;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();

        // Set the max angular velocity
        m_rb.maxAngularVelocity = m_fMaxAngularVelocity;
    }

    void FixedUpdate()
    {
        m_vTorque = Vector3.Cross(m_vForce, m_vForcePoint - m_vCenterOfMass);
        m_rb.AddTorque(m_vTorque);
    }
}
