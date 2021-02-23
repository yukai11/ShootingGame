using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Porting and analyzing the part that did not work with the GameController
//Get the position of the player and the position of the missile and make a hit judgment
public class CollisionEnemyMissile : MonoBehaviour
{
    private const float c_fCollisionDistance = 0.5f;
    public GameObject player;
    public Material red;
    private PlayerController plCon;
    private Vector3 m_vMissilePos;
    private Vector3 m_vPlPos;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Strl_Jager_MK1");
    }

    private void CollisionColorChange(GameObject target,Material color){
       for (int i =0; i<3; i++){
           target.transform.GetChild(i).GetComponent<Renderer>().material= color;
       }
   }

   public void CollisionEnemyMissileUpdate(){
       m_vPlPos = player.transform.position;
        m_vMissilePos = this.transform.position;
        float distance = (m_vPlPos - m_vMissilePos).magnitude;
        if(distance<c_fCollisionDistance){
            plCon = player.GetComponent<PlayerController>();
            plCon.m_sHP -= 1;
            Debug.Log("プレイヤーのHPを表示します");
            Debug.Log(plCon.m_sHP);
            CollisionColorChange(player,red);
            Destroy(this.gameObject);
            }
   }

    // Update is called once per frame
    void Update()
    {
        CollisionEnemyMissileUpdate();
    }
}
