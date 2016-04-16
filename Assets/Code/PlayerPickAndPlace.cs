using UnityEngine;
using System.Collections;

public class PlayerPickAndPlace : MonoBehaviour {

    ChunkBuilder m_activeChunk;
    public GameObject m_inventoryCanvas;

    enum State { inventory, playing };

    State m_state = State.playing;

    void Update()
    {
        if (m_state == State.playing)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 v;
                if (PickThisBlock(out v, 4))
                {
                    int iType = m_activeChunk.m_terrainArray[(int)v.x, (int)v.y, (int)v.z];
                    print(iType);

                    m_activeChunk.SetBlock(v, m_voxelType.empty);

                    m_voxelType vType = m_voxelType.grass;
                    switch (iType)
                    {
                        case 1:
                        vType = m_voxelType.grass;
                        break;
                        case 2:
                        vType = m_voxelType.stone;
                        break;
                        case 3:
                        vType = m_voxelType.dirt;
                        break;
                        case 4:
                        vType = m_voxelType.sand;
                        break;
                    }
                    //spawn block there
                    m_activeChunk.SpawnSmallVoxel(v, vType);
                }
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                Vector3 v;
                if (PickEmptyBlock(out v, 4))
                {
                    //Debug.Log(v);
                    m_activeChunk.SetBlock(v, m_voxelType.sand);
                }
            }
            else if (Input.GetButtonUp("Inventory"))
            {
                GetComponent<FPSInputController>().enabled = false;
                GetComponent<CharacterMotor>().enabled = false;
                foreach (MouseLook ml in GetComponentsInChildren<MouseLook>())
                {
                    ml.enabled = false;
                }
                
                Screen.showCursor = true;
                Screen.lockCursor = false;
                m_inventoryCanvas.SetActive(true);
                GetComponent<InventoryManager>().RefreshInventory();
                m_state = State.inventory;
            }
        }
        else
        {
            if (Input.GetButtonUp("Inventory"))
            {
                GetComponent<FPSInputController>().enabled = true;
                GetComponent<CharacterMotor>().enabled = true;
                foreach (MouseLook ml in GetComponentsInChildren<MouseLook>())
                {
                    ml.enabled = true;
                }

                Screen.showCursor = false;
                Screen.lockCursor = true;
                m_inventoryCanvas.SetActive(false);
                m_state = State.playing;
            }
        }

    }

    bool PickThisBlock(out Vector3 v, float dist)
    {
        v = new Vector3();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(
        Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, dist))
        {
            //set the active chunk to be the one associated with the block we hit
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
    }    //void SpawnSmallVoxel(Vector3 index, m_voxelType voxelType)
    //{
    //    GameObject voxel = new GameObject();
    //    voxel.AddComponent<MeshGenerator>();
    //    voxel.AddComponent<Rigidbody>();
    //    voxel.GetComponent<MeshCollider>().convex = true;
    //    voxel.AddComponent<SmallVoxel>();

    //    voxel.transform.position = index;
    //    voxel.transform.Translate(0.25f, 0, 0.25f); //center the voxel

    //    voxel.GetComponent<MeshGenerator>().WorldInit();
    //    //print(voxel.transform.position);

    //    //get texture
    //    string tex;
    //    switch (voxelType)
    //    {
    //        case m_voxelType.grass:
    //        tex = "Grass";
    //        break;
    //        case m_voxelType.stone:
    //        tex = "Stone";
    //        break;
    //        case m_voxelType.dirt:
    //        tex = "Dirt";
    //        break;
    //        case m_voxelType.sand:
    //        tex = "Sand";
    //        break;
    //        default:
    //        tex = "Grass";
    //        break;
    //    }
    //    //print(voxelType);
    //    voxel.GetComponent<MeshGenerator>().m_voxelScale = 0.5f;
    //    //x
    //    voxel.GetComponent<MeshGenerator>().CreateNegativeXFace(voxel.transform.position.x, voxel.transform.position.y, voxel.transform.position.z, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
    //    voxel.GetComponent<MeshGenerator>().CreatePositiveXFace(voxel.transform.position.x, voxel.transform.position.y, voxel.transform.position.z, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
    //    //y
    //    voxel.GetComponent<MeshGenerator>().CreateNegativeYFace(voxel.transform.position.x, voxel.transform.position.y, voxel.transform.position.z, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
    //    voxel.GetComponent<MeshGenerator>().CreatePositiveYFace(voxel.transform.position.x, voxel.transform.position.y, voxel.transform.position.z, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
    //    //z
    //    voxel.GetComponent<MeshGenerator>().CreateNegativeZFace(voxel.transform.position.x, voxel.transform.position.y, voxel.transform.position.z, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
    //    voxel.GetComponent<MeshGenerator>().CreatePositiveZFace(voxel.transform.position.x, voxel.transform.position.y, voxel.transform.position.z, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));

    //    voxel.GetComponent<MeshGenerator>().UpdateWorld();
    //    //print(voxel.transform.position);

    //    voxel = null; //lose reference to the voxel
    //}
}
