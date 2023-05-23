using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float zoomSensitivity;
    public float dragSpeed = 6f;

    public Vector3 newPos;

    void Update()
    {
        if (Input.GetMouseButton(0) && !HexGrid.inMenu)
        {
            transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, 0, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed);
            transform.position = new Vector3(transform.position.x, newPos.y, transform.position.z);
        }
        if(!HexGrid.inMenu)
        {
            ZoomCam();
        }
    }
    private void ZoomCam()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            newPos = transform.position;
            newPos.y -= zoomSensitivity;
            transform.position = newPos;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            newPos = transform.position;
            newPos.y += zoomSensitivity;
            transform.position = newPos;
        }
    }
}
