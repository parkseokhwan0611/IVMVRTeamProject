using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Update()
    {
         if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SceneManager.LoadScene("Daniel_Scene_01");
        }
    }
}
