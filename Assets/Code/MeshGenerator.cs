using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class MeshGenerator : MonoBehaviour {

	#region Member Variables

	//mesh generation
	Mesh m_mesh;
	List<Vector3> m_vertexList;
	List<int> m_triIndexList;
    int m_quadNumber = 0;
    public float m_voxelScale = 1;

    //mesh collision
    MeshCollider m_collider;

	//UV mapping
	float m_tileSize; //used to specify how big each texture-slice is
    Vector2 m_UVOffset;
    Dictionary<string, Vector2> m_texture;
                      
    //public int m_tileXOffset; //how far it is in the tileset (multiplied by the tile size it gives us the tile we are looking for)
    //public int m_tileYOffset; //same as above
    //discarded in favour of Vector2

    //[SerializeField]
	List<Vector2> m_UVList;

    public ChunkBuilder m_parent;

	#endregion


	// Use this for initialization
	void Start ()
	{
        //first we initialize the world
        //WorldInit();
        //we now create the voxel
        //CreateVoxel(0,0,0,"Sand");
        //UpdateWorld();
    }

    public void WorldInit()
    {
        m_vertexList = new List<Vector3>();
        m_triIndexList = new List<int>();
        m_UVList = new List<Vector2>();
        m_texture = new Dictionary<string, Vector2>();

        m_mesh = GetComponent<MeshFilter>().mesh;
        m_collider = GetComponent<MeshCollider>();

        //we take care of the textures here
        m_tileSize = 0.5f;
        m_texture.Add("Grass", new Vector2(0, 0));
        m_texture.Add("Stone", new Vector2(1, 0));
        m_texture.Add("Dirt", new Vector2(0, 1));
        m_texture.Add("Sand", new Vector2(1, 1));
    }

	public void CreateVoxel(int x, int y, int z, string texture)
	{
        x *= m_voxelScale;
        y *= m_voxelScale;
        z *= m_voxelScale;
        Vector2 offset = m_texture[texture];
        CreateNegativeXFace(x, y, z, offset);
        CreatePositiveXFace(x, y, z, offset);
        CreateNegativeYFace(x, y, z, offset);
        CreatePositiveYFace(x, y, z, offset);
        CreateNegativeZFace(x, y, z, offset);
        CreatePositiveZFace(x, y, z, offset);
	}

    public void CreateNegativeZFace(int x, int y, int z, Vector2 offset)
    {
        //add vertices to the vertexList
        m_vertexList.Add(new Vector3(x, y + m_voxelScale, z));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y + m_voxelScale, z));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y, z));
        m_vertexList.Add(new Vector3(x, y, z));

        //we now add the triangles to the indices list
        AddTriangleIndices();

        //we now take care of the UV
        SetUVCoords(offset);
    }

    public void CreatePositiveZFace(int x, int y, int z, Vector2 offset)
    {
        m_vertexList.Add(new Vector3(x + m_voxelScale, y, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y + m_voxelScale, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x, y + m_voxelScale, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x, y, z + m_voxelScale));
        AddTriangleIndices();
        SetUVCoords(offset);
    }

    public void CreateNegativeXFace(int x, int y, int z, Vector2 offset)
    {
        m_vertexList.Add(new Vector3(x, y, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x, y + m_voxelScale, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x, y + m_voxelScale, z));
        m_vertexList.Add(new Vector3(x, y, z));
        AddTriangleIndices();
        SetUVCoords(offset);
    }

    public void CreatePositiveXFace(int x, int y, int z, Vector2 offset)
    {
        m_vertexList.Add(new Vector3(x + m_voxelScale, y, z));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y + m_voxelScale, z));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y + m_voxelScale, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y, z + m_voxelScale));
        AddTriangleIndices();
        SetUVCoords(offset);
    }

    public void CreateNegativeYFace(int x, int y, int z, Vector2 offset)
    {
        m_vertexList.Add(new Vector3(x, y, z));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y, z));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x, y, z + m_voxelScale));
        AddTriangleIndices();
        SetUVCoords(offset);
    }

    public void CreatePositiveYFace(int x, int y, int z, Vector2 offset)
    {
        m_vertexList.Add(new Vector3(x, y + m_voxelScale, z));
        m_vertexList.Add(new Vector3(x, y + m_voxelScale, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y + m_voxelScale, z + m_voxelScale));
        m_vertexList.Add(new Vector3(x + m_voxelScale, y + m_voxelScale, z));
        AddTriangleIndices();
        SetUVCoords(offset);
    }

    void AddTriangleIndices()
    {
        //add indices to the triIndexList
        m_triIndexList.Add(m_quadNumber * 4);
        m_triIndexList.Add((m_quadNumber * 4) + 1);
        m_triIndexList.Add((m_quadNumber * 4) + 3);
        m_triIndexList.Add((m_quadNumber * 4) + 1);
        m_triIndexList.Add((m_quadNumber * 4) + 2);
        m_triIndexList.Add((m_quadNumber * 4) + 3);
        m_quadNumber++;
    }

    void SetUVCoords(Vector2 offset)
	{
        //Debug.Log(m_tileSize + ": " + xOffest + ", " + yOffset);
        //Debug.Log((m_tileSize * m_tileXOffset) + m_tileSize);
		m_UVList.Add(new Vector2((m_tileSize * offset.x), (m_tileSize * offset.y) + m_tileSize));
        m_UVList.Add(new Vector2((m_tileSize * offset.x) + m_tileSize, (m_tileSize * offset.y) + m_tileSize));
        m_UVList.Add(new Vector2((m_tileSize * offset.x) + m_tileSize, (m_tileSize * offset.y)));
        m_UVList.Add(new Vector2((m_tileSize * offset.x), (m_tileSize * offset.y)));
    }

    public void UpdateWorld()
    {
        m_mesh.Clear();
        m_mesh.vertices = m_vertexList.ToArray();
        m_mesh.triangles = m_triIndexList.ToArray();

        m_mesh.uv = m_UVList.ToArray();

        m_mesh.RecalculateNormals();
        m_collider.sharedMesh = null;
        m_collider.sharedMesh = m_mesh;
    }

    public Vector2 GetAssociatedVector(string texture)
    {
        Vector2 associatedV2;
        m_texture.TryGetValue(texture, out associatedV2);
        return associatedV2;
    }

    // Clear previous data structures used to create the mesh
    public void ClearPreviousData()
    {
        m_vertexList.Clear();
        m_triIndexList.Clear();
        m_UVList.Clear();
        m_quadNumber = 0;
    }
}
