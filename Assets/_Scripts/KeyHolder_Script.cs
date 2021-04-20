using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder_Script : MonoBehaviour
{
    // Get KeyLight Component of the Keyholder
    [SerializeField] private Light KeyLight;
    [SerializeField] private GameObject SimsDiamond;

    [SerializeField] private float m_fMaxOffsetY = 5f;
    [SerializeField] private float m_fMinOffsetY = -5f;
    [SerializeField] private float m_fSpeed = -0f;

    private float m_fDefaultX = 0f;
    private float m_fDefaultY = 0f;
    private float m_fDefaultZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_fDefaultX = SimsDiamond.transform.position.x;
        m_fDefaultY = SimsDiamond.transform.position.y;
        m_fDefaultZ = SimsDiamond.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset the key light to red color because player has died
        if (GameManager.m_bGameManagerResetCall)
        {
            Reset();
        }

        // PingPong the position of the key indicator to show it as collectible
        SimsDiamond.transform.position = new Vector3(m_fDefaultX, Mathf.PingPong(Time.time * m_fSpeed, m_fMaxOffsetY - m_fMinOffsetY) + m_fMinOffsetY, m_fDefaultZ);
    }

    // Change color of the Keyholder light to green - meaning player has activated key for next door
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // It's active
            KeyLight.color = Color.green;
            SimsDiamond.SetActive(false);
        }
    }

    // Reset
    public void Reset()
    {
        // In case player dies in possession of the key
        KeyLight.color = Color.red;
        SimsDiamond.SetActive(true);
    }
}
