using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : IUserInterface
{
    [SerializeField] Image eventIcon = default;
    [SerializeField] Text eventDes = default;
    [SerializeField] Text eventChoice1Des = default;
    [SerializeField] Text eventChoice2Des = default;
    [SerializeField] Text eventChoice3Des = default;
    [SerializeField] Button btn1 = default;
    [SerializeField] Button btn2 = default;

    private List<TileEvent> TileEvents = new List<TileEvent>();

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        btn1.onClick.AddListener(Hide);
        btn2.onClick.AddListener(Hide);
    }

    public void SetEvent(TileEvent tileEvent)
    {
        TileEvents.Add(tileEvent);
        eventDes.text = tileEvent.EventDes;
        eventChoice1Des.text = tileEvent.EventChoice1Des;
        eventChoice2Des.text = tileEvent.EventChoice2Des;
        eventChoice3Des.text = tileEvent.EventChoice3Des;
        btn1.onClick.AddListener(tileEvent.BtnClick1);
        btn2.onClick.AddListener(tileEvent.BtnClick2);
    }

    public void PrepareNextWave()
    {
        for(int i = 0; i < TileEvents.Count; i++)
        {
            TileEvents[i].TurnRemain--;
            if (TileEvents[i].TurnRemain <= 0)
            {
                TileEvents[i].TriggerReward();
                TileEvents.Remove(TileEvents[i]);
            }
            else
            {
                TileEvents[i].TriggerEventEffect();
            }
        }
    }

}
