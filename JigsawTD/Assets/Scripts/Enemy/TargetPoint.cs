using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    [SerializeField]Enemy enemy;
    public Vector2 Position => transform.position;

    public Enemy Object { get => enemy; set => enemy = value; }
}
