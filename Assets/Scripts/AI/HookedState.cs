using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookedState : State
{
    public HookedState(StateController stateController) : base(stateController) { }

    public override void CheckTransition()
    {
        if (stateController.CheckIfCaught())
        {
            stateController.SetState(new CaughtState(stateController));
        }
    }

    public override void Act()
    {
        
    }

    public override void OnStateEnter()
    {
        stateController.manager.onHook = true;
        stateController.manager.isChasing = false;

        if (stateController.fishHook != null)
        {
            stateController.fishHook.GetComponent<TetherHook>().HookFish(stateController.ai.transform);

            stateController.ai.Hooked(stateController.fishHook.transform);
        }
    }

    public override void OnStateExit()
    {
        stateController.manager.onHook = false;
    }
}
