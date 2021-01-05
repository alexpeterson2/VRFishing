using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimState : State
{
    public SwimState(StateController stateController) : base(stateController) { }

    public override void CheckTransition()
    {
        if (stateController.CheckIfInRange("Hook") && !stateController.manager.isChasing && !stateController.manager.onHook)
        {
            stateController.SetState(new ChaseState(stateController));
        }
    }

    public override void Act()
    {
        stateController.ai.Swim();
        stateController.ai.WasTargetReached();
    }

    public override void OnStateEnter()
    {
        stateController.ai.Swim();
    }
}
