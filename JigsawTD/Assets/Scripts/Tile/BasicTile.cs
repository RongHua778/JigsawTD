using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : GameTile
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer turretBaseDeco = default;
    [SerializeField] Sprite basicTurretBase = default;
    [SerializeField] Sprite compositeTurretBase = default;

    public override void OnSpawn()
    {
        base.OnSpawn();
        SetBaseSprite(0);

    }

    public void SetBaseSprite(int type)
    {
        TileRenderers = GetComponentsInChildren<SpriteRenderer>();
        switch (type)
        {
            case 0:
                TileRenderers[0].sprite = sprites[Random.Range(0, sprites.Length)];
                break;
            case 1:
                TileRenderers[0].sprite = basicTurretBase;
                break;
            case 2:
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
