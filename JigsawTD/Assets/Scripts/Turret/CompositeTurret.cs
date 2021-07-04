using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.CompositeTurret;


    public Blueprint CompositeBluePrint;

    public override bool GameUpdate()
    {
        foreach (var skill in Strategy.TurretSkills)
        {
            if (skill.Duration > 0)
                skill.Tick(Time.deltaTime);
        }
        return base.GameUpdate();
    }


    public override void ContentLanded()
    {
        base.ContentLanded();
        GameManager.Instance.compositeTurrets.Add(this);
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        CompositeBluePrint = null;
        GameManager.Instance.compositeTurrets.Remove(this);

    }

}
