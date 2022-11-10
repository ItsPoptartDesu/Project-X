using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectionManager : MonoBehaviour
{
    [SerializeField]
    private Transform ActiveTeamRoot;
    [SerializeField]
    private Transform RandomSpawnRoot;
    public void AttachNewMember(Transform _toBeAttached)
    {
        _toBeAttached.SetParent(ActiveTeamRoot);
        //_toBeAttached.localPosition = Vector3.zero;
    }
    public void OnSpawnAttachment(Transform _toBeAttached)
    {
        _toBeAttached.SetParent(RandomSpawnRoot);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
