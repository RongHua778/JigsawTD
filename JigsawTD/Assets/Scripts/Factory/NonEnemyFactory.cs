using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Factory/NonEnemyFactory", fileName = "nonEnemyFactory")]
public class NonEnemyFactory : GameObjectFactory
{
    [SerializeField] Aircraft aircraftPrefab;
    public Aircraft GetAircraft()
    {
        Aircraft aircraft = ObjectPool.Instance.Spawn(aircraftPrefab) as Aircraft;
        return (aircraft);
    }
}
