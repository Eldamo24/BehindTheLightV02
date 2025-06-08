using System.Collections;
using TMPro;
using UnityEngine;

public class LightedObjects : MonoBehaviour, IInteractable
{
    [SerializeField] private Material mat;
    [SerializeField] private TMP_Text textObj;
    private float fadeSpeed = 1.5f;
    private Coroutine fadeCoroutine;
    [SerializeField] private string onInteractMsg;

    public string OnInteractMsg => onInteractMsg;

    private void Start()
    {
        TryGetComponent<TMP_Text>(out textObj);
        if(textObj != null)
        {
            Color c = textObj.color;
            c.a = 0f;
            textObj.color = c;
        }
    }

    public void StartFadeIn()
    {
        StartFadeToAlpha(1f);
    }

    public void StartFadeOut()
    {
        StartFadeToAlpha(0f);
    }

    private void StartFadeToAlpha(float alpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeTo(alpha));
    }

    /// <summary>
    /// Coroutine that makes the object visible controlling it alpha.
    /// </summary>
    IEnumerator FadeTo(float alpha)
    {
        Color c;
        if(textObj != null)
        {
            c = textObj.color;
        }
        else
        {
            c = mat.color;
        }
        float startAlpha = c.a;

        while (Mathf.Abs(c.a - alpha) > 0.01f)
        {
            c.a = Mathf.MoveTowards(c.a, alpha, fadeSpeed * Time.deltaTime);
            if(textObj != null)
            {
                textObj.color = c;
            }
            else
            {
                mat.color = c;
            }
            yield return null;
        }
        c.a = alpha;
        if (textObj != null)
        {
            textObj.color = c;
        }
        else
        {
            mat.color = c;
        }
    }

    public bool IsVisible()
    {
        if (textObj != null)
            return textObj.color.a > 0.95f;
        return mat.color.a > 0.95f;
    }

    public void OnInteract()
    {
        print("Interactuaste");
    }

    private void OnDestroy()
    {
        Color c;
        if (textObj != null)
        {
            c = textObj.color;
        }
        else
        {
            c = mat.color;
        }
        c.a = 0f;
        if (textObj != null)
        {
            textObj.color = c;
        }
        else
        {
            mat.color = c;
        }
    }
}