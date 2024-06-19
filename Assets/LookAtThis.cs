using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtThis : MonoBehaviour
{
    public void OnPointerEnter()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.cyan;
    }

    public void OnPointerExit()
    {
        GetComponent<Renderer>().sharedMaterial.color = Color.magenta;
    }
}
