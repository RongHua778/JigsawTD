using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicTile : GameTile
{
    [SerializeField] Sprite[] sprites = default;
    [SerializeField] SpriteRenderer turretBaseDeco = default;
    [SerializeField] Sprite basicTurretBase = default;
    [SerializeField] Sprite compositeTurretBase = default;


    public override void SetContent(GameTileContent content)
    {
        base.SetContent(content);
        SetBaseSprite(content.ContentType);
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
                TileRenderers[0].sprite = basicTurretBase;
                break;
            case GameTileContentType.CompositeTurret:
                TileRenderers[0].sprite = compositeTurretBase;
                break;
        }
    }

    public void SetDeco(Sprite sprite)
    {
        turretBaseDeco.sprite = sprite;
        turretBaseDeco.gameObject.SetActive(true);
    }

}
