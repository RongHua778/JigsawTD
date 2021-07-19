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
        switch (type)
        {
            case 0:
                BaseRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
                break;
            case 1:
                BaseRenderer.sprite = basicTurretBase;
                break;
            case 2:
                BaseRenderer.sprite = compositeTurretBase;
                break;
        }
    }

    public void SetDeco(Sprite sprite)
    {
        turretBaseDeco.sprite = sprite;
        turretBaseDeco.gameObject.SetActive(true);
    }

}
