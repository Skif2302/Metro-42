using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MetroShadows : MonoBehaviour
{
	public bool recount = false;

	[Range(0f, 3f)]
	public	float			width			=	2f;

	Mesh	mesh_;
	int[]	changes_;

	List<Vector3>	positions_		=	new List<Vector3>();
	List<Vector3>	normals_		=	new List<Vector3>();
	List<Vector2>	uvs_			=	new List<Vector2>();
	List<int>		triangles_		=	new List<int>();

	//	
	Mesh mesh
	{
		get 
		{ 
			if (mesh_ == null)
			{
				var meshFilter	=	GetComponent<MeshFilter>();
				if (meshFilter.sharedMesh == null)
					meshFilter.sharedMesh = new Mesh();

				mesh_	=	meshFilter.sharedMesh;
			}

			return mesh_;
		}
	}

    // 
    void Update()
    {
		
        if (recount) {
			recount = false;

			positions_.Clear();
			normals_.Clear();
			uvs_.Clear();
			triangles_.Clear();
		}
    }


	//	
}
