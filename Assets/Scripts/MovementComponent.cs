using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{

    [SerializeField]
    private float walkSpeed = 5;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float jumpForce = 5;

    private PlayerController playerController;

    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector2 lookInput = Vector2.zero;

    public float aimSensitivity = 1.0f;
    Rigidbody rigidBody;
    Animator playerAnimator;
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");
    public readonly int verticalAimHash = Animator.StringToHash("AimVertical");

    public GameObject followTarget;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
     

    }

    void Start()
    {
        if (!GameManager.instance.cursorActive)
        {
            AppEvents.InvokeMouseCursorEnable(false);
        }


    }
    // Update is called once per frame
    void Update()
    {
        //AIMING
        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);

        followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.left);

        var angles = followTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.transform.localEulerAngles.x;

        float min = -60;
        float max = 70.0f;
        float range = max - min;
        float offsetToZero = 0 - min;
        float aimAngle = followTarget.transform.localEulerAngles.x;
        aimAngle = (aimAngle > 180) ? aimAngle - 360 : aimAngle;
        float val = (aimAngle + offsetToZero) / (range);
        print(val);
        playerAnimator.SetFloat(verticalAimHash, val);


        if (angle > 180 && angle < 300)
        {
            angles.x = 300;
        }
        else if (angle < 180 && angle > 70)
        {
            angles.x = 70;
        }
        //followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensitivity, Vector3.up);
        //followTarget.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensitivity, Vector3.left);

        //var angles = followTarget.transform.localEulerAngles;
        //angles.z = 0;
        //var angle = followTarget.transform.localEulerAngles.x;
        //if (angle > 180 && angle < 300)
        //{
        //    angles.x = 300;
        //}
        //else if (angle < 180 && angle > 70)
        //{
        //    angles.x = 70;
        //}

        //followTarget.transform.localEulerAngles = angles;
        //transform.rotation = Quaternion.Euler(0, followTarget.transform.rotation.eulerAngles.y, 0);
        //followTarget.transform.localEulerAngles = new Vector3(angles.x, 0, 0);


        if (playerController.isJumping) return;
        if (!(inputVector.magnitude > 0)) moveDirection = Vector3.zero;
        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;
        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);
        transform.position += movementDirection;

     


    }
    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        playerAnimator.SetFloat(movementXHash, inputVector.x);
        playerAnimator.SetFloat(movementYHash, inputVector.y);
    }
    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        playerAnimator.SetBool(isRunningHash, playerController.isRunning);
    }
    public void OnJump(InputValue value)
    {
       if(playerController.isJumping)
        {
            return;
        }
        playerController.isJumping = value.isPressed;
        rigidBody.AddForce((transform.up + moveDirection)*jumpForce, ForceMode.Impulse);
        playerAnimator.SetBool(isJumpingHash, playerController.isJumping);
    }

    public void OnAim(InputValue value)
    {
        playerController.isAiming = value.isPressed;
       
    }
    public void OnLook(InputValue value)
    {

        lookInput= value.Get<Vector2>();
       
       
      
    }
   


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") && !playerController.isJumping) return;
        playerController.isJumping = false;
        playerAnimator.SetBool(isJumpingHash, false);
    }
}
