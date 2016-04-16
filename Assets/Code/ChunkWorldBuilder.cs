using UnityEngine;
using System.Collections;
using System.Xml;

public class ChunkWorldBuilder : MonoBehaviour
{
    // delegate signature
    public delegate void EventBlockChanged(m_voxelType blockType);
    // event instances for EventBlockChanged
    public static event EventBlockChanged OnEventBlockDestroyed;
    public static event EventBlockChanged OnEventBlockPlaced;

    public ChunkBuilder m_chunk;
    //public int m_worldXSize = 16;
    //public int m_worldZSize = 16;

    //chunk related data
    public int m_chunkSize = 32;
    public int m_chunkHeight = 32;

    void Start()
    {
        //create the new chunk
        GameObject newChunk = new GameObject();
        newChunk.name = "Chunk at: " + 0 + " : " + 0;
        newChunk.AddComponent<ChunkBuilder>();
        newChunk.AddComponent<MeshGenerator>();

        //customise the new chunk
        ChunkBuilder cb = newChunk.GetComponent<ChunkBuilder>();
        cb.m_chunkSize = m_chunkSize;
        cb.m_chunkHeight = m_chunkHeight;
        m_chunk = cb;
        cb.GenerateChunk();

        //place player in the middle of it
        GameObject.FindWithTag("Player").transform.position = transform.position + new Vector3(m_chunkSize / 2, 5, m_chunkSize/ 2);


        //CreateWorld(m_chunks);
    }

    public void CreateWorld()//ChunkBuilder[,] newChunks)
    {
        //newChunks = new ChunkBuilder[m_worldXSize, m_worldZSize];
        //for (int z = 0; z < m_worldZSize; z++)
        //{
        //    for (int x = 0; x < m_worldZSize; x++)
        //    {
        //        //create the new chunk
        //        GameObject newChunk = new GameObject();
        //        newChunk.name = "Chunk at: " + x + " : " + z;
        //        newChunk.AddComponent<ChunkBuilder>();
        //        newChunk.AddComponent<MeshGenerator>();

        //        //put new chunk in position
        //        newChunk.transform.position = new Vector3(x * m_chunkSize, 0, z * m_chunkSize);

        //        //customise the new chunk
        //        ChunkBuilder cb = newChunk.GetComponent<ChunkBuilder>();
        //        cb.m_chunkSize = m_chunkSize;
        //        cb.m_chunkHeight = m_chunkHeight;
        //        cb.m_world = this;
        //        cb.GenerateChunk();
        //        newChunks[x, z] = cb;
        //    }
        //}

        //place the player in the middle of the chunk
        //GameObject.FindWithTag("Player").transform.position = transform.position + new Vector3((m_chunkSize * m_worldXSize) / 2, 5, (m_chunkSize * m_worldZSize) / 2);
        //m_chunks = newChunks;
    }

    public void BlockChanged(m_voxelType type)//ADD AGAIN AFTER GETTING THE SOUNDS!!!
    {
        if (type == m_voxelType.empty)
        {
            //OnEventBlockDestroyed(m_voxelType.empty);
        }
        else
        {
            //OnEventBlockPlaced(type);
        }
    }
}