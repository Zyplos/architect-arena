using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;
using System;

public class LobbyManager : NetworkBehaviour
{
    // array of strings with available rooms
    // private string[] rooms = { "TrapRoomResource", "StairwellRoomResource", "AlternatingPlatformRoomResource" };
    private string[] rooms = { "TrapRoomResource", "LavaHallwayResource" };

    private Dictionary<string, string> friendlyRoomNames = new Dictionary<string, string>
    {
        { "TrapRoomResource", "Lava Room" },
        { "StairwellRoomResource", "Stairwell Climb" },
        { "LavaHallwayResource", "Shaky Hallway" },
        { "EndRoomResource", "[!] Finish Line"}
    };

    private List<string> upcomingRooms;

    public TMPro.TMP_Text upcomingRoomList;
    public TMPro.TMP_Text statusText;
    private bool isArchitect = false;

    public bool isBuilding = false;

    public GameObject lobbyBarrier;

    public bool gameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        isArchitect = OwnerClientId == 0;

        upcomingRooms = new List<string>();

        if (isArchitect)
        {
            Debug.Log("ARCHITECT | Generating random room order");
            GenerateRandomRoomOrder();
        }

        // in 3 minutes, set gameOver to true
        Invoke("EndGame", 180);
    }

    private void EndGame()
    {
        gameOver = true;
    }

    private void GenerateRandomRoomOrder()
    {
        // add trap room first
        upcomingRooms.Add("LavaHallwayResource");

        for (int i = 0; i < 5; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, rooms.Length);
            upcomingRooms.Add(rooms[randomIndex]);
        }

        // last one should be the end point
        upcomingRooms.Add("EndRoomResource");

        // print list
        Debug.Log("ARCHITECT | Upcoming rooms:");
        foreach (string room in upcomingRooms)
        {
            Debug.Log("ARCHITECT | " + room);
        }
    }

    public string getUpcomingFriendlyRoomName()
    {
        if (upcomingRooms.Count > 0)
        {

            return friendlyRoomNames[upcomingRooms.First()];
        }
        else
        {
            return "";
        }
    }

    public string getUpcomingRoomKey()
    {
        if (upcomingRooms.Count > 0)
        {
            return upcomingRooms.First();
        }
        else
        {
            return "";
        }

    }

    public void removeFirstRoom()
    {
        upcomingRooms.RemoveAt(0);
    }

    // Update is called once per frame
    void Update()
    {

        // dont do anything if networking not connected
        if (!NetworkManager.IsConnectedClient) return;

        isBuilding = upcomingRooms.Count > 0;

        if (isArchitect)
        {
            upcomingRoomList.text = "Upcoming Rooms:\n";
            for (int i = 0; i < upcomingRooms.Count; i++)
            {
                upcomingRoomList.text += friendlyRoomNames[upcomingRooms[i]] + "\n";
            }

            if (isBuilding)
            {
                statusText.text = "Building level...";
            }
            else
            {
                statusText.text = "Stop them!";

            }
        }

        if (!isArchitect)
        {
            upcomingRoomList.text = "Next Room: " + getUpcomingFriendlyRoomName();

            if (isBuilding)
            {
                statusText.text = "Building level...";
                SetStatusTextServerRpc("Building level...");
            }
            else
            {
                statusText.text = "Get to the end!";
                SetStatusTextServerRpc("Get to the end!");
            }
        }

        if (!isBuilding)
        {
            upcomingRoomList.text = "";

            // disable the lobby barrier
            DisableLobbyBarrierServerRpc();
        }

        if (gameOver)
        {
            statusText.text = "Game Over!";
            SetStatusTextServerRpc("Game Over!");
        }
    }

    // server rpc call that disables the lobby barrier
    [ServerRpc]
    public void DisableLobbyBarrierServerRpc()
    {
        DisableLobbyBarrierClientRpc();
    }

    // client rpc call that disables the lobby barrier
    [ClientRpc]
    public void DisableLobbyBarrierClientRpc()
    {
        lobbyBarrier.SetActive(false);
    }

    [ServerRpc]
    public void SetStatusTextServerRpc(String text)
    {
        DisableLobbyBarrierClientRpc();
    }

    // client rpc call that disables the lobby barrier
    [ClientRpc]
    public void SetStatusTextClientRpc(String text)
    {
        statusText.text = text;
    }
}
