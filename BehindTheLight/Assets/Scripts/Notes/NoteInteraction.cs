using TMPro;
using UnityEngine;

public class NoteInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string onInteractMsg;
    [SerializeField] private string noteMSG;
    [SerializeField] private GameObject notePanel;
    [SerializeField] private GameObject cameraNotes;
    [SerializeField] private TMP_Text textNotes;
    [SerializeField] private float offset;
    [SerializeField] private CameraController principalCamera;
    [SerializeField] private PlayerMovement pMovement;
    private Quaternion startRotation;
    [SerializeField] private float rotationSpeed;
    private bool isInteracting;
    public string OnInteractMsg => onInteractMsg;

    private void Start()
    {
        isInteracting = false;
        startRotation = transform.rotation;
    }

    private void Update()
    {
        if (isInteracting)
        {
            NoteRotation();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DesactivateInteraction();
            }
        }
    }

    public void OnInteract()
    {
        cameraNotes.transform.position = transform.position + new Vector3(0, offset, 0);
        cameraNotes.SetActive(true);
        textNotes.text = noteMSG;
        notePanel.SetActive(true);
        isInteracting = true;
        principalCamera.enabled = false;
        pMovement.enabled = false;
    }

    private void DesactivateInteraction()
    {
        cameraNotes.SetActive(false);
        textNotes.text = "";
        notePanel.SetActive(false);
        isInteracting = false;
        principalCamera.enabled = true;
        pMovement.enabled = true;
        transform.rotation = startRotation;
    }

    private void NoteRotation()
    {
        float rotationX = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.deltaTime;
        float rotationY = Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationX, rotationY, 0f);
    }

}
