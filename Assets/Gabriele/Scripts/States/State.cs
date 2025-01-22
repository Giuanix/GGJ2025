using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{
    protected PlayerController player;
    protected Animator anim;

    public State(PlayerController player)
    {
        this.player = player;
    }

    public State(PlayerController player,Animator anim)
    {
        this.player = player;
        this.anim = anim;
    }
    public abstract void OnEnter(State oldState);
    public abstract void Update();
    public abstract void OnExit();
    public abstract void JumpCall(InputAction.CallbackContext context);
}

