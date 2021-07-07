using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectState : FSMState
{
    public ProtectState(FSMSystem fsm) : base(fsm)
    {
        StateID = StateID.Protect;
    }
    public override void Act(AirProtector agent)
    {
        agent.Protect();

    }
    public override void Reason(AirProtector agent)
    {

    }

}
