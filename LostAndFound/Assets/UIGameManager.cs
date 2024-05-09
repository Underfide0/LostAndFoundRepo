using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIGameManager : MonoBehaviour
{
    [SerializeField] private Image Stamina;

    [SerializeField] private GameObject controllerInteractUI;

    [SerializeField] private GameObject keyboardInteractUI;

    void Start()
    {
        UpdateStamina(100);

        InputSystem.onDeviceChange += (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    keyboardInteractUI.SetActive(false);
                    controllerInteractUI.SetActive(true);
                    Debug.Log("Mando conectado");
                    break;
                case InputDeviceChange.Removed:
                    keyboardInteractUI.SetActive(true);
                    controllerInteractUI.SetActive(false);
                    Debug.Log("Mando desconectado");
                    break;
            }
        };
    }

    // Update is called once per frame
    private void OnEnable()
    {
        PlayerMovement.OnStaminaChange += UpdateStamina;
    }

    private void OnDisable()
    {
        PlayerMovement.OnStaminaChange -= UpdateStamina;
    }
    private void UpdateStamina(float currentStamina)
    {
        Stamina.fillAmount = currentStamina/100;
    }

}
