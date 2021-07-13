using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : GameTile
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer turretBaseDeco = default;

    public override void OnSpawn()
    {
        base.OnSpawn();
        BaseRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

    }

    public void SetDeco(Sprite sprite)
    {
        turretBaseDeco.sprite = sprite;
        turretBaseDeco.gameObject.SetActive(true);
    }

}
