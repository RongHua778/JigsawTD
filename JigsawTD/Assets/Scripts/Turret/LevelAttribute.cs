using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelAttribute : ScriptableObject
{
    public string Name;
    [TextArea(3,4)]
    public string Description;
    public GameObject TilePrefab;
    public virtual void Upgrade()
    {

    }

}
