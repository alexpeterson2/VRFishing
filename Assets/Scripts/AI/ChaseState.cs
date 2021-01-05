using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public ChaseState(StateController stateController) : base(stateController) { }

    public override void CheckTransition()
    {
        // Checks to see if the fish is too far away to see the hook
        if (!stateController.CheckIfInRange("Hook"))
        {
            stateController.SetState(new SwimState(stateController));
        }
        // Checks to see if the fish is close enough to bite the hook
        if (stateController.CheckIfBit("Hook"))
        {
            stateController.SetState(new HookedState(stateController));
        }
    }

    public override void Act()
    {
        if(stateController.fishHook != null)
        {
            // Swims torwards the fish hook
            stateController.ai.SwimTo(stateController.fishHook.transform);
        }
    }

    public override void OnStateEnter()
    {
        stateController.manager.isChasing = true;
    }

    public override void OnStateExit()
    {
        stateController.manager.isChasing = false;
    }
}
