using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardboardUI;
    public GameObject[] notes;
    [SerializeField] private GameObject Camera;

    public int noteNumber;


    private void Start()
    {
        
    }
    

    public void spawnNote()
    {
        GameObject nota = notes[noteNumber];
        nota.SetActive(true);
    }

    public void closeCardboard()
    {
        cardboardUI.SetActive(false);
        Time.timeScale = 1;
        Camera.GetComponent<FirstPersonCamera>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void changePanel(GameObject menu)
    {
        EventSystem.current.SetSelectedGameObject(menu);
    }
}
