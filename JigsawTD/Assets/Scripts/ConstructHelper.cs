using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstructHelper
{
    static TileFactory m_TileFactory;
    static TileShapeFactory m_ShapeFactory;
    static TileContentFactory m_ContentFactory;
    static TurretStrategyFactory m_StrategyFactory;


    public static void Initialize()
    {
        m_TileFactory = StaticData.Instance.TileFactory;
        m_ShapeFactory = StaticData.Instance.ShapeFactory;
        m_ContentFactory = StaticData.Instance.ContentFactory;
        m_StrategyFactory = StaticData.Instance.StrategyFactory;
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
    //new
    public static RefactorStrategy GetRandomRefactorStrategy()
    {
        TurretAttribute attribute = m_ContentFactory.GetRandomCompositeAtt();
        return m_StrategyFactory.GetRandomRefactorStrategy(attribute);
    }


    public static TileShape GetRefactorTurretByStrategy(RefactorStrategy strategy)
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = m_TileFactory.GetBasicTile();
        RefactorTurret content = m_ContentFactory.GetRefactorTurret(strategy);

        //if (GameRes.NextCompositeCallback != null)//下一次合成获得额外加成
        //{
        //    GameRes.NextCompositeCallback(bluePrint.ComStrategy);
        //    GameRes.NextCompositeCallback = null;
        //}
        //strategy.CompositeSkill();

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

    public static TileShape GetRefactorTurretByNameAndElement(string name, int e1, int e2, int e3)
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        List<int> elements = new List<int> { e1, e2, e3 };
        List<int> qualites = new List<int> { 1, 1, 1 };
        RefactorStrategy strategy = m_StrategyFactory.GetSpecificRefactorStrategy(attribute, elements, qualites);
        TileShape shape = GetRefactorTurretByStrategy(strategy);
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
        shape.SetTile(tile, shapeInfo.TurretPos, shapeInfo.TurretDir);
        return shape;
    }



    public static RefactorStrategy GetSpecificStrategy(string name, List<int> elements, List<int> qualities, int quality = 1)
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        RefactorStrategy strategy = m_StrategyFactory.GetSpecificRefactorStrategy(attribute, elements, qualities, quality);
        return strategy;
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
        RefactorStrategy strategy = null;
        for (int i = 0; i < contentStruct.SkillList.Count; i++)
        {
            if (i == 0)
            {
                elements = contentStruct.SkillList["1"];
                strategy = GetSpecificStrategy(contentStruct.ContentName, elements, new List<int> { 1, 1, 1 });
                content = m_ContentFactory.GetRefactorTurret(strategy);
                strategy.ElementSKillSlot = contentStruct.ElementSlotCount;
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
        strategy.InitAttackModify = (float)contentStruct.InitModifies[0];
        strategy.InitFirerateModify = (float)contentStruct.InitModifies[1];
        strategy.InitCriticalRateModify = (float)contentStruct.InitModifies[2];
        strategy.InitSlowRateModify = (float)contentStruct.InitModifies[3];
        strategy.InitSplashRangeModify = (float)contentStruct.InitModifies[4];
        return tile;
    }


}
