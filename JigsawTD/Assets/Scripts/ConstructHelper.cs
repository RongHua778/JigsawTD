using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstructHelper//建造外观模式
{

    static TileShapeFactory m_ShapeFactory;
    static TileFactory m_TileFactory;
    static GameTileContentFactory m_ContentFactory;
    public static void Initialize(TileFactory tileFactory,TileShapeFactory shapeFactory,GameTileContentFactory contentFactory)
    {
        m_TileFactory = tileFactory;
        m_ShapeFactory = shapeFactory;
        m_ContentFactory = contentFactory;
    }

    public static TileShape GetRandomShapeByLevel()
    {
        TileShape shape = m_ShapeFactory.GetRandomShape();
        GameTile specialTile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetRandomElementTurret();
        specialTile.SetContent(content);
        shape.SetTile(specialTile);
        return shape;
    }

    public static GameTile GetNormalTile(GameTileContentType type)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetBasicContent(type);
        tile.SetContent(content);
        return tile;
    }

    public static GroundTile GetGroundTile()
    {
        return m_TileFactory.GetGroundTile();
    }

}
