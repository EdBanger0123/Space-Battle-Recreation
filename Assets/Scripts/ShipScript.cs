using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
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

    public bool arriveEnabled = false;
    public float slowingDistance = 10;

    [Range(0.0f, 10.0f)]
    public float banking = 0.1f;

    public bool playerSteeringEnabled = false;
    public float playerForce = 100;

    public float damping = 0.1f;

    public bool pathFollowingEnabled = false;
    public float waypointDistance = 3;
    public Path path;

    public ShipScript pursueTarget;
    public bool pursueEnabled;
    public Vector3 pursueTargetPos;

    public GameObject enemyShipsGo, shotGO;

    float shotTime;

    public int enemyCount, camChoice = 0;

    bool hasShot;

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

        if(Vector3.Distance(transform.position, path.waypoints[4]) < 1f)
        {
            enemyShipsGo.SetActive(true);
        }

        if (Vector3.Distance(transform.position, path.waypoints[camChoice]) < 5f)
        {
            if(camChoice < GameObject.Find("Camera Positions").GetComponent<CameraScript>().cameraPos.Length - 1)
            {
                camChoice++;
            }
            
            GameObject.Find("Camera Positions").GetComponent<CameraScript>().ChangeCam(camChoice);
        }

        

        if (Vector3.Distance(transform.position, path.waypoints[12]) < 5f && hasShot == false)
        {
            StartCoroutine("Shoot");
            hasShot = true;
            Debug.Log("ship is shoot");
            
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
        /*if (!path.looped && path.IsLast())
        {
            return null;
        }
        else
        {
            
        }*/
    }

    public Vector3 Calculate()
    {
        nextWaypoint = path.NextWaypoint();
        if (Vector3.Distance(transform.position, nextWaypoint) < 3)
        {
            path.AdvanceToNext();
        }
        
        return nextWaypoint;

        /*if (!path.looped && path.IsLast())
        {
            return boid.ArriveForce(nextWaypoint, 20);
        }
        else
        {
            return boid.SeekForce(nextWaypoint);
        }*/
    }

    /*Vector3 Arrive(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        float dist = toTarget.magnitude;

        float ramped = (dist / slowingDistance) * maxSpeed;
        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = (toTarget / dist) * clamped;

        return desired - velocity;

        
    }*/

    Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        Vector3 desired = toTarget.normalized * maxSpeed;

        return desired - velocity;
    }

    public Vector3 CalculateForce()
    {
        Vector3 force = Vector3.zero;
        /*if (seekEnabled)
        {
            force += Seek(target);
        }*/
        /*if (arriveEnabled)
        {
            force += Arrive(target);
        }*/
        /*if (playerSteeringEnabled)
        {
            force += PlayerSteering();
        }*/

        if (pathFollowingEnabled)
        {
            force += FollowPath();
        }

        /*if (pursueEnabled)
        {
            force += Pursue(pursueTarget);
        }*/

        return force;
    }

    IEnumerator Shoot()
    {
        Debug.Log("shooting");
        Instantiate(shotGO, this.transform.position, this.transform.rotation);
        
        yield return new WaitForSeconds(shotTime);

        if (enemyCount >= 0)
        {
            enemyCount--;
            StartCoroutine("Shoot");
        }
    }

}
