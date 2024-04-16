using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
  public float scrollSpeed = 1.0f;

  void Update()
  {
    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0.0f)
    {
      Vector3 newPosition = transform.localPosition;
      newPosition.z += scroll * scrollSpeed;
      transform.localPosition = newPosition;
    }
  }
}