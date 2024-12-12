using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    public InputField createInput; // Input for room name to create
    public InputField joinInput;   // Input for room name to join
    public Text missionDisplay;   // UI Text to display the selected mission
    public Button createButton;   // Button to create a room
    public Button joinButton;     // Button to join a room
    public Button readyButton;    // Button to indicate readiness in lobby
    public Text readyStatus;      // UI Text to display readiness status

    // Array of possible mission names
    private string[] missions = new string[] {
        "Capture the Outpost",
        "Escort the Convoy",
        "Destroy the Barricade",
        "Activate the Relay Tower",
        "Defend the Base"
    };

    private string selectedMission;
    private bool isReady = false;

    void Start()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);

        readyButton.gameObject.SetActive(false); // Hide ready button initially
        readyButton.onClick.AddListener(ToggleReadyStatus);

        // Display a random mission initially
        missionDisplay.text = "Mission: " + missions[Random.Range(0, missions.Length)];
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(createInput.text))
        {
            // Select a random mission for the room
            selectedMission = missions[Random.Range(0, missions.Length)];

            // Attach custom properties to the room for the mission
            ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable { { "Mission", selectedMission } };

            // Create the room with custom properties
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4; // Example: limit to 4 players
            roomOptions.CustomRoomProperties = roomProperties;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "Mission" };

            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrEmpty(joinInput.text))
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Mission", out object mission))
        {
            missionDisplay.text = "Mission: " + mission.ToString();
        }

        // Transition to the Lobby scene
        PhotonNetwork.LoadLevel("Lobby");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room creation failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Room joining failed: " + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Updated Room List");
        foreach (var room in roomList)
        {
            Debug.Log($"Room: {room.Name}, Players: {room.PlayerCount}/{room.MaxPlayers}");
        }
    }

    private void ToggleReadyStatus()
    {
        isReady = !isReady;
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable { { "IsReady", isReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        readyStatus.text = "Ready: " + (isReady ? "Yes" : "No");

        CheckAllReady();
    }

    private void CheckAllReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.TryGetValue("IsReady", out object isPlayerReady) || !(bool)isPlayerReady)
            {
                return; // Not all players are ready
            }
        }

        // All players are ready; start the game
        PhotonNetwork.LoadLevel("Main");
    }
}