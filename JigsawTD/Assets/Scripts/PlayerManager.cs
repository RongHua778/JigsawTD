using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWish
{
    Element, Composition, none
}
public class PlayerManager : MonoBehaviour
{
    public LevelUIManager levelUIManager;
    [SerializeField] private BlueprintFactory blueprintFactory;
    //List<Blueprint> blueprintsInPocket = new List<Blueprint>();
    // Blueprint blueprintInBuilding;
    PlayerWish playerWish;
    public PlayerWish PlayerWish { get => playerWish; set => playerWish = value; }


    public Blueprint GetSingleBluePrint(TurretAttribute attribute)
    {
        Blueprint bluePrint = blueprintFactory.GetComposedTurret(attribute);
        return bluePrint;
    }
}

//Íæ¼ÒÏëÒªÉ¶

