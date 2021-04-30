using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void execute();
}

public class MoveCommand : Command
{
    public override void execute()
    {

    }
}