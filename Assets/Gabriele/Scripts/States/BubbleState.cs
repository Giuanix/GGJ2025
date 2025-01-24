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
        player.rb.simulated = false;
        player.GetComponent<Collider2D>().isTrigger = true;
        player.animator.SetLayerWeight(2, 1);

    }

    public override void OnExit()
    {
        player.rb.simulated = true;
        player.GetComponent<Collider2D>().isTrigger = false;
        player.animator.SetLayerWeight(2, 0);
        AudioManager.instance.PlayExplosionBubble();
    }

    public override void Update()
    {
    }

    public override void JumpCall(InputAction.CallbackContext context)
    {
        //animation stretch Bubble
    }

    public override void AnyKeyCall(InputAction.CallbackContext context)
    {
        if(player.transform.parent.TryGetComponent<IncapsulateBubble>(out IncapsulateBubble bubble))
        {
            bubble.TryToBeFree();
            AudioManager.instance.PlayTryToBeFree();
        }
    }
}