using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TurretContent : GameTileContent, IGameBehavior
{
    private float frostTime = 0;
    public bool Activated = true;
    private FrostEffect m_FrostEffect;

    public StrategyBase Strategy;//数值计算规则

    public override bool IsWalkable => false;
    public bool Dropped { get; set; }
    public List<TargetPoint> targetList = new List<TargetPoint>();

    RangeHolder m_RangeHolder;

    private float nextAttackTime;
    private Quaternion look_Rotation;


    protected RangeType RangeType;
    protected Bullet bulletPrefab;

    //**********美术，动画及音效
    protected Animator turretAnim;
    protected AudioSource audioSource;
    protected SpriteRenderer CannonSprite;
    protected AudioClip ShootClip;
    [SerializeField] protected ParticleSystem ShootEffect = default;
    //**********

    private List<TargetPoint> target = new List<TargetPoint>();
    public List<TargetPoint> Target { get => target; set => target = value; }

    protected Collider2D detectCollider;
    protected bool ShowingRange = false;
    protected Transform rotTrans;
    protected Transform shootPoint;
    //protected float CheckAngle = 10f;

    //控制sprite颜色
    protected List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    protected List<Color> originalColors = new List<Color>();



    private void Awake()
    {
        rotTrans = transform.Find("RotPoint");
        shootPoint = rotTrans.Find("ShootPoint");
        CannonSprite = rotTrans.Find("Cannon").GetComponent<SpriteRenderer>();

        turretAnim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        SpriteRenderer[] ss = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < ss.Length; i++)
        {
            sprites.Add(ss[i]);
            originalColors.Add(sprites[i].color);
        }


    }

    public virtual void InitializeTurret()
    {
        rotTrans.localRotation = Quaternion.identity;
        bulletPrefab = Strategy.m_Att.Bullet;
        ShootClip = Strategy.m_Att.ShootSound;
        SetGraphic();
        GenerateRange();
        Activated = true;
    }


    protected virtual void PlayAudio(AudioClip clip, bool isAuto)
    {
        audioSource.volume = StaticData.EnvrionmentBaseVolume;
        audioSource.clip = clip;
        if (isAuto)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(clip);
        }
    }


    //设置不同等级的美术资源
    public virtual void SetGraphic()
    {
        shootPoint.transform.localPosition = Strategy.m_Att.TurretLevels[Strategy.Quality - 1].ShootPointOffset;
        CannonSprite.sprite = Strategy.m_Att.TurretLevels[Strategy.Quality - 1].CannonSprite;
    }



    public void AddTarget(TargetPoint target)
    {
        if (target.gameObject.activeInHierarchy)
            targetList.Add(target);
        Strategy.EnterSkill(target.Enemy);
        AcquireTarget();
    }

    public virtual void RemoveTarget(TargetPoint target)
    {
        if (targetList.Contains(target))
        {
            if (this.Target.Contains(target))
            {
                this.Target.Remove(target);
            }
            targetList.Remove(target);
            Strategy.ExitSkill(target.Enemy);
        }
    }


    public virtual void Frost(float time, FrostEffect effect = null)
    {
        Activated = false;
        frostTime = time;
        if (effect != null)
            m_FrostEffect = effect;
    }

    //在塔被激活后每一帧都会调用的方法
    public virtual void OnActivating()
    {
        if (frostTime > 0)
        {
            frostTime -= Time.deltaTime;
            if (frostTime <= 0)
            {
                Activated = true;
                m_FrostEffect.Broke();
                m_FrostEffect = null;
            }
        }
        if (TrackTarget() || AcquireTarget())
        {
            if (!Activated)
                return;
            RotateTowards();
            FireProjectile();
        }
    }


    public virtual bool GameUpdate()
    {
        OnActivating();
        return true;
    }

    private bool TrackTarget()
    {
        if (Target.Count == 0)
        {
            return false;
        }
        for (int i = 0; i < Target.Count; i++)
        {
            if (!Target[i].gameObject.activeSelf)
            {
                targetList.Remove(Target[i]);
                Target.Remove(Target[i]);
            }
        }
        if (Target.Count == 0)
            return false;

        return true;
    }
    private bool AcquireTarget()
    {
        if (targetList.Count <= 0)
            return false;
        else
        {
            if (Strategy.FinalTargetCount > Target.Count)
            {
                Target.Clear();
                List<int> returnInt = StaticData.SelectNoRepeat(targetList.Count, Strategy.FinalTargetCount);
                var ints = returnInt.GetEnumerator();
                while (ints.MoveNext())
                {
                    Target.Add(targetList[ints.Current]);
                }
            }
            else if (Target.Count <= 0)
            {
                Target.Add(targetList[UnityEngine.Random.Range(0, targetList.Count - 1)]);
            }

            return false;
        }
    }

    public void ShowRange(bool show)
    {
        m_RangeHolder.ShowRange(show);
    }
    public void GenerateRange()
    {
        if (m_RangeHolder == null)
            m_RangeHolder = Instantiate(StaticData.Instance.CircleCol, transform);
        m_RangeHolder.SetRange(Strategy.FinalRange, Strategy.ForbidRange, Strategy.RangeType);
    }

    protected virtual void RotateTowards()
    {
        if (Target.Count == 0)
            return;
        var dir = Target[0].transform.position - rotTrans.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotTrans.rotation = Quaternion.LerpUnclamped(rotTrans.rotation, look_Rotation, Strategy.RotSpeed * Time.deltaTime);
    }

    protected bool AngleCheck()
    {
        var angleCheck = Quaternion.Angle(rotTrans.rotation, look_Rotation);
        if (angleCheck < Strategy.CheckAngle)
        {
            return true;
        }
        return false;
    }

    protected virtual void FireProjectile()
    {
        if (Time.time - nextAttackTime > 1 / Strategy.FinalSpeed)
        {
            if (Target != null && AngleCheck())
            {
                Shoot();
            }
            else
            {
                return;
            }
            nextAttackTime = Time.time;
        }
    }

    protected virtual void Shoot()
    {
        turretAnim.SetTrigger("Attack");
        ShootEffect.Play();
        PlayAudio(ShootClip, false);
        var targets = Target.GetEnumerator();
        while (targets.MoveNext())
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as Bullet;
            bullet.transform.position = shootPoint.position;
            bullet.Initialize(this, targets.Current);
        }
    }

    //content类重载*************

    public override void ContentLanded()
    {
        base.ContentLanded();
        m_GameTile.tag = "UnDropablePoint";
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);

        Dropped = true;
        StaticData.SetNodeWalkable(m_GameTile, false, false);

        GameManager.Instance.CheckDetectSkill();//任何一个塔放下来，都要所有防御塔检测一次侦测效果
    }

    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        ShowRange(value);
        if (value)
        {
            GameManager.Instance.ShowTurretTips(this.Strategy, StaticData.LeftTipsPos);
            GameEvents.Instance.TutorialTrigger(TutorialType.TurretSelect);
        }

    }
    //public override void CorretRotation()
    //{
    //    base.CorretRotation();
    //    TurretBaseSprite.transform.rotation = Quaternion.identity;
    //}

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            ObjectPool.Instance.UnSpawn(tile);
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (Dropped)
            StaticData.SetNodeWalkable(m_GameTile, false, true);
        Dropped = false;
        targetList.Clear();
        Strategy = null;
        frostTime = 0;
        ShowRange(false);

    }

}
