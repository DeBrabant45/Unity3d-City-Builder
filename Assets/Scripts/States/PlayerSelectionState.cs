using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    public PlayerSelectionState(GameManager gameManager) 
        :base(gameManager)
    {

    }
    public override void EnterState(string model)
    {
        base.EnterState(model);
    }

    public override void OnCancel()
    {
        return;
    }
}
