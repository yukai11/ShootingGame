using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private float timeElapsed;
  private float timeOut;


  private Vector3 plPos; // Define player position
  private Vector3 mousePos; // Define player position
  private float plSpeed; // Define player speed

  // PLayer's missile
  public GameObject missile;
  public List<GameObject> missileList = new List<GameObject>();
  
  
  public int HP; //Player's HP
  private Vector3 plDirection;

  


    //--------------------------------------------------------------------
    //This function is to shoot a missile and control missils
    //plPos is player Position
    //missile is missile GameObject
    private void ShootingMissile(GameObject player,GameObject missile,List<GameObject> missileList){
      Vector3 plPos = player.transform.position;
      GameObject　ms = Instantiate(missile,plPos,Quaternion.identity,player.transform);
      missileList.Add(ms);
      for(int i = missileList.Count - 1; i > -1; i--) // clean missileList
      {
        if (missileList[i] == null){missileList.RemoveAt(i);}
      }
      //Debug.Log(missileList.Count);
    }
    //---------------------------------------------------------------------------


    void Start()
    {
      plPos = new Vector3(0,-3,0); //Setting player position
      plSpeed = 0.2f;
      timeElapsed = 0.0f;
      timeOut = 0.5f;
      HP =10;
    }


    void Update()
    {
      plPos = this.transform.position;
      // mouse position change screen to world point
      mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePos.z=0;
      plDirection = (mousePos-plPos).normalized; //Player direction
      if((mousePos-plPos).magnitude<0.1f){
        plSpeed=0;
      }else{
        plSpeed=0.1f;
      }

      // Adjusting missile launch intervals
      timeElapsed += Time.deltaTime;
      if (Input.GetKey(KeyCode.Space)){
        if(timeElapsed >= timeOut){
            ShootingMissile(this.gameObject,missile,missileList);
            timeElapsed = 0.0f;
        }
      }

     
      //　Setting the movable area of the player
      if(plPos.y>4){this.transform.position += new Vector3(0,-0.1f,0);}
      if(plPos.y<-4){this.transform.position += new Vector3(0,0.1f,0);}
      if(plPos.x>2){this.transform.position += new Vector3(-0.1f,0,0);}
      if(plPos.x<-2){this.transform.position += new Vector3(0.1f,0,0);}


      //Calculation of player position (flaot * Vector3)
      this.transform.position += plSpeed*plDirection;
      
      //Change direction of player
      //Insufficient understanding of the principle (study)
      this.transform.rotation = Quaternion.LookRotation(plDirection);
      this.transform.eulerAngles += new Vector3(0,0,90);

      
      if(HP<=0){
        Debug.Log("GameOver");
      }

    

    }
}
