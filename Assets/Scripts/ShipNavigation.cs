using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipNavigation : MonoBehaviour
{
    public Vector3 velocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public Vector3 force = Vector3.zero;

    public float mass = 1.0f;

    public float maxSpeed = 5;
    public float maxForce = 10;

    public float speed = 0;

    public bool seekEnabled = false;
    public Vector3 target;
    public Transform targetTransform;

    public bool pathFollowingEnabled = false;
    public float waypointDistance = 3;
    public Path path;

    public float banking = 0.1f;
    public float damping = 0.1f;

    public GameObject enemyShipsGo, shotGO, trailGO, explosionGO;

    float shotTime;


    public int enemyCount, camChoice = 0;

    bool hasShot, laserEnabled = false;

    //public Path path;

    Vector3 nextWaypoint;

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled && Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, nextWaypoint);
        }
    }

    public void Start()
    {
        shotTime = 0.5f;
        enemyCount = 2;
    }

    void Update()
    {
        MoveShip();
        CameraCheck();
        if (Vector3.Distance(transform.position, path.waypoints[4]) < 1f)
        {
            enemyShipsGo.SetActive(true);
        }

        if (Vector3.Distance(transform.position, path.waypoints[12]) < 5f && hasShot == false)
        {
            StartCoroutine("Shoot");
            hasShot = true;
            Debug.Log("ship is shoot");

        }

        if (Vector3.Distance(transform.position, path.waypoints[13]) < 5f && laserEnabled == false)
        {
            //Instantiate(trailGO, this.transform.position, this.transform.rotation);
            trailGO.SetActive(true);
            laserEnabled = true;
            Debug.Log("big laser time");
        }

        if(laserEnabled)
        {
            /*GameObject laserGo = GameObject.FindGameObjectWithTag("Laser");
            laserGo.transform.LookAt(GameObject.Find("Collector Ship").transform);
            laserGo.transform.position += transform.forward * 100f * Time.deltaTime;*/

            trailGO.transform.localScale += new Vector3(0, 0, 2f * Time.deltaTime);

            StartCoroutine("ShipExplosion");
        }
    }

    void CameraCheck()
    {
        if (Vector3.Distance(transform.position, path.waypoints[camChoice]) < 5f)
        {
            if (camChoice < GameObject.Find("Camera Positions").GetComponent<CameraScript>().cameraPos.Length - 1)
            {
                camChoice++;
            }

            GameObject.Find("Camera Positions").GetComponent<CameraScript>().ChangeCam(camChoice);
        }

    }

    public void MoveShip()
    {
        if (targetTransform != null)
        {
            target = targetTransform.position;
        }


        force = CalculateForce();
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
        speed = velocity.magnitude;

        if (speed > 0)
        {
            Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking), Time.deltaTime * 3.0f);
            transform.LookAt(transform.position + velocity, tempUp);
            //transform.forward = velocity;
            velocity -= (damping * velocity * Time.deltaTime);
        }
    }

    public Vector3 FollowPath()
    {
        Vector3 nextWaypoint = path.NextWaypoint();


        if (Vector3.Distance(transform.position, nextWaypoint) < waypointDistance)
        {
            path.AdvanceToNext();
        }
        return Seek(nextWaypoint);
        
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        Vector3 desired = toTarget.normalized * maxSpeed;

        return desired - velocity;
    }

    Vector3 CalculateForce()
    {
        Vector3 force = Vector3.zero;

        force += FollowPath();

        return force;
    }
   
    IEnumerator Shoot()
    {
        Debug.Log("shooting");
        Instantiate(shotGO, this.transform.position, transform.rotation);

        yield return new WaitForSeconds(shotTime);

        if (enemyCount > 0)
        {
            enemyCount--;
            StartCoroutine("Shoot");
        }
        
    }

    IEnumerator ShipExplosion()
    {
        yield return new WaitForSeconds(2f);

        explosionGO.SetActive(true);

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Main Scene");

        
    }

    
}
