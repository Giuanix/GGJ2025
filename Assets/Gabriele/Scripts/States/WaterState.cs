using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterState : State
{

    public WaterState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter(State oldState)
    {
        player.animator.SetLayerWeight(1, 1);
    }

    public override void OnExit()
    {
        player.animator.SetLayerWeight(1, 0);

    }

    public override void Update()
    {
        player.Movement();
        player.Gravity();

        if (!player.floatingObject.IsInWater())
            player.ChangeState(player.jumpState);
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        //Act like swim
        if (context.performed && player.floatingObject.IsInWater())
        {
            player.animator.SetTrigger("Jump");
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }
    }

    public override void AnyKeyCall(InputAction.CallbackContext context)
    {
    }
}