using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal;

public class BasicTile : GameTile
{
    [SerializeField] Sprite[] sprites = default;
    [SerializeField] Sprite basicTurretBase = default;
    [SerializeField] Sprite compositeTurretBase = default;
    [SerializeField] Sprite compositeTurretBase2 = default;

    public override void SetContent(GameTileContent content)
    {
        base.SetContent(content);
        Highlight(false);
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
        TileRenderers = srs.Take(2).ToList();//ֻȡǰ2��ͼƬ������ȡ����������Χָʾ��
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
            case GameTileContentType.RefactorTurret:
                TileRenderers[0].sprite = compositeTurretBase;
                break;
        }
    }

    public void EquipTurret()
    {
        TileRenderers[0].sprite = compositeTurretBase2;
    }

}
