using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GroundTile : TileBase
{
    float rangeIntensify;
    public float RangeIntensify { get => rangeIntensify; set { rangeIntensify = value; txt.text = value.ToString(); } }
    public float AttackIntensify;
    public float SpeedIntensify;
    [SerializeField] TextMeshPro txt = default;

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {

    }
}
