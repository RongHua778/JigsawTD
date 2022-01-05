using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndlessMode : MonoBehaviour
{
    [SerializeField] Text endlessHighScore = default;
    [SerializeField] Text endlessUnlockText = default;
    [SerializeField] GameObject endlessStartBtnObj = default;

    public void SetInfo()
    {
        endlessHighScore.text = LevelManager.Instance.EndlessHighScore + GameMultiLang.GetTraduction("WAVE");
        //是否已解锁无尽模式
        endlessStartBtnObj.SetActive(LevelManager.Instance.PassDiifcutly > 4);
        endlessUnlockText.gameObject.SetActive(LevelManager.Instance.PassDiifcutly <= 4);
    }

    public void EndlessModeStart()
    {
        LevelManager.Instance.StartNewGame(11);
    }

}
