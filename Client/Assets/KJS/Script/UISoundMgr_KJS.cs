using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundMgr_KJS : MonoBehaviour
{
    [Header("UI Ȱ��/��Ȱ�� ����")]
    [SerializeField] private AudioClip onEnableSound;  // Ȱ��ȭ�� �� ����� ����
    [SerializeField] private AudioClip onDisableSound; // ��Ȱ��ȭ�� �� ����� ����

    [Header("��ũ�� ����")]
    [SerializeField] private Scrollbar scrollbar;     // ��ũ�ѹ� (�ν����Ϳ��� ���� �Ҵ�)
    [SerializeField] private AudioClip scrollSound;   // ��ũ�� �� ����� ����
    [SerializeField] private float scrollSoundCooldown = 0.1f; // ��ũ�� �Ҹ� ��ٿ�

    [Header("����� ����")]
    [SerializeField] private AudioSource audioSource; // ���带 ����� AudioSource

    private float lastScrollSoundTime = 0f; // ���������� ��ũ�� �Ҹ��� ����� �ð�

    private void Awake()
    {
        // ��ũ�ѹٰ� �Ҵ�� ���, �� ���� �̺�Ʈ ���
        if (scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        }
    }

    private void OnDestroy()
    {
        // ��ũ�ѹ� �̺�Ʈ ������ ����
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
        // ��ũ�� �Ҹ� ��� (��ٿ� ����)
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