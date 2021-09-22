using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WaveInfoSetter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image[] enemyIcons = default;
    [SerializeField] Text waveTxt = default;
    public void SetWaveInfo(int wave, List<EnemySequence> sequences)
    {
        string lang = PlayerPrefs.GetString("_language");
        switch (lang)
        {
            case "ch":
                waveTxt.text = GameMultiLang.GetTraduction("NUM") + wave + "/" + LevelManager.Instance.CurrentLevel.Wave + GameMultiLang.GetTraduction("WAVE");
                break;
            case "en":
                waveTxt.text = GameMultiLang.GetTraduction("WAVE") + wave + "/" + LevelManager.Instance.CurrentLevel.Wave;
                break;
        }
        foreach (var obj in enemyIcons)
        {
            obj.gameObject.SetActive(false);
        }
        for (int i = 0; i < sequences.Count; i++)
        {
            enemyIcons[i].gameObject.SetActive(true);
            EnemyAttribute attribute = StaticData.Instance.EnemyFactory.Get(sequences[i].EnemyType);
            enemyIcons[i].sprite = attribute.EnemyIcon;
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
