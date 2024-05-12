using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Serializable]
    private class Inventory
    {
        [SerializeField] public GameObject slot;
        [SerializeField] public int slotID;
        
        [SerializeField] public GameObject objectInGame;

        public Inventory(GameObject slot, int slotID, GameObject objectInGame)
        {
            this.slot = slot;
            this.slotID = slotID;
            
            this.objectInGame = objectInGame;
        }
    }
    [Header("INVENTORY")]
    [SerializeField] private Inventory[] inventories;

    [SerializeField] private GameObject notePad;

    public int currentSlot;

    public GameObject player;

    private PlayerInput playerInput;

    private PlayerController playerController;

    private float InventoryInput;

    public float timer;

    [SerializeField] private SanityMeter mySanityMeter;

    private Animator myAnimator;
    [Header("CAMERA")]
    [SerializeField] private GameObject CinemachineCameraPrincipal;

    [SerializeField] private GameObject CinemachineCameraSecondary;

    [SerializeField] private GameObject CameraUI;

    [SerializeField] private PhotoManager photoManager;

    [SerializeField] private GameObject normalUI;

    [SerializeField] private float maxDistance;

    [SerializeField] private CardBoardManager cardBoardManager;

    [SerializeField] private Animator photoTakenAnimator;

    [SerializeField] private LayerMask photoLayerMask;
    [Header("---FlashLight---")]
    public bool flashlightOn;
    [SerializeField] private GameObject FlashLightGO;
    private float inventoryInputs()
    {
        InventoryInput = playerInput.actions["Inventory"].ReadValue<float>();
        
        return InventoryInput;

    }

    // Start is called before the first frame update
    void Start()
    {

        playerInput.actions["ActivateCamera"].started += InventoryManager_ActivateCamera;

        playerInput.actions["ActivateCamera"].canceled += InventoryManager_DesactivateCamera;

        playerInput.actions["TakePhoto"].started += InventoryManager_TakePhoto;

        playerInput.actions["Lights"].started += InventoryManager_FlashLightOn;

        playerInput.actions["InventoryController+"].started += InventoryManager_InventoryPositive;

        playerInput.actions["InventoryController-"].started += InventoryManager_InventoryNegative;
    }

    private void InventoryManager_InventoryNegative(InputAction.CallbackContext obj)
    {
        currentSlot--;
    }

    private void InventoryManager_InventoryPositive(InputAction.CallbackContext obj)
    {
        currentSlot++;
    }

    private void InventoryManager_FlashLightOn(InputAction.CallbackContext obj)
    {
        if (currentSlot == 1 && flashlightOn == false && !playerController.isDriving && !playerController.isCopiling)
        {
            FlashLightOn();
            return;
        }
        if (currentSlot == 1 && flashlightOn == true)
        {
            FlashLightOff();
            return;
        }
        
        if (currentSlot != 1)
        {
            FlashLightOff();
        }
    }
    
    private void InventoryManager_ActivateCamera(InputAction.CallbackContext obj)
    {
        if (currentSlot == 2)
        {
            normalUI.SetActive(false);
            myAnimator.SetBool("CameraMode", true);
        }
        if (currentSlot != 2)
        {
            normalUI.SetActive(true);
            myAnimator.SetBool("CameraMode", false);
        }
    }

    private void InventoryManager_DesactivateCamera(InputAction.CallbackContext obj)
    {
        if (currentSlot == 2)
        {
            normalUI.SetActive(true);
            myAnimator.SetBool("CameraMode", false);
        }
    }

    private void InventoryManager_TakePhoto(InputAction.CallbackContext obj)
    {
        RaycastHit hit;
        if (Physics.Raycast(CinemachineCameraSecondary.transform.position, CinemachineCameraSecondary.transform.forward,out hit,maxDistance, photoLayerMask))
        {
            photoManager.numberPhoto = hit.transform.gameObject.GetComponent<PhotoScript>().ID;
            cardBoardManager.noteNumber = hit.transform.gameObject.GetComponent<PhotoScript>().ID;
            hit.transform.gameObject.GetComponent<PhotoScript>().photoCollider.enabled = false;
            mySanityMeter.RecoverSanity();
            photoTakenAnimator.Play("photoTaken");

            if (myAnimator.GetBool("CameraMode"))
            {
                cardBoardManager.spawnNote();
                photoManager.Screenshot();
                myAnimator.SetBool("CameraMode", false);
            }
        }
        
    }
    private void Awake()
    {
        playerInput = player.GetComponent<PlayerInput>();

        myAnimator = GetComponent<Animator>();

        playerController = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {


        //Debug.Log(InventoryInput);
        myAnimator.SetInteger("Slot", currentSlot);
                                         
        timer = timer +Time.deltaTime;
        if (timer > 0.1f)
        {
            if (inventoryInputs() == 0) return;

            if (inventoryInputs() >= 0.1)
            {
                Debug.Log("mas slot");
                currentSlot++;
                InventoryInput = 0;
            }
            if (inventoryInputs() <= -0.1)
            {
                currentSlot--;
                Debug.Log("menos slot");
            }

            if (currentSlot >= 4)
            {
                currentSlot = 1;
            }

            if (currentSlot <= 0)
            {
                currentSlot = 3;
            }
        }

        InventoryInput = 0;
    }

 



    public void Slot1F()
    {

        Inventory firstSlot = inventories[0];

        Inventory secondSlot = inventories[1];

        Inventory thirdSlot = inventories[2];

        GameObject Object1 = firstSlot.objectInGame;

        GameObject Object2 = secondSlot.objectInGame;

        GameObject Object3 = thirdSlot.objectInGame;

        Object1.SetActive(true);

        Object2.SetActive(false);

        Object3.SetActive(false);

        GameObject Slot1 = firstSlot.slot;

        GameObject Slot2 = secondSlot.slot;

        GameObject Slot3 = thirdSlot.slot;

        Slot1.SetActive(true);

        Slot2.SetActive(false);

        Slot3.SetActive(false); 

        timer = 0;

        Debug.Log("Slot1");

        CameraDesactivation();

        notePad.SetActive(false);

    }

    public void Slot2F()
    {
        Debug.Log("Slot2");

        Inventory firstSlot = inventories[0];

        Inventory secondSlot = inventories[1];

        Inventory thirdSlot = inventories[2];

        GameObject Object1 = firstSlot.objectInGame;

        GameObject Object2 = secondSlot.objectInGame;

        GameObject Object3 = thirdSlot.objectInGame;

        Object1.SetActive(false);

        Object2.SetActive(true);

        Object3.SetActive(false);

        GameObject Slot1 = firstSlot.slot;

        GameObject Slot2 = secondSlot.slot;

        GameObject Slot3 = thirdSlot.slot;

        Slot1.SetActive(false);

        Slot2.SetActive(true);

        Slot3.SetActive(false);

        timer = 0;

        notePad.SetActive(false);

        FlashLightOff();
    }

    public void Slot3F()
    {


        Inventory firstSlot = inventories[0];

        Inventory secondSlot = inventories[1];

        Inventory thirdSlot = inventories[2];

        GameObject Object1 = firstSlot.objectInGame;

        GameObject Object2 = secondSlot.objectInGame;

        GameObject Object3 = thirdSlot.objectInGame;

        Object1.SetActive(false);

        Object2.SetActive(false);

        Object3.SetActive(true);

        GameObject Slot1 = firstSlot.slot;

        GameObject Slot2 = secondSlot.slot;

        GameObject Slot3 = thirdSlot.slot;

        Slot1.SetActive(false);

        Slot2.SetActive(false);

        Slot3.SetActive(true);

        timer = 0;

        Debug.Log("Slot3");

        CameraDesactivation();

        FlashLightOff();

        notePad.SetActive(false);
    }
    public void OptionsF()
    {


        Inventory firstSlot = inventories[0];

        Inventory secondSlot = inventories[1];

        Inventory thirdSlot = inventories[2];

        GameObject Object1 = firstSlot.objectInGame;

        GameObject Object2 = secondSlot.objectInGame;

        GameObject Object3 = thirdSlot.objectInGame;

        Object1.SetActive(false);

        Object2.SetActive(false);

        Object3.SetActive(false);

        GameObject Slot1 = firstSlot.slot;

        GameObject Slot2 = secondSlot.slot;

        GameObject Slot3 = thirdSlot.slot;

        Slot1.SetActive(false);

        Slot2.SetActive(false);

        Slot3.SetActive(false);

        timer = 0;

        Debug.Log("Options");

        CameraDesactivation();

        FlashLightOff();

        notePad.SetActive(true);
    }

    public void CameraActivation()
    {
        CinemachineCameraPrincipal.SetActive(false);
        CinemachineCameraSecondary.SetActive(true);
        CameraUI.SetActive(true);
    }

    public void CameraDesactivation()
    {
        CinemachineCameraPrincipal.SetActive(true);
        CinemachineCameraSecondary.SetActive(false);
        CameraUI.SetActive(false);
        
    }
    public void FlashLightOn()
    {
        if (!playerController.isDriving || !playerController.isCopiling)
        {
            FlashLightGO.SetActive(true);
            flashlightOn = true;
            
        }
    }

    public void FlashLightOff()
    {
        
            FlashLightGO.SetActive(false);
            flashlightOn = false;
            
    }

    public void deactivateAllGO()
    {
        Inventory firstSlot = inventories[0];

        Inventory secondSlot = inventories[1];

        Inventory thirdSlot = inventories[2];

        GameObject Object1 = firstSlot.objectInGame;

        GameObject Object2 = secondSlot.objectInGame;

        GameObject Object3 = thirdSlot.objectInGame;

        Object1.SetActive(false);

        Object2.SetActive(false);

        Object3.SetActive(false);
    }
    
}
