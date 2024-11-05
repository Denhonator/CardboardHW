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
    override public void Interact(RaycastHit hit)
    {
        player.transform.position = hit.point + Vector3.up * 1.6f;
    }
}
