using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NullState : State
{

    public NullState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter(State oldState)
    {
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
    }

    public override void AnyKeyCall(InputAction.CallbackContext context)
    {
    }
}