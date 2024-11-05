using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtThis : TiltInteractable
{
    override public void Interact(RaycastHit hit)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(3.0f, 5.0f), Random.Range(-1.0f, 1.0f)), ForceMode.Impulse);
    }
    public void OnPointerEnter()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.cyan;
    }

    public void OnPointerExit()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.magenta;
    }
}
