using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController instance;
    public Transform spriteTransform;
    public Vector3 endPosition;
    public float duration = 2f;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator MoveSprite()
    {
        Vector3 startPos = spriteTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float easedT = 1f - Mathf.Pow(1f - t, 2f); // Ease-out effect

            spriteTransform.position = Vector3.Lerp(startPos, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteTransform.position = endPosition;
    }


}
