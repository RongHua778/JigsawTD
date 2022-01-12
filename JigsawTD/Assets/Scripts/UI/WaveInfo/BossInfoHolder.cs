using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossInfoHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image BossIcon = default;

    public void SetBossInfo(EnemyType bossType)
    {
        EnemyAttribute att = StaticData.Instance.EnemyFactory.Get(bossType);
        BossIcon.sprite = att.Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.ShowBossTips();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.HideBossTips();
    }
}
