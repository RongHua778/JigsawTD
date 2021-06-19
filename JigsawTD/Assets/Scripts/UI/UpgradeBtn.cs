using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TurretTips turretTips = default;
    public void OnPointerEnter(PointerEventData eventData)
    {
        turretTips.showingTurret = false;
        turretTips.UpdateLevelUpInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        turretTips.showingTurret = true;
    }
}
