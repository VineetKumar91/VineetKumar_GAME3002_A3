using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePlatform_Script : MonoBehaviour
{
    // Spring and Damping constants
    [SerializeField]
    private float m_fSpringConstant;
    [SerializeField]
    private float m_fDampingConstant;
    // Resting position
    [SerializeField]
    private Vector3 m_vRestPos;
    [SerializeField]
    private float m_fMass;
    [SerializeField]

    // Platform
    private Rigidbody m_attachedBody = null;

    // Trigger for Hint
    [SerializeField] private Collider m_TriggerHint;

    private Vector3 m_vForce;
    private Vector3 m_vPrevVel;


    // Start is called before the first frame update
    void Start()
    {
        m_fMass = m_attachedBody.mass;
        m_fSpringConstant = CalculateSpringConstant();
    }

    private void FixedUpdate()
    {
        UpdateSpringForce();
    }

    private void Update()
    {
        if (GameManager.m_bGameManagerResetCall)
        {
            m_attachedBody.velocity = Vector3.zero;
        }
    }

    private float CalculateSpringConstant()
    {
        // k = F / dX
        // F = m * a
        // k = m * a / (xf - xi)

        float fDX = (m_vRestPos - m_attachedBody.transform.position).magnitude;

        if (fDX <= 0f)
        {
            return Mathf.Epsilon;
        }

        return (m_fMass * Physics.gravity.y) / (fDX);
    }

    private void UpdateSpringForce()
    {
        // Hooke's Law
        // F = -kx
        // F = -kx -bv
        m_vForce = -m_fSpringConstant * (m_vRestPos - m_attachedBody.transform.position) -
                   m_fDampingConstant * (m_attachedBody.velocity - m_vPrevVel);

        m_attachedBody.AddForce(m_vForce, ForceMode.Acceleration);

        m_vPrevVel = m_attachedBody.velocity;
    }

    // In case player lands on platform and decides to jump forward
    // .. this message should make the player wait for second
    // .. because there is key that is available only by using the stable platform correctly
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !Player_Script.m_bKeyInPossession && !this.gameObject.CompareTag("UnstablePlatform2"))
        {
            GameManager.DisplayMessage("Keep patience, key nearby.");
        }
    }
}
