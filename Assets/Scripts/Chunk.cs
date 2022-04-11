using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk 
{
    
    public Block[,,] chunkdata;

    public GameObject goChunk;

    Material material;

    public enum ChunkStatus {DRAW,DONE};
    public ChunkStatus status;

    public Chunk(Vector3 pos,Material material)
    {
        goChunk = new GameObject(World.CreateChunkName(pos));
        goChunk.transform.position = pos;
        this.material = material;
        BuildChunk();
    }
    void BuildChunk()
    {
        chunkdata = new Block[World.chunkSize, World.chunkSize, World.chunkSize];
        for (int z = 0; z < World.chunkSize; z++)
        {
            for (int y = 0; y < World.chunkSize; y++)
            {
                for (int x = 0; x < World.chunkSize; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);

                    int worldX = (int)goChunk.transform.position.x + x;
                    int worldY = (int)goChunk.transform.position.y + y;
                    int worldZ = (int)goChunk.transform.position.z + z;

                    int h=Utils.GenerateHeight(worldX, worldZ);
                    int hs= Utils.GenerateStoneHeight(worldX, worldZ);
                    if (worldY <= hs)
                    {
                        if(Utils.fBM3D(worldX,worldY,worldZ,1,0.5f)<0.495f) {
                            if (Random.Range(0, 10)<=8) {
                                chunkdata[x, y, z] = new Block(Block.BlockType.STONE, pos, this, material);
                            }
                            else {
                                chunkdata[x, y, z] = new Block(Block.BlockType.DIAMOND, pos, this, material);
                            }
                        }  
                        else {
                            chunkdata[x, y, z] = new Block(Block.BlockType.AIR, pos, this, material);
                        }
                    }
                    else if (worldY == h) {
                        chunkdata[x, y, z] = new Block(Block.BlockType.GRASS, pos, this, material);
                    }
                    else if (worldY < h)
                    {
                        chunkdata[x, y, z] = new Block(Block.BlockType.DIRT, pos, this, material);
                    }
                    else
                    {
                        chunkdata[x, y, z] = new Block(Block.BlockType.AIR, pos, this, material);
                    }

                    
                }

            }

        }
        status = ChunkStatus.DRAW;
    }
    public void DrawChunk() {
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                    chunkdata[x, y, z].Draw();

        CombineQuads();
        MeshCollider collider =goChunk.AddComponent<MeshCollider>();
        collider.sharedMesh = goChunk.GetComponent<MeshFilter>().mesh;
        status=ChunkStatus.DONE;
    
    }
    void CombineQuads()
    {
        MeshFilter[] meshFilters = goChunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }
        MeshFilter mf = goChunk.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();

        mf.mesh.CombineMeshes(combine);

        MeshRenderer renderer = goChunk.AddComponent<MeshRenderer>();
        renderer.material = material;

        foreach (Transform quad in goChunk.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }
    }
    // Start is called before the first frame update
    
}
