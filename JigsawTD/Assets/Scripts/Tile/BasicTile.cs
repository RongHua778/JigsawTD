using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : GameTile
{
    [SerializeField] Sprite[] sprites;

    public override void OnSpawn()
    {
        base.OnSpawn();
        BaseRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

    }

    private void OnMouseDown()
    {
        GameManager.Instance.HideTips();
    }
}
