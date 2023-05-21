using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputField;

    // Start is called before the first frame update

    public void createRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(inputField.text, roomOptions);
    }

    public void joinRoom()
    {
        PhotonNetwork.JoinRoom(inputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
