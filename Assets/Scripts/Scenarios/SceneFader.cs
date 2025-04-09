using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;
    public Image fadePanel; // assign in Inspector
    public float fadeDuration = 1f;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0f, 1f));
    }

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadePanel.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }

        c.a = endAlpha;
        fadePanel.color = c;
    }
}
