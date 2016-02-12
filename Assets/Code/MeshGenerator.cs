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

    //mesh collision
    MeshCollider m_collider;

	//UV mapping
	float m_tileSize; //used to specify how big each texture-slice is
	public int m_tileXOffset; //how far it is in the tileset (multiplied by the tile size it gives us the tile we are looking for)
	public int m_tileYOffset; //same as above

    [SerializeField]
	List<Vector2> m_UVList;

	#endregion


	// Use this for initialization
	void Start ()
	{
		m_vertexList = new List<Vector3>();
		m_triIndexList = new List<int>();
		m_UVList = new List<Vector2>();

		m_mesh = GetComponent<MeshFilter>().mesh;
        m_collider = GetComponent<MeshCollider>();

        m_tileSize = 0.5f;
		
        //we now create the voxel
		CreateVoxel(0,0,0,0,0);

		m_mesh.vertices = m_vertexList.ToArray();
		m_mesh.triangles = m_triIndexList.ToArray();

		m_mesh.uv = m_UVList.ToArray();

		m_mesh.RecalculateNormals();
        m_collider.sharedMesh = null;
        m_collider.sharedMesh = m_mesh;
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

	void CreateVoxel(int x, int y, int z, float xOffset, float yOffset)
	{

        CreateNegativeXFace(x, y, z, xOffset, yOffset);
        CreatePositiveXFace(x, y, z, xOffset, yOffset);
        CreateNegativeYFace(x, y, z, xOffset, yOffset);
        CreatePositiveYFace(x, y, z, xOffset, yOffset);
        CreateNegativeZFace(x, y, z, xOffset, yOffset);
        CreatePositiveZFace(x, y, z, xOffset, yOffset);
	}

    void CreateNegativeZFace(int x, int y, int z, float xOffset, float yOffset)
    {
        //add vertices to the vertexList
        m_vertexList.Add(new Vector3(x, y + 1, z));
        m_vertexList.Add(new Vector3(x + 1, y + 1, z));
        m_vertexList.Add(new Vector3(x + 1, y, z));
        m_vertexList.Add(new Vector3(x, y, z));

        //we now add the triangles to the indices list
        AddTriangleIndices();

        //we now take care of the UV
        SetUVCoords(xOffset, yOffset);
    }

    void CreatePositiveZFace(int x, int y, int z, float xOffset, float yOffset)
    {
        m_vertexList.Add(new Vector3(x + 1, y, z + 1));
        m_vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        m_vertexList.Add(new Vector3(x, y + 1, z + 1));
        m_vertexList.Add(new Vector3(x, y, z + 1));
        AddTriangleIndices();
        SetUVCoords(xOffset, yOffset);
    }    void CreateNegativeXFace(int x, int y, int z, float xOffset, float yOffset)
    {
        m_vertexList.Add(new Vector3(x, y, z + 1));
        m_vertexList.Add(new Vector3(x, y + 1, z + 1));
        m_vertexList.Add(new Vector3(x, y + 1, z));
        m_vertexList.Add(new Vector3(x, y, z));
        AddTriangleIndices();
        SetUVCoords(xOffset, yOffset);
    }    void CreatePositiveXFace(int x, int y, int z, float xOffset, float yOffset)
    {
        m_vertexList.Add(new Vector3(x + 1, y, z));
        m_vertexList.Add(new Vector3(x + 1, y + 1, z));
        m_vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        m_vertexList.Add(new Vector3(x + 1, y, z + 1));
        AddTriangleIndices();
        SetUVCoords(xOffset, yOffset);
    }    void CreateNegativeYFace(int x, int y, int z, float xOffset, float yOffset)
    {
        m_vertexList.Add(new Vector3(x, y, z));
        m_vertexList.Add(new Vector3(x + 1, y, z));
        m_vertexList.Add(new Vector3(x + 1, y, z + 1));
        m_vertexList.Add(new Vector3(x, y, z + 1));
        AddTriangleIndices();
        SetUVCoords(xOffset, yOffset);
    }    void CreatePositiveYFace(int x, int y, int z, float xOffset, float yOffset)
    {
        m_vertexList.Add(new Vector3(x, y + 1, z));
        m_vertexList.Add(new Vector3(x, y + 1, z + 1));
        m_vertexList.Add(new Vector3(x + 1, y + 1, z + 1));
        m_vertexList.Add(new Vector3(x + 1, y + 1, z));
        AddTriangleIndices();
        SetUVCoords(xOffset, yOffset);
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

    void SetUVCoords(float xOffest, float yOffset)
	{
        //Debug.Log(m_tileSize + ": " + xOffest + ", " + yOffset);
        //Debug.Log((m_tileSize * m_tileXOffset) + m_tileSize);
		m_UVList.Add(new Vector2((m_tileSize * xOffest), (m_tileSize * yOffset) + m_tileSize));
        m_UVList.Add(new Vector2((m_tileSize * xOffest) + m_tileSize, (m_tileSize * yOffset) + m_tileSize));
        m_UVList.Add(new Vector2((m_tileSize * xOffest), (m_tileSize * yOffset)));
        m_UVList.Add(new Vector2((m_tileSize * xOffest) + m_tileSize, (m_tileSize * yOffset)));
    }

}
