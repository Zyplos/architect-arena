using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUpDown: MonoBehaviour
{
    public GameObject mazepiece;

    private void OnMouseOver()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            mazepiece.transform.Translate(0f, .005f, 0f);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            mazepiece.transform.Translate(0f, -.005f, 0f);
        }
    }
}