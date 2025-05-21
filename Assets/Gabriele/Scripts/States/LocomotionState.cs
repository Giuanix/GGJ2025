using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocomotionState : State
{

    bool canParticles;
    public LocomotionState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter(State oldState)
    {
        if (Mathf.Abs(player.rb.velocity.x) > 0.1f)
            player.particles.Play("RunDustAnimation");
    }

    public override void OnExit()
    {
        player.particles.Play("None");
        player.animator.SetBool("Falling", false);
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.jumpState.canJump = player.IsGrounded();

            player.animator.SetTrigger("Jump");
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
            player.ChangeState(player.jumpState, true);

        }
    }

    public override void Update()
    {
        player.Movement(); 
        player.Gravity();
        player.animator.SetBool("Falling", !player.IsGrounded());

        if (Mathf.Abs(player.rb.velocity.x) < 0.1f)
        {
            canParticles = true;
        }
        else if (canParticles)
        {
            canParticles = false;
            player.particles.Play("RunDustAnimation");

        }
    }

    public override void AnyKeyCall(InputAction.CallbackContext context)
    {
    }
}