using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// adding namespaces
using Unity.Netcode;
// because we are using the NetworkBehaviour class
// NewtorkBehaviour class is a part of the Unity.Netcode namespace
// extension of MonoBehaviour that has functions related to multiplayer
public class NetworkedChallengerMovement : NetworkBehaviour
{
    public float walkingSpeed = 10.0f;
    public float runningSpeed = 20.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;
    // create a list of colors
    public List<Color> colors = new List<Color>();

    // getting the reference to the prefab
    // [SerializeField]
    // private GameObject spawnedPrefab;
    // // save the instantiated prefab
    // private GameObject instantiatedPrefab;

    // public GameObject cannon;
    // public GameObject bullet;

    // reference to the camera audio listener
    [SerializeField] private AudioListener audioListener;
    // reference to the camera
    [SerializeField] private Camera playerCamera;

    public Vector3 architectSpawnLocation;
    public Vector3 architectSpawnRotation;

    private bool isArchitect = false;


    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;



        // everyone but the architect should have their cursor locked
        if (!isArchitect)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // check if the player is the owner of the object
        // makes sure the script is only executed on the owners 
        // not on the other prefabs 
        if (!IsOwner) return;

        bool isRunning = false;

        // Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = 0f;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        transform.position += moveDirection * Time.deltaTime;


        // dont continue if the player is the architect
        if (isArchitect) return;

        // rotate camera controls only for challengers
        if (playerCamera != null)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    // this method is called when the object is spawned
    // we will change the color of the objects
    public override void OnNetworkSpawn()
    {
        isArchitect = OwnerClientId == 0;

        Debug.Log("SPAWNING PLAYER | " + OwnerClientId);
        GetComponent<MeshRenderer>().material.color = colors[(int)OwnerClientId];

        // move player up by 2
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

        // check if the player is the owner of the object
        if (!IsOwner) return;

        Debug.Log("SETTING CAMERA");
        // if the player is the owner of the object
        // enable the camera and the audio listener
        audioListener.enabled = true;
        playerCamera.enabled = true;

        // tag playerCamera with "MainCamera"
        playerCamera.tag = "MainCamera";
        // change object name to "Player Camera"
        playerCamera.name = "[" + OwnerClientId + "] Player's Camera";

        // disable "Lobby Camera" in scene
        GameObject.Find("Lobby Camera").SetActive(false);

        // only do the following if the player is the architect
        if (OwnerClientId == 0)
        {
            Debug.Log("SETTING ARCHITECT CAMERA POSITION");
            // disable gravity
            GetComponent<Rigidbody>().useGravity = false;

            // copy position and rotation of the lobby camera to the player camera
            transform.position = architectSpawnLocation;
            transform.rotation = Quaternion.Euler(architectSpawnRotation);
        }
    }

    // // need to add the [ServerRPC] attribute
    // [ServerRpc]
    // // method name must end with ServerRPC
    // private void BulletSpawningServerRpc(Vector3 position, Quaternion rotation)
    // {
    //     // call the BulletSpawningClientRpc method to locally create the bullet on all clients
    //     BulletSpawningClientRpc(position, rotation);
    // }

    // [ClientRpc]
    // private void BulletSpawningClientRpc(Vector3 position, Quaternion rotation)
    // {
    //     GameObject newBullet = Instantiate(bullet, position, rotation);
    //     newBullet.GetComponent<Rigidbody>().velocity += Vector3.up * 2;
    //     newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 1500);
    //     // newBullet.GetComponent<NetworkObject>().Spawn(true);
    // }
}