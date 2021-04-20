using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player_Script : MonoBehaviour
{

    // Keycode inputs for character's movement - walk front and back, and jump
    [Header("Keyboard Input & Movement")]
    [SerializeField] private KeyCode MoveForward;
    [SerializeField] private KeyCode MoveBackward;
    [SerializeField] private KeyCode Jump;
    [SerializeField] private float MovementForce;
    [SerializeField] public float MaxVelocity;
    [SerializeField] private float JumpForce;
    [SerializeField] private float FallMultiplier;
    [SerializeField] private LayerMask LayerMaskForGround;
    private float m_fSqrMaxVelocity;
    private bool m_bRotateLeft = false;

    // Rigidbody for physics
    private Rigidbody m_rb;

    // Capsule Collider component reference
    private CapsuleCollider m_capsuleCollider;

    // Animator to have control of animation
    [Header("Animation Stuff")]
    [SerializeField] private Animator CharAnimator;

    // For Reset
    [Header("Reset Player")] 
    [SerializeField] private Vector3 m_vResetPosition;

    // For Key Holder
    [Header("Key Possession")]
    public static bool m_bKeyInPossession = false;
    [SerializeField]
    private GameObject Key;

    // Icey modification
    private bool m_bInIcey = false;

    // Final Trophy object for fixing a bug that was encoutnered
    [Header("Trophy")] 
    [SerializeField] 
    private GameObject WinningTrophy;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = m_vResetPosition;
        m_rb = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();
        if (m_rb == null)       // Just in case
        {
            Debug.LogWarning("Rigidbody not found.");
        }
        Key.SetActive(false);
        m_bKeyInPossession = false;
    }

    void Awake()
    {
        // Performance benefits
        m_fSqrMaxVelocity = MaxVelocity * MaxVelocity;
    }
    
    // Physics related calculations in FixedUpdate
    void FixedUpdate()
    {
        // Clamp the velocity
        ClampVelocity();

        // Optimize fall for better UX
        JumpOptimization();

        // Move
        MoveCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        // Jump
        if (Input.GetKeyDown(Jump) && IsGrounded())
        {
            JumpCharacter();
            CharAnimator.SetTrigger("Trigger_Jump");
        }

        // Set velocity to 0, as soon as player releases the movement button for
        // .. better control as it's a challenging platformer
        if (!Input.GetKey(MoveForward) && !m_bRotateLeft && !m_bInIcey)
        {
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
        }

        if (!Input.GetKey(MoveBackward) && m_bRotateLeft && !m_bInIcey)
        {
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
        }

        // Camera lag bool for stating that player is moving by jumping
        if (!IsGrounded())
        {
            Camera_Script.m_bIsJumping = true;
        }
        else
        {
            Camera_Script.m_bIsJumping = false;
        }

        // Camera lag bool for stating that player is moving by running
        if (Mathf.Abs(m_rb.velocity.x) > 2)
        {
            Camera_Script.m_bIsPlayerMoving = true;
        }
        else
        {
            Camera_Script.m_bIsPlayerMoving = false;
        }

        // rotate the character towards left/right as per the direction she is moving forward, smoothly.
        if (m_bRotateLeft && transform.rotation != Quaternion.Euler(0, -180, 0))
        {
            // Flip to the left side
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -180, 0), 10);
        }
        else if(!m_bRotateLeft && transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            // Flip to the right side
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 10);
        }

        // If Key is in possession of the player, show that is active by rotation
        if (Key.activeSelf)
        {
            Key.transform.Rotate(Vector3.up, 3);
        }

        // If Key not in possession anymore, deactivate it
        if (!m_bKeyInPossession)
        {
            Key.SetActive(false);
        }

        // If timer gets over, reset call is initiated from GameManager, reset player immediately
        if (GameManager.m_bGameManagerTimeOver)
        {
            Reset();
            GameManager.m_bGameManagerTimeOver = false;
        }

        // R button to initiate Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        // ESC button to Quit Game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Movement
    private void MoveCharacter()
    {
        if (Input.GetKey(MoveForward))
        {
            // Add force on right vector - clamp values later if necessary
            m_rb.AddForce(Vector3.right * MovementForce,ForceMode.Force);
            m_bRotateLeft = false;
        }
        else if (Input.GetKey(MoveBackward))
        {
            // Add force on left vector and flip - clamp values later if necessary
            m_rb.AddForce(Vector3.left * MovementForce, ForceMode.Force);
            m_bRotateLeft = true;
        }

        // Update the animator's Speed Parameter for correct animation
        CharAnimator.SetFloat("Speed", Mathf.Abs(m_rb.velocity.x));
    }

    // Jump
    private void JumpCharacter()
    {
        // Add force to up vector - keydown prevents repeated force implementation
        // Later, check if grounded and then only attempt jump
        m_rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    // Clamp Velocity
    private void ClampVelocity()
    {
        Vector3 velocity = m_rb.velocity;

        // Comparing the sqrS. of magnitude of current velocity and max velocity for performance optimization
        if (velocity.sqrMagnitude > m_fSqrMaxVelocity)
        {
            // set it to max without affecting y velocity
            m_rb.velocity = new Vector3(
                    velocity.normalized.x * MaxVelocity, 
                    m_rb.velocity.y, 
                    velocity.normalized.z * MaxVelocity);
        }
    }

    // Check for ground collision to permit jumping
    private bool IsGrounded()
    {
        // Using Capsule cast to check if colliding with ground
        Vector3 centerOfSphere1 = transform.position + Vector3.up * (m_capsuleCollider.radius + Physics.defaultContactOffset);
        Vector3 centerOfSphere2 = transform.position + Vector3.up * (m_capsuleCollider.height - m_capsuleCollider.radius + Physics.defaultContactOffset);

        if (Physics.CapsuleCast(centerOfSphere1, centerOfSphere2, m_capsuleCollider.radius - Physics.defaultContactOffset, Vector3.down, 0.2f, LayerMaskForGround))
        {
            return true;
        }
        return false;
    }

    // Fall multiplier to tweak gravity
    private void JumpOptimization()
    {
        // Fall Multiplier implementation for better falling experience by tweaking gravity during fall
        if (m_rb.velocity.y < 0)
        {
            m_rb.AddForce(Physics.gravity * (FallMultiplier - 1), ForceMode.Acceleration);
        }
    }


    // Reset player location
    private void Reset()
    {
        GameManager.m_bGameManagerResetCall = true;
        m_rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = m_vResetPosition;
        Key.SetActive(false);
        m_bKeyInPossession = false;
        m_bInIcey = false;
        MaxVelocity = 10f;

        if (!WinningTrophy.activeSelf)
        {
            WinningTrophy.SetActive(true);
        }
    }

    // On Collision actions
    private void OnCollisionEnter(Collision collision)
    {
        // Collided with enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Reset();
        }

        // Collision with unstable platform
        if (collision.gameObject.CompareTag("UnstablePlatform2"))
        {
            GameManager.DisplayMessage("Again, keep patience, this will take you to above door.");
        }

        // Collision with a keyholder platform
        if (collision.gameObject.CompareTag("KeyHolder"))
        {
            if (!m_bKeyInPossession)
            {
                m_bKeyInPossession = true;
            }
            Key.SetActive(true);
        }

        // If colliding with icey platform, increase max velocity for better speed
        if (collision.gameObject.CompareTag("Icey"))
        {
            MaxVelocity = 15f;
            m_bInIcey = true;
        }

        // In case colliding with a bouncy, gives hint to the player
        if (collision.gameObject.CompareTag("Bouncy"))
        {
            GameManager.DisplayMessage("Sometimes, right can be wrong, choose your directions wisely (UP).");
        }

        // Winning Message
        if (collision.gameObject.CompareTag("WinningCup"))
        {
            GameManager.DisplayMessage("You Have Won!!!");
            collision.gameObject.SetActive(false);
            StartCoroutine(ChangeTheScene());
        }
    }

    // Load the End Scene
    IEnumerator ChangeTheScene()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        StopAllCoroutines();
    }
}

