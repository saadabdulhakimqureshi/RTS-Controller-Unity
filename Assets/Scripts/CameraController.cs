using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraController instance;

    [Header("Child References")]
    public Transform cameraTransform;

    [Header("Parameters")]
    public float originalSpeed;
    public float movementSpeed;
    
    public float fastSpeed;
    public float slowSpeed;
    public float rotationAmount;
    public float movementTime;
    public Vector3 zoomAmount;
    public Vector3 maxZoom;
    public Vector3 minZoom;

    [Header("Results")]
    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    [Header("Controls")]
    public KeyCode rotateLeft;
    public KeyCode rotateRight;
    public KeyCode up1, up2;
    public KeyCode down1, down2;
    public KeyCode left1, left2;
    public KeyCode right1, right2;
    public KeyCode fastMove, slowMove;
    public KeyCode zoomIn, zoomOut;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        DontDestroyOnLoad(this);
        newPosition = transform.position;
        newRotation = transform.rotation;
        movementSpeed = originalSpeed;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(fastMove))
        {
            movementSpeed = fastSpeed;
        }
        
        if (Input.GetKey(slowMove))
        {
            movementSpeed = slowSpeed;
        }
        if(Input.GetKey(up1) || Input.GetKey(up2))
        {
            newPosition += (transform.forward * movementSpeed);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            newPosition = transform.position;
            movementSpeed = originalSpeed;
        }

        if (Input.GetKey(down1) || Input.GetKey(down2))
        {
            newPosition += (transform.forward * -movementSpeed);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            newPosition = transform.position;
            movementSpeed = originalSpeed;
        }

        if (Input.GetKey(left1) || Input.GetKey(left2))
        {
            newPosition += (transform.right * -movementSpeed);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            newPosition = transform.position;
            movementSpeed = originalSpeed;
        }

        if (Input.GetKey(right1) || Input.GetKey(right2))
        {
            newPosition += (transform.right * movementSpeed);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            newPosition = transform.position;
            movementSpeed = originalSpeed;
        }

        if (Input.GetKey(rotateRight))
        {
            Vector3 rotation = transform.rotation.eulerAngles;

            // Increase the Y rotation value
            rotation.y -= rotationAmount * Time.deltaTime;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(rotation);
        }

        if (Input.GetKey(rotateLeft))
        {
            // Get the current rotation
            Vector3 rotation = transform.rotation.eulerAngles;

            // Increase the Y rotation value
            rotation.y += rotationAmount * Time.deltaTime;

            // Apply the new rotation
            transform.rotation = Quaternion.Euler(rotation);
        }


        if (Input.GetKey(zoomOut))
        {
            newZoom += zoomAmount;
            if (newZoom.y <= minZoom.y && newZoom.z >= minZoom.z)
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
            newZoom = cameraTransform.localPosition;
        }
        if (Input.GetKey(zoomIn))
        {

            newZoom -= zoomAmount;
            Debug.Log(newZoom);
            Debug.Log(maxZoom);
            if (newZoom.y >= maxZoom.y && newZoom.z <= maxZoom.z)
                cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
            newZoom = cameraTransform.localPosition;
        }
    }
}
