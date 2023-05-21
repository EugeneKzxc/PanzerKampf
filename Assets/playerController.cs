using UnityEngine;
using Photon.Pun;

public class CharacterMovementController : MonoBehaviourPunCallbacks
{
    public CharacterController characterController;
    public Camera cam;
    public PhotonView view;
    public float moveSpeed;
    public float sprintSpeedMultiplier = 2f;
    public float jumpHeight = 3f;
    private float _gravity = -10f;
    private float _yAxisVelocity;
    public float lookSpeed = 10f;
    private float _camRotation;

    private void Start()
    {
        if(!view.IsMine)
        {
            cam.enabled = false;
            characterController.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float y = Input.GetAxis("Mouse Y");
        float x = Input.GetAxis("Mouse X");

        _camRotation -= y * lookSpeed * Time.deltaTime * 10f;
        _camRotation = Mathf.Clamp(_camRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(_camRotation, 0f, 0f);

        gameObject.transform.Rotate(x * lookSpeed * Time.deltaTime * 10f * Vector3.up);

        if (Input.GetKey(KeyCode.LeftShift))
            vertical *= sprintSpeedMultiplier;

        Vector3 movement = horizontal * moveSpeed * Time.deltaTime * transform.right +
                           vertical * moveSpeed * Time.deltaTime * transform.forward;

        if (characterController.isGrounded)
            _yAxisVelocity = -0.5f;


        if (Input.GetKeyDown(KeyCode.Space))
            _yAxisVelocity = Mathf.Sqrt(jumpHeight * -2f * _gravity);

        _yAxisVelocity += _gravity * Time.deltaTime;
        movement.y = _yAxisVelocity * Time.deltaTime;

        characterController.Move(movement);
    }
}
