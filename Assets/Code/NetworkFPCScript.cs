using UnityEngine;
using System.Collections;

public class NetworkFPCScript : MonoBehaviour {

    float m_lastSyncTime = 0f;
    float m_syncDelay = 0f;
    float m_syncTime = 0f;
    Vector3 m_startPosition = Vector3.zero;
    Vector3 m_endPosition = Vector3.zero;

    void Start()
    {
        if (networkView.isMine)
        {
            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour m in components)
            {
                m.enabled = true;
            }
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(true);
            }
        }
    }

    void Update()
    {
        if (!networkView.isMine)
        {
            m_syncTime += Time.deltaTime;
            if (m_syncTime < m_syncDelay)
            {
                transform.position = Vector3.Lerp(m_startPosition, m_endPosition, m_syncTime / m_syncDelay);
            }
        }        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                Vector3 v;
                ChunkBuilder vcs;
                if (PickThisBlock(out v, out vcs, 4))
                {
                    NetworkView nv = vcs.GetComponent<NetworkView>();
                    if (nv != null)
                    {
                        int iType = vcs.m_terrainArray[(int)v.x, (int)v.y, (int)v.z];
                        m_voxelType vType = m_voxelType.grass;
                        switch (iType)
                        {
                            case 1:
                            vType = m_voxelType.grass;
                            break;
                            case 2:
                            vType = m_voxelType.dirt;
                            break;
                            case 3:
                            vType = m_voxelType.stone;
                            break;
                            case 4:
                            vType = m_voxelType.sand;
                            break;
                        }
                        vcs.SpawnSmallVoxel(v, vType);
                        nv.RPC("SetBlock", RPCMode.All, v, (int)m_voxelType.empty);
                    }
                }
            }
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);
            syncVelocity = rigidbody.velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);
            m_syncTime = 0f;
            m_syncDelay = Time.time - m_lastSyncTime;
            m_lastSyncTime = Time.time;
            // last predicted position
            m_startPosition = rigidbody.position;
            // predicted position
            m_endPosition = syncPosition + syncVelocity * m_syncDelay;
        }
    }    bool PickThisBlock(out Vector3 v, out ChunkBuilder voxelChunkScript, float dist)
    {
        v = new Vector3();
        voxelChunkScript = null;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, dist))
        {
            // check if the target we hit has a VoxelChunk script
            voxelChunkScript = hit.collider.gameObject.GetComponent<ChunkBuilder>();
            if (voxelChunkScript != null)
            {
                // offset toward centre of the block hit
                v = hit.point - hit.normal / 2;
                // round down to get the index of the block hit
                v.x = Mathf.Floor(v.x);
                v.y = Mathf.Floor(v.y);
                v.z = Mathf.Floor(v.z);
                return true;
            }
        }
        return false;
    }}
