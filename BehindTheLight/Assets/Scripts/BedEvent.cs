using System.Collections.Generic;
using UnityEngine;

public class BedEvent : MonoBehaviour, IInteractable
{
    [SerializeField] private string onInteractMsg;

    [Header("Linterna")]
    [Tooltip("Objetos de linterna que queremos desactivar")]
    [SerializeField] private List<GameObject> lanternObjects;

    [Header("Vela")]
    [Tooltip("Prefab de la vela que vamos a instanciar o activar")]
    [SerializeField] private GameObject candlePrefab;
    [Tooltip("Punto donde se instanciará la vela (por ejemplo, mano del jugador)")]
    [SerializeField] private Transform candleSpawnPoint;

    [Header("Puzzles y demás")]
    [SerializeField] private List<GameObject> objectsToActive;
    [SerializeField] private List<GameObject> puzzleObjects;

    public string OnInteractMsg => onInteractMsg;

    public void OnInteract()
    {
        GameManager.instance.StartFadeIn();

        var player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
        Camera.main.GetComponent<CameraController>().enabled = false;

        foreach (var obj in lanternObjects)
            obj.SetActive(false);

        GiveCandleToPlayer();

        Invoke(nameof(FadeOut), 3f);
    }

    private void GiveCandleToPlayer()
    {
        if (candlePrefab == null || candleSpawnPoint == null) return;

        candlePrefab.transform.SetParent(candleSpawnPoint, worldPositionStays: false);
        candlePrefab.transform.localPosition = Vector3.zero;
        candlePrefab.transform.localRotation = Quaternion.identity;
        candlePrefab.SetActive(true);
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

        // Rehabilita controles
        var player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = true;

        Destroy(this);
    }
}
