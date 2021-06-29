using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    Enemy enemy;
    public Vector2 Position => transform.position;

    public Enemy Enemy { get => enemy; set => enemy = value; }

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
    }


}
