using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Soilder, Runner, Restorer, Tanker,  Healer, Froster,
    Fat, SixArmor, Divider,Blinker,
    Ninja,  Borner, Armorer,
    Random, AircraftCarrier
}
[CreateAssetMenu(menuName = "Factory/EnemyFactory", fileName = "EnemyFactory")]
public class EnemyFactory : ScriptableObject
{
    [SerializeField] List<EnemyAttribute> enemies = new List<EnemyAttribute>();
    private Dictionary<EnemyType, EnemyAttribute> EnemyDIC;

    public void InitializeFactory()
    {
        EnemyDIC = new Dictionary<EnemyType, EnemyAttribute>();
        foreach (var enemy in enemies)
        {
            EnemyDIC.Add(enemy.EnemyType, enemy);
        }
    }
    public EnemyAttribute Get(EnemyType type)
    {
        if (EnemyDIC.ContainsKey(type))
        {
            return EnemyDIC[type];
        }
        Debug.Log("使用了未定义的敌人类型");
        return null;
        //if (EnemyDIC.ContainsKey(type))
        //{
        //    instance = CreateInstance(EnemyDIC[type].gameObject).GetComponent<Enemy>();
        //    HealthBar healthInstance = CreateInstance(healthBarPrefab.gameObject).GetComponent<HealthBar>();
        //    instance.Initialize(Random.Range(-pathOffset, pathOffset), healthInstance);
        //}
        //else
        //{
        //    Debug.LogWarning("使用了未定义的敌人类型");
        //}
    }

  

}
