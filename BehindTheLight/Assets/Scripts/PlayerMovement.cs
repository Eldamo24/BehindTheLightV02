using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float movSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private LightController lightController;

    private Vector3 direction;

    Animator animator;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!GameManager.instance.IsPaused)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0;
            right.Normalize();
            direction = forward * z + right * x;

            animator.SetFloat("xMov", x);
            animator.SetFloat("zMov", z);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                lightController.LightsOn = !lightController.LightsOn;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                MissionManager.instance.CheckMission();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.SetPaused();
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.IsPaused)
        {
            if (direction.sqrMagnitude != 0)
            {
                Movement(direction);
            }
            Rotation();
        }
    }


    /// <summary>
    /// Function that manages simple player character movement
    /// </summary>
    /// <param name="dir">Takes direction</param>
    private void Movement(Vector3 dir)
    {
        rb.MovePosition(transform.position + movSpeed * Time.fixedDeltaTime * dir);
    }

    /// <summary>
    /// Function that manages simple player character rotation using the camera.
    /// </summary>
    private void Rotation()
    {
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0;
        lookDirection.Normalize();

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothedRotation);
        }
    }
}
