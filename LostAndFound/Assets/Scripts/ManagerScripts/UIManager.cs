using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cameraM;
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject settingsMenuFirst;
    [SerializeField] private GameObject exitMenuFirst;
    [SerializeField] private GameObject creditsMenuFirst;




    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }
    public void currentMenu(GameObject menu)
    {
        EventSystem.current.SetSelectedGameObject(menu);
    }
    public void activateCamera(CinemachineVirtualCamera target)
    {
        //target.gameObject.SetActive(true);

        target.Priority= 15;
    }
    public void deactivateCamera(CinemachineVirtualCamera target)
    {
        target.Priority = 10;
        //target.gameObject.SetActive(false);
    }
    public void loadGame()
    {
        SceneManager.LoadScene("UlisesScene");
    }

    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void loadCutscene()
    {
        SceneManager.LoadScene("MiguelAScene");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
