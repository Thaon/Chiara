using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//making the enum available to everybody
public enum m_voxelType { empty, grass, stone, dirt, sand };

public class ChunkBuilder : MonoBehaviour {

    public MeshGenerator m_voxelGenerator;
    public int[,,] m_terrainArray;
    public int m_chunkSize = 16;
    public int m_chunkHeight = 255;
    public ChunkWorldBuilder m_world;

    Sprite m_grassSpr;
    Sprite m_dirtSpr;
    Sprite m_sandSpr;
    Sprite m_stoneSpr;


    public GameObject m_player;

    void Start()
    {
        m_grassSpr = Resources.Load<Sprite>("Grass");
        m_dirtSpr = Resources.Load<Sprite>("Dirt");
        m_sandSpr = Resources.Load<Sprite>("Sand");
        m_stoneSpr = Resources.Load<Sprite>("Stone");
    }

	// Use this for initialization
	public void GenerateChunk ()
    {
        m_voxelGenerator = GetComponent<MeshGenerator>();
        //we now set the parent
        m_voxelGenerator.m_parent = this;

        m_terrainArray = new int[m_chunkSize, m_chunkHeight, m_chunkSize];

        m_voxelGenerator.WorldInit();
        
        PopulateTerrain();

        //do terrain modifications here
        //CreatePath();

        DisplayTerrain();


        //finish up the model
        m_voxelGenerator.UpdateWorld();
	}

    public void UpdateChunk()
    {
        m_voxelGenerator = GetComponent<MeshGenerator>();
        //we now set the parent
        m_voxelGenerator.m_parent = this;

        //m_terrainArray = new int[m_chunkSize, m_chunkHeight, m_chunkSize];

        m_voxelGenerator.ClearPreviousData();

        //do terrain modifications here
        //CreatePath();

        DisplayTerrain();


        //finish up the model
        m_voxelGenerator.UpdateWorld();
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
                for (int z = 0; z < m_terrainArray.GetLength(2); z++)
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

    public void DisplayTerrain()
    {
        // iterate horizontally on width
        for (int x = 0; x < m_terrainArray.GetLength(0); x++)
        {
            // iterate vertically
            for (int y = 0; y < m_terrainArray.GetLength(1); y++)
            {
                // iterate per voxel horizontally on depth
                for (int z = 0; z < m_terrainArray.GetLength(2); z++)
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
                        if (z == m_terrainArray.GetLength(2) - 1 ||
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

    void CreatePath()
    {
        m_terrainArray[0, 3, 1] = (int)m_voxelType.stone;
        m_terrainArray[0, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[0, 3, 3] = (int)m_voxelType.stone;
        m_terrainArray[1, 3, 3] = (int)m_voxelType.stone;
        m_terrainArray[1, 3, 4] = (int)m_voxelType.stone;
        m_terrainArray[2, 3, 4] = (int)m_voxelType.stone;
        m_terrainArray[3, 3, 4] = (int)m_voxelType.stone;
        m_terrainArray[4, 3, 4] = (int)m_voxelType.stone;
        m_terrainArray[5, 3, 4] = (int)m_voxelType.stone;
        m_terrainArray[5, 3, 3] = (int)m_voxelType.stone;
        m_terrainArray[5, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[6, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[7, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[8, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[9, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[10, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[11, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[12, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[13, 3, 2] = (int)m_voxelType.stone;
        m_terrainArray[13, 3, 3] = (int)m_voxelType.stone;
        m_terrainArray[14, 3, 3] = (int)m_voxelType.stone;
        m_terrainArray[15, 3, 3] = (int)m_voxelType.stone;
    }

    public int GetWorldXZ()
    {
        return m_chunkSize;
    }

    public int GetWorldY()
    {
        return m_chunkHeight;
    }

    public string GetVoxelNameAtPosition(int x, int y, int z)
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
            return tex;
        }
        else return "NULL";
    }

    public void SetBlock(Vector3 index, m_voxelType voxelType)
    {
        if ((index.x >= 0 && index.x < m_terrainArray.GetLength(0)) && (index.y >= 0 && index.y < m_terrainArray.GetLength(1)) && (index.z >= 0 && index.z < m_terrainArray.GetLength(2)))
        {
            //clear the previous mesh data
            m_voxelGenerator.ClearPreviousData();
            // Change the block to the required type
            m_terrainArray[(int)index.x, (int)index.y, (int)index.z] = (int)voxelType;
            // Create the new mesh
            DisplayTerrain();
            // Update the mesh data
            m_voxelGenerator.UpdateWorld();
            m_world.BlockChanged(voxelType);
        }
    }

    public void SpawnSmallVoxel(Vector3 index, m_voxelType voxelType)
    {
        GameObject voxel = new GameObject();
        voxel.AddComponent<MeshGenerator>();
        voxel.AddComponent<Rigidbody>();
        voxel.GetComponent<MeshCollider>().convex = true;
        voxel.AddComponent<SmallVoxel>();

        voxel.transform.position = index;
        voxel.transform.Translate(0.25f, 0, 0.25f); //center the voxel

        voxel.GetComponent<MeshGenerator>().WorldInit();
        //print(voxel.transform.position);

        //get texture
        string tex;
        Sprite spr;
        switch (voxelType)
        {
            case m_voxelType.grass:
            tex = "Grass";
            spr = m_grassSpr;
            break;
            case m_voxelType.stone:
            tex = "Stone";
            spr = m_stoneSpr;
            break;
            case m_voxelType.dirt:
            tex = "Dirt";
            spr = m_dirtSpr;
            break;
            case m_voxelType.sand:
            tex = "Sand";
            spr = m_sandSpr;
            break;
            default:
            tex = "Grass";
            spr = m_grassSpr;
            break;
        }
        //print(voxelType);
        voxel.GetComponent<MeshGenerator>().m_voxelScale = 0.5f;
        //x
        voxel.GetComponent<MeshGenerator>().CreateNegativeXFace(0,0,0, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
        voxel.GetComponent<MeshGenerator>().CreatePositiveXFace(0, 0, 0, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
        //y
        voxel.GetComponent<MeshGenerator>().CreateNegativeYFace(0, 0, 0, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
        voxel.GetComponent<MeshGenerator>().CreatePositiveYFace(0, 0, 0, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
        //z
        voxel.GetComponent<MeshGenerator>().CreateNegativeZFace(0, 0, 0, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));
        voxel.GetComponent<MeshGenerator>().CreatePositiveZFace(0, 0, 0, voxel.GetComponent<MeshGenerator>().GetAssociatedVector(tex));

        voxel.GetComponent<MeshGenerator>().UpdateWorld();
        //print(voxel.transform.position);

        voxel.GetComponent<SmallVoxel>().m_sprite = spr;
        voxel.GetComponent<SmallVoxel>().m_name = tex;

        voxel = null; //lose reference to the voxel
    }
}
