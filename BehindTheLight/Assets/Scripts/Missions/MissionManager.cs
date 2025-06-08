using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    private int idMission;
    [SerializeField] private List<Mission> missions;
    [SerializeField] private TMP_Text missionText;
    [SerializeField] private AudioSource sfxComponent;
    [SerializeField] private AudioClip newMissionAudio;

    [Header("Missions canvas fade")]
    [SerializeField] private CanvasGroup missionPanel;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float timeOnScreen;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        instance = this;
        idMission = 0;
        Invoke("NextMission", 2f);

    }

    public void NextMission()
    {
        StartMission(missions[idMission]);
    }

    private void StartMission(Mission actualMission)
    {
        missionText.text = actualMission.missionTitle;
        sfxComponent.clip = newMissionAudio;
        sfxComponent.Play();
        StartCoroutine("FadePanel");
    }

    public void CheckMission()
    {
        ShowMission();
    }

    public void CompleteMission()
    {
        missions[idMission].isCompleted = true;
        idMission++;
        StartMission(missions[idMission]);
    }

    public void ShowMission()
    {
        StopAllCoroutines();
        StartCoroutine("FadePanel");
    }

    IEnumerator FadePanel()
    {
        yield return StartCoroutine(FadeCanvasGroup(missionPanel, 0, 1, fadeDuration));
        yield return new WaitForSeconds(timeOnScreen);
        yield return StartCoroutine(FadeCanvasGroup(missionPanel, 1, 0, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup panel, float from, float to, float duration)
    {
        float time = 0f;
        panel.alpha = from;
        panel.blocksRaycasts = to > 0;
        panel.interactable = to > 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            panel.alpha = Mathf.Lerp(from, to, time / duration);
            yield return null;
        }

        panel.alpha = to;
    }
}
