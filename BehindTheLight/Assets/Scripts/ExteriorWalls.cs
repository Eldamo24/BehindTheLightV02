using UnityEngine;

public class ExteriorWalls : MonoBehaviour
{
    private AudioSource AudioSource;
    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GameObject.Find("VoicePlayer").GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioSource.clip = clip;
            AudioSource.Play();
        }
    }
}
