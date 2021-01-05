using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    protected StateController stateController;

    public State(StateController stateController)
    {
        this.stateController = stateController;
    }

    public abstract void CheckTransition();
    public abstract void Act();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}
