using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    IDamageable enemy;
    public Vector2 Position => transform.position;

    public IDamageable Enemy { get => enemy; set => enemy = value; }

    private void Awake()
    {
        Enemy = transform.root.GetComponent<IDamageable>();
    }


}
