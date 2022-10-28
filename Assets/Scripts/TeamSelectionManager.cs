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
        _toBeAttached.SetParent(ActiveTeamRoot, false);
    }
    public void OnSpawnAttachment(Transform _toBeAttached)
    {
        RandomSpawnRoot.SetParent(_toBeAttached, false);
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
