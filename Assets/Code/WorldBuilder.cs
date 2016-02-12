using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshGenerator))]

public class WorldBuilder : MonoBehaviour {

    MeshGenerator m_generator;
    string[] m_worldGrid;

	// Use this for initialization
	void Start ()
    {
        m_generator = GetComponent<MeshGenerator>();

        m_worldGrid = new string[2];
        m_worldGrid[0] =
            @"
            00003
            00011
            00010
            31110
            00000";
        m_worldGrid[1] =
            @"
            00000
            00000
            00000
            b0000
            00000";

        m_generator.WorldInit();
        Debug.Log("World initialized");
        PopulateWorld();
        Debug.Log("World populated");
        m_generator.UpdateWorld();
        Debug.Log("World updated");
    }

    void PopulateWorld ()
    {
        for(int z = 0; z < 2; z++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    char tile = m_worldGrid[z][x+y];
                    if (tile != '0')
                    {
                        switch (tile)
                        {
                            //special cases here
                            case 'b':
                                //create the lerping gameobject
                            break;

                            //now for the voxels
                            case '1':
                                m_generator.CreateVoxel(x, y, z, "Grass");
                                Debug.Log("Grass created");
                            break;

                            case '3':
                                m_generator.CreateVoxel(x, y, z, "Dirt");
                                Debug.Log("Dirt created");
                                break;
                        }
                    }
                }
            }
        }
	}
}
