using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContentAttribute : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    [TextArea(3,4)]
    public string Description;
    public GameTileContent ContentPrefab;
    public virtual void Upgrade()
    {

    }

}
