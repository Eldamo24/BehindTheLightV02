using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private float finalAngle;
    [SerializeField] private float startAngle;
    [SerializeField] private AudioClip clip;
    [SerializeField] private bool canBeOpen = true;

    private float speed = 100f;
    private float targetAngle;
    private Vector3 direction;
    private bool isOpen;
    private Quaternion initialPivotRotation;
    private Transform doorPivot;
    private AudioSource sfxSource;
    private string onInteractMsg;

    public bool doorWasOpen { get; private set; }
    public string OnInteractMsg => onInteractMsg;

    void Start()
    {
        sfxSource = GameObject.Find("SFX").GetComponent<AudioSource>();
        doorPivot = transform.parent;
        initialPivotRotation = doorPivot.localRotation;
        isOpen = false;
        targetAngle = startAngle;
        doorPivot.localEulerAngles = new Vector3(doorPivot.localEulerAngles.x, startAngle, doorPivot.localEulerAngles.z);

        UpdateInteractMsg();
    }

    void Update()
    {
        if (Mathf.Round(doorPivot.localEulerAngles.y) != Mathf.Round(targetAngle))
            doorPivot.Rotate(direction * speed * Time.deltaTime);
    }

    public void OnInteract()
    {
        if (!canBeOpen) return;

        doorWasOpen = true;
        isOpen = !isOpen;

        if (isOpen)
        {
            targetAngle = finalAngle;
            direction = Vector3.up;
        }
        else
        {
            targetAngle = startAngle;
            direction = Vector3.down;
        }

        PlaySfx();
        UpdateInteractMsg();
    }

    private void PlaySfx()
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    private void UpdateInteractMsg()
    {
        onInteractMsg = canBeOpen
            ? (isOpen ? "Close Door" : "Open Door")
            : "Door Locked";
    }

    public void UnlockDoor()
    {
        canBeOpen = true;
        UpdateInteractMsg();
    }

    public void LockDoor()
    {
        canBeOpen = false;
        UpdateInteractMsg();
    }

    public void ResetDoor()
    {
        doorPivot.localRotation = initialPivotRotation;
        isOpen = false;
        doorWasOpen = false;
        canBeOpen = false;
        targetAngle = startAngle;
        UpdateInteractMsg();
    }
}
