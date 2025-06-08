using System.Collections.Generic;
using UnityEngine;

public class BedEvent : MonoBehaviour, IInteractable
{
    [SerializeField] private string onInteractMsg;
    [SerializeField] private List<GameObject> lanternObjects;
    [SerializeField] private List<GameObject> objectsToActive;
    public string OnInteractMsg => onInteractMsg;

    public void OnInteract()
    {
        GameManager.instance.StartFadeIn();
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
        Camera.main.gameObject.GetComponent<CameraController>().enabled = false;
        foreach(var obj in lanternObjects)
        {
            obj.SetActive(false);
        }
        Invoke("FadeOut", 3f);
    }

    private void FadeOut()
    {
        foreach(var obj in objectsToActive)
        {
            obj.SetActive(true);
        }
        GameManager.instance.StartFadeOut();
        MissionManager.instance.CompleteMission();
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = true;
        Camera.main.gameObject.GetComponent<CameraController>().enabled = true;
        Destroy(this);
    }

}
