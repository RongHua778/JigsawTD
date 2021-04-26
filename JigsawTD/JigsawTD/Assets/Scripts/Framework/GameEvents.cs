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

    public event Action<ShapeType, List<TileInfo>> onGenerateShape;
    public void GenerateShape(ShapeType type,List<TileInfo> infoList)
    {
        onGenerateShape?.Invoke(type, infoList);
    }

}
