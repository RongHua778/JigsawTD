using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WaveInfoSetter : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] GameObject[] EnemyCountArea = default;
    [SerializeField] Image[] enemyIcons = default;
    [SerializeField] Text waveTxt = default;
    public void SetWaveInfo(int wave, EnemySequence sequence)
    {
        waveTxt.text = "µÚ" + wave + "/" + StaticData.Instance.LevelMaxWave.ToString() + "²¨";
        int count = sequence.EnemyAttribute.Count;
        foreach (var obj in EnemyCountArea)
        {
            obj.SetActive(false);
        }
        EnemyCountArea[count - 1].SetActive(true);
        switch (count)
        {
            case 1:
                enemyIcons[0].sprite = sequence.EnemyAttribute[0].EnemyIcon;
                break;
            case 2:
                enemyIcons[1].sprite = sequence.EnemyAttribute[0].EnemyIcon;
                enemyIcons[2].sprite = sequence.EnemyAttribute[1].EnemyIcon;
                break;
            case 3:
                enemyIcons[3].sprite = sequence.EnemyAttribute[0].EnemyIcon;
                enemyIcons[4].sprite = sequence.EnemyAttribute[1].EnemyIcon;
                enemyIcons[5].sprite = sequence.EnemyAttribute[2].EnemyIcon;
                break;
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
