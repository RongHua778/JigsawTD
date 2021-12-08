using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBtn : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] LevelInfoPanel infoPanel = default;
    [SerializeField] ParticleSystem LevelUpPartical = default;
    private void Start()
    {
        infoPanel.Initialize();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoPanel.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.Hide();
    }

    public void LevelBtnClick()
    {
        if (GameRes.SystemLevel < StaticData.Instance.SystemMaxLevel)
        {
            if (GameManager.Instance.ConsumeMoney(GameRes.SystemUpgradeCost))
            {
                GameRes.SystemLevel++;
                LevelUpPartical.Play();
                Sound.Instance.PlayEffect("LevelUp");
                infoPanel.SetInfo();
                GameEvents.Instance.TutorialTrigger(TutorialType.SystemBtnClick);
            }
        }
    }
}
