using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject turret;
    [SerializeField] GameObject body;
    [SerializeField] GameObject gun;
    [SerializeField] Camera mainCam;
    [SerializeField] Camera scope;
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject shotPoint;
    [SerializeField] GameObject bulletPrefab;

    public PhotonView view;
    public GunSync gunner;
    private Camera activeCamera;

    public float rotationSpeedGun = 10f;
    public float rotationSpeedBody = 50f;
    public float moveSpeed = 5f;

    private float _gravity = -10f;
    private float _yAxisVelocity;

    void Start()
    {
        activeCamera = mainCam;

        if (!view.IsMine)
        {
            mainCam.enabled = false;
            scope.enabled = false;
            characterController.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxis("Mouse X");

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 rotationMovement = horizontal * rotationSpeedBody * Time.deltaTime * Vector3.up;
        body.transform.Rotate(rotationMovement);

        Vector3 movement = vertical * moveSpeed * Time.deltaTime * body.transform.forward;

        if (characterController.isGrounded)
            _yAxisVelocity = -0.5f;

        _yAxisVelocity += _gravity * Time.deltaTime;
        movement.y = _yAxisVelocity * Time.deltaTime;

        characterController.Move(movement);

        turret.transform.Rotate(x * rotationSpeedGun * Time.deltaTime * 10f * Vector3.up);

        if (photonView.IsMine)
        {
            float gunRotation = gunner.transform.localRotation.eulerAngles.x;
            photonView.RPC("SyncGunRotation", RpcTarget.Others, gunRotation);

            if (Input.GetMouseButtonDown(0))
            {
                CreateBullet();

            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (activeCamera == mainCam)
                {
                    activeCamera = scope;
                    mainCam.enabled = false;
                    scope.enabled = true;
                }
                else
                {
                    activeCamera = mainCam;
                    mainCam.enabled = true;
                    scope.enabled = false;
                }
            }
        }
    }

    [PunRPC]
    private void SyncGunRotation(float gunRotation)
    {
        gunner.transform.localRotation = Quaternion.Euler(gunRotation, 0f, 0f);
    }

    private void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.transform.position, shotPoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100f;

        photonView.RPC("SpawnBullet", RpcTarget.Others, bullet.transform.position, bullet.transform.rotation);
    }

    [PunRPC]
    private void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 100f;
    }
}
