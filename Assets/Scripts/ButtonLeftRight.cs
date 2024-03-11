using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLeftRight : MonoBehaviour
{
    public GameObject mazepiece;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mazepiece.transform.Translate(.1f, 0f, 0f);
        }
        if (Input.GetMouseButtonDown(1))
        {
            mazepiece.transform.Translate(-.1f, 0f, 0f);
        }
    }
}