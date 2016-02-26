using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour {
    [SerializeField]
    List<ChunkBuilder> m_chunks;
    [SerializeField]
    List<Vector3> m_waypoints;
    int m_currentWaypoint = 0;
    public bool m_isTraversing = false;
    float m_startTime;
    float m_speed = 1.0f;

    // Use this for initialization
    void Start ()
    {
        m_chunks = new List<ChunkBuilder>();
        m_waypoints = new List<Vector3>();

        GetChunks();
        BuildPathFromChunk(m_chunks[0]);

        //we now go to the beginning of the path
        GoToBeginningOfPath();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (m_isTraversing)
        {
            TraversePath();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !m_isTraversing)
        {
            m_startTime = Time.time;
            m_currentWaypoint = 0;
            GoToBeginningOfPath();
            m_isTraversing = true;
        }
	}

    void GetChunks()
    {
        foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunk"))
        {
            m_chunks.Add(chunk.GetComponent<ChunkBuilder>());
        }
    }

    void BuildPathFromChunk(ChunkBuilder chunk)
    {
        //we take into consideration all the stone tiles that have at least 2 empty blocks above
        //z
        for (int z = 0; z < chunk.GetWorldXZ(); z++)
        {
            //y
            for (int y = 0; y < chunk.GetWorldY(); y++)
            {
                for (int x = 0; x < chunk.GetWorldXZ(); x++)
                {
                    //x
                    if (chunk.GetVoxelNameAtPosition(x,y,z) == "Stone")
                    {
                        if (chunk.GetVoxelNameAtPosition(x, y + 1, z) == "NULL" && chunk.GetVoxelNameAtPosition(x, y + 2, z) == "NULL")
                        {
                            //we add the chunk's position to the voxel's position in the chunk, plus the offset from the corner of the block to the center
                            Vector3 actualPosition = chunk.gameObject.transform.position + new Vector3(x, y + 1, z) + new Vector3 (.5f, .5f, .5f);
                            m_waypoints.Add(actualPosition);
                        }
                    }
                }

            }
        }
    }

    void GoToBeginningOfPath()
    {
        transform.position = m_waypoints[0];
    }

    void TraversePath()
    {
        if (m_currentWaypoint < m_waypoints.Count - 1)
        {
            if (((Time.time - m_startTime) * m_speed) / Vector3.Distance(transform.position, m_waypoints[m_currentWaypoint + 1]) < 1)
            {
                transform.position = Vector3.Lerp(transform.position, m_waypoints[m_currentWaypoint + 1], ((Time.time - m_startTime) * m_speed) / Vector3.Distance(transform.position, m_waypoints[m_currentWaypoint + 1]));
                Debug.Log(((Time.time - m_startTime) * m_speed)/Vector3.Distance(transform.position, m_waypoints[m_currentWaypoint + 1]));
            }
            else
            {
                m_currentWaypoint++;
                m_startTime = Time.time;
                Debug.Log(m_currentWaypoint);
            }
        }
        else
        {
            transform.position = m_waypoints[m_waypoints.Count - 1];
            m_isTraversing = false;
        }
    }
}
