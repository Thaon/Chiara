using UnityEngine;
using System.Collections;

public class PlayerPickAndPlace : MonoBehaviour {

    ChunkBuilder m_activeChunk;


	void Start () {
	
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 v;
            if (PickThisBlock(out v, 4))
            {
                m_activeChunk.SetBlock(v, m_voxelType.empty);
                //create a new block
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Vector3 v;
            if (PickEmptyBlock(out v, 4))
            {
                Debug.Log(v);
                m_activeChunk.SetBlock(v, m_voxelType.sand);
            }
        }
    }

    public GameObject SpawnBlock(Vector3 location, string type) //FINISH THIS!!!
    {
        GameObject block = new GameObject();
        block.AddComponent<MeshGenerator>();
        MeshGenerator gen = block.GetComponent<MeshGenerator>();
        gen.m_voxelScale = 0.5f;
        gen.WorldInit();
        gen.CreateVoxel((int)location.x, (int)location.y, (int)location.z, type);
        return block;
    }

    bool PickThisBlock(out Vector3 v, float dist)
    {
        v = new Vector3();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(
        Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, dist))
        {
            //set the active chunk to be the one associated with the block we hut
            m_activeChunk = hit.collider.GetComponent<MeshGenerator>().m_parent;
            // offset towards the centre of the block hit
            v = hit.point - hit.normal / 2;
            // round down to get the index of the block hit
            v.x = Mathf.Floor(v.x - m_activeChunk.transform.position.x);
            v.y = Mathf.Floor(v.y - m_activeChunk.transform.position.y);
            v.z = Mathf.Floor(v.z - m_activeChunk.transform.position.z);
            //Debug.Log(v.x + ", " + v.y + ", " + v.z);
            //Debug.Log(hit.collider.GetComponent<MeshGenerator>().m_parent);
            return true;
        }
        return false;
    }

    bool PickEmptyBlock(out Vector3 v, float dist)
    {
        v = new Vector3();
        Ray ray = Camera.main.ScreenPointToRay(new
        Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, dist))
        {
            //see above
            m_activeChunk = hit.collider.GetComponent<MeshGenerator>().m_parent;
            // offset towards centre of the neighbouring block
            v = hit.point + hit.normal / 2;
            // round down to get the index of the empty
            v.x = Mathf.Floor(v.x - m_activeChunk.transform.position.x);
            v.y = Mathf.Floor(v.y - m_activeChunk.transform.position.y);
            v.z = Mathf.Floor(v.z - m_activeChunk.transform.position.z);
            return true;
        }
        return false;
    }
}
