using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public RawImage img_Black;
    public RawImage img_Logo;
    
    public float fadeDuration;

    void SetAlpha(float alpha)
    {
        Color imgColor = img_Logo.color;

        imgColor.a = alpha;

        img_Logo.color = imgColor;
    }

    void Start()
    {
        // 로고 이미지 초기 알파값
        SetAlpha(0f);

        StartCoroutine(FadeInAndOut());
    }

    // 로고 이미지 페이드 인/아웃
    IEnumerator FadeInAndOut()
    {
        yield return new WaitForSeconds(0.5f);

        // 로고 페이드 인
        yield return StartCoroutine(FadeTo(0f, 1f, fadeDuration));

        yield return new WaitForSeconds(2f);

        // 로고 페이드 아웃
        yield return StartCoroutine(FadeTo(1f, 0f, fadeDuration));

        yield return new WaitForSeconds(0.5f);

        // 오프닝 활성화
        MaskOffAndMoving();
    }

    IEnumerator FadeTo(float startAlpha, float endAlpha, float duration)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);

            SetAlpha(newAlpha);
            yield return null;
        }
    }

    // 오프닝
    public void MaskOffAndMoving()
    {
        img_Black.transform.DOScale(2200, 3f).SetEase(Ease.OutQuart);
    }
}
