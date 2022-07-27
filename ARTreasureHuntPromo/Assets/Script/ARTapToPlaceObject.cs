using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject plane;
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    private ARRaycastManager aRRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    // Start is called before the first frame update
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if(placementPoseIsValid&& Input.touchCount>0&& Input.GetTouch(0).phase == TouchPhase.Began){
      
            PlaceObject();
        }
    }

    private void PlaceObject(){
        Instantiate(plane, placementPose.position, placementPose.rotation);//here in y increase to change the height
        Instantiate(objectToPlace,new Vector3(placementPose.position.x, (placementPose.position.y +15),placementPose.position.z), new Quaternion(placementPose.rotation.x-1f,placementPose.rotation.y,placementPose.rotation.z,placementPose.rotation.w)); ///here change x and w for rotation
    }
    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x,0,cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }
}