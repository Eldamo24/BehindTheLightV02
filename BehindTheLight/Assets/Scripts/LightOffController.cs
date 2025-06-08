using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LightOffController : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip audioClipVoice;

    private AudioSource audioSourceSFX;
    private AudioSource voicePlayer;

    [SerializeField] private List<GameObject> lanternObjects = new List<GameObject>();
    [SerializeField] private TMP_Text infoText;

    private void Start()
    {
        audioSourceSFX = GameObject.Find("SFX").GetComponent<AudioSource>();
        voicePlayer = GameObject.Find("VoicePlayer").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject[] lights = GameObject.FindGameObjectsWithTag("LightOff");
            foreach (GameObject lgtGO in lights)
            {
                Light lgt = lgtGO.GetComponent<Light>();
                if (lgt != null)
                {
                    lgt.enabled = false;
                }
            }
            audioSourceSFX.clip = audioClip;
            voicePlayer.clip = audioClipVoice;
            audioSourceSFX.Play();
            voicePlayer.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject obj in lanternObjects)
            {
                obj.SetActive(true);
            }
        }
        infoText.text = "Presiona click izq. para encender o apagar  la linterna";
        Invoke("DestroyObject", 3f);
        GetComponent<BoxCollider>().enabled = false;
    }

    private void DestroyObject()
    {
        infoText.text = "";
        MissionManager.instance.CompleteMission();
        Destroy(gameObject);
    }
}
