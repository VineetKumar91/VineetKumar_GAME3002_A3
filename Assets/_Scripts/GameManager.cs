using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Get Player Game Object
    [SerializeField] private GameObject Player;
    [SerializeField] public TextMeshProUGUI MessageTMP;
    [SerializeField] public TextMeshProUGUI TimerTMP;

    // Static bool for reset
    public static bool m_bGameManagerResetCall;
    public static bool m_bGameManagerTimeOver;

    // Message on screen variables
    private static float m_ftempTime;
    private static bool m_bTextExecuteOnce = false;
    private static bool m_bTextShow = false;
    private static string Text = "";

    // Timer variable
    public float m_dCountdownTimer = 120;

    // An execute once bool due to usage of Update function
    private bool m_bExecuteOnce = false;


    // Start is called before the first frame update
    void Start()
    {
        m_bGameManagerResetCall = false;
        m_bGameManagerTimeOver = false;
        m_ftempTime = 0f;
        m_bTextExecuteOnce = false;
        m_bTextShow = false;
        Text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // If Reset call is activated, invoke coroutine for reset, and stop after 2 seconds
        if (m_bGameManagerResetCall && !m_bExecuteOnce)
        {
            m_bExecuteOnce = true;
            StartCoroutine(Reset());
        }

        // Show message on the screen, whatever it is
        if (!m_bTextShow)
        {
            MessageTMP.SetText(Text);
            m_bTextShow = false;
        }

        // Clear Message on the screen after 3 seconds and reset it.
        if (Time.time - m_ftempTime > 3f && !m_bTextExecuteOnce)
        {
            MessageTMP.SetText("");
            Text = "";
            m_bTextExecuteOnce = true;
            m_bTextShow = true;
            m_ftempTime = 0;
        }

        // Timer countdown function
        if (m_dCountdownTimer > 0)
        {
            m_dCountdownTimer -= Time.deltaTime;
            TimerTMP.SetText("Time Remaining: " + ((int)m_dCountdownTimer).ToString());
        }
        else
        {
            StartCoroutine(Reset());
            m_bGameManagerTimeOver = true;
            DisplayMessage("Game Over, Resetting..");
            m_dCountdownTimer = 120;
        }

        // Reset game forcefully
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 122 due to the delay of 2 seconds in co-routine
            m_dCountdownTimer = 122;
        }
    }
    
    // Reset call on hold for all functions that are referring to game manager to check if
    // .. they need to restart in any case and stop the co-routine immediately.
    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2f);
        m_bGameManagerResetCall = false;
        m_bExecuteOnce = false;
        StopCoroutine("Reset");
    }

    // Display Message static function for receiving messages from other classes
    public static void DisplayMessage(string p_sMessageToShow)
    {
        // Time factor
        m_ftempTime = Time.time;
        m_bTextExecuteOnce = false;
        m_bTextShow = false;
        Text = p_sMessageToShow;
    }
    
}
