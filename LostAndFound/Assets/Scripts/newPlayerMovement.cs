using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class newPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody myRigidbody;
    private PlayerInput playerInput;
    public float speed,speedRun, maxForce;
    private Vector2 move;
    private Vector3 moveVector;

    bool canSprint;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isWalking;

    private Animator myAnimator;
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

    [Header("---Player Headbob---")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    private float defaultYpos = 0;
    private float timer;
    [SerializeField] private GameObject playerCamera;



    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    void Start()
    {
        
        playerInput.actions["Run"].started += PlayerMovement_Run;
        playerInput.actions["Run"].canceled += PlayerMovement_RunCancel;
        
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        myRigidbody = GetComponent<Rigidbody>();
        defaultYpos = playerCamera.transform.localPosition.y;
    }
    private void PlayerMovement_Run(InputAction.CallbackContext obj)
    {
        isRunning = true;
    }
    private void PlayerMovement_RunCancel(InputAction.CallbackContext obj)
    {
        isRunning = false;
    }
    private void FixedUpdate()
    {
        Move();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (isRunning && canSprint)
        {
            speed = speedRun;
        }
        else if (!isRunning)
        {
            speed = 4f;
        }

        if (useStamina)
        {
            handleStamina();
        }

        if (currentStamina <= 0 && isRunning)
        {
            isRunning = false;
        }

        animationCheck();

        handleHeadBob();
    }

    void Move()
    {

        moveVector = (transform.TransformDirection(Vector3.forward) * move.x) + (transform.TransformDirection(Vector3.right) * move.y);

        Vector3 currentVelocity = myRigidbody.velocity;

        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);

        targetVelocity *= speed;

        targetVelocity = transform.TransformDirection(targetVelocity);

        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3 (velocityChange.x, 0, velocityChange.z);

        Vector3.ClampMagnitude(velocityChange, maxForce);

        myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);


    }
    private void animationCheck()
    {
        if (move.x ==0 && move.y ==0)
        {
            myAnimator.SetBool("isWalking", false);
            myAnimator.SetBool("isRunning", false);
            isWalking = false;
        }
        else
        {
            if (isRunning)
            {
                myAnimator.SetBool("isRunning", true);
                isWalking = false;
            }
            else
            {
                myAnimator.SetBool("isWalking", true);
                isWalking = true;
            }
        }

    }
    private void handleStamina()
    {
        if (isRunning /* falta que pille si estoy moviendo con los inputs o no */)
        {
            if (regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            currentStamina -= StaminaUseMultiplier * Time.deltaTime;



            if (currentStamina < 0)
            {
                currentStamina = 0;
            }

            OnStaminaChange?.Invoke(currentStamina);

            if (currentStamina <= 0)
            {
                canSprint = false;
            }
        }

        if (!isRunning && currentStamina < maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }

    private IEnumerator RegenerateStamina()
    {

        yield return new WaitForSeconds(timeBeforeStaminaRegens);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);

        while (currentStamina < maxStamina)
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

    private void handleHeadBob()
    {
        if (Mathf.Abs(moveVector.x) > 0.1f || Mathf.Abs(moveVector.z) > 0.1f)
        {
            timer += Time.deltaTime * (isWalking ? walkBobSpeed : sprintBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYpos + Mathf.Sin(timer) * (isWalking ? walkBobAmount : sprintBobAmount)
                , playerCamera.transform.localPosition.z);
        }
    }
}
