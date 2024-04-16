using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditExit : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer;
    public string sceneName;

    public void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
