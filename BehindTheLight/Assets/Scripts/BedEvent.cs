using System.Collections.Generic;
using UnityEngine;

public class BedEvent : MonoBehaviour, IInteractable
{
    [SerializeField] private string onInteractMsg;
    [SerializeField] private List<GameObject> lanternObjects;
    [SerializeField] private List<GameObject> objectsToActive;
    [SerializeField] private List<GameObject> puzzleObjects;

    public string OnInteractMsg => onInteractMsg;

    public void OnInteract()
    {
        GameManager.instance.StartFadeIn();
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
        Camera.main.GetComponent<CameraController>().enabled = false;
        foreach (var obj in lanternObjects)
            obj.SetActive(false);
        Invoke(nameof(FadeOut), 3f);
    }

    private void FadeOut()
    {
        foreach (var obj in objectsToActive)
            obj.SetActive(true);

        foreach (var p in puzzleObjects)
            p.SetActive(true);

        foreach (var door in FindObjectsOfType<Door>())
            door.ResetDoor();

        GameManager.instance.StartFadeOut();
        MissionManager.instance.CompleteMission();

        var player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = true;

        Destroy(this);
    }
}
