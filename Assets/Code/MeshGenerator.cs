using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MeshGenerator : MonoBehaviour {

	#region Member Variables

	//mesh generation
	Mesh m_mesh;
	List<Vector3> m_vertexList;
	List<int> m_triIndexList;

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

        m_tileSize = 0.5f;
		
        //we now create the voxel
		CreateVoxel(0,0,0,0,0);

		m_mesh.vertices = m_vertexList.ToArray();
		m_mesh.triangles = m_triIndexList.ToArray();

		m_mesh.uv = m_UVList.ToArray();

		m_mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

	void CreateVoxel(int x, int y, int z, float xOffset, float yOffset)
	{
		//add vertices to the vertexList
		//starting with frontal face
		m_vertexList.Add(new Vector3 (x, y+1, z));
		m_vertexList.Add(new Vector3 (1, y+1, z));
		m_vertexList.Add(new Vector3 (1, 0, z));
		m_vertexList.Add(new Vector3 (0, 0, z));


		//add indices to the triIndexList
		//again, starting with front face
		m_triIndexList.Add(0);
		m_triIndexList.Add(1);
		m_triIndexList.Add(3);
		m_triIndexList.Add(1);
		m_triIndexList.Add(2);
		m_triIndexList.Add(3);

        //we now take care of the UV
        SetUVCoords(xOffset, yOffset);
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
