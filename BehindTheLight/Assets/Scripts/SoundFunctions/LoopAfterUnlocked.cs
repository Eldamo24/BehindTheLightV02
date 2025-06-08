using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LoopAfterUnlocked : MonoBehaviour, IInteractable
{
    [SerializeField] private Door door;

    [SerializeField] private AudioClip loopClip;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float delaySeconds = 10f;

    [SerializeField] private string onInteractMsg;
    public string OnInteractMsg => onInteractMsg;

    AudioSource source;
    Coroutine delayRoutine;
    bool loopPlaying;
    bool alarmSilenced;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = loopClip;
        source.playOnAwake = false;
        source.loop = true;
        source.spatialBlend = 1f;
        source.maxDistance = maxDistance;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.Stop();
    }

    void Update()
    {
        if (door == null || loopClip == null || alarmSilenced) return;

        if (delayRoutine == null && !loopPlaying && door.doorWasOpen)
            delayRoutine = StartCoroutine(StartLoopAfterDelay());
    }

    IEnumerator StartLoopAfterDelay()
    {
        yield return new WaitForSeconds(delaySeconds);

        if (alarmSilenced) { delayRoutine = null; yield break; }

        source.Play();
        loopPlaying = true;
        delayRoutine = null;
        gameObject.layer = 3;
    }

    void StopLoop()
    {
        if (source.isPlaying) source.Stop();
        loopPlaying = false;
        alarmSilenced = true;
        gameObject.layer = 0;
        
    }

    public void OnInteract() => StopLoop();
}
