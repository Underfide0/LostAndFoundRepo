using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winAndDeathScript : MonoBehaviour
{
    public void changeToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
