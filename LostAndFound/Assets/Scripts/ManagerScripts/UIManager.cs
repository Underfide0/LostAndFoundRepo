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
        
    }
    public void currentMenu(GameObject menu)
    {
        EventSystem.current.SetSelectedGameObject(menu);
    }
    public void activateCamera(CinemachineVirtualCamera target)
    {
        target.gameObject.SetActive(true);
    }
    public void deactivateCamera(CinemachineVirtualCamera target)
    {
        target.gameObject.SetActive(false);
    }
    public void loadGame()
    {
        SceneManager.LoadScene("UlisesScene");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
