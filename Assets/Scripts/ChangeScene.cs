using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : TwistInteractable
{
    public string sceneName = "UnlitScene";
    override public void Interact(Transform player, RaycastHit hit)
    {
        SceneManager.LoadScene(sceneName);
    }
}
