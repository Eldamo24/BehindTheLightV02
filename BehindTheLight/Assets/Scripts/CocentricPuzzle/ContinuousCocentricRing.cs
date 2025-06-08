using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class ContinuousCocentricRing : MonoBehaviour, IInteractable, ICocentricRing
{
    [SerializeField] private float angleStep = 6f;        
    [SerializeField] private float correctAngle = 0f;     

    [SerializeField] private float rotationSpeed = 15f;    

    [SerializeField] private Transform pivotPoint;
    [SerializeField] private CocentricController controller;
    [SerializeField] private string onInteractMsg;

    private Renderer rend;
    private bool isCorrect;

    private bool isDragging = false;

    public string OnInteractMsg => onInteractMsg;
    public float CorrectAngle => correctAngle;
    public float CurrentAngleY => pivotPoint == null ? 0f : pivotPoint.localEulerAngles.y;
    public bool IsCorrect => isCorrect;

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        if (pivotPoint == null)
        {
            pivotPoint = transform.parent;
            if (pivotPoint == null)
                Debug.LogError($"{name}: no encontré pivotPoint ni parent para usar.");
            else
                Debug.LogWarning($"{name}: pivotPoint no asignado, usando parent '{pivotPoint.name}'.");
        }

        if (controller == null)
            Debug.LogError($"{name}: asigna CocentricController en el Inspector.");

        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isDragging)
        {
            if (Input.GetKey(KeyCode.E))
            {
                float delta = -rotationSpeed * Time.deltaTime;
                pivotPoint.Rotate(0f, delta, 0f, Space.Self);
            }
            else
            {
                FinishRotation();
            }
        }
    }

    public void OnInteract()
    {
        if (controller == null || controller.IsSolved)
            return;
        isDragging = true;
    }

    private void FinishRotation()
    {
        isDragging = false;

        float rawAngle = pivotPoint.localEulerAngles.y;
        float snappedAngle = Mathf.Round(rawAngle / angleStep) * angleStep;
        pivotPoint.localEulerAngles = new Vector3(
            pivotPoint.localEulerAngles.x,
            snappedAngle,
            pivotPoint.localEulerAngles.z
        );

        float target = correctAngle;
        float diff = Mathf.Abs(Mathf.DeltaAngle(snappedAngle, target));

        if (Mathf.Approximately(target, 360f))
        {
            float diff0 = Mathf.Abs(Mathf.DeltaAngle(snappedAngle, 0f));
            if (diff0 < 0.1f)
                isCorrect = true;
            else
                isCorrect = diff < 0.1f;
        }
        else if (Mathf.Approximately(target, 0f))
        {
            float diff0 = Mathf.Abs(Mathf.DeltaAngle(snappedAngle, 0f));
            isCorrect = diff0 < 0.1f;
        }
        else
        {
            isCorrect = diff < 0.1f;
        }

        controller.CheckSolution();
    }

    public void PaintSolved(Color solvedColor)
    {
        rend.material.color = solvedColor;
    }
}
