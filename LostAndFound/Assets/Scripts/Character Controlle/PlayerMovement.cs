using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("---Player movement---")]

    private Vector3 velocity;
    private Vector3 PlayerMovementInput;
    public float Speed = 5f;
    public float SpeedRun = 8f;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isWalking;
    private Vector3 moveVector;
    [SerializeField] private float gravity = 30.0f;
    [Header("---Player booleans---")]

    [SerializeField] bool isMovementPressed;
    bool isGrounded;
    bool canSprint;
    bool canJump =true;
    [SerializeField] private bool canUseHeadbob;
    private Animator myAnimator;
    private CharacterController controller;
    private PlayerInput playerInput;

    [Header("---Player Satmina---")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float StaminaUseMultiplier = 30;
    [SerializeField] private float timeBeforeStaminaRegens = 5;
    [SerializeField] private float staminaValueIncrement = 5;
    [SerializeField] private float staminaTimeIncrement = 1f;
    private bool useStamina = true;
    private float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;
    [Header("---Player Jump---")]
    [SerializeField] private float jumpForce = 8.0f;
    

    [Header("---Player Headbob---")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    private float defaultYpos = 0;
    private float timer;
    [SerializeField] private GameObject playerCamera;
    
    private Vector2 Inputs()
    {
        return playerInput.actions["Movement"].ReadValue<Vector2>();

    }


    public void Awake()
    {
        currentStamina = maxStamina;
        defaultYpos = playerCamera.transform.localPosition.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        myAnimator = GetComponent<Animator>();
        playerInput.actions["Jump"].started += PlayerMovement_Jump;
        playerInput.actions["Run"].started += PlayerMovement_Run;
        playerInput.actions["Run"].canceled += PlayerMovement_RunCancel;
    }

    private void PlayerMovement_Run(InputAction.CallbackContext obj)
    {
        isRunning = true;
    }
    private void PlayerMovement_RunCancel(InputAction.CallbackContext obj)
    {
        isRunning = false;
    }
    private void PlayerMovement_Jump(InputAction.CallbackContext obj)
    {
        if (controller.isGrounded)
        {
            handleJump();
            Debug.Log("saltooo");
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isRunning && canSprint)
        {
            Speed = SpeedRun;
        }
        else if (!isRunning)
        {
            Speed = 5f;
        }

        MovePlayer();

        AnimationChanger();

        if (useStamina)
        {
            handleStamina();
        }

        if (currentStamina <= 0 && isRunning)
        {
            isRunning = false;
        }

        if (canUseHeadbob)
        {
            handleHeadBob();
        }
        
        applyFinalMovements();

        
    }

    private void MovePlayer()
    {
        PlayerMovementInput = new Vector2(Speed * Inputs().y, Speed * Inputs().x);
        float moveDirectionY = moveVector.y;
        moveVector = (transform.TransformDirection(Vector3.forward) * PlayerMovementInput.x) + (transform.TransformDirection(Vector3.right) * PlayerMovementInput.y);
        
        moveVector.y = moveDirectionY;

        isMovementPressed = Inputs().x !=0f || Inputs().y !=0f;

        if (isMovementPressed)
        {
            isWalking = true;
        }
        else { isWalking = false; }
    }

    private void AnimationChanger()
    {

        if (isWalking)
        {
            myAnimator.SetBool("isWalking", true);
        }
        else if (!isWalking)
        {
            myAnimator.SetBool("isWalking", false);
        }

        if (controller.isGrounded)
        {
            myAnimator.SetBool("isGrounded", true);
        }
        else if (!controller.isGrounded) 
        {
            myAnimator.SetBool("isGrounded", false);
        }

        if (isRunning)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else if (!isRunning) 
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    private void handleStamina()
    {
        if ( isRunning && PlayerMovementInput != Vector3.zero)
        {
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            currentStamina -= StaminaUseMultiplier * Time.deltaTime;



            if(currentStamina < 0)
            {
                currentStamina = 0;
            }

            OnStaminaChange ?.Invoke(currentStamina);

            if(currentStamina <= 0)
            {
                canSprint = false;
            }
        }

        if (!isRunning && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }

    private void handleHeadBob()
    {
        if (!controller.isGrounded) return;
        
        if (Mathf.Abs(moveVector.x) > 0.1f || Mathf.Abs(moveVector.z) > 0.1f)
        {
            timer += Time.deltaTime * (isWalking ? walkBobSpeed : sprintBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYpos + Mathf.Sin(timer) * (isWalking ? walkBobAmount : sprintBobAmount)
                , playerCamera.transform.localPosition.z);
        }
    }

    private void handleJump()
    {
        
        moveVector.y = jumpForce;
        
    }
    private IEnumerator RegenerateStamina()
    {

        yield return new WaitForSeconds(timeBeforeStaminaRegens);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);
        
        while(currentStamina < maxStamina)
        {
            if (currentStamina > 0)
            {
                canSprint = true;
            }

            currentStamina += staminaValueIncrement;

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }

            OnStaminaChange?.Invoke(currentStamina);

            yield return timeToWait;
        }

        regeneratingStamina = null;
    }

    private void applyFinalMovements()
    {
        if (!controller.isGrounded)
            moveVector.y -= gravity * Time.deltaTime;

       controller.Move(moveVector * Time.deltaTime);
        
    }
}
