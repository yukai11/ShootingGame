using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int HP; // enemy's HP
    public float speed=0.05f;
    public GameObject missile;
    public List<GameObject> enemyMissileList = new List<GameObject>();

    private float timeElapsed;
    private float timeOut;
    
    //----------------------------
    // Not in use
    public GameController gmCon;
    public GameObject world;
    //----------------------------
    
    
    // create missile
    private void attackPlayer(GameObject enemy,GameObject missile,List<GameObject> enemyMissileList){
      Vector3 enPos = enemy.transform.position;
      GameObject　ms = Instantiate(missile,enPos,Quaternion.identity,enemy.transform);
      enemyMissileList.Add(ms);
      for(int i = enemyMissileList.Count - 1; i > -1; i--)
      {
        if (enemyMissileList[i] == null){enemyMissileList.RemoveAt(i);}
      }
    }
    
    
    void Start()
    {
        enemyMissileList = world.GetComponent<GameController>().enemyMissileList;
        HP = 1;
        //this.transform.position = new Vector3(0,5,0); //setting first position
        attackPlayer(this.gameObject,missile,enemyMissileList);
        timeElapsed = 0.0f;
        timeOut = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //enemy Destroy
        if(HP<=0 || this.transform.position.y < -10){
            Destroy(this.gameObject);
        }
        
        //Calculation of enemy position (flaot * Vector3)
         this.transform.position += speed * new Vector3(0.0f,-1.0f,0.0f);

        // create missile interval
         timeElapsed += Time.deltaTime;
         if(timeElapsed >= timeOut){
            attackPlayer(this.gameObject,missile,enemyMissileList);
            timeElapsed=0.0f;
            }
    }
}
