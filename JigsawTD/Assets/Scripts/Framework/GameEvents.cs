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
    ElementBenefitEnter
}

public class GameEvents : Singleton<GameEvents>
{
    // public event Action<int> onEventName;
    // public void EventName(int para)
    // {
    //     if (onEventName != null)
    //         onEventName(para);
    // }


    public event Action<TutorialType> onTutorialTrigger;
    public void TutorialTrigger(TutorialType tutorialType)
    {
        onTutorialTrigger?.Invoke(tutorialType);
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
