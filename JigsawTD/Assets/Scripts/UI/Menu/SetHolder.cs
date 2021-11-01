using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHolder : MonoBehaviour
{
    [SerializeField] protected TurretSlot[] turretSlots = default;
    [SerializeField] protected Text m_PickCountTxt = default;
    protected List<ItemLockInfo> SaveList;

    [SerializeField] protected int maxSelectedCount = default;
    public virtual int MaxSelectCount => maxSelectedCount;

    private int selectedCount;
    public virtual int SelectedCount
    {
        get => selectedCount;
        set
        {
            selectedCount = Mathf.Clamp(value, 0, MaxSelectCount);
            m_PickCountTxt.text = GameMultiLang.GetTraduction("SELECTEDCOUNT") + selectedCount + "/" + MaxSelectCount;
        }
    }

    public virtual bool CheckSet()
    {
        if (SelectedCount < MaxSelectCount)
        {
            MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOENOUGHELEMENT"));
            return false;
        }
        else
        {
            SetData();
            return true;
        }
    }

    protected void SetData()
    {
        //for (int i = 0; i < turretSlots.Length; i++)
        //{
        //    SaveList[i].isSelect = turretSlots[i].IsSelect;
        //    SaveList[i].isLock = turretSlots[i].IsLock;
        //}
    }
}
