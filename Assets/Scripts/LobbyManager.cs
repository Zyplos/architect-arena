using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class LobbyManager : NetworkBehaviour
{
    // array of strings with available rooms
    private string[] rooms = { "TrapRoomResource", "StairwellRoomResource", "AlternatingPlatformRoomResource" };

    private Dictionary<string, string> friendlyRoomNames = new Dictionary<string, string>
    {
        { "TrapRoomResource", "Lava Room" },
        { "StairwellRoomResource", "Stairwell Climb" },
        { "AlternatingPlatformRoomResource", "Shaky Hallway" },
        { "EndRoomResource", "[!] Finish Line"}
    };

    private List<string> upcomingRooms;

    public TMPro.TMP_Text upcomingRoomList;
    public TMPro.TMP_Text statusText;
    private bool isArchitect = false;

    public bool isBuilding = false;


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

    }

    private void GenerateRandomRoomOrder()
    {
        // add trap room first
        upcomingRooms.Add("TrapRoomResource");

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, rooms.Length);
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

        if (!isBuilding)
        {
            upcomingRoomList.text = "";
        }

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
            }
            else
            {
                statusText.text = "Get to the end!";
            }
        }
    }
}
