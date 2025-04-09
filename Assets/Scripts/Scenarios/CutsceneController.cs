using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController instance;
    public Transform trainTransform;
    public Vector3 endPosition;
    public float duration = 2f;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator MoveSprite()
    {
        Vector3 startPos = trainTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            trainTransform.position = Vector3.Lerp(startPos, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        trainTransform.position = endPosition;
    }
}
