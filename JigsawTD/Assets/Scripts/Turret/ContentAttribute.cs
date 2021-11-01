using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttType
{
    Turret,
    Mark,
    Enemy
}

public class ContentAttribute : ScriptableObject
{
    public AttType AttType;
    public string Name;
    public Sprite Icon;
    public string Description;
    public ReusableObject Prefab;

    public virtual void MenuShowTips(Vector2 pos)
    {
    }

}
