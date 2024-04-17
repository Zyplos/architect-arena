using UnityEngine;
using Unity.Netcode;

public class StairwellTrap : NetworkBehaviour
{

  LobbyManager lobbyManager;
  GameObject thingThatContainsPointLights;

  private void Start()
  {
    lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
  }
  private void OnMouseDown()
  {
    // set point lights intensity to 0
  }

  void turnOffAllLights()
  {

  }
}