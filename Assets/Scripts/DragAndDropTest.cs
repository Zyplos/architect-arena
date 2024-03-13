using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropTest : MonoBehaviour
{
    private Color mouseOverColor = Color.blue;
    private Color originalColor;
    private bool dragging = false;
    private float distance;
 
    void Start()
    {
        Debug.Log("START DEBUG BLOCK DRAG");
        originalColor = GetComponent<Renderer>().material.color;
    }
   
    void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
        GetComponent<Renderer>().material.color = mouseOverColor;
    }
 
    void OnMouseExit()
    {
        Debug.Log("Mouse exit");
        GetComponent<Renderer>().material.color = originalColor;
    }
 
    void OnMouseDown()
    {
        Debug.Log("Mouse down");
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }
 
    void OnMouseUp()
    {
        Debug.Log("Mouse up");
        dragging = false;
    }
 
    void Update()
    {
        if (dragging)
        {
            Debug.Log("Dragging");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
        }
    }
}
