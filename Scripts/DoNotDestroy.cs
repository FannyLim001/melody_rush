using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Music");
        if(obj.Length > 1)
        {
            Destroy(this.gameObject);
        } else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
