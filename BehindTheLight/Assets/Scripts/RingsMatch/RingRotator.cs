using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRotator : MonoBehaviour, IInteractable
{
    [SerializeField] private PuzzleRingsMatchController controller;
    [SerializeField] private string onInteractMsg = "Press 'E' to rotate";

    public string OnInteractMsg => onInteractMsg;

    public void OnInteract()
    {
        transform.Rotate(0f, 0f, 30f);
        controller.CheckPuzzle();
    }

    public void CheckIfCorrect(float targetAngle)
    {
        float currentZ = transform.localEulerAngles.z;
        float diff = Mathf.Abs(Mathf.DeltaAngle(currentZ, targetAngle));
        bool isCorrect = diff < 0.1f;

        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.color = isCorrect ? Color.green : Color.gray;
        }
    }

}
