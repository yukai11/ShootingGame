using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int m_sHP; // enemy's m_sHP
    public float m_fSpeed=0.05f;
    public GameObject missile;
    public List<GameObject> m_pEnemyMissileList = new List<GameObject>();

    public Vector3 m_vEnemeyDirection;
    public float m_fTimeElapsed = 0.0f;
    public float m_fTimeOut = 1.0f;
    
    //----------------------------
    // Not in use
    public GameController gmCon;
    //----------------------------
    public GameObject world;
    
    
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //　@Brife : create missile
    private void _AttackPlayer(GameObject enemy,GameObject missile,List<GameObject> enemyMissileList){
      Vector3 enPos = enemy.transform.position;
      GameObject　ms = Instantiate(missile,enPos,Quaternion.identity,enemy.transform);
      enemyMissileList.Add(ms);
      for(int i = enemyMissileList.Count - 1; i > -1; i--)
      {
        if (enemyMissileList[i] == null){enemyMissileList.RemoveAt(i);}
      }
    }
    //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    
    

    void Start()
    {
        m_pEnemyMissileList = world.GetComponent<GameController>().m_pEnemyMissileList;
        m_sHP = 1;
        //this.transform.position = new Vector3(0,5,0); //setting first position
        _AttackPlayer(this.gameObject,missile,m_pEnemyMissileList);
    }

    public void EnemyControllerUpdate(){
      //enemy Destroy
        if(m_sHP<=0 || this.transform.position.z < -10){
            Destroy(this.gameObject);
        }
        //Calculation of enemy position (flaot * Vector3)
        this.transform.position += m_fSpeed * m_vEnemeyDirection;

        // create missile interval
         m_fTimeElapsed += Time.deltaTime;
         if(m_fTimeElapsed >= m_fTimeOut){
            _AttackPlayer(this.gameObject,missile,m_pEnemyMissileList);
            m_fTimeElapsed=0.0f;
           }
    }

    // Update is called once per frame
    void Update()
    {
      EnemyControllerUpdate();
    }
}
