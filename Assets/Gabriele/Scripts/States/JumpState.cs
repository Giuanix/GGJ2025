using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : State
{
    public bool canJump = true;
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

        AudioManager.instance.PlayJump();

        player.particles.Play("JumpDustAnimation");
    }

    public override void OnExit()
    {
        player.particles.Play("None");
        
        canJump = true;
    }

    public override void Update()
    {
        player.Movement();
        player.Gravity();

        if (player.IsGrounded()) player.ChangeState(player.locomotionState);
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        if (context.performed && canJump)
        {
            player.animator.SetTrigger("Jump");
            AudioManager.instance.PlayJump();
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce * jumpMultiplier);
            canJump = false;
        }
    }

    public override void AnyKeyCall(InputAction.CallbackContext context)
    {
    }
}