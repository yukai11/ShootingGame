using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Missile motion regulation

public class MissileController : MonoBehaviour
{
  
  private Vector3 missilePos; // Define missile position
  private Vector3 plDirection; //Player direction
  public float missileSpeed; // Define missile position
  public GameObject player;

  public float m_fPlSpeed;


    void Start()
    {
      try
      {
        m_fPlSpeed = player.GetComponent<PlayerController>().m_fPlSpeed;
        Debug.Log(m_fPlSpeed);
      }
      catch
      {
         
      }
      plDirection = this.transform.parent.forward;
      missilePos = this.transform.position;
      this.transform.position += missileSpeed*plDirection + m_fPlSpeed*plDirection; //Shift the initial position of the missile forward
    }

    void Update()
    {
      if (Mathf.Abs(this.transform.position.magnitude)>10){
        Destroy(this.gameObject);
      }
       //this.transform.position = missilePos + missileSpeed*plDirection + m_fPlSpeed*plDirection;
       this.transform.position = missilePos + missileSpeed*plDirection + m_fPlSpeed*plDirection;
       missilePos = this.transform.position;
    }
}
