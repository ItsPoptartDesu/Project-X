using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class NPC_Trainer : MonoBehaviour
{
    [SerializeField]
    [Range(2f, 10f)]
    private float LookDistance = 2;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField]
    private Transform rayCastPoint;
    [SerializeField]
    Vector2 LookDir = Vector2.left;
    [SerializeField]
    JSONTrainerInfo trainerInfo;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        LookDistance = 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(rayCastPoint.position, LookDir * LookDistance);
        var hit = Physics2D.Raycast(rayCastPoint.position, LookDir * LookDistance);
        if (hit.collider != null)
        {
            Debug.Log($"Hit: {hit.transform.gameObject.name}");
        }
    }

    public void LoadTrainerData(JSONTrainerInfo _trainerData)
    {
        trainerInfo = _trainerData;
    }
}
