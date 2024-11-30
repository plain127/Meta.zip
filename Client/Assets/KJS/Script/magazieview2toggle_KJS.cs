using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagazineViewToggle_KJS : MonoBehaviour
{
    [SerializeField] private GameObject[] uiPanels; // 토글할 UI 오브젝트 배열
    private int activePanelIndex = -1; // 현재 활성화된 패널의 인덱스, 초기값은 -1 (모두 비활성화 상태)

    public void ToggleUI(int index)
    {
        // 선택된 인덱스가 현재 활성화된 패널이라면 해당 패널을 비활성화하고 종료
        if (activePanelIndex == index)
        {
            uiPanels[index].SetActive(false);
            activePanelIndex = -1; // 현재 활성화된 패널이 없음
            return;
        }

        // 만약 다른 패널이 활성화되어 있었다면, 그 패널을 비활성화
        if (activePanelIndex != -1)
        {
            uiPanels[activePanelIndex].SetActive(false);
        }

        // 선택된 인덱스의 UI 패널을 활성화하고, activePanelIndex를 업데이트
        if (index >= 0 && index < uiPanels.Length)
        {
            uiPanels[index].SetActive(true);
            activePanelIndex = index;
        }
    }
}