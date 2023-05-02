using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 2F;
    public float sensitivityY = 2F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -90F;
    public float maximumY = 90F;
    public float zoomSensitivity;

    void Update()
    {
        MouseInput();
        ZoomCam();
    }
    void MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            MouseClicked();
        }
    }
    void MouseClicked()
    {
        Vector3 NewPosition = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        Vector3 pos = transform.position;
        if (NewPosition.x > 0)
        {
            pos -= transform.right;
            pos.y = transform.position.y;
            transform.position = pos;
        }
        else if (NewPosition.x < 0)
        {
            pos += transform.right;
            pos.y = transform.position.y;
            transform.position = pos;
        }
        if (NewPosition.z > 0)
        {
            pos -= transform.forward;
            pos.y = transform.position.y;
            transform.position = pos;
        }
        if (NewPosition.z < 0)
        {
            pos += transform.forward;
            pos.y = transform.position.y;
            transform.position = pos;
        }
    }
    private void ZoomCam()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            Vector3 newPos = transform.position;
            newPos.y -= zoomSensitivity;
            transform.position = newPos;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            Vector3 newPos = transform.position;
            newPos.y += zoomSensitivity;
            transform.position = newPos;
        }
    }
}
