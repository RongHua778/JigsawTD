using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicTile : GameTile
{
    [SerializeField] Sprite[] sprites = default;
    [SerializeField] Sprite basicTurretBase = default;
    [SerializeField] Sprite compositeTurretBase = default;
    [SerializeField] Sprite compositeTurretBase2 = default;



    public override void OnTilePass(Enemy enemy)
    {
        base.OnTilePass(enemy);
        enemy.DamageStrategy.TileDamageIntensify = TileDamageIntensify;
        enemy.DamageStrategy.TileSlowIntensify = TileSlowIntensify;
        enemy.DamageStrategy.BonusCoin = BounsCoin;
    }

    public override void OnTileLeave(Enemy enemy)
    {
        base.OnTileLeave(enemy);
    }

    public void CheckContent(int index, List<BasicTile> path)
    {
        Content.OnContentPreCheck(index,path);
    }

    public override void SetContent(GameTileContent content)
    {
        base.SetContent(content);
        SetBaseSprite(content.ContentType);
    }

    public override void OnTileSelected(bool value)
    {
        base.OnTileSelected(value);
        Content.OnContentSelected(value);
    }

    private void SetBaseSprite(GameTileContentType type)
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        TileRenderers = srs.Take(2).ToList();//只取前2个图片，避免取到防御塔范围指示器
        switch (type)
        {
            case GameTileContentType.Empty:
            case GameTileContentType.Destination:
            case GameTileContentType.SpawnPoint:
            default:
                TileRenderers[0].sprite = sprites[Random.Range(0, sprites.Length)];
                break;
            case GameTileContentType.ElementTurret:
            case GameTileContentType.Trap:
                TileRenderers[0].sprite = basicTurretBase;
                break;
            case GameTileContentType.CompositeTurret:
                TileRenderers[0].sprite = compositeTurretBase;
                break;
        }
    }

    public void EquipTurret()
    {
        TileRenderers[0].sprite = compositeTurretBase2;
    }

    public void ResetTile()
    {
        TileDamageIntensify = 0;
        TrapIntensify = 1;
        TileSlowIntensify = 0;
        BounsCoin = 0;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        ResetTile();
    }
}
