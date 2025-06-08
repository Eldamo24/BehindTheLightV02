using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlaySoundOnInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private string onInteractMsg;
    public string OnInteractMsg => onInteractMsg;

    AudioSource source;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.maxDistance = maxDistance;
        source.rolloffMode = AudioRolloffMode.Linear;
    }

    public void OnInteract()
    {
        if (clip == null) return;
        source.PlayOneShot(clip);
    }
}