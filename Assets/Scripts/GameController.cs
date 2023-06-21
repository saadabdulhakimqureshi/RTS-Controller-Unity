using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Vector3 startPosition, endPosition;

    public string stopKey;
    public string deselectKey;
    
    
    public List<RTSController> selectedRTSUnits;
    public GameObject[] rtsUnits;

    [SerializeField] public Camera MainCamera;
    public GameController instance;

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        selectedRTSUnits = new List<RTSController>();
        rtsUnits = GameObject.FindGameObjectsWithTag("RTS");
        
    }

    void Update()
    {

        StartSelection();
        DuringSelection();
        EndSelection();

        OrderSelectionUnitsByClick();
        Deselection();
        OrderSelectionUnitsByKey();
        
    }

    void OrderSelectionUnitsByClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (RTSController rtsUnitController in selectedRTSUnits)
            {

                rtsUnitController.OrderUnit();
            }
        }
    }

    void Deselection()
    {
        if (Input.GetKeyDown(deselectKey)){
            foreach (RTSController rtsUnitController in selectedRTSUnits)
            {
                rtsUnitController.DisableHealthBar();
            }
            selectedRTSUnits.Clear();
        }
    }

    void OrderSelectionUnitsByKey()
    {
        if (Input.GetKeyDown(stopKey))
        {
            foreach (RTSController rtsUnitController in selectedRTSUnits)
            {
                rtsUnitController.OrderUnit(false, "Stop");
            }
        }
    }

    void StartSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UIController.instance != null)
                UIController.instance.EnableSelectionArea(true);
            startPosition = Input.mousePosition;

            foreach (RTSController rtsUnitController in selectedRTSUnits)
            {
                rtsUnitController.DisableHealthBar();
            }

            selectedRTSUnits.Clear();
            //Debug.Log("Start Position: " + startPosition);
        }
    }

    void EndSelection()
    {
        if (Input.GetMouseButtonUp(0))
        {
            endPosition = Input.mousePosition;


            //Debug.Log("End Position: " + endPosition);

            float minX = Mathf.Min(startPosition.x, endPosition.x);
            float maxX = Mathf.Max(startPosition.x, endPosition.x);
            float minY = Mathf.Min(startPosition.y, endPosition.y);
            float maxY = Mathf.Max(startPosition.y, endPosition.y);

            selectedRTSUnits.Clear();

            foreach (GameObject rtsUnit in rtsUnits)
            {
                Vector3 worldPosition = rtsUnit.GetComponent<Transform>().position;
                Vector3 screenPosition = MainCamera.WorldToScreenPoint(worldPosition);
                if (screenPosition.x >= minX && screenPosition.x <= maxX)
                {
                    if (screenPosition.y >= minY && screenPosition.y <= maxY)
                    {
                        RTSController rtsUnitController = rtsUnit.GetComponent<RTSController>();
                        selectedRTSUnits.Add(rtsUnitController);
                        rtsUnitController.EnableHealthBar();
                    }
                }
            }
            if (UIController.instance != null)
                UIController.instance.EnableSelectionArea(false);
        }
    }

    void DuringSelection()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float width = currentMousePosition.x - startPosition.x;
            float height = currentMousePosition.y - startPosition.y;
            if (UIController.instance != null)
            {
                UIController.instance.UpdateSelectionArea(width, height, startPosition.x, startPosition.y);
            }
        }
    }
}