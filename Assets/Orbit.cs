using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : TiltInteractable
{
    float speed = 1.0f;
    override public void Interact(RaycastHit hit)
    {
        speed = Random.Range(1.0f, 100.0f);
    }
    void Update()
    {
        transform.Rotate(0, speed*Time.deltaTime, 0);
    }
}

