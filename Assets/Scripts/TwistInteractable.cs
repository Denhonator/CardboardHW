using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistInteractable : MonoBehaviour
{
    public bool relative = false;
    virtual public void Interact(RaycastHit hit = new RaycastHit())
    {
    }
}
