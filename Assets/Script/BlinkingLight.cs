using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlinkingLight : MonoBehaviour
{
    Light blinker;

    [SerializeField]
    float minDelay;
    [SerializeField]
    float maxDelay;

    float previousDelay = 0.5f;

    private void Start()
    {
        blinker = GetComponent<Light>();
        StartBlink();
    }

    void StartBlink()
    {
        var min = Mathf.Clamp(previousDelay - 0.5f, minDelay, maxDelay);
        var max = Mathf.Clamp(previousDelay + 0.5f, minDelay, maxDelay);
        float delay = Random.Range(min, max);
        StartCoroutine(WaitAndBlink(delay));
    }

    IEnumerator WaitAndBlink(float delay)
    {
        yield return new WaitForSeconds(delay);
        blinker.DOIntensity(Mathf.Abs(blinker.intensity - 1), 0.15f).SetEase(Ease.InOutCubic).OnComplete(StartBlink);
    }
}
