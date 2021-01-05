using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtState : State
{
    public CaughtState(StateController stateController) : base(stateController) { }

    public override void CheckTransition()
    {
        
    }

    public override void Act()
    {
        // Add fish to caught counter
        stateController.manager.caughtFish();

        // Remove tether rope from fish and hook
        stateController.fishHook.GetComponent<TetherHook>().DetachFish();

        // Destroy the fish
        stateController.DestroyObject();
    }
}
