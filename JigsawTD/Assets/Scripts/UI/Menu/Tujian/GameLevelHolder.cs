using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLevelHolder : MonoBehaviour
{
    [SerializeField] Text LevelTxt = default;
    [SerializeField] Text ExpTxt = default;
    [SerializeField] Image ExpProgress = default;
    private int changeSpeed = 50;
    private int currentExp;
    private int nextExp;
    private int currentLevel;

    public int CurrentExp
    {
        get => currentExp;
        set
        {
            currentExp = value;
            ExpTxt.text = value + "/" + NextExp;
            ExpProgress.fillAmount = (float)currentExp / NextExp;
        }
    }
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
            LevelTxt.text = currentLevel.ToString();
            if (currentLevel == LevelManager.Instance.MaxGameLevel)
            {
                CurrentExp = NextExp;
            }
            else
            {
                NextExp = LevelManager.Instance.GetLevelInfo(currentLevel).ExpRequire;
            }

        }
    }

    public int NextExp
    {
        get => nextExp;
        set
        {
            nextExp = value;
            ExpTxt.text = CurrentExp + "/" + nextExp;
            ExpProgress.fillAmount = (float)currentExp / NextExp;
        }
    }
    public void SetData()
    {
        CurrentLevel = LevelManager.Instance.GameLevel;
        CurrentExp = LevelManager.Instance.GameExp;
        NextExp = LevelManager.Instance.GetLevelInfo(CurrentLevel).ExpRequire;
    }

    public void AddExp(int Exp)
    {
        ExpCor(Exp);
    }
    private void ExpCor(int Exp)
    {
        StartCoroutine(AddExpCor(Exp));
    }

    IEnumerator AddExpCor(int Exp)
    {
        float delta;
        if (CurrentLevel >= LevelManager.Instance.MaxGameLevel)
        {
            int final = CurrentExp + Exp;
            delta = (float)Exp / changeSpeed;
            for (int i = 0; i < changeSpeed; i++)
            {
                CurrentExp += (int)delta;
                yield return new WaitForSeconds(0.02f);
            }
            CurrentExp = final;
        }
        else
        {
            int need = NextExp - CurrentExp;
            if (Exp >= need)
            {
                delta = (float)need / changeSpeed;
                for (int i = 0; i < changeSpeed; i++)
                {
                    CurrentExp += (int)delta;
                    yield return new WaitForSeconds(0.02f);
                }
                CurrentExp = 0;
                CurrentLevel++;
                GameManager.Instance.ShowBonusTips(LevelManager.Instance.GetLevelInfo(CurrentLevel));
                Sound.Instance.PlayEffect("Sound_LevelUp");
                ExpCor(Exp - need);
            }
            else
            {
                int final = CurrentExp + Exp;
                delta = (float)Exp / changeSpeed;
                for (int i = 0; i < changeSpeed; i++)
                {
                    CurrentExp += (int)delta;
                    yield return new WaitForSeconds(0.02f);
                }
                CurrentExp = final;
            }
        }

    }

}
