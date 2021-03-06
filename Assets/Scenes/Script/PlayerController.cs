using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private const float c_fPlSpeedMove = 0.1f; //if player move, player speed
  private const float c_fPlSpeedStop = 0.0f; //if player stop, player speed
  private const float c_fDeltaAngle = 3.0f;
  private const float c_fDistanceFromItem = 0.5f;
  
  private  Vector2 m_vMovableRangeX = new Vector2(-2,2); // Define the area which player can move (Coordinate　X)
  private  Vector2 m_vMovableRangeZ = new Vector2(-4,4);// Define the area which player can move (Coordinate　Z)
  private const float c_fElasticityMagnitude = 0.1f;


  // Time setting
  private float m_fTimeElapsed; //Elapsed time since shooting missiles
  public  float m_fTimeOut; //shooting missiles interval
  private float m_fItemTimeElapsed; //Elapsed time since create item
  public  float m_fItemTimeOut; // create item interval



  private Vector3 m_vPlPos; // Define player position
  //private Vector3 mousePos; // Define player position
  public float m_fPlSpeed; // Define player speed

  // PLayer's missile
  public GameObject missile;
  public List<GameObject> m_pMissileList = new List<GameObject>();

  // Player's Item
  public GameObject AddMissleItem;
  public GameObject RemoveMissleItem;
  public List<GameObject> m_pItemList = new List<GameObject>();
  public int m_sItemNum;
  
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
    //sType number is missiles number
    //Launch a missile on a straight line
    private void _ShootingMissileType2(GameObject player,GameObject missile,List<GameObject> pMissileList,int sType){
      if(sType==1){
        _ShootingMissile(player,missile,pMissileList);
        }else{
      Vector3 vPlPos = player.transform.position;
      Vector3 vMissilePosDifference = 2 * new Vector3(m_vPlDirection.z,0,-m_vPlDirection.x);
      for(int i = 0; i<sType; i++){
        GameObject msi = Instantiate(missile,vPlPos+i*vMissilePosDifference/(sType-1) - new Vector3(m_vPlDirection.z,0,-m_vPlDirection.x),Quaternion.identity,player.transform);
        pMissileList.Add(msi);
      }
      for(int i = pMissileList.Count - 1; i > -1; i--) // clean m_pMissileList
      {
        if (pMissileList[i] == null){pMissileList.RemoveAt(i);}
        }
        }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brife : Processing to change the number of _ShoothingMissileType2’s missile num
    private void _MissileNumber(GameObject player,List<GameObject> pItemList){
      Vector3 vPlPos = player.transform.position;
      try
      {
          for(int i = pItemList.Count-1; i>-1; i--){
        Vector3 vItemList = pItemList[i].transform.position;
        if((vPlPos-vItemList).magnitude<c_fDistanceFromItem){
          if(pItemList[i].name=="AddMissileItem(Clone)"){m_sItemNum+=1;}
          if(pItemList[i].name=="RemoveMissileItem(Clone)" && m_sItemNum>0){m_sItemNum -=1;}
          Destroy(pItemList[i]);
        }
      }
      }
      catch 
      {
      }

      

    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //@Brife : Create player's item
    private void _CreateItem(GameObject CreationTarget){
      Vector3 vItemPos = new Vector3(Random.Range(-2.0f,2.0f),0,5);
      m_pItemList.Add(Instantiate(CreationTarget,vItemPos,Quaternion.identity));
      // delete null list data
      for(int i = m_pItemList.Count - 1; i > -1; i--)
      {
        if (m_pItemList[i] == null){m_pItemList.RemoveAt(i);}
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
    //         and setting m_vPlDirection
    private void _RotationPlayerUseKeyBoard(){
      float fAngle = this.transform.eulerAngles.y;
        if(Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.RightArrow)){fAngle += c_fDeltaAngle;}
        if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.LeftArrow)){fAngle -= c_fDeltaAngle;}
      this.transform.eulerAngles = new Vector3(0,fAngle,0);
      m_vPlDirection = new Vector3(Mathf.Sin(-Mathf.PI*fAngle/180),0,-Mathf.Cos(Mathf.PI*fAngle/180)).normalized;
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
      m_sItemNum=1;
      m_vPlDirection = new Vector3(Mathf.Sin(-Mathf.PI*this.transform.eulerAngles.y/180),0,-Mathf.Cos(Mathf.PI*this.transform.eulerAngles.y/180)).normalized;
    }

    public void PlayerControllerUpdate(){
      

      m_vPlPos = this.transform.position;
      //_UseMousePosition(m_vPlPos);//create player speed
      _UseKeyBoardPosition();
      // Adjusting missile launch intervals
      m_fTimeElapsed += Time.deltaTime;
      if (Input.GetKey(KeyCode.Space)){
        if(m_fTimeElapsed >= m_fTimeOut){
            _ShootingMissileType2(this.gameObject,missile,m_pMissileList,m_sItemNum);
            m_fTimeElapsed = 0.0f;
        }
      }
      //Calculation of player position (flaot * Vector3)
      this.transform.position += m_fPlSpeed*m_vPlDirection; //　decision player position
      //_RotationPlayerUseMouse(); //Change direction of player
      _RotationPlayerUseKeyBoard();
      _MovableRange(m_vPlPos); //Define Movable range
      
      _MissileNumber(this.gameObject,m_pItemList);
      m_fItemTimeElapsed += Time.deltaTime;
      if(m_fItemTimeElapsed >= m_fItemTimeOut){
        _CreateItem(AddMissleItem);
        _CreateItem(RemoveMissleItem);
            m_fItemTimeElapsed = 0.0f;
        }


      _GameOver(); //game over message
    }


    void Update()
    {
      PlayerControllerUpdate();
    }
}
