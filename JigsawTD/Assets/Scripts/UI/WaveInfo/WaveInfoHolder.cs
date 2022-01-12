using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WaveInfoHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] enemyIcons = default;
    public void SetWaveInfo(List<EnemySequence> sequences)
    {
        foreach (var obj in enemyIcons)
        {
            obj.gameObject.SetActive(false);
        }
        for (int i = 0; i < sequences.Count; i++)
        {
            enemyIcons[i].gameObject.SetActive(true);
            EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(sequences[i].EnemyType);
            enemyIcons[i].sprite = attribute.Icon;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.ShowEnemyTips();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.HideEnemyTips();
    }
}
