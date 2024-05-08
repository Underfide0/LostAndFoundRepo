using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Security;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    private PlayerInput playerInput;
    private Collider myCollider;
    [SerializeField] private float maxDistance;
    [SerializeField] private GameObject InteractableUI;
    private PlayerMovement playerMovement;
    [SerializeField] private Animator myAnimator;
    private InventoryManager inventoryManager;

    [Header("CAR")]
    [SerializeField] private GameObject car;
    public bool isDriving;
    public bool isCopiling;
    [SerializeField]private bool pilot, copilot;
    [SerializeField] Transform insideCar, outsideCar;
    [SerializeField] Transform insideCarCopilot, outsideCarCopilot;
    [SerializeField] private LayerMask carLayer;

    [Header("Cameras")]
    [SerializeField] private GameObject Camera;

    [Header("CardBoard")]
    [SerializeField] private GameObject cardBoardTrigger;
    [SerializeField] private bool insideCardboard;
    [SerializeField] private LayerMask cardboardLayer;
    [SerializeField] private GameObject cardboardUI;
    [SerializeField] private GameObject closeButton;

    [Header("OptionsMenu")]
    private bool isPaused;
    
    [SerializeField] GameObject firstOption;

    [Header("Enemy")]
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private GameObject enemyHand;
    [SerializeField] private GameObject normalUI;


    // Start is called before the first frame update
    void Start()
    {
        playerInput.actions["Interact"].performed += PlayerController_Interact;

        playerInput = GetComponent<PlayerInput>();

        playerMovement = GetComponent<PlayerMovement>();

        playerInput.actions["Pause"].started += PauseMenuManager_started;

       
    }

    private void PauseMenuManager_started(InputAction.CallbackContext obj)
    {
        if (!isPaused)
        {
            myAnimator.SetBool("isPaused", true);
        }
        else
        {
            Unpause();
        }
    }
    private void PlayerController_Interact(InputAction.CallbackContext context)
    {
        
        
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, maxDistance, carLayer))
        {
            if (isDriving&& pilot)
            {
                exitCar();
                
                return;

            }
            if (!isDriving&& pilot)
            {
                enterCar();
               
                return;

            }
            if (isCopiling&&copilot)
            {
                exitCarCopilot();
                
                return;

            }
            if (!isCopiling && copilot)
            {
                enterCarCopilot();
                
                return;

            }
            
        }
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, maxDistance, cardboardLayer))
        {
            if (insideCardboard)
            {
                cardboardInteract();
            }
        }
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        myCollider = GetComponent<Collider>();

        car.GetComponent<CarController>();

        myAnimator = GetComponent<Animator>();

        inventoryManager = GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.transform.position, Camera.transform.forward * 30f, Color.green);

        RaycastChecker();

        if (isDriving == true)
        {
            transform.position = insideCar.position;
        }
        if (isCopiling == true)
        {
            transform.position = insideCarCopilot.position;
        }
    }

    private void enterCar()
    {
            playerMovement.enabled = false;
            myCollider.enabled = false;
            transform.position = insideCar.position;
            transform.parent = car.transform;
            car.GetComponent<CarController>().enabled = true;
            this.gameObject.transform.IsChildOf(car.transform);
            isDriving=true;
            car.GetComponent<CarStopper>().enabled = false;
            Camera.GetComponent<FirstPersonCamera>().enabled = false;
            Camera.GetComponent<FirstPersonCameraCar>().enabled = true;
            transform.localRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            car.transform.rotation = Quaternion.Euler(0, car.transform.eulerAngles.y, 0);
    }

    private void enterCarCopilot()
    {
        playerMovement.enabled = false;
        myCollider.enabled = false;
        transform.position = insideCarCopilot.position;
        transform.rotation = insideCarCopilot.rotation;
        transform.parent = car.transform;
        this.gameObject.transform.IsChildOf(car.transform);
        isCopiling = true;
        car.GetComponent<CarStopper>().enabled = false;
        Camera.GetComponent<FirstPersonCamera>().enabled = false;
        Camera.GetComponent<FirstPersonCameraCar>().enabled = true;
        transform.localRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        car.transform.rotation= Quaternion.Euler(0, car.transform.eulerAngles.y, 0);
    }

    private void exitCarCopilot()
    {
        playerMovement.enabled = true;
        transform.position = outsideCarCopilot.position;
        myCollider.enabled = true;
        transform.parent = null;
        isCopiling = false;
        car.GetComponent<CarStopper>().enabled = true;
        Camera.GetComponent<FirstPersonCamera>().enabled = true;
        Camera.GetComponent<FirstPersonCameraCar>().enabled = false;
        transform.localRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        car.transform.localRotation = Quaternion.Euler(0, car.transform.eulerAngles.y, 0);
    }
    private void exitCar()
    {
            playerMovement.enabled = true;
            transform.position = outsideCar.position;
            myCollider.enabled = true;
            transform.parent = null;
            car.GetComponent<CarController>().enabled = false;
            isDriving = false;
            car.GetComponent<CarStopper>().enabled = true;
            Camera.GetComponent<FirstPersonCamera>().enabled = true;
            Camera.GetComponent<FirstPersonCameraCar>().enabled = false;
            transform.localRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            car.transform.localRotation = Quaternion.Euler(0, car.transform.eulerAngles.y, 0);
    }

    private void RaycastChecker()
    {
        if (pilot)
        {
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, maxDistance, carLayer))
            {
                InteractableUI.SetActive(true);
               

            }
            else
            {
                InteractableUI.SetActive(false);
            }
        }

        if (copilot)
        {
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, maxDistance, carLayer))
            {
                InteractableUI.SetActive(true);
               

            }
            else
            {
                InteractableUI.SetActive(false);
            }
        }

        if (insideCardboard)
        {
            if(Physics.Raycast(Camera.transform.position, Camera.transform.forward, maxDistance, cardboardLayer))
            {
                InteractableUI.SetActive(true);
            }
            else
            {
                InteractableUI.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarPilot"))
        {
            pilot = true;
            
        }

        if (other.CompareTag("CarCoPilot")) 
        {
            copilot = true;
            
        }

        if (other.CompareTag("Cardboard"))
        {
            insideCardboard = true;
        }

        if (other.CompareTag("Enemy"))
        {
            Death();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CarPilot"))
        {
            InteractableUI.SetActive(false);
            
            pilot = false;
        }
        if (other.CompareTag("CarCoPilot"))
        {
            InteractableUI.SetActive(false);    
          
            copilot = false;
        }

        if (other.CompareTag("Cardboard"))
        {
            insideCardboard = false;
            InteractableUI.SetActive(false);
        }
    }

    private void cardboardInteract()
    {
        InteractableUI.SetActive(false);
        cardboardUI.SetActive(true);
        Time.timeScale = 0;
        Camera.GetComponent<FirstPersonCamera>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        EventSystem.current.SetSelectedGameObject(closeButton);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Camera.GetComponent<FirstPersonCamera>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        EventSystem.current.SetSelectedGameObject(firstOption);

    }
   
    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        myAnimator.SetBool("isPaused", false);
        Camera.GetComponent<FirstPersonCamera>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    
    public void Death()
    {
        enemyScript.killPlayer();
        playerMovement.enabled = false;
        Camera.GetComponent<FirstPersonCamera>().enabled = false;
        inventoryManager.enabled = false;
        Camera.GetComponent<FirstPersonCameraCar>().enabled = false;
        Camera.transform.parent = enemyHand.transform;
        Camera.transform.localPosition = new Vector3(0, 0.00619f, 0.00111f);
        Camera.transform.localRotation = Quaternion.Euler(73.43f, 60.823f, -278.812f);
        transform.position = new Vector3(0, 0, 0);
        normalUI.SetActive(false);

    }
  


}
