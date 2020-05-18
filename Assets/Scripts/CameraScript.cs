using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject normandyGO, enemiesGO, cam;

    public Transform[] cameraPos;

    List<Vector3> waypoints;

    

    private void Start()
    {
        normandyGO = GameObject.Find("Normandy");
        enemiesGO = GameObject.FindGameObjectWithTag("Enemy");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        waypoints = GameObject.Find("Path").GetComponent<Path>().waypoints;
    }

    public void ChangeCam(int camChoice)
    {
        Debug.Log("cam: " + camChoice);
        cam.transform.position = cameraPos[camChoice].transform.position;
        cam.transform.rotation = cameraPos[camChoice].transform.rotation;
    }
}
