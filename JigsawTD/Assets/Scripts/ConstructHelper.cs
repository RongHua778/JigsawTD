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
        return m_TileFactory.GetGroundTile();
    }

    //Ԫ����
    public static TurretAttribute GetElementAttribute(Element element)
    {
        return m_ContentFactory.GetElementAttribute(element);
    }


    //����
    public static GameTile GetRandomTrap()
    {
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetRandomTrapContent();
        tile.SetContent(content);
        return tile;
    }

    //�ϳ���
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
        tile.SetContent(content);
        shape.SetTile(tile);
        return shape;
    }


    //������
    public static TileShape GetTrapByName(string name)//�����ã�����һ��������õ�����
    {
        TileShape shape = m_ShapeFactory.GetDShape();
        GameTile tile = m_TileFactory.GetBasicTile();
        GameTileContent content = m_ContentFactory.GetTrapContentByName(name);
        tile.SetContent(content);
        shape.SetTile(tile);
        return shape;
    }

    public static TileShape GetCompositeTurretByName(string name)//�����ã�����һ��ָ���ϳ�����������䷽
    {
        TurretAttribute attribute = m_ContentFactory.GetCompositeTurretByName(name);
        Blueprint bluePrint = m_BlurPrintFactory.GetRandomBluePrint(attribute);
        TileShape shape= GetCompositeTurretByBluePrint(bluePrint);
        return shape;
    }

}
