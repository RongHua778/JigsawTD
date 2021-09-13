using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstructHelper
{
    static TileFactory m_TileFactory;
    static TileShapeFactory m_ShapeFactory;
    static TileContentFactory m_ContentFactory;
    static BlueprintFactory m_BlurPrintFactory;


    public static void Initialize()
    {
        m_TileFactory = GameManager.Instance.TileFactory;
        m_ShapeFactory = GameManager.Instance.ShapeFactory;
        m_ContentFactory = GameManager.Instance.ContentFactory;
        m_BlurPrintFactory = GameManager.Instance.BluePrintFactory;
    }


    public static TileShape GetRandomShapeByLevel(int playerLevel)
    {
        TileShape shape = m_ShapeFactory.GetRandomShape();
        GameTile specialTile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetRandomElementTurret(playerLevel);
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
        GroundTile tile = m_TileFactory.GetGroundTile();
        return tile;
    }

    //元素塔
    public static TurretAttribute GetElementAttribute(Element element)
    {
        return m_ContentFactory.GetElementAttribute(element);
    }


    //陷阱
    public static GameTile GetRandomTrap()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetRandomTrapContent();
        tile.SetContent(content);
        return tile;
    }

    public static GameTile GetSpawnPoint()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetSpawnPoint();
        tile.SetContent(content);
        return tile;
    }

    public static GameTile GetDestinationPoint()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetDestinationPoint();
        tile.SetContent(content);
        return tile;
    }
    //基座
    public static GameTile GetRandomTurretBase()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetRandomTurretBase();
        tile.SetContent(content);
        return tile;
    }


    //合成塔
    public static Blueprint GetRandomBluePrintByLevel(int level)
    {
        TurretAttribute attribute = m_ContentFactory.GetRandomCompositeAttributeByLevel(level);
        return m_BlurPrintFactory.GetRandomBluePrint(attribute);
    }

    public static TileShape GetCompositeTurretByBluePrint(Blueprint bluePrint)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = m_TileFactory.GetBasicTile();
        CompositeTurret content = m_ContentFactory.GetCompositeTurret(bluePrint);
        if (GameRes.NextCompositeCallback != null)//下一次合成获得额外加成
        {
            GameRes.NextCompositeCallback(bluePrint.ComStrategy);
            GameRes.NextCompositeCallback = null;
        }
        bluePrint.ComStrategy.CompositeSkill();

        tile.SetContent(content);
        shape.SetTile(tile);
        return shape;
    }


    //测试用
    public static TileShape GetTrapByName(string name)//测试用，生成一个随意放置的陷阱
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetTrapContentByName(name);
        tile.SetContent(content);
        shape.SetTile(tile);
        return shape;
    }

    public static TileShape GetCompositeTurretByName(string name)//测试用，生成一个指定合成塔搭配随机配方
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        Blueprint bluePrint = m_BlurPrintFactory.GetRandomBluePrint(attribute);
        TileShape shape = GetCompositeTurretByBluePrint(bluePrint);
        GameEndUI.TotalComposite++;
        return shape;
    }

    public static TileShape GetCompositeTurretByNameAndElement(string name, int e1, int e2, int e3)
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        Blueprint bluePrint = m_BlurPrintFactory.GetSpecificBluePrint(attribute, e1, e2, e3);
        TileShape shape = GetCompositeTurretByBluePrint(bluePrint);
        GameEndUI.TotalComposite++;
        return shape;
    }

    public static TileShape GetElementTurretByQualityAndElement(Element element, int quality)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetElementTurret(element, quality);
        tile.SetContent(content);
        shape.SetTile(tile);
        return shape;
    }


    //教学用
    public static TileShape GetTutorialShape(ShapeInfo shapeInfo)
    {
        TileShape shape = m_ShapeFactory.GetShape(shapeInfo.ShapeType);
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetElementTurret(shapeInfo.Element, shapeInfo.Quality);
        tile.SetContent(content);
        shape.SetTile(tile, shapeInfo.TurretPos);
        shape.SetForcePlace(shapeInfo.ForceDir, shapeInfo.ForcePos);
        GameManager.Instance.SetTutorialPoss(true, shapeInfo.TutorialPoss);
        return shape;
    }

    public static Blueprint GetSpecificBlueprint(string name, int e1, int e2, int e3)
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        Blueprint bluePrint = m_BlurPrintFactory.GetSpecificBluePrint(attribute, e1, e2, e3);
        return bluePrint;
    }

}
