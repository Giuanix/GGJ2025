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
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        player.Movement();
        
        if (!player.floatingObject.IsInWater())
            player.ChangeState(player.jumpState);
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        //Act like swim
        if (context.performed && player.floatingObject.IsInWater())
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }
    }
   
}