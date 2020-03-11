using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Transform normandy;

    Rigidbody eRb;

    public float eSpeed, shotTime;

    [SerializeField] GameObject shotGO;


    private void Start()
    {
        normandy = GameObject.Find("Normandy").transform;
        eRb = GetComponent<Rigidbody>();

        eSpeed = Random.Range(1f, 1.5f);
        shotTime = Random.Range(2f, 4f);

        StartCoroutine("Shoot");
    }

    private void FixedUpdate()
    {
        transform.LookAt(normandy);
        eRb.velocity = transform.forward * (GameObject.Find("Normandy").GetComponent<ShipScript>().speed - eSpeed);
        
    }

    IEnumerator Shoot()
    {
        Debug.Log("shooting");
        Instantiate(shotGO, this.transform.position, this.transform.rotation);
        
        yield return new WaitForSeconds(shotTime);
        StartCoroutine("Shoot");

    }
}
