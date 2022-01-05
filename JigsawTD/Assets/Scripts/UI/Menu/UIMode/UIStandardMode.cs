using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStandardMode : MonoBehaviour
{
    [SerializeField] Text difficultyInfo_Txt = default;
    [SerializeField] Text difficultyTxt = default;

    private int selectDifficulty;
    private int SelectDifficulty
    {
        get => selectDifficulty;
        set => selectDifficulty = Mathf.Clamp(value, 0, LevelManager.Instance.PassDiifcutly);
    }

    public void SetInfo()
    {
        selectDifficulty = LevelManager.Instance.PassDiifcutly;
        DifficultyBtnClick(0);
    }

    public void DifficultyBtnClick(int count)
    {
        SelectDifficulty += count;
        difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + SelectDifficulty);
        difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + SelectDifficulty.ToString();
    }

    public void StandardModeStart()
    {
        LevelManager.Instance.StartNewGame(SelectDifficulty);
    }
}
