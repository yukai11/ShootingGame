using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private const float c_fItemSpeed = 0.01f;
    private Vector3 c_vItemDirection = new Vector3(0,0,-1);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void _ItemControllerUpdate(){
        Vector3 ItmePos = this.transform.position;
        this.transform.position += c_fItemSpeed * c_vItemDirection;
        if(ItmePos.z<-5){Destroy(this.gameObject);}
    }
    void Update()
    {
        _ItemControllerUpdate();
    }
}
