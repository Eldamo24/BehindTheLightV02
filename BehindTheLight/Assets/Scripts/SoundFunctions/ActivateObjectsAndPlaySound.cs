using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectsAndPlaySound : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToActivate;

    [SerializeField] private AudioClip activationClip;
    [SerializeField] private bool playOnStart = false;

    private AudioSource source;
    private bool hasPlayed;

    void Awake()
    {
        source = gameObject.GetComponent<AudioSource>() ??
                  gameObject.AddComponent<AudioSource>();

        source.clip = activationClip;
        source.playOnAwake = false;
    }

    void Start()
    {
        if (playOnStart)
            DoActivationTrigger();
    }
    public void DoActivationTrigger()
    {
        foreach (var go in objectsToActivate)
        {
            if (go != null) go.SetActive(true);
        }

        if (!hasPlayed && activationClip != null)
        {
            source.Play();
            hasPlayed = true;
        }
    }
}
