using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : Singleton<GameEvents>
{
    // public event Action<int> onEventName;
    // public void EventName(int para)
    // {
    //     if (onEventName != null)
    //         onEventName(para);
    // }




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

    public event Action<GameTile> onTileUp;
    public void TileUp(GameTile tile)
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


    public event Action<int> onGuideTrigger;
    public void GuideTrigger(int index)
    {
        onGuideTrigger?.Invoke(index);
    }
}
