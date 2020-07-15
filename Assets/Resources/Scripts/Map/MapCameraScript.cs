using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapCameraScript : MonoBehaviour
{
    public GameObject map; //map must have its center at (0,0,0) local position
    public float minSize = 4; //min max size of this orthogonal camer
    public float maxSize = 9;
    public float touchZoomSpeed = 0.01f; //multiplier for touch zoom to otho camera size

    public float inertiaDampingVal = 1.03f; //dividing factor for how quick inertia damp off

    private Camera cam;

    //drag vars
    private Vector3 dragStart;
    private float dragSSx, dragSSy;
    private bool wasDown = false;
    private bool nowDown;
    private Vector3 mapCenter;
    private Vector3 originalWS;

    private float mapHalfWidth, mapHalfHeight, camHalfWidth, camHalfHeight;

    private float minX, maxX, minZ, maxZ;
    private Vector3 newLocalPos;

    private Vector3 inertiaVector;

    //DEBUG
    void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, 150, 100),
            "minX" + minX + "\n" +
            "maxX" + maxX + "\n" +
            "minZ" + minZ + "\n" +
            "maxZ" + maxZ + "\n" +
            "newLocalPos" + newLocalPos.ToString()
        );

    }

    // Use this for initialization
    void Start()
    {
        inertiaVector = new Vector3(0, 0, 0);
        cam = GetComponent<Camera>();
        mapCenter = map.transform.localPosition;
        newLocalPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        processDrag();
        //processZoom();

    }

    private void processDrag()
    {
        //damp inertia
        inertiaVector = inertiaVector / inertiaDampingVal;
        if (inertiaVector.magnitude < 0.005)
        {
            inertiaVector = new Vector3(0, 0, 0);
        }

        //check if input is activated (mouse down or finger touching screen)
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            nowDown = (Input.touchCount > 0);
        }
        else
        {
            nowDown = (Input.GetMouseButton(0) || Input.GetAxis("Mouse ScrollWheel") != 0);
        }

        //calcualte how much camera need to change position
        Vector3 WSDiff = Vector3.zero;

        //if mouse currently holding or finger touching the screen
        if (nowDown)
        {
            //get screenspace input points
            //phone
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount == 1)
                {
                    //one finger
                    dragSSx = Input.GetTouch(0).position.x;
                    dragSSy = Input.GetTouch(0).position.y;
                }
                else if (Input.touchCount == 2)
                {
                    //check if player is switching between 1 finger to 2 finger
                    if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Ended)
                    {
                        //if player add or remove finger, recalculate origional World Space point so that the map wouldn't jump around
                        wasDown = false;
                        return;
                    }
                    //two finger
                    dragSSx = (Input.GetTouch(0).position.x + Input.GetTouch(1).position.x) / 2;
                    dragSSy = (Input.GetTouch(0).position.y + Input.GetTouch(1).position.y) / 2;
                }
            }
            else
            {
                dragSSx = Input.mousePosition.x;
                dragSSy = Input.mousePosition.y;
            }

            //calcualte how much camera need to change position
            if (!wasDown)
            {
                originalWS = cam.ScreenToWorldPoint(new Vector3(dragSSx, dragSSy, 1));
            }
            else if (wasDown)
            {
                Vector3 newWS = cam.ScreenToWorldPoint(new Vector3(dragSSx, dragSSy, 1));
                WSDiff = (originalWS - newWS);
                inertiaVector = WSDiff;

            }
        }
        wasDown = nowDown;

        //move camera by WS diff vector or inertiaVector
        if (nowDown)
        {
            newLocalPos = transform.localPosition + WSDiff;
        }
        else
        {
            newLocalPos = transform.localPosition + inertiaVector;
        }

        //make sure camera does not go out of range
        mapHalfWidth = (map.GetComponent<MeshFilter>().mesh.bounds.size.x * map.transform.localScale.x / 2);
        mapHalfHeight = (map.GetComponent<MeshFilter>().mesh.bounds.size.z * map.transform.localScale.z / 2);
        camHalfHeight = 2f * cam.orthographicSize / 2;
        camHalfWidth = camHalfHeight * cam.aspect;
        minX = mapCenter.x - mapHalfWidth + camHalfWidth; //min position camera should be at
        maxX = mapCenter.x + mapHalfWidth - camHalfWidth;
        minZ = mapCenter.z - mapHalfHeight + camHalfHeight;
        maxZ = mapCenter.z + mapHalfHeight - camHalfHeight;

        if (minX > maxX)
        { //incase the scale is too small, put camera at center
            newLocalPos = new Vector3(mapCenter.x, newLocalPos.y, newLocalPos.z);
        }
        else
        {
            if (newLocalPos.x - mapCenter.x < minX)
            {
                newLocalPos = new Vector3(minX, newLocalPos.y, newLocalPos.z);
            }
            if (newLocalPos.x - mapCenter.x > maxX)
            {
                newLocalPos = new Vector3(maxX, newLocalPos.y, newLocalPos.z);
            }
        }
        if (minZ > maxZ)
        { //incase the scale is too small, put camera at center
            newLocalPos = new Vector3(newLocalPos.x, newLocalPos.y, mapCenter.z);
        }
        else
        {
            if (newLocalPos.z - mapCenter.z < minZ)
            {
                //Debug.Log("MIN Z");
                newLocalPos = new Vector3(newLocalPos.x, newLocalPos.y, minZ);
            }
            if (newLocalPos.z - mapCenter.z > maxZ)
            {
                //Debug.Log("MAX Z");
                newLocalPos = new Vector3(newLocalPos.x, newLocalPos.y, maxZ);
            }
        }

        transform.localPosition = newLocalPos;
    }

    private void processZoom()
    {
        //Get increment values
        float increment = 0;
        //touch screen
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount == 2)
            {
                //two finger
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                //scale the zoom increment value base on touchZoomSpeed
                increment = deltaMagnitudeDiff * touchZoomSpeed;
            }
        }
        else
        {
            //mice and keyboard
            increment = Input.GetAxis("Mouse ScrollWheel");
        }
        //calculate new orthographic camera size
        float currentSize = cam.orthographicSize;
        currentSize += increment;

        //check if size is out of the defined bound
        if (currentSize >= maxSize)
        {
            currentSize = maxSize;
        }
        else if (currentSize <= minSize)
        {
            currentSize = minSize;
        }

        //set new size
        cam.orthographicSize = currentSize;
    }
}

