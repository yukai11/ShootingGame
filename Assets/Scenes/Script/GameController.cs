using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    private List<GameObject> m_pPlayerList = new List<GameObject>();

    public GameObject enemy;
    List<GameObject> m_pEnemyList = new List<GameObject>();

    public List<GameObject> m_pEnemyMissileList = new List<GameObject>();

    public Material[] m_pMaterial = new Material[2];// Material[0] = black , Material[1] = red;


    public PlayerController plCon;
    public EnemyController EnCon;

    private float m_fTimeOut;
    private float m_fTimeElapsed;
    private float m_fColorChangeTime;
    private float m_fColorChangeTimeElapsed;

    

    //CreateEnemy
    private void _CreateEnemy(List<GameObject> pEnemyList){
        Vector3 enPos = new Vector3(Random.Range(-2.0f,2.0f),0,5);
        pEnemyList.Add(Instantiate(enemy,enPos,Quaternion.Euler(0,180,0)));
        for(int i = pEnemyList.Count - 1; i > -1; i--)
      {
        if (pEnemyList[i] == null){pEnemyList.RemoveAt(i);}
      }
    }


   //---------------------------------------------------------------------------------------------------------------
   //Function for hit judgment
   //CollisionDetection(attacked things,attacker,Type of attack)
   //Currently, the missile and player hit judgment does not work, so other programs are used instead. Cause unknown. (study)

   　private void _CollisionDetection(List<GameObject> enemyList,List<GameObject> pMissileList,string pTarget){
       for (int i= enemyList.Count-1; i>-1; i--){
           if(enemyList[i] != null){
               Vector3 enemyPos = enemyList[i].transform.position;
               for(int k= pMissileList.Count-1; k>-1; k--){
                   if(pMissileList[k] != null){
                       Vector3 missilePos = pMissileList[k].transform.position;
                       float distance = (enemyPos - missilePos).magnitude;  // distance is length  which enemy from missile.
                       if (distance<1.0f && pTarget=="enemy"){
                           EnCon = enemyList[i].GetComponent<EnemyController>();
                           EnCon.m_sHP -= 1;
                           Destroy(pMissileList[k]);
                       }else if(distance<1.0f && pTarget=="player"){
                        //instead of Collision Enemy Missile

                          /* plCon = enemyList[i].GetComponent<PlayerController>();
                           plCon.m_sHP -= 1;
                           Debug.Log("プレイヤーのHPを表示しますA");
                           Debug.Log(plCon.m_sHP);
                           CollisionColorChange(enemyList[i],m_pMaterial[1]);
                           Destroy(pMissileList[k]);*/
                       }else if(distance<0.5f && pTarget=="P&E"){
                           plCon = enemyList[i].GetComponent<PlayerController>();
                           EnCon = pMissileList[k].GetComponent<EnemyController>();
                           plCon.m_sHP -= 1;
                           EnCon.m_sHP -= 1;
                           _CollisionColorChange(enemyList[i],m_pMaterial[1]);
                           Debug.Log("プレイヤーのHPを表示します");
                           Debug.Log(plCon.m_sHP);
                       }
                   }
                }
            }
       }
   }

   //---------------------------------------------------------------------------------------------------------------

   // Playre color change
   private void _CollisionColorChange(GameObject target,Material color){
       for (int i =0; i<3; i++){
           target.transform.GetChild(i).GetComponent<Renderer>().material= color;
       }
   }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("シフトでミサイル発射、マウスで移動");
        m_fTimeElapsed = 0.0f;
        m_fTimeOut = 1.0f;
        m_fColorChangeTimeElapsed = 0.0f;
        m_fColorChangeTime = 0.1f;
        m_pPlayerList.Add(player);
    }

    public void GameControllerUpdate(){
        plCon = player.GetComponent<PlayerController>();
        List<GameObject> pMissileList = plCon.m_pMissileList; // Player's missile

        // Setting CreateEnemy interval
        m_fTimeElapsed += Time.deltaTime;
        if(m_fTimeElapsed >= m_fTimeOut){
            _CreateEnemy(m_pEnemyList);
            m_fTimeElapsed = 0.0f;
        }
        

        // collision detection (Player's missile => enemy and Player <=> enemy)
        _CollisionDetection(m_pEnemyList,pMissileList,"enemy");
        //CollisionDetection(m_pPlayerList,m_pEnemyMissileList,"player");
        _CollisionDetection(m_pPlayerList,m_pEnemyList,"P&E");

        // red => black interval
        m_fColorChangeTimeElapsed += Time.deltaTime;
        if(m_fColorChangeTimeElapsed >= m_fColorChangeTime){
            _CollisionColorChange(player,m_pMaterial[0]);
            m_fColorChangeTimeElapsed = 0.0f;
            }
    }

    // Update is called once per frame
    void Update()
    {
        GameControllerUpdate();
    }
}
