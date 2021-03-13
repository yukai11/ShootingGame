using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Missile motion regulation

public class MissileController : MonoBehaviour
{
  
  private Vector3 missilePos; // Define missile position
  public Vector3 m_vMissileDirection; //Player direction
  public bool m_pDiffusionType; // change type missile (true=>missile is changed)
  public float missileSpeed; // Define missile position
  public GameObject player;

  public float m_fPlSpeed;

    // Do not set the start position
    void Start()
    {
      try
      {
        m_fPlSpeed = player.GetComponent<PlayerController>().m_fPlSpeed;
      }
      catch
      {
      }
      if(m_pDiffusionType==false){m_vMissileDirection = this.transform.parent.forward;}
      missilePos = this.transform.position;
      //this.transform.position += m_vMissileDirection + m_fPlSpeed*m_vMissileDirection; //Shift the initial position of the missile forward
    }

    void Update()
    {
      if (Mathf.Abs(this.transform.position.magnitude)>10){
        Destroy(this.gameObject);
      }
       //this.transform.position = missilePos + missileSpeed*m_vMissileDirection + m_fPlSpeed*m_vMissileDirection;
       this.transform.position = missilePos + missileSpeed*m_vMissileDirection;
       missilePos = this.transform.position;
    }
}
