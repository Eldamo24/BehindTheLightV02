using UnityEngine;
using System.Collections;

public class ChangeAlbedoOnTrigger : MonoBehaviour
{
    [SerializeField] private bool playOnStart = false;
    [SerializeField] private int targetId = 0;
    [SerializeField] private int materialIndex = 0;
    [SerializeField] private Texture2D newAlbedo;

    [SerializeField] private AudioClip[] clips;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    private Renderer targetRenderer;
    private bool hasActivated;

    private void Awake()
    {
        targetRenderer = FindTargetRenderer(targetId);
        if (targetRenderer == null)
            Debug.LogWarning($"{name}: No se encontró Renderer con ID {targetId}");
    }

    private void Start()
    {
        if (playOnStart)
            ChangeAlbedoOnActivate();
    }

    public void ChangeAlbedoOnActivate()
    {
        Debug.Log($"Enter with [{targetId}] hasActivated: {hasActivated}.");
        if (hasActivated || targetRenderer == null || newAlbedo == null) return;
        hasActivated = true;

        var mats = targetRenderer.materials;
        if (materialIndex < mats.Length)
        {
            var mat = mats[materialIndex];

            string prop = mat.HasProperty("_MainTex")
                          ? "_MainTex"
                          : mat.HasProperty("_BaseMap")
                            ? "_BaseMap"
                            : null;

            if (prop != null)
            {
                mat.SetTexture(prop, newAlbedo);
                targetRenderer.materials = mats;
            }
            else
            {
                Debug.LogWarning($"[{name}] el shader '{mat.shader.name}' no expone _MainTex ni _BaseMap.");
            }
        }
        else
        {
            Debug.LogWarning($"[{name}] materialIndex {materialIndex} fuera de rango.");
            return;
        }

        if (clips != null && clips.Length > 0)
            StartCoroutine(PlayClipsSequentially());
    }

    private IEnumerator PlayClipsSequentially()
    {
        Debug.Log($"Enter with [{clips}].");
        foreach (var clip in clips)
        {
            if (clip == null)
                continue;
            AudioSource.PlayClipAtPoint(clip, targetRenderer.transform.position, volume);
            yield return new WaitForSeconds(clip.length);
        }
    }

    private Renderer FindTargetRenderer(int id)
    {
        foreach (var target in FindObjectsOfType<AlbedoTarget>(true))
        {
            if (target.Id == id)
                return target.GetComponent<Renderer>();
        }
        return null;
    }
}
