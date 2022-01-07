using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks;

public enum BillBoard
{
    Endless
}

public class UIBillBoard : IUserInterface
{
    Animator anim;
    [SerializeField] BillboardItem billboardItemPrefab = default;
    [SerializeField] Transform contentParent = default;
    List<BillboardItem> m_Items = new List<BillboardItem>();

    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
    }

    public override void Show()
    {
        base.Show();
        anim.SetBool("isOpen", true);
        SetBillBoard(BillBoard.Endless);
    }

    public override void ClosePanel()
    {
        anim.SetBool("isOpen", false);
    }

    public void SetBillBoard(BillBoard billboard)
    {
        ClearBillBoard();
        switch (billboard)
        {
            case BillBoard.Endless:
                foreach (var entry in SteamLeaderboard.EndlessLeaderBoard)
                {
                    BillboardItem billBoardItem = Instantiate(billboardItemPrefab, contentParent);
                    billBoardItem.SetContent(entry.m_nGlobalRank, SteamFriends.GetFriendPersonaName(entry.m_steamIDUser), entry.m_nScore);
                    m_Items.Add(billBoardItem);
                }
                break;
        }
    }


    private void ClearBillBoard()
    {
        foreach (var item in m_Items)
        {
            Destroy(item.gameObject);
        }
        m_Items.Clear();
    }
}
