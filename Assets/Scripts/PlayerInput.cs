using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private GameObject SpawnedObject = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (SpawnedObject != null)
                Destroy(SpawnedObject);
            SpawnedObject = GameEntry.Instance.GenerateRandomSlime();
        }
    }
}
