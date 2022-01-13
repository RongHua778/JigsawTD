using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.RefactorTurret;
    public override bool GameUpdate()
    {
        if (GameManager.Instance.OperationState.StateName == StateName.WaveState)
        {
            foreach (var skill in Strategy.TurretSkills)
            {
                if (skill.Duration > 0)
                    skill.Tick(Time.deltaTime);
            }
        }
        return base.GameUpdate();
    }


    public override void ContentLanded()
    {
        GameManager.Instance.compositeTurrets.Add(this);
        base.ContentLanded();
        m_GameTile.tag = "OnlyCompositeTurret";
    }

    public override void SaveContent(out ContentStruct contentStruct)
    {
        base.SaveContent(out contentStruct);
        contentStruct = m_ContentStruct;
        m_ContentStruct.ContentName = Strategy.Attribute.Name;
        m_ContentStruct.Quality = Strategy.Quality;
        m_ContentStruct.ElementSlotCount = Strategy.ElementSKillSlot;
        m_ContentStruct.SkillList = new Dictionary<string, List<int>>();
        for (int i = 1; i < Strategy.TurretSkills.Count; i++)//第2个开始都是元素技能
        {
            ElementSkill skill = Strategy.TurretSkills[i] as ElementSkill;
            m_ContentStruct.SkillList.Add(i.ToString(), skill.Elements);//Litjson存储Key必须为String

        }
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            if (tile.Content.ContentType == GameTileContentType.RefactorTurret)//装备元素技能
            {
                RefactorTurret turret = tile.Content as RefactorTurret;
                ElementSkill skill = (ElementSkill)this.Strategy.TurretSkills[1];
                turret.Strategy.AddElementSkill(skill);
                skill.OnEquip();
                ((BasicTile)turret.m_GameTile).EquipTurret();
                BoardSystem.PreviewEquipTile = null;

                ObjectPool.Instance.UnSpawn(m_GameTile);
            }
            else//正常部署
            {
                //上传一次重构塔重构次数
                ObjectPool.Instance.UnSpawn(tile);
            }
        }
        ShowLandedEffect();
    }

    public void ShowLandedEffect()
    {
        ReusableObject partical = ObjectPool.Instance.Spawn(StaticData.Instance.LandedEffect);
        partical.transform.position = transform.position + Vector3.up * 0.2f;
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        GameManager.Instance.compositeTurrets.Remove(this);

    }

}
