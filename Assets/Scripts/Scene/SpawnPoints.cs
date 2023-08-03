using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField]
    private BoardPos spot;
    public ObjectType GetObjectType() { return myType; }
    [SerializeField]
    private ObjectType myType;
    public BoardPos GetSpot() { return spot; }
    public Vector3 position { get { return transform.position; } }
    public HealthBar myHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        if (myHealthBar != null)
            myHealthBar.ToggleHealthBar(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
