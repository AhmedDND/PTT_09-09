using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSizeButton : MonoBehaviour
{
    [SerializeField] private int _rows = 2;
    [SerializeField] private int _columns = 2;
    [SerializeField] private MainMenuManager _mainMenumanager;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            _mainMenumanager.OnGridSizeChosen(_rows, _columns);
        });
    }
}
