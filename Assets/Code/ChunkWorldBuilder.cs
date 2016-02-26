using UnityEngine;
using System.Collections;

public class ChunkWorldBuilder : MonoBehaviour {

    // delegate signature
    public delegate void EventBlockChanged(m_voxelType blockType);
    // event instances for EventBlockChanged
    public static event EventBlockChanged OnEventBlockDestroyed;
    public static event EventBlockChanged OnEventBlockPlaced;

    ChunkBuilder m_chunks;
    int[,,] m_worldArray;
    int m_worldXSize = 16;
    int m_worldZSize = 16;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void BlockChanged(m_voxelType type)
    {
        if (type == m_voxelType.empty)
        {
            OnEventBlockDestroyed(m_voxelType.empty);
        }
        else
        {
            OnEventBlockPlaced(type);
        }
    }
}
