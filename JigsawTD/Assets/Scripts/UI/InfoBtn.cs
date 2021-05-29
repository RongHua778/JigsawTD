using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 4)]
    [SerializeField] string tipsInfo = default;
    [SerializeField] bool isLevelInfo = default;
    private string GetLevelInfo()
    {
        float[] levelChances = new float[5];
        for (int i = 0; i < 5; i++)
        {
            levelChances[i] = StaticData.Instance.LevelChances[LevelUIManager.Instance.PlayerLevel - 1, i];
        }
        string text = "";
        text += "\n当前等级概率:\n";
        for (int x = 0; x < 5; x++)
        {
            text += "等级" + x + ":" + levelChances[x] * 100 + "%\n";
        }
        return text;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLevelInfo)
            LevelUIManager.Instance.ShowTempTips(GetLevelInfo(), transform.position);
        else
            LevelUIManager.Instance.ShowTempTips(tipsInfo, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LevelUIManager.Instance.HideTempTips();
    }


}
