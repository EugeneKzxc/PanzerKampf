using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject turret;
    [SerializeField] GameObject body;
    [SerializeField] GameObject gun;
    [SerializeField] Camera cam;
    [SerializeField] CharacterController characterController;

    public PhotonView view;
    public GunSync gunner;

    public float rotationSpeedGun = 10f;
    public float rotationSpeedBody = 50f;
    public float moveSpeed = 5f;

    private float rotation;
    private float _gravity = -10f;
    private float _yAxisVelocity;

    void Start()
    {
        if (!view.IsMine)
        {
            cam.enabled = false;
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
            // Получение значения поворота пушки из компонента GunSync
            float gunRotation = gunner.transform.localRotation.eulerAngles.x;
            // Отправка значения поворота пушки по сети
            photonView.RPC("SyncGunRotation", RpcTarget.Others, gunRotation);
        }
    }

    [PunRPC]
    private void SyncGunRotation(float gunRotation)
    {
        // Применение значения поворота пушки к локальной пушке
        gunner.transform.localRotation = Quaternion.Euler(gunRotation, 0f, 0f);
    }
}
