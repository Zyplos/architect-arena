using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardTest : MonoBehaviour
{
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
    //  transform.LookAt(mainCamera.transform);

    //  transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    Vector3 dir = transform.position - mainCamera.transform.position;
 dir.y = 0.0F;
 transform.rotation = Quaternion.LookRotation(dir);
    }
}
