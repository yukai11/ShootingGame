using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Porting and analyzing the part that did not work with the GameController
//Get the position of the player and the position of the missile and make a hit judgment
public class CollisionEnemyMissile : MonoBehaviour
{
    public GameObject player;
    public Material red;
    private PlayerController plCon;
    private Vector3 missilePos;
    private Vector3 plPos;

    
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

    // Update is called once per frame
    void Update()
    {
        plPos = player.transform.position;
        missilePos = this.transform.position;
        float distance = (plPos - missilePos).magnitude;
        if(distance<0.5f){
            plCon = player.GetComponent<PlayerController>();
            plCon.HP -= 1;
            Debug.Log("プレイヤーのHPを表示します");
            Debug.Log(plCon.HP);
            CollisionColorChange(player,red);
            Destroy(this.gameObject);
            }
    }
}
