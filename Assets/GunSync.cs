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
        // ”правление опусканием пушки на локальном игроке
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
            // ќтправка значени€ поворота пушки по сети
            stream.SendNext(gunRotation);
        }
        else
        {
            // ѕолучение значени€ поворота пушки по сети и применение к локальной пушке
            gunRotation = (float)stream.ReceiveNext();
        }
    }
}
