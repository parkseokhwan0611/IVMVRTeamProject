using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public static bool isUiOn = false;
    void Update()
    {
         if (OVRInput.GetDown(OVRInput.Button.One) && isUiOn == true)
        {
            SceneManager.LoadScene("Daniel_Scene_01");
        }
    }
}
