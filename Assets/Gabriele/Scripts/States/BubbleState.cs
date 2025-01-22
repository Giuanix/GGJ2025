using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BubbleState : State
{
    public BubbleState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter(State oldState)
    {
    }

    public override void OnExit()
    {
        player.rb.simulated = true;
    }

    public override void Update()
    {
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        //animation stretch Bubble
    }

}