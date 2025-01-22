using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : State
{
    bool canJump = true;
    float jumpMultiplier = 1f;
    public JumpState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter(State oldState)
    {
        if (oldState == player.waterState)
            jumpMultiplier = 0.45f;
        else
            jumpMultiplier = 1f;
    }

    public override void OnExit()
    {
        canJump = true;
    }

    public override void Update()
    {
        player.Movement();

        if (player.IsGrounded()) player.ChangeState(player.locomotionState);
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        Debug.Log(canJump);
        if (context.performed && canJump)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce * jumpMultiplier);
            canJump = false;
        }
    }
   
}