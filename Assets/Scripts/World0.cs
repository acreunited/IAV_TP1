using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World0 : MonoBehaviour
{

    public GameObject block;
    public int size;
    
    IEnumerator BuildWorld() {
        for( int z = 0; z < size; z++) {
            for (int y = 0; y<size; y++) {
                for (int x = 0; x < size; x++) {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject cube = GameObject.Instantiate(block, pos, Quaternion.identity);
                    cube.transform.parent = this.transform;
                    cube.name = x + " " + y + " " + z;
                    if(Random.Range(0,100)<50) {
                        cube.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                }
                
            }
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( BuildWorld() );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
