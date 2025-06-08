using System.Collections;
using UnityEngine;

public class RotateMirror : MonoBehaviour, IInteractable
{
    [SerializeField] private string onInteractMsg;
    [SerializeField] private float rotateAngle;
    private float rotDuration = 0.2f;
    private bool isRotating;
    public string OnInteractMsg => onInteractMsg;

    public void OnInteract()
    {
        if(!isRotating)
            StartCoroutine("SmoothRotate");
    }

    IEnumerator SmoothRotate()
    {
        isRotating = true;
        Quaternion initialRotation = transform.rotation;
        Quaternion endRotation = initialRotation * Quaternion.Euler(0, rotateAngle, 0);
        float elapsedTime = 0f;
        while (elapsedTime < rotDuration) 
        {
            transform.rotation = Quaternion.Lerp(initialRotation, endRotation, elapsedTime/rotDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;
        isRotating = false;
    }
}
