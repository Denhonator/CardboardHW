using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportArea : TiltInteractable
{
    GameObject player;
    private void Update()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
    }
    override public bool Interact(Transform crosshair, Transform lineup, RaycastHit hit)
    {
        if (base.Interact(crosshair, lineup, hit))
        {
            if (player)
            {
                player.transform.position = hit.point + Vector3.up * 1.6f;
            }
            return true;
        }
        return false;
    }
}
