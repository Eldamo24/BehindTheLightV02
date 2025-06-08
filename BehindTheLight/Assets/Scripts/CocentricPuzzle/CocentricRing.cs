using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CocentricRing : MonoBehaviour, IInteractable, ICocentricRing
{
    [SerializeField] private float angleStep = 30f;
    [SerializeField] private float correctAngle = 0f;

    [SerializeField] private Transform pivotPoint;
    [SerializeField] private CocentricController controller;
    [SerializeField] private string onInteractMsg;

    private Renderer rend;
    private bool isCorrect;

    public string OnInteractMsg => onInteractMsg;

    public float CorrectAngle => correctAngle;

    public float CurrentAngleY
    {
        get
        {
            if (pivotPoint == null) return 0f;
            return pivotPoint.localEulerAngles.y;
        }
    }

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
    }

    public void OnInteract()
    {
        if (controller == null || controller.IsSolved) return;

        pivotPoint.Rotate(0f, -angleStep, 0f, Space.Self);

        float angleY = pivotPoint.localEulerAngles.y;
        float diff = Mathf.Abs(Mathf.DeltaAngle(angleY, correctAngle));
        isCorrect = diff < 0.1f;

        controller.CheckSolution();
    }

    public void PaintSolved(Color solvedColor)
    {
        rend.material.color = solvedColor;
    }
}
