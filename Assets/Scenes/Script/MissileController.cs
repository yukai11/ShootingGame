using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Missile motion regulation

public class MissileController : MonoBehaviour
{
  private Vector3 missilePos; // Define missile position
  private Vector3 plDirection; //Player direction
  public float missileSpeed = 0.1f; // Define missile position

    void Start()
    {
      plDirection = this.transform.parent.forward;
      missilePos = this.transform.position;
      this.transform.position += missileSpeed*plDirection; //Shift the initial position of the missile forward
    }

    void Update()
    {
      if (Mathf.Abs(this.transform.position.magnitude)>10){
        Destroy(this.gameObject);
      }
       this.transform.position = missilePos + missileSpeed*plDirection;
       missilePos = this.transform.position;
    }
}
