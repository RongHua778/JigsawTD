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
        m_TileFactory = StaticData.Instance.TileFactory;
        m_ShapeFactory = StaticData.Instance.ShapeFactory;
        m_ContentFactory = StaticData.Instance.ContentFactory;
        m_BlurPrintFactory = StaticData.Instance.BluePrintFactory;
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
        GroundTile tile = m_TileFactory.GetGroundTile();
        return tile;
    }

    //元素塔
    public static TurretAttribute GetElementAttribute(ElementType element)
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
    public static Blueprint GetRandomBluePrintByLevel()
    {
        TurretAttribute attribute = m_ContentFactory.GetRandomCompositeAtt();
        return m_BlurPrintFactory.GetRandomBluePrint(attribute);
    }

    public static TileShape GetCompositeTurretByBluePrint(Blueprint bluePrint)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = m_TileFactory.GetBasicTile();
        RefactorTurret content = m_ContentFactory.GetCompositeTurret(bluePrint);
        //if (GameRes.NextCompositeCallback != null)//下一次合成获得额外加成
        //{
        //    GameRes.NextCompositeCallback(bluePrint.ComStrategy);
        //    GameRes.NextCompositeCallback = null;
        //}
        //bluePrint.ComStrategy.CompositeSkill();

        tile.SetContent(content);
        shape.SetTile(tile);
        return shape;
    }


    //测试用
    public static TileShape GetTrapShapeByName(string name)//测试用，生成一个随意放置的陷阱
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        shape.SetTile(GetTrap(name));
        return shape;
    }


    public static TileShape GetCompositeTurretByNameAndElement(string name, int e1, int e2, int e3)
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        Blueprint bluePrint = m_BlurPrintFactory.GetSpecificBluePrint(attribute, e1, e2, e3);
        TileShape shape = GetCompositeTurretByBluePrint(bluePrint);
        return shape;
    }

    public static TileShape GetElementTurretByQualityAndElement(ElementType element, int quality)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = GetElementTurret(element, quality);
        shape.SetTile(tile);
        return shape;
    }


    //教学用
    public static TileShape GetTutorialShape(ShapeInfo shapeInfo)
    {
        TileShape shape = m_ShapeFactory.GetShape(shapeInfo.ShapeType);
        GameTile tile = GetElementTurret(shapeInfo.Element, shapeInfo.Quality);
        shape.SetTile(tile, shapeInfo.TurretPos);
        return shape;
    }

    public static Blueprint GetSpecificBlueprint(string name, int e1, int e2, int e3, int quality = 1)
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        Blueprint bluePrint = m_BlurPrintFactory.GetSpecificBluePrint(attribute, e1, e2, e3, quality);
        return bluePrint;
    }

    //读取保存数据
    public static GameTile GetElementTurret(ElementType element, int quality)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetElementTurret(element, quality);
        tile.SetContent(content);
        return tile;
    }

    public static GameTile GetTrap(string name)
    {
        GameTileContent content = m_ContentFactory.GetTrapContentByName(name);
        if (content == null)
        {
            return null;
        }
        GameTile tile = m_TileFactory.GetBasicTile();
        tile.SetContent(content);
        return tile;
    }

    public static GameTile GetRefactorTurret(ContentStruct contentStruct)
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        List<int> elements;
        RefactorTurret content = null;
        for (int i = 0; i < contentStruct.SkillList.Count; i++)
        {
            if (i == 0)
            {
                elements = contentStruct.SkillList["1"];
                Blueprint blueprint = GetSpecificBlueprint(contentStruct.ContentName, elements[0], elements[1], elements[2], contentStruct.Quality);
                content = m_ContentFactory.GetCompositeTurret(blueprint);
                content.Strategy.ElementSKillSlot = contentStruct.ElementSlotCount;
                tile.SetContent(content);
            }
            else
            {
                elements = contentStruct.SkillList[i.ToString()];
                ElementSkill skill = TurretEffectFactory.GetElementSkill(elements);
                content.Strategy.AddElementSkill(skill);
                ((BasicTile)tile).EquipTurret();
            }
        }
        //还差ddd没做
        return tile;

    }


}
