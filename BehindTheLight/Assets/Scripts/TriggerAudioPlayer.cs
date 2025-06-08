using UnityEngine;

public class TriggerAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Start()
    {
        audioSource = GameObject.Find("VoicePlayer").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            Destroy(gameObject);
        }
    }

}
