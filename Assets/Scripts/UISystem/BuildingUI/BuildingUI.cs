using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    const float FadeDuration = 0.3f;

    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    GameObject constructSign;
    [SerializeField]
    GameObject deconstructSign;
    [SerializeField]
    GameObject resourceObj;
    [SerializeField]
    TextMeshProUGUI resourceAmount;

    Coroutine coroutine;

    public void SetUIActive(bool value)
    {
        if ((value && canvasGroup.alpha == 1) || (!value && canvasGroup.alpha == 0))
            return;

        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(Fading(value));
    }

    IEnumerator Fading(bool value)
    {
        float startValue = canvasGroup.alpha;
        float endValue = value ? 1 : 0;

        float startTime = Time.time;
        float progress;
        while (Time.time - startTime < FadeDuration)
        {
            progress = (Time.time - startTime) / FadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startValue, endValue, progress);
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = endValue;
        coroutine = null;
    }

    public void ShowConstructSign()
    {
        DisableAll();
        constructSign.SetActive(true);
    }

    public void ShowDeconstructSign()
    {
        DisableAll();
        deconstructSign.SetActive(true);
    }

    public void ShowResourceSign()
    {
        DisableAll();
        resourceObj.SetActive(true);
    }

    public void OnResourceCollectClick()
    {
        Debug.Log("Resource Collect");
    }

    public void DisableAll()
    {
        constructSign.SetActive(false);
        deconstructSign.SetActive(false);
        resourceObj.SetActive(false);
    }
}
