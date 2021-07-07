using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Factory/NonEnemyFactory", fileName = "nonEnemyFactory")]
public class NonEnemyFactory : GameObjectFactory
{
    [SerializeField] AirAttacker aircraftPrefab;
    [SerializeField] AirProtector strongerAircraftPrefab;
    public AirAttacker GetAirAttacker()
    {
        AirAttacker aircraft = ObjectPool.Instance.Spawn(aircraftPrefab) as AirAttacker;
        return (aircraft);
    }

    public AirProtector GetAirProtector()
    {
        AirProtector aircraft = ObjectPool.Instance.Spawn(strongerAircraftPrefab) as AirProtector;
        return (aircraft);
    }
}
