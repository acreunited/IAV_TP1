using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera cam;

    enum InteractionType { DESTROY,BUILD};
    InteractionType interactionType;
    Block.BlockType []type;
    int pointer = 0;
    public Text blockType;
    string[] blocks;
    void Start()
    {
        type = new Block.BlockType[]{ Block.BlockType.STONE, Block.BlockType.DIRT, Block.BlockType.GOLD};
        blocks = new string[] { "STONE", "DIRT", "GOLD" };
        blockType.text = blocks[pointer];

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pointer = (pointer + 1);
            pointer = pointer % type.Length;
            blockType.text = blocks[pointer];
        }
        bool interaction = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        if (interaction)
        {
            Debug.Log("ajaj");
            interactionType = Input.GetMouseButtonDown(0) ? InteractionType.DESTROY : InteractionType.BUILD;
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, 10))
            {
                string chunkName = hit.collider.name;
                float chunkx = hit.collider.gameObject.transform.position.x;
                float chunky = hit.collider.gameObject.transform.position.y;
                float chunkz = hit.collider.gameObject.transform.position.z;
                Vector3 hitBlock;
                if (interactionType == InteractionType.DESTROY)
                {
                    hitBlock = hit.point - hit.normal / 2f;
                }
                else
                {
                    hitBlock = hit.point + hit.normal / 2f;
                }

                int blockx = (int)(Mathf.Round(hitBlock.x) - chunkx);
                int blocky = (int)(Mathf.Round(hitBlock.y) - chunky);
                int blockz = (int)(Mathf.Round(hitBlock.z) - chunkz);
                Chunk c;
                if(World.chunkDict.TryGetValue(chunkName,out c))
                {
                    if (interactionType == InteractionType.DESTROY)
                    {
                        c.chunkdata[blockx, blocky, blockz].SetType(Block.BlockType.AIR);
                    }
                    else
                    {
                        c.chunkdata[blockx, blocky, blockz].SetType(type[pointer]);
                    }
                    DestroyImmediate(c.goChunk.GetComponent<MeshFilter>());
                    DestroyImmediate(c.goChunk.GetComponent<MeshRenderer>());
                    DestroyImmediate(c.goChunk.GetComponent<MeshCollider>());
                    c.DrawChunk();
                }
            }
        }
    }
}
