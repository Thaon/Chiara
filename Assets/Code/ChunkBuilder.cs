using UnityEngine;
using System.Collections;

public class ChunkBuilder : MonoBehaviour {

    enum m_voxelType { empty, grass, stone, dirt, sand};

    MeshGenerator m_voxelGenerator;
    int[,,] m_terrainArray;
    int m_chunkSize = 16;

	// Use this for initialization
	void Start ()
    {
        m_voxelGenerator = GetComponent<MeshGenerator>();
        m_terrainArray = new int[m_chunkSize, m_chunkSize, m_chunkSize];

        m_voxelGenerator.WorldInit();
        PopulateTerrain();
        DisplayTerrain();
        m_voxelGenerator.UpdateWorld();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void PopulateTerrain()
    {
        // iterate horizontally on width
        for (int x = 0; x < m_terrainArray.GetLength(0); x++)
        {
            // iterate vertically
            for (int y = 0; y < m_terrainArray.GetLength(1); y++)
            {
                // iterate per voxel horizontally on depth
                for (int z = 0; z < m_terrainArray.GetLength(2);
                z++)
                {
                    // if we are operating on 4th layer
                    if (y == 3)
                    {
                        m_terrainArray[x, y, z] = (int)m_voxelType.grass;
                    }
                    //else if the the layer is below the fourth
                    else if (y < 3)
                    {
                        m_terrainArray[x, y, z] = (int)m_voxelType.dirt;
                    }
                }
            }
        }

    }

    void DisplayTerrain()
    {
        // iterate horizontally on width
        for (int x = 0; x < m_terrainArray.GetLength(0); x++)
        {
            // iterate vertically
            for (int y = 0; y < m_terrainArray.GetLength(1); y++)
            {
                // iterate per voxel horizontally on depth
                for (int z = 0; z < m_terrainArray.GetLength(2);
                z++)
                {
                    // if this voxel is not empty
                    if (m_terrainArray[x, y, z] != (int)m_voxelType.empty)
                    {
                        string tex;
                        // set texture name by value
                        switch (m_terrainArray[x, y, z])
                        {
                            case (int)m_voxelType.grass:
                                tex = "Grass";
                                break;
                            case (int)m_voxelType.stone:
                                tex = "Stone";
                                break;
                            case (int)m_voxelType.dirt:
                                tex = "Dirt";
                                break;
                            case (int)m_voxelType.sand:
                                tex = "Sand";
                                break;
                            default:
                                tex = "Grass";
                                break;
                        }
                        //optimization stuff here below

                        // check if we need to draw the negative x face
                        if (x == 0 || m_terrainArray[x - 1, y, z] == 0)
                        {
                            m_voxelGenerator.CreateNegativeXFace(x, y, z, m_voxelGenerator.GetAssociatedVector(tex));
                        }
                        // check if we need to draw the positive x face
                        if (x == m_terrainArray.GetLength(0) - 1 ||
                        m_terrainArray[x + 1, y, z] == 0)
                        {
                            m_voxelGenerator.CreatePositiveXFace(x, y, z, m_voxelGenerator.GetAssociatedVector(tex));
                        }

                        //we do the same for the Y axis
                        //negative
                        if (y == 0 || m_terrainArray[x, y - 1, z] == 0)
                        {
                            m_voxelGenerator.CreateNegativeYFace(x, y, z, m_voxelGenerator.GetAssociatedVector(tex));
                        }
                        //positive
                        if (y == m_terrainArray.GetLength(1) - 1 ||
                        m_terrainArray[x, y + 1, z] == 0)
                        {
                            m_voxelGenerator.CreatePositiveYFace(x, y, z, m_voxelGenerator.GetAssociatedVector(tex));
                        }

                        //and finally for the Z axis
                        //negative
                        if (z == 0 || m_terrainArray[x, y, z - 1] == 0)
                        {
                            m_voxelGenerator.CreateNegativeZFace(x, y, z, m_voxelGenerator.GetAssociatedVector(tex));
                        }
                        //positive
                        if (z == m_terrainArray.GetLength(1) - 1 ||
                        m_terrainArray[x, y, z + 1] == 0)
                        {
                            m_voxelGenerator.CreatePositiveZFace(x, y, z, m_voxelGenerator.GetAssociatedVector(tex));
                        }

                        //m_voxelGenerator.CreateVoxel(x, y, z, tex);
                        //print("Created " + tex + " block,");
                    }
                }
            }
        }
    }
}
