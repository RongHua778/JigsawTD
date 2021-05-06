using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; set; }
    public Vector2 Position => transform.position;
    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
    }


}
