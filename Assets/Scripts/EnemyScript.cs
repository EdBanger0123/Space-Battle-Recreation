using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Transform normandy;

    Rigidbody eRb;

    public float eSpeed, shotTime;

    [SerializeField] GameObject shotGO;

    public Path path;

    public bool pursue;

    public AudioSource enemyAudio;

    [SerializeField] AudioClip laserShot;


    private void Start()
    {
        enemyAudio = GetComponent<AudioSource>();

        normandy = GameObject.Find("Normandy").transform;
        eRb = GetComponent<Rigidbody>();

        eSpeed = Random.Range(2f, 2.5f);
        shotTime = Random.Range(2f, 4f);

        
        StartCoroutine("Shoot");
        pursue = true;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, path.waypoints[9]) < 10f)
        {
            pursue = false;
        }

        
    }

    private void FixedUpdate()
    {
        if(pursue)
        {
            transform.LookAt(normandy);
            eRb.velocity = transform.forward * (GameObject.Find("Normandy").GetComponent<ShipNavigation>().speed - eSpeed);
        }

        if (pursue == false)
        {
            transform.LookAt(path.waypoints[13]);
            eRb.velocity = transform.forward * 5f;
        }
    }

    IEnumerator Shoot()
    {
        Debug.Log("shooting");
        Instantiate(shotGO, this.transform.position, this.transform.rotation);
        enemyAudio.clip = laserShot;
        enemyAudio.Play();
        //shotGO.GetComponent<ShotScript>().target = shotGO.GetComponent<ShotScript>().normandy;
        yield return new WaitForSeconds(shotTime);

        if(pursue)
        {
            StartCoroutine("Shoot");
        }
    }
}
