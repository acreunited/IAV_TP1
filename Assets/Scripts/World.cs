using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;

public class World : MonoBehaviour {
    public Material material;
    public Material playerMaterial;
    public GameObject cube;
    public GameObject player;
    public static int chunkSize = 16;
    public static int radius = 3;
    public static ConcurrentDictionary<string, Chunk> chunkDict;
    public static List<string> toRemove = new List<string>();
    Vector3 lastBuildPos;
    bool drawing;
    public static float cooldown = 1.5f;
    public static float lastCube;
    public static bool canCreate = true;
    [SerializeField] private AudioSource createCubeEffect;

    public static string CreateChunkName(Vector3 v) {
        return (int)v.x + " " + (int)v.y + " " + (int)v.z;
    }

    IEnumerator BuildRecursiveWorld(Vector3 chunkPos, int rad) {


        int x = (int)chunkPos.x;
        int y = (int)chunkPos.y;
        int z = (int)chunkPos.z;
        BuildChunkAt(chunkPos);
        yield return null;
        if (--rad < 0) {
            yield break;
        }

        Building(new Vector3(x, y, z + chunkSize), rad);
        yield return null;
        Building(new Vector3(x, y, z - chunkSize), rad);
        yield return null;
        Building(new Vector3(x + chunkSize, y, z), rad);
        yield return null;
        Building(new Vector3(x - chunkSize, y, z), rad);
        yield return null;
        Building(new Vector3(x, y + chunkSize, z), rad);
        yield return null;
        Building(new Vector3(x, y - chunkSize, z), rad);
    }


    void BuildChunkAt(Vector3 chunkPos) {
        string name = CreateChunkName(chunkPos);
        Chunk c;
        if (!chunkDict.TryGetValue(name, out c)) {
            c = new Chunk(chunkPos, material);
            c.goChunk.transform.parent = this.transform;
            chunkDict.TryAdd(c.goChunk.name, c);
        }
    }
    IEnumerator RemoveChunks() {
        for (int i = 0; i < toRemove.Count; i++) {
            string name = toRemove[i];
            Chunk c;
            if (chunkDict.TryGetValue(name, out c)) {
                Destroy(c.goChunk);
                chunkDict.TryRemove(name, out c);
                yield return null;
            }
        }
    }


    IEnumerator DrawChunks() {
        drawing = true;
        foreach (KeyValuePair<string, Chunk> c in chunkDict) {
            if (c.Value.status == Chunk.ChunkStatus.DRAW) {
                c.Value.DrawChunk();
                yield return null;

            }
            if (c.Value.goChunk && Vector3.Distance(player.transform.position, c.Value.goChunk.transform.position) > chunkSize * radius) {
                toRemove.Add(c.Key);
            }
        }
        StartCoroutine(RemoveChunks());
        drawing = false;

    }
    void Building(Vector3 chunkPos, int rad) {
        StartCoroutine(BuildRecursiveWorld(chunkPos, rad));
    }
    void Drawing() {
        StartCoroutine(DrawChunks());
    }
    Vector3 WhichChunk(Vector3 position) {
        Vector3 chunkPos = new Vector3();
        chunkPos.x = Mathf.Floor(position.x / chunkSize) * chunkSize;
        chunkPos.y = Mathf.Floor(position.y / chunkSize) * chunkSize;
        chunkPos.z = Mathf.Floor(position.z / chunkSize) * chunkSize;
        return chunkPos;
    }

    // Start is called before the first frame update
    void Start() {

        chunkDict = new ConcurrentDictionary<string, Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        Vector3 ppos = player.transform.position;
        player.transform.position = new Vector3(ppos.x, Utils.GenerateHeight(ppos.x, ppos.z) + 1, ppos.z);
        lastBuildPos = player.transform.position;
        //BuildChunkAt(WhichChunk(player.transform.position));
        Building(WhichChunk(lastBuildPos), radius);
        Drawing();
        player.SetActive(true);
    }

 
    // Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.E)&& canCreate) {
            canCreate = false;
            createCubeEffect.Play();
            Chunk c = new Chunk(player.transform.position, material);
            c.goChunk.AddComponent<MeshCollider>();
            Vector3 pos = new Vector3(player.transform.position.x,player.transform.position.y+1,player.transform.position.z+1);
            Block block = new Block(Block.BlockType.GOLD, pos, c, material);
            block.DrawOnClick();
            this.StartCoroutine(TimeOfCube());
              
            
        }
        else if (Input.GetKey(KeyCode.R) && canCreate)
        {
            canCreate = false;
            createCubeEffect.Play();
            Chunk c = new Chunk(player.transform.position, material);
            c.goChunk.AddComponent<MeshCollider>();
            Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y+1, player.transform.position.z - 1);
            Block block = new Block(Block.BlockType.GOLD, pos, c, material);
            block.DrawOnClick();
            this.StartCoroutine(TimeOfCube());


        }
        else if (Input.GetKey(KeyCode.T) && canCreate)
        {
            canCreate = false;
            createCubeEffect.Play();
            Chunk c = new Chunk(player.transform.position, material);
            c.goChunk.AddComponent<MeshCollider>();
            Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            Block block = new Block(Block.BlockType.GOLD, pos, c, material);
            block.DrawOnClick();
            this.StartCoroutine(TimeOfCube());


        }
        else if (Input.GetKey(KeyCode.Y) && canCreate)
        {
            canCreate = false;
            createCubeEffect.Play();
            Chunk c = new Chunk(player.transform.position, material);
            c.goChunk.AddComponent<MeshCollider>();
            Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y+3, player.transform.position.z);
            Block block = new Block(Block.BlockType.GOLD, pos, c, material);
            block.DrawOnClick();
            this.StartCoroutine(TimeOfCube());


        }
        else if (Input.GetKey(KeyCode.U) && canCreate)
        {
            canCreate = false;
            createCubeEffect.Play();
            Chunk c = new Chunk(player.transform.position, material);
            c.goChunk.AddComponent<MeshCollider>();
            Vector3 pos = new Vector3(player.transform.position.x+1, player.transform.position.y + 1, player.transform.position.z);
            Block block = new Block(Block.BlockType.GOLD, pos, c, material);
            block.DrawOnClick();
            this.StartCoroutine(TimeOfCube());


        }
        else if (Input.GetKey(KeyCode.I) && canCreate)
        {
            canCreate = false;
            createCubeEffect.Play();
            Chunk c = new Chunk(player.transform.position, material);
            c.goChunk.AddComponent<MeshCollider>();
            Vector3 pos = new Vector3(player.transform.position.x - 1, player.transform.position.y + 1, player.transform.position.z);
            Block block = new Block(Block.BlockType.GOLD, pos, c, material);
            block.DrawOnClick();
            this.StartCoroutine(TimeOfCube());


        }

        Vector3 movement = player.transform.position - lastBuildPos;
        if (movement.magnitude > chunkSize) {
            lastBuildPos = player.transform.position;
            Building(WhichChunk(lastBuildPos), radius);
            Drawing();
        }
        if (!drawing) {
            Drawing();
        }
    }

    IEnumerator TimeOfCube()
    {
        yield return new WaitForSeconds(1);
        canCreate = true;

    }
}