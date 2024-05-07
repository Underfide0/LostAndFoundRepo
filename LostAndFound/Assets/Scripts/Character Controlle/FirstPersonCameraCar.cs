using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraCar : MonoBehaviour
{
    [Header("Camera and mouse")]
    public Transform playerTransform;
    public float mouseSensitivity = 2f;
    public float controllerSensitivity = 10f;
    float cameraVerticalRotation = 0;
    float cameraHorizontalRotation = 0;
    bool lockedCursor = true;

    [Header("Limit Rotation")]
    public float maxYRot ;
    public float minYRot ;

    public GameObject player;

    private PlayerInput playerInput;

    private Vector2 Inputs()
    {
        return playerInput.actions["CameraMovement"].ReadValue<Vector2>();

    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }
    private void Awake()
    {
        playerInput = player.GetComponent<PlayerInput>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;

        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        float inputXC = Inputs().x * controllerSensitivity;

        float inputYC = Inputs().y * controllerSensitivity;
        //Rotacion vertical de la camara jeje

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -50f, 50f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        cameraVerticalRotation -= inputYC;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -50f, 50f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;


        //Rotacion horizontal de la camara jeje

        playerTransform.Rotate(Vector3.up * inputX);

        playerTransform.Rotate(Vector3.up * inputXC);

        LimitRotationX();
    }
    private void LimitRotationX()
    {
        Vector3 playerEulerAngles = playerTransform.localRotation.eulerAngles;

        playerEulerAngles.y = (playerEulerAngles.y > 180) ? playerEulerAngles.y - 360 : playerEulerAngles.y;
       playerEulerAngles.y = Mathf.Clamp(playerEulerAngles.y, minYRot, maxYRot);

        playerTransform.localRotation = Quaternion.Euler(playerEulerAngles);
    }
}  



