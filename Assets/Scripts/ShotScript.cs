using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    Rigidbody bRb;

    public Transform normandy, enemyShips;

    int enemyCount;

    public Transform target;

    public GameObject explosionGO;

    [SerializeField] AudioClip smallExplosion;

    private void Awake()
    {
        bRb = GetComponent<Rigidbody>();
        //transform.rotation = new Quaternion(0, 90, 90, 1);
    }

    private void Start()
    {
        normandy = GameObject.Find("Normandy").transform;
        enemyShips = GameObject.Find("Small Enemies").transform;

        if (this.gameObject.tag == "ShipShot")
        {
            target = enemyShips.GetChild(GameObject.Find("Normandy").GetComponent<ShipNavigation>().enemyCount);
        } else
        {
            target = normandy;
        }

        if(target == normandy)
        {
            transform.LookAt(target);
            StartCoroutine("End");
        }

        
    }

    private void FixedUpdate()
    {
        if(target != normandy)
        {
            transform.LookAt(target);
        }


        bRb.AddForce(transform.forward * 50f, ForceMode.Impulse);
        bRb.velocity = Vector3.ClampMagnitude(bRb.velocity, 10f);
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider collision)
    {
        

        if (collision.gameObject.transform.parent.tag == "Enemy")
        {

            
            if (target != normandy)
            {
                
                
                Instantiate(explosionGO, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
            

        }

        
    }

}
