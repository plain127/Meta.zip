using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundMgr_KJS : MonoBehaviour
{
    [Header("UI 활성/비활성 사운드")]
    [SerializeField] private AudioClip onEnableSound;  // 활성화될 때 재생할 사운드
    [SerializeField] private AudioClip onDisableSound; // 비활성화될 때 재생할 사운드

    [Header("스크롤 사운드")]
    [SerializeField] private Scrollbar scrollbar;     // 스크롤바 (인스펙터에서 직접 할당)
    [SerializeField] private AudioClip scrollSound;   // 스크롤 시 재생할 사운드
    [SerializeField] private float scrollSoundCooldown = 0.1f; // 스크롤 소리 쿨다운

    [Header("오디오 설정")]
    [SerializeField] private AudioSource audioSource; // 사운드를 재생할 AudioSource

    private float lastScrollSoundTime = 0f; // 마지막으로 스크롤 소리가 재생된 시간

    private void Awake()
    {
        // 스크롤바가 할당된 경우, 값 변경 이벤트 등록
        if (scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        }
    }

    private void OnDestroy()
    {
        // 스크롤바 이벤트 리스너 제거
        if (scrollbar != null)
        {
            scrollbar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
        }
    }

    private void OnEnable()
    {
        PlaySound(onEnableSound);
    }

    private void OnDisable()
    {
        PlaySound(onDisableSound);
    }

    private void OnScrollbarValueChanged(float value)
    {
        // 스크롤 소리 재생 (쿨다운 적용)
        if (audioSource != null && scrollSound != null)
        {
            if (Time.time - lastScrollSoundTime >= scrollSoundCooldown)
            {
                audioSource.PlayOneShot(scrollSound);
                lastScrollSoundTime = Time.time;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}