using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TeamSelectionManager teamSelectionManager;
    [SerializeField]
    private GameObject MenuIdle_UI;
    [SerializeField]
    private GameObject MenuCollection_UI;
    public void ResetUI()
    {
        MenuCollection_UI.SetActive(false);
        MenuIdle_UI.SetActive(true);
    }
    public void ShowCollectionUI()
    {
        MenuIdle_UI.SetActive(false);
        MenuCollection_UI.SetActive(true);
    }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
