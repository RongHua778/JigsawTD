using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelAttribute : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public string Description;
    public GameObject TilePrefab;
    public virtual void Upgrade()
    {

    }

}
