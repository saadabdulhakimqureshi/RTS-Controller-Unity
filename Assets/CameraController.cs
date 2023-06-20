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
    public float movementTime;
    public float fastSpeed;
    public float slowSpeed;
    public float rotationAmount;
    public Vector3 zoomAmount;

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
        StopMovementInput();
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
        }

        if (Input.GetKey(down1) || Input.GetKey(down2))
        {
            newPosition += (transform.forward * -movementSpeed);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        }

        if (Input.GetKey(left1) || Input.GetKey(left2))
        {
            newPosition += (transform.right * -movementSpeed);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        }

        if (Input.GetKey(right1) || Input.GetKey(right2))
        {
            newPosition += (transform.right * movementSpeed);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        }

        if (Input.GetKey(rotateLeft))
        {
            newRotation.y += rotationAmount;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationAmount);
        }

        if (Input.GetKey(rotateRight))
        {
            newRotation.y -= rotationAmount;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationAmount);
        }


        if (Input.GetKey(zoomOut))
        {
            newZoom += zoomAmount;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
        if (Input.GetKey(zoomIn))
        {
            newZoom -= zoomAmount;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
    }

    void StopMovementInput()
    {
        
        
        if (Input.GetKeyUp(fastMove))
        {
            movementSpeed = originalSpeed;
        }

        if (Input.GetKeyUp(slowMove))
        {
            movementSpeed = originalSpeed;
        }
        if (Input.GetKeyUp(up1) || Input.GetKeyUp(up2))
        {
            newPosition = transform.position;
        }
        if (Input.GetKeyUp(down1) || Input.GetKeyUp(down2))
        {
            newPosition = transform.position;
        }
        if (Input.GetKeyUp(left1) || Input.GetKeyUp(left2))
        {
            newPosition = transform.position;
        }
        if (Input.GetKeyUp(right1) || Input.GetKeyUp (right2))
        {
            newPosition = transform.position;
        }
        if (Input.GetKeyUp(rotateLeft))
        {
            newRotation = transform.rotation;
        }
        if (Input.GetKeyUp(rotateRight))
        {
            newRotation = transform.rotation;
        }
        if (Input.GetKeyUp(zoomIn))
        {
            newZoom = 
            cameraTransform.localPosition;
        }
        if (Input.GetKeyUp(zoomOut))
        {
            newZoom =
            cameraTransform.localPosition;
        }
    }
}
