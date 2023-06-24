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

            
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Destroyable")
                {
                    foreach (RTSController rtsUnitController in selectedRTSUnits)
                    {

                        rtsUnitController.OrderUnit( hit, new Vector3(0,0,0),"Attack");
                    }
                }
                if (hit.collider.tag == "Ground")
                {
                    int positionCount = selectedRTSUnits.Count;
                    Vector3 hitPositin = hit.point;
                    List<Vector3> targetPositionList;
                    Debug.Log(hit.point);

                    targetPositionList = GetNonRepeatingRandomPlacements(hit.point, positionCount*0.4f, positionCount);

                    int i = 0;

                    foreach (RTSController rtsUnitController in selectedRTSUnits)
                    {
                        Debug.Log("Target Position " + i.ToString() + " " + targetPositionList[i].ToString());
                        rtsUnitController.OrderUnit(hit, targetPositionList[i], "Move");
                        i++;
                    }
                }

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
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 
            Physics.Raycast(ray, out hit);
            foreach (RTSController rtsUnitController in selectedRTSUnits)
            {
                rtsUnitController.OrderUnit(hit, new Vector3(0,0,0), "Stop");
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

    List<Vector3> GetPositionListAroundCircle(Vector3 startPosition, float distance, int positionCount)// Finds positions arround the mouse click point in the form of a circle.
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3  dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }

        return positionList;
    }
    List<Vector3> GetPositionListInGrid(Vector3 startPosition, float distance, int positionCount)
    {
        int rowCount;
        int columnCount;
        if (positionCount % 2 == 0)
        {
            rowCount = positionCount / 2;
            columnCount = positionCount / 2;
        }
        else
        {
            rowCount = positionCount-1 / 2 + 1;
            columnCount = positionCount / 2;
        }

        List<Vector3> positionList = new List<Vector3>();

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                Vector3 offset = new Vector3(column * distance, 0f, row * distance);
                Vector3 position = startPosition + offset;
                positionList.Add(position);
            }
        }

        return positionList;
    }
    Vector3 ApplyRotationToVector(Vector3 vector, float angle)
    {
        // Convert the angle from degrees to radians
        float radianAngle = angle * Mathf.Deg2Rad;

        // Calculate the cosine and sine of the angle
        float cosAngle = Mathf.Cos(radianAngle);
        float sinAngle = Mathf.Sin(radianAngle);

        // Apply rotation to the vector using 2D rotation formulas
        float rotatedX = vector.x * cosAngle - vector.y * sinAngle;
        float rotatedY = vector.x * sinAngle + vector.y * cosAngle;

        // Return the rotated vector
        return new Vector3(rotatedX, 0f, rotatedY);
    }

    List<Vector3> GetNonRepeatingRandomPlacements(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();

        // Generate a list of positions
        for (int i = 0; i < positionCount; i++)
        {
            Vector3 position = startPosition + new Vector3(UnityEngine.Random.Range(-distance, distance), 0f, UnityEngine.Random.Range(-distance, distance));
            positionList.Add(position);
        }

        // Shuffle the list of positions
        for (int i = positionList.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Vector3 temp = positionList[i];
            positionList[i] = positionList[randomIndex];
            positionList[randomIndex] = temp;
        }

        return positionList;
    }
}