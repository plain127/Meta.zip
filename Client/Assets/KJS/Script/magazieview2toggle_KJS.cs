using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagazineViewToggle_KJS : MonoBehaviour
{
    [SerializeField] private GameObject[] uiPanels; // ����� UI ������Ʈ �迭
    private int activePanelIndex = -1; // ���� Ȱ��ȭ�� �г��� �ε���, �ʱⰪ�� -1 (��� ��Ȱ��ȭ ����)

    public void ToggleUI(int index)
    {
        // ���õ� �ε����� ���� Ȱ��ȭ�� �г��̶�� �ش� �г��� ��Ȱ��ȭ�ϰ� ����
        if (activePanelIndex == index)
        {
            uiPanels[index].SetActive(false);
            activePanelIndex = -1; // ���� Ȱ��ȭ�� �г��� ����
            return;
        }

        // ���� �ٸ� �г��� Ȱ��ȭ�Ǿ� �־��ٸ�, �� �г��� ��Ȱ��ȭ
        if (activePanelIndex != -1)
        {
            uiPanels[activePanelIndex].SetActive(false);
        }

        // ���õ� �ε����� UI �г��� Ȱ��ȭ�ϰ�, activePanelIndex�� ������Ʈ
        if (index >= 0 && index < uiPanels.Length)
        {
            uiPanels[index].SetActive(true);
            activePanelIndex = index;
        }
    }
}