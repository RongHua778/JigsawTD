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
    public event Action<string> onMessage;
    public void Message(string content)
    {
        onMessage?.Invoke(content);
    }

    public event Action<List<GameTile>> onAddTiles;
    public void AddTiles(List<GameTile> tiles)
    {
        onAddTiles?.Invoke(tiles);
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

    public event Action<GameTile> onTileUp;
    public void TileUp(GameTile tile)
    {
        onTileUp?.Invoke(tile);
    }
}
