using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocomotionState : State
{
    public LocomotionState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter(State oldState)
    {
    }

    public override void OnExit()
    {
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        if (context.performed && player.IsGrounded())
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
            player.ChangeState(player.jumpState, true);
        }
    }

    public override void Update()
    {
        player.Movement();
        player.Gravity();
    }

}