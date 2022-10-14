using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsUI : MonoBehaviourPunCallbacks
{
    private int CountOfPlayers = -1;
    public TMP_Text CountOfPlayersText;

    private int CountOfRooms = -1;
    public TMP_Text CountOfRoomsText;

    private int CountOfPlayersInRooms = -1;
    public TMP_Text CountOfPlayersInRoomsText;

    private int CountOfPlayersOnMaster = -1;
    public TMP_Text CountOfPlayersOnMasterText;

    void Update()
    {
        UpdateApplicationInfo();
    }

    private void UpdateApplicationInfo()
    {
        if (PhotonNetwork.NetworkingClient.Server == Photon.Realtime.ServerConnection.MasterServer)
        {
            RefreshApplicationInfo();
        }
        else 
        {
            ResetApplicationInfo();
        }
    }

    private void ResetApplicationInfo()
    {
        if (CountOfPlayers != -1)
        {
            CountOfPlayers = -1;
            CountOfPlayersText.text = "n/a";
        }
        if (CountOfRooms != -1)
        {
            CountOfRooms = -1;
            CountOfRoomsText.text = "n/a";
        }
        if (CountOfPlayersInRooms != -1)
        {
            CountOfPlayersInRooms = -1;
            CountOfPlayersInRoomsText.text = "n/a";
        }
        if (CountOfPlayersOnMaster != -1)
        {
            CountOfPlayersOnMaster = -1;
            CountOfPlayersOnMasterText.text = "n/a";
        }
    }

    private void RefreshApplicationInfo()
    {
        if (!PhotonNetwork.IsConnected)
        {
            ResetApplicationInfo();
            return;
        }
        if (CountOfPlayers != PhotonNetwork.CountOfPlayers)
        {
            CountOfPlayers = PhotonNetwork.CountOfPlayers;
            CountOfPlayersText.text = CountOfPlayers.ToString();
        }
        if (CountOfRooms != PhotonNetwork.CountOfRooms)
        {
            CountOfRooms = PhotonNetwork.CountOfRooms;
            CountOfRoomsText.text = CountOfRooms.ToString();
        }
        if (CountOfPlayersInRooms != PhotonNetwork.CountOfPlayersInRooms)
        {
            CountOfPlayersInRooms = PhotonNetwork.CountOfPlayersInRooms;
            CountOfPlayersInRoomsText.text = CountOfPlayersInRooms.ToString();
        }
        if (CountOfPlayersOnMaster != PhotonNetwork.CountOfPlayersOnMaster)
        {
            CountOfPlayersOnMaster = PhotonNetwork.CountOfPlayersOnMaster;
            CountOfPlayersOnMasterText.text = CountOfPlayersOnMaster.ToString();
        }
    }
}
