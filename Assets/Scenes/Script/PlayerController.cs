using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private const float c_fPlSpeedMove = 0.1f; //if player move, player speed
  private const float c_fPlSpeedStop = 0.0f; //if player stop, player speed
  private const float c_fDeltaAngle = 3.0f;
  
  private  Vector2 m_vMovableRangeX = new Vector2(-2,2); // Define the area which player can move (Coordinate　X)
  private  Vector2 m_vMovableRangeZ = new Vector2(-4,4);// Define the area which player can move (Coordinate　Z)
  private const float c_fElasticityMagnitude = 0.1f;

  private float m_fTimeElapsed; //Elapsed time since shooting missiles
  public  float m_fTimeOut = 0.5f; //shooting missiles interval

  private Vector3 m_vPlPos; // Define player position
  //private Vector3 mousePos; // Define player position
  public float m_fPlSpeed; // Define player speed

  // PLayer's missile
  public GameObject missile;
  public List<GameObject> m_pMissileList = new List<GameObject>();
  
  
  public int m_sHP; //Player's m_sHP
  private Vector3 m_vPlDirection; // player direction

  


    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brief : This function is to shoot a missile and control missils
    //vPlPos is player Position
    //missile is missile GameObject
    private void _ShootingMissile(GameObject player,GameObject missile,List<GameObject> pMissileList){
      Vector3 vPlPos = player.transform.position;
      GameObject　ms = Instantiate(missile,vPlPos,Quaternion.identity,player.transform);
      pMissileList.Add(ms);
      for(int i = pMissileList.Count - 1; i > -1; i--) // clean m_pMissileList
      {
        if (pMissileList[i] == null){pMissileList.RemoveAt(i);}
      }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

　　//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brief : This function is to shoot a missile and control missils
    //vPlPos is player Position
    //missile is missile GameObject
    private void _ShootingMissileType2(GameObject player,GameObject missile,List<GameObject> pMissileList,string pType){
      Vector3 vPlPos = player.transform.position;
      if(pType=="normal"){
      GameObject　ms = Instantiate(missile,vPlPos,Quaternion.identity,player.transform);
      pMissileList.Add(ms);
      }else if(pType=="double"){
          GameObject ms1 = Instantiate(missile,vPlPos+new Vector3(-vPlPos.z,vPlPos.y,vPlPos.x).normalized,Quaternion.identity,player.transform);
          GameObject ms2 = Instantiate(missile,vPlPos+new Vector3(vPlPos.z,vPlPos.y,-vPlPos.x).normalized,Quaternion.identity,player.transform);
          pMissileList.Add(ms1);
          pMissileList.Add(ms2);
      }
      for(int i = pMissileList.Count - 1; i > -1; i--) // clean m_pMissileList
      {
        if (pMissileList[i] == null){pMissileList.RemoveAt(i);}
        }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brief : The mouse control player's direction
    //
    //
    private void _UseMousePosition(Vector3 vPlPos){
      // mouse position change screen to world point
      Vector3 vMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      vMousePos.y=0;
      m_vPlDirection = (vMousePos-vPlPos).normalized; //Player direction
      if((vMousePos-vPlPos).magnitude<0.1f){
        m_fPlSpeed = c_fPlSpeedStop;
      }else{
        m_fPlSpeed = c_fPlSpeedMove;
      }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brief : using keyboard can control player direction
    private void _UseKeyBoardPosition(){
      m_fPlSpeed = c_fPlSpeedStop;
      if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
        m_fPlSpeed = c_fPlSpeedMove;
        m_vPlDirection = new Vector3(Mathf.Sin(Mathf.PI*this.transform.eulerAngles.y/180),0,Mathf.Cos(Mathf.PI*this.transform.eulerAngles.y/180)).normalized;
      }
      //Below code do not use (use now)
      //It is because real jet cannot move back direction
      if(Input.GetKey(KeyCode.S)|| Input.GetKey(KeyCode.DownArrow)){
        m_fPlSpeed = c_fPlSpeedMove;
        m_vPlDirection = new Vector3(Mathf.Sin(-Mathf.PI*this.transform.eulerAngles.y/180),0,-Mathf.Cos(Mathf.PI*this.transform.eulerAngles.y/180)).normalized;
      }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //　@Brife : Setting the movable area of the player
    private void _MovableRange(Vector3 vPlPos){
      if(vPlPos.z>m_vMovableRangeZ.x){this.transform.position += new Vector3(0,0,-c_fElasticityMagnitude);}
      if(vPlPos.z<m_vMovableRangeZ.y){this.transform.position += new Vector3(0,0,c_fElasticityMagnitude);}
      if(vPlPos.x>m_vMovableRangeX.x){this.transform.position += new Vector3(-c_fElasticityMagnitude,0,0);}
      if(vPlPos.x<m_vMovableRangeX.y){this.transform.position += new Vector3(c_fElasticityMagnitude,0,0);}
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brife : using mouse control player's rotarion
    private void _RotationPlayerUseMouse(){
      float fDot = Vector3.Dot(m_vPlDirection,new Vector3(0,0,1));
      float fAngle = 0;
      if(m_vPlDirection.x<=0){
          fAngle = -180 * Mathf.Acos(fDot) / Mathf.PI;
        }else{
          fAngle = 180 * Mathf.Acos(fDot) / Mathf.PI;
        }
      this.transform.eulerAngles = new Vector3(0,fAngle,0);
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brife : using keyboard control player's rotarion
    private void _RotationPlayerUseKeyBoard(){
      float fAngle = this.transform.eulerAngles.y;
        if(Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow)){fAngle += c_fDeltaAngle;}
        if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow)){fAngle -= c_fDeltaAngle;}
      this.transform.eulerAngles = new Vector3(0,fAngle,0);
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    private void _GameOver(){
      if(m_sHP<=0){
        Debug.Log("GameOver");
      }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



    void Start()
    {
      Application.targetFrameRate = 60;
      //m_vPlPos = new Vector3(0,0,0); //Setting player position
      //m_fTimeElapsed = 0.0f;
      m_sHP =10;
    }

    public void PlayerControllerUpdate(){
      m_vPlPos = this.transform.position;
      //_UseMousePosition(m_vPlPos);//create player speed
      _UseKeyBoardPosition();
      // Adjusting missile launch intervals
      m_fTimeElapsed += Time.deltaTime;
      if (Input.GetKey(KeyCode.Space)){
        if(m_fTimeElapsed >= m_fTimeOut){
            _ShootingMissile(this.gameObject,missile,m_pMissileList);
            m_fTimeElapsed = 0.0f;
        }
      }
      //Calculation of player position (flaot * Vector3)
      this.transform.position += m_fPlSpeed*m_vPlDirection; //　decision player position
      //_RotationPlayerUseMouse(); //Change direction of player
      _RotationPlayerUseKeyBoard();
      _MovableRange(m_vPlPos); //Define Movable range
      _GameOver(); //game over message
    }


    void Update()
    {
      PlayerControllerUpdate();
      
    }
}
