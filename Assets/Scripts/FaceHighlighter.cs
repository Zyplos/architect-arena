using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FaceHighlighter : NetworkBehaviour
{
    private BoxCollider boxCollider;

    // map of directions to boolean values
    private Dictionary<string, bool> facePlaced = new Dictionary<string, bool>
    {
        { "x+", false },
        { "x-", false },
        { "y+", true }, // disable top and bottom for now
        { "y-", true }, // disable top and bottom for now
        { "z+", false },
        { "z-", false }
    };

    public int delta = 40;

    private bool isArchitect = false;

    private bool buildingMode = true;

    public GameObject LevelWalls;

    private TMPro.TMP_Text hintText;

    // array of strings with available rooms
    private string[] rooms = { "TrapRoomResource", "StairwellRoomResource", "AlternatingPlatformRoomResource" };

    private Dictionary<string, string> friendlyRoomNames = new Dictionary<string, string>
    {
        { "TrapRoomResource", "Lava Room" },
        { "StairwellRoomResource", "Stairwell Climb" },
        { "AlternatingPlatformRoomResource", "Shaky Hallway" }
    };

    private string[] upcomingRooms;

    void Start()
    {
        // Assuming the BoxCollider is attached to the same GameObject as this script
        boxCollider = GetComponent<BoxCollider>();

        // get the hint text object from Canvas
        hintText = GameObject.Find("Canvas").transform.Find("LevelBlockHintText").GetComponent<TMPro.TMP_Text>();

        isArchitect = OwnerClientId == 0;

        GenerateRandomRoomOrder();
    }

    private void GenerateRandomRoomOrder()
    {
        // randomly add 10 rooms to the upcomingRooms array
        upcomingRooms = new string[10];

        for (int i = 0; i < 10; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, rooms.Length);
            upcomingRooms[i] = rooms[randomIndex];
        }
    }

    void Update()
    {
        if (!isArchitect) return; // Only the architect can place blocks
        if (!Camera.main) return; // If there is no camera, don't do anything (happens when game's launched)

        if (buildingMode && LevelWalls)
        {
            LevelWalls.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0.2f);
        }

        // Cast a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (boxCollider.Raycast(ray, out hit, Mathf.Infinity))
        {

            string direction = CalculateDirection(hit);

            // dont do anything at all for faces that have already have a block placed
            if (facePlaced[direction]) return;

            DrawHintText(hit);
            // Debug.Log("Hovering over face: " + direction);

            // grab child object called "highlight-${direction}"
            GameObject highlight = transform.Find("highlight-" + direction).gameObject;

            // set opacity to 1
            Color color = highlight.GetComponent<Renderer>().material.color;
            color.a = 1;
            highlight.GetComponent<Renderer>().material.color = color;

            // set opacity to 0 for all other objects with name "highlight-"
            foreach (Transform child in transform)
            {
                if (child.name.StartsWith("highlight-") && child.name != "highlight-" + direction)
                {
                    Color otherColor = child.GetComponent<Renderer>().material.color;
                    otherColor.a = 0;
                    child.GetComponent<Renderer>().material.color = otherColor;
                }
            }

            // get coordinates of current object
            Vector3 position = transform.position;

            // add 1 to the x, y, or z coordinate of the current object based on the direction
            switch (direction)
            {
                case "x+":
                    position.x += delta;
                    break;
                case "x-":
                    position.x -= delta;
                    break;
                case "y+":
                    position.y += delta;
                    break;
                case "y-":
                    position.y -= delta;
                    break;
                case "z+":
                    position.z += delta;
                    break;
                case "z-":
                    position.z -= delta;
                    break;
            }

            // create new instance of the "BlockTemplate" prefab at the new position
            if (Input.GetMouseButtonDown(0))
            {
                // eventually we'll have a lot of block types, this'll be better than having a lot of class variables
                // https://docs.unity3d.com/ScriptReference/Resources.html
                Debug.Log("UPDATE | GOT CLICK " + direction);
                SpawnBlockServerRpc(position);
                facePlaced[direction] = true;
                Debug.Log("UPDATE | SPAWNED BLOCK?");
            }
        }
        else
        {
            // mouse not over any face, set opacity to 0 for all objects with name "highlight-"
            foreach (Transform child in transform)
            {
                if (child.name.StartsWith("highlight-"))
                {
                    Color otherColor = child.GetComponent<Renderer>().material.color;
                    otherColor.a = 0;
                    child.GetComponent<Renderer>().material.color = otherColor;
                }
            }

            // hide hint text
            hintText.text = "";
        }
    }

    private void DrawHintText(RaycastHit hit)
    {
        // get screen position of hit point
        Vector3 screenPos = Camera.main.WorldToScreenPoint(hit.point);

        // set hint text position to screen position
        hintText.rectTransform.position = screenPos;

        hintText.text = "Placing next:\nTrap Room";
    }

    string CalculateDirection(RaycastHit hit)
    {
        // Calculate hit point in local space of the box collider
        Vector3 hitLocalPoint = transform.InverseTransformPoint(hit.point);

        // Calculate the absolute value of hitLocalPoint to avoid negative values
        Vector3 absHitLocalPoint = new Vector3(Mathf.Abs(hitLocalPoint.x), Mathf.Abs(hitLocalPoint.y), Mathf.Abs(hitLocalPoint.z));

        // Determine which face is being hovered over
        if (absHitLocalPoint.x > absHitLocalPoint.y && absHitLocalPoint.x > absHitLocalPoint.z)
        {
            if (hitLocalPoint.x > 0)
                return "x+";
            else
                return "x-";
        }
        else if (absHitLocalPoint.y > absHitLocalPoint.x && absHitLocalPoint.y > absHitLocalPoint.z)
        {
            if (hitLocalPoint.y > 0)
                return "y+";
            else
                return "y-";
        }
        else
        {
            if (hitLocalPoint.z > 0)
                return "z+";
            else
                return "z-";
        }
    }

    [ServerRpc]
    private void SpawnBlockServerRpc(Vector3 position)
    {
        Debug.Log("SERVERRPC | CALLING CLIENTRPC TO SPAWN LEVEL BLOCK");
        SpawnBlockClientRpc("LevelBlocks/TrapRoomResource", position);
    }

    [ClientRpc]
    private void SpawnBlockClientRpc(string type, Vector3 position)
    {
        Debug.Log("CLIENTRPC | TRYING TO SPAWN LEVEL BLOCK");
        // Instantiate the block on the client side
        GameObject newBlock = Instantiate(Resources.Load(type), position, Quaternion.identity) as GameObject;

        newBlock.GetComponent<NetworkObject>().Spawn(true);

        Debug.Log("CLIENTRPC | SPAWNED LEVEL BLOCK");
        // Add any additional logic or modifications to the newBlock here
        // ...
        // Optionally, you can also synchronize the block's network object
        // newBlock.GetComponent<NetworkObject>().Spawn(true);
    }
}