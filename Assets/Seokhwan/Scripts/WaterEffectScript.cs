using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffectScript : MonoBehaviour
{
    public float duration = 1f; // 스케일 변화 시간
    public Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f); // 시작 스케일
    public Vector3 endScale = new Vector3(0.2f, 0.2f, 0.2f); // 최종 스케일

    void Start()
    {
        transform.localScale = startScale;

        StartCoroutine(ChangeScaleOverTime());
    }

    IEnumerator ChangeScaleOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = endScale;
    }
}
