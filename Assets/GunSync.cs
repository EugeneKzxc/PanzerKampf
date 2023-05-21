using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSync : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;

    private float gunRotation;
    public float rotationSpeedGun = 10f;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        // ���������� ���������� ����� �� ��������� ������
        if (photonView.IsMine)
        {
            float y = Input.GetAxis("Mouse Y");
            gunRotation -= y * rotationSpeedGun * Time.deltaTime * 10f;
            gunRotation = Mathf.Clamp(gunRotation, -7f, 7f);

            gameObject.transform.localRotation = Quaternion.Euler(gunRotation, 0f, 0f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �������� �������� �������� ����� �� ����
            stream.SendNext(gunRotation);
        }
        else
        {
            // ��������� �������� �������� ����� �� ���� � ���������� � ��������� �����
            gunRotation = (float)stream.ReceiveNext();
        }
    }
}
