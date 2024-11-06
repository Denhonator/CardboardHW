using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportArea : TwistInteractable
{
    override public void Interact(Transform player, RaycastHit hit)
    {
        player.position = hit.point + Vector3.up * 1.6f;
    }
}
