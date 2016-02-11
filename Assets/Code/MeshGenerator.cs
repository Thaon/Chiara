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
	int m_tileSize; //used to specify how big each texture-slice is
	int m_tileXOffset; //how far it is in the tileset (multiplied by the tile size it gives us the tile we are looking for)
	int m_tileYOffset; //same as above

	List<Vector2> m_UVList;

	#endregion


	// Use this for initialization
	void Start ()
	{
		m_vertexList = new List<Vector3>();
		m_triIndexList = new List<int>();
		m_UVList = new List<Vector2>();

		m_mesh = GetComponent<MeshFilter>().mesh;

		//we now create the voxel
		CreateVoxel();

		m_mesh.vertices = m_vertexList.ToArray();
		m_mesh.triangles = m_triIndexList.ToArray();

		m_mesh.uv = m_UVList.ToArray();

		m_mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateVoxel()
	{
		//add vertices to the vertexList
		//starting with frontal face
		m_vertexList.Add(new Vector3 (0, 1, 0));
		m_vertexList.Add(new Vector3 (1, 1, 0));
		m_vertexList.Add(new Vector3 (1, 0, 0));
		m_vertexList.Add(new Vector3 (0, 0, 0));


		//add indices to the triIndexList
		//again, starting with front face
		m_triIndexList.Add(0);
		m_triIndexList.Add(1);
		m_triIndexList.Add(3);
		m_triIndexList.Add(1);
		m_triIndexList.Add(2);
		m_triIndexList.Add(3);
		
	}

	void SetUVCoords()
	{
		m_UVList.Add(new Vector2(0, 0.5f));
		m_UVList.Add(new Vector2(0.5f, 0.5f));
		m_UVList.Add(new Vector2(0.5f, 0));
		m_UVList.Add (new Vector2 (0,0));
	}

}
