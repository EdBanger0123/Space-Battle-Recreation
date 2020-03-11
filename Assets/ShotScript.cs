using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    Rigidbody bRb;

    public Transform normandy; 

    private void Start()
    {
        normandy = GameObject.Find("Normandy").transform;
        bRb = GetComponent<Rigidbody>();

        
        transform.LookAt(normandy);

        bRb.AddForce(transform.forward * 1000f);

        StartCoroutine("End");

    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }


}
