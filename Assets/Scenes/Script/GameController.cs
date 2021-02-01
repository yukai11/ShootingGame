using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    List<GameObject> playerList = new List<GameObject>();

    public GameObject enemy;
    List<GameObject> enemyList = new List<GameObject>();

    public List<GameObject> enemyMissileList = new List<GameObject>();

    public Material[] Material = new Material[2];// Material[0] = black , Material[1] = red;




    public PlayerController plCon;
    public EnemyController EnCon;
    private float timeOut;
    private float timeElapsed;
    private float colorChangeTime;
    private float colorChangeTimeElapsed;

    //CreateEnemy
    private void CreateEnemy(List<GameObject> enemyList){
        Vector3 enPos = new Vector3(Random.Range(-2.0f,2.0f),5,0);
        enemyList.Add(Instantiate(enemy,enPos,Quaternion.Euler(90f,0,0)));
        for(int i = enemyList.Count - 1; i > -1; i--)
      {
        if (enemyList[i] == null){enemyList.RemoveAt(i);}
      }
    }


   //---------------------------------------------------------------------------------------------------------------
   //Function for hit judgment
   //CollisionDetection(attacked things,attacker,Type of attack)
   //Currently, the missile and player hit judgment does not work, so other programs are used instead. Cause unknown. (study)

   　private void CollisionDetection(List<GameObject> enemyList,List<GameObject> missileList,string target){
       for (int i= enemyList.Count-1; i>-1; i--){
           if(enemyList[i] != null){
               Vector3 enemyPos = enemyList[i].transform.position;
               for(int k= missileList.Count-1; k>-1; k--){
                   if(missileList[k] != null){
                       Vector3 missilePos = missileList[k].transform.position;
                       float distance = (enemyPos - missilePos).magnitude;  // distance is length  which enemy from missile.
                       if (distance<1.0f && target=="enemy"){
                           EnCon = enemyList[i].GetComponent<EnemyController>();
                           EnCon.HP -= 1;
                           Destroy(missileList[k]);
                       }else if(distance<1.0f && target=="player"){
                           plCon = enemyList[i].GetComponent<PlayerController>();
                           plCon.HP -= 1;
                           Debug.Log("プレイヤーのHPを表示しますA");
                           Debug.Log(plCon.HP);
                           CollisionColorChange(enemyList[i],Material[1]);
                           Destroy(missileList[k]);
                       }else if(distance<0.5f && target=="P&E"){
                           plCon = enemyList[i].GetComponent<PlayerController>();
                           EnCon = missileList[k].GetComponent<EnemyController>();
                           plCon.HP -= 1;
                           EnCon.HP -= 1;
                           CollisionColorChange(enemyList[i],Material[1]);
                           Debug.Log("プレイヤーのHPを表示します");
                           Debug.Log(plCon.HP);
                       }
                   }
                }
            }
       }
   }

   //---------------------------------------------------------------------------------------------------------------

   // Playre color change
   private void CollisionColorChange(GameObject target,Material color){
       for (int i =0; i<3; i++){
           target.transform.GetChild(i).GetComponent<Renderer>().material= color;
       }
   }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("シフトでミサイル発射、マウスで移動");
        timeElapsed = 0.0f;
        timeOut = 1.0f;
        colorChangeTimeElapsed = 0.0f;
        colorChangeTime = 0.1f;
        playerList.Add(player);
    }

    // Update is called once per frame
    void Update()
    {
        plCon = player.GetComponent<PlayerController>();
        List<GameObject> missileList = plCon.missileList; // Player's missile

        // Setting CreateEnemy interval
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= timeOut){
            CreateEnemy(enemyList);
            timeElapsed = 0.0f;
        }

        // collision detection (Player's missile => enemy and Player <=> enemy)
        CollisionDetection(enemyList,missileList,"enemy");
        //CollisionDetection(playerList,enemyMissileList,"player");
        CollisionDetection(playerList,enemyList,"P&E");

        // red => black interval
        colorChangeTimeElapsed += Time.deltaTime;
        if(colorChangeTimeElapsed >= colorChangeTime){
            CollisionColorChange(player,Material[0]);
            colorChangeTimeElapsed = 0.0f;
        }


    }
}
