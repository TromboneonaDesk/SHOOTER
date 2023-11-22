using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : WeaponBase
{

    [SerializeField] private Rigidbody bullet;
    [SerializeField] private Rigidbody chargedBullet;
    [SerializeField] private float force = 50;
    [SerializeField] private int spread = 3;
    [SerializeField] private int bullets = 8;
    // Start is called before the first frame update
    protected override void Attack(float percent, AmmoType ammo)
    {
        //print("attacked! Charge Percent " + percent);
        Ray camRay = InputManager.GetCameraRay();
        print(ammo);
        for (int i = 0; i < bullets; i++)
        {

            Rigidbody rb = Instantiate(percent > 0.5f ? chargedBullet : bullet, camRay.origin, transform.rotation);

            var xSpread = Random.Range(0f, spread) * 90;
            var ySpread = Random.Range(0f, spread) * 90;
            var spreadVec = new Vector3(xSpread, ySpread, 0);//WE DONT NEED TO TILT IT

            rb.transform.Rotate(spreadVec);

            rb.AddForce(Mathf.Max(percent, 0.1f) * camRay.direction * force, ForceMode.Impulse);
            Destroy(rb.gameObject, 5);

            /*
            Base = 0, 
    Fire = 1, 
    Reverse = 2, 
    Moon = 3
             */
            if (((int)ammo & (int)AmmoType.Fire) == (int)AmmoType.Fire)
            {
                rb.AddForce(rb.velocity.normalized * 25,ForceMode.Acceleration);//double force
            }
            if (((int)ammo & (int)AmmoType.Reverse) == (int)AmmoType.Reverse)
            {
                rb.useGravity = false;
                rb.AddForce(Vector3.up, ForceMode.Acceleration);
            }
            if (((int)ammo & (int)AmmoType.Moon) == (int)AmmoType.Moon)
            {
                rb.AddForce(-rb.velocity.normalized/5);
                rb.transform.localScale = new Vector3(2,2,2);
            }
        }
      
    }

}
