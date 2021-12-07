using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TutorialType
{
    None,
    DrawBtnClick,
    NextWaveBtnClick,
    TurretSelect,
    BlueprintSelect,
    MouseMove,
    WheelMove,
    NextWaveStart,
    ConfirmShape,
    ShopBtnClick,
    ElementBenefitEnter,
    RefactorBtnClick,
    SelectShape,
    SystemBtnClick
}

public enum TempWordType
{
    StandardLose,
    StandardWin,
    EndlessEnd,
    RefreshShop,
    Refactor
}

public struct TempWord
{
    public TempWordType WordType;
    public int ID;
    public TempWord(TempWordType type,int id)
    {
        WordType = type;
        ID = id;
    }
}

public class GameEvents : Singleton<GameEvents>
{


    public event Action<TutorialType> onTutorialTrigger;
    public void TutorialTrigger(TutorialType tutorialType)
    {
        onTutorialTrigger?.Invoke(tutorialType);
    }

    public event Action<TempWord> onTempWord;
    public void TempWordTrigger(TempWord word)
    {
        onTempWord?.Invoke(word);
    }


    public event Action onSeekPath;

    public void SeekPath()
    {
        onSeekPath?.Invoke();
    }

    public event Action onTileClick;
    public void TileClick()
    {
        onTileClick?.Invoke();
    }

    public event Action<TileBase> onTileUp;
    public void TileUp(TileBase tile)
    {
        onTileUp?.Invoke(tile);
    }


    public event Action<Enemy> onEnemyReach;
    public void EnemyReach(Enemy enemy)
    {
        onEnemyReach?.Invoke(enemy);
    }

    public event Action<Enemy> onEnemyDie;
    public void EnemyDie(Enemy enemy)
    {
        onEnemyDie?.Invoke(enemy);
    }


}
