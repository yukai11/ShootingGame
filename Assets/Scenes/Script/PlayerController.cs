using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private const float c_fPlSpeedMove = 0.1f; //if player move, player speed
  private const float c_fPlSpeedStop = 0.0f; //if player stop, player speed

  private float fTimeElapsed; //Elapsed time since shooting missiles
  public  float fTimeOut = 0.5f; //shooting missiles interval

  private Vector3 plPos; // Define player position
  private Vector3 mousePos; // Define player position
  private float plSpeed; // Define player speed

  // PLayer's missile
  public GameObject missile;
  public List<GameObject> missileList = new List<GameObject>();
  
  
  public int HP; //Player's HP
  private Vector3 plDirection;

  


    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
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
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private void UseMousePosition(Vector3 plPos){
      // mouse position change screen to world point
      mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePos.y=0;
      plDirection = (mousePos-plPos).normalized; //Player direction
      if((mousePos-plPos).magnitude<0.1f){
        plSpeed = c_fPlSpeedStop;
      }else{
        plSpeed = c_fPlSpeedMove;
      }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private void UseKeyBoardPosition(){
      plSpeed = c_fPlSpeedStop;
      if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
        plSpeed = c_fPlSpeedMove;
        plDirection = new Vector3(Mathf.Sin(Mathf.PI*this.transform.eulerAngles.y/180),0,Mathf.Cos(Mathf.PI*this.transform.eulerAngles.y/180)).normalized;
      }
      //Below code do not use
      //It is because real jet cannot move back direction
      /*if(Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow)){
        plSpeed = c_fPlSpeedMove;
        plDirection = new Vector3(Mathf.Sin(-Mathf.PI*this.transform.eulerAngles.y/180),0,-Mathf.Cos(Mathf.PI*this.transform.eulerAngles.y/180)).normalized;
      }*/
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private void MovableRange(Vector3 plPos){
      //　Setting the movable area of the player
      if(plPos.z>4){this.transform.position += new Vector3(0,0,-0.1f);}
      if(plPos.z<-4){this.transform.position += new Vector3(0,0,0.1f);}
      if(plPos.x>2){this.transform.position += new Vector3(-0.1f,0,0);}
      if(plPos.x<-2){this.transform.position += new Vector3(0.1f,0,0);}
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private void RotationPlayerUseMouse(){
      float s_fDot = Vector3.Dot(plDirection,new Vector3(0,0,1));
      float s_fAngle = 0;
      if(plDirection.x<=0){
          s_fAngle = -180 * Mathf.Acos(s_fDot) / Mathf.PI;
        }else{
          s_fAngle = 180 * Mathf.Acos(s_fDot) / Mathf.PI;
        }
      this.transform.eulerAngles = new Vector3(0,s_fAngle,0);
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private void RotationPlayerUseKeyBoard(){
      float s_fAngle = this.transform.eulerAngles.y;
        if(Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow)){s_fAngle += 10;}
        if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow)){s_fAngle -= 10;}
      this.transform.eulerAngles = new Vector3(0,s_fAngle,0);
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private void GameOver(){
      if(HP<=0){
        Debug.Log("GameOver");
      }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    void Start()
    {
      plPos = new Vector3(0,0,0); //Setting player position
      plSpeed = 0.2f;
      fTimeElapsed = 0.0f;
      HP =10;
    }

    public void PlayerControllerUpdate(){
      plPos = this.transform.position;
      //UseMousePosition(plPos);//create player speed
      UseKeyBoardPosition();
      // Adjusting missile launch intervals
      fTimeElapsed += Time.deltaTime;
      if (Input.GetKey(KeyCode.Space)){
        if(fTimeElapsed >= fTimeOut){
            ShootingMissile(this.gameObject,missile,missileList);
            fTimeElapsed = 0.0f;
        }
      }
      //Calculation of player position (flaot * Vector3)
      this.transform.position += plSpeed*plDirection; //　decision player position
      //RotationPlayerUseMouse(); //Change direction of player
      RotationPlayerUseKeyBoard();
      MovableRange(plPos); //Define Movable range
      GameOver(); //game over message
    }


    void Update()
    {
      PlayerControllerUpdate();
    }
}
