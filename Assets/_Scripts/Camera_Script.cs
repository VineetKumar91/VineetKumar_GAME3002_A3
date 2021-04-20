using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Camera_Script : MonoBehaviour
{
    // Player's transform component
    [SerializeField] private Transform player;

    // Camera Lag Component
    [SerializeField] private float m_fLagValueForIdle;
    [SerializeField] private float m_fLagValueForMovement;
    [SerializeField] private float m_fLagValueForJump;

    // Spring arm Position for side view, an offset from player's location to the desired location
    [SerializeField] private Vector3 CameraOffset;

    // A camera lag style inspired from GTA5's 3rd Person Camera lag when driving a car
    // .. similar to Unreal's camera lag
    [SerializeField] private Vector3 GTACameraOffset;

    // Spring arm Position for jumping in case of better view angle of the entire map
    [SerializeField] private Vector3 JumpingCameraOffset;

    // Initialize velocity
    private Vector3 velocity = Vector3.zero;

    // Static bool for access and reset
    public static bool m_bIsPlayerMoving = false;
    public static bool m_bIsJumping = false;
    public static bool m_bIsFinale = false;

    // Late Update, an idle way to perform 
    void LateUpdate()
    {
        if (m_bIsJumping || m_bIsFinale)
        {
            // Get player position, and add the offset to push the camera to exact viewing position of Jumping
            Vector3 OffsetPosition = player.position + JumpingCameraOffset;
            Vector3 CameraLagPosition = Vector3.SmoothDamp(transform.position, OffsetPosition, ref velocity, m_fLagValueForJump); // 0.2 ideal value
            transform.position = CameraLagPosition;
        }
        else if (m_bIsPlayerMoving)
        {
            // Get player position, and add the offset to push the camera to exact viewing position of GTA
            Vector3 OffsetPosition = player.position + GTACameraOffset;
            Vector3 CameraLagPosition = Vector3.SmoothDamp(transform.position, OffsetPosition, ref velocity, m_fLagValueForMovement);   // 0.1 ideal value
            transform.position = CameraLagPosition;
        }
        else
        {
            // Get player position, and add the offset to push the camera to exact viewing position, because she is idle now
            Vector3 OffsetPosition = player.position + CameraOffset;
            Vector3 CameraLagPosition = Vector3.SmoothDamp(transform.position, OffsetPosition, ref velocity, m_fLagValueForIdle);       // 0.4 ideal value
            transform.position = CameraLagPosition;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_bIsPlayerMoving = false;
        m_bIsJumping = false;
        m_bIsFinale = false;
    }

    void Awake()
    {
        // When started, set the camera to player at standard offset for the spring arm
        transform.position = player.position + CameraOffset;
    }
}
