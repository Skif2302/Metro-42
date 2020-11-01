using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class MetroLine : MonoBehaviour
{
	[Range(0f, 2f)]
	public	static float	top			=	1f;

	[Range(0f, 2f)]
	public	static float	bottom		=	0.10f;

	[Range(0f, 1f)]
	public	static float	height		=	0.3f;

	//	
	public bool	noActive	=	false;


	public bool	close		=	false;

	[Serializable]
    public class Vertex 
	{
        public Vector2	point;
        public Vector2	normal;
        public float	uCoord;

        public Vertex (Vector2 _point, Vector2 _normal, float _uCoord) 
		{
            point		= _point;
            normal		= _normal;
            uCoord		= _uCoord;
        }
        public Vertex (Vertex _other) 
		{
            point	= _other.point;
            normal	= _other.normal;
            uCoord	= _other.uCoord;
        }
    }

	public class Node 
	{
		public Vector3		position;
		public Quaternion	rotation;

		public Node ()
		{

		}

		public Node (Vector3 _position, Quaternion _rotation)
		{
			position	=	_position;
			rotation	=	_rotation;
		}
	}

	public int GetStationIndex (MetroStation _station)
	{
		for (var i=0; i<stations_.Count; i++)
			if (stations_[i] == _station)
				return i;

		return -1;
	}

	public	Color	color		=	Color.gray;

	//[CanEditMultipleObjects]
	public	bool	recount		=	false;

	Vertex[]		shape_		=	new Vertex[4];
	Mesh			mesh_;

	float	top_			=	-1;
	float	bottom_			=	-1;
	float	height_			=	-1;

	Color	color_		=	Color.gray;

	List<MetroStation>	stations_	=	new List<MetroStation>();

	float	length_			=	-1;

	List<Vector3>	points1_	=	new List<Vector3>();
	List<Vector3>	points2_	=	new List<Vector3>();
		
	Material		material_;
	MeshRenderer	meshRenderer_;

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

	MeshRenderer meshRenderer
	{
		get 
		{ 
			if (meshRenderer_ == null)
				meshRenderer_	=	GetComponent<MeshRenderer>();

			return meshRenderer_;
		}
	}

	public float length
	{
		get {return length_; }
	}

    //
    void Start()
    {
        recount	=	true;
    }

	public Color GetColor ()
    {
		return color_;
	}

	public void SetColor (Color _color)
    {
		color				=	_color;

		if (Application.isPlaying)
		{
			material_		=	Application.isPlaying ? meshRenderer.material : meshRenderer.sharedMaterial;

			if (material_ != null)
			{
				_color.a		=	noActive ? 0.1f : 1f;

				material_.SetColor("_Color", _color);
			}

			if (stations_.Count != transform.childCount)
				recount	=	true;
		}
	}


    //	
    void Update()
    {
		if (Application.isPlaying)
		{
			material_		=	Application.isPlaying ? meshRenderer.material : meshRenderer.sharedMaterial;

			if (material_ != null)
			{
				material_.SetColor("_Color", color);
			}

			if (stations_.Count != transform.childCount)
				recount	=	true;
		}


		if (recount || top_ != top || bottom_ != bottom || height_ != height || stations_.Count != transform.childCount || !color_.Equals(color))
		{
			recount			=	false;	
			top_			=	top;
			bottom_			=	bottom;
			height_			=	height;

			color_			=	color;

			stations_.Clear();

			if (transform.childCount > 0)
			{
				var	child			=	transform.GetChild(0).GetComponent<MetroStation>();
					child.distance	=	0;
					child.SetColor(color);

				stations_.Add(child);

				length_			=	0f;

				for (var i=1; i<transform.childCount; i++)
				{
					length_			+=	(transform.GetChild(i).position - transform.GetChild(i-1).position).magnitude;

					child			=	transform.GetChild(i).GetComponent<MetroStation>();
					child.distance	=	length_;
					child.SetColor(color);

					stations_.Add(transform.GetChild(i).GetComponent<MetroStation>());
				}

				shape_[0]	=	new Vertex(new Vector2(top_/2, height),		Vector2.up,		0);
				shape_[1]	=	new Vertex(new Vector2(bottom_/2, 0),		Vector2.right,	1);
				shape_[2]	=	new Vertex(new Vector2(-bottom_/2, 0),		Vector2.left,	0);
				shape_[3]	=	new Vertex(new Vector2(-top_/2, height),	Vector2.up,		1);


				DrawMesh();
			}
			
		}

    }

	public Vector3 GetPosition (float _distance)
    {
		if (_distance <= 0)
			return stations_[0].position;

		if (_distance >= length_)
			return stations_[stations_.Count-1].position;

		for (var i=0; i<stations_.Count; i++)
		{
			if (_distance == stations_[i].distance)
				return stations_[i].position;

			if (_distance < stations_[i+1].distance)
			{
				var max		=	stations_[i+1].distance - stations_[i].distance;
				var cur		=	_distance - stations_[i].distance;

				return Vector3.Lerp(stations_[i].position, stations_[i+1].position, cur / max);
			}
		}

		return stations_[stations_.Count-1].position;
	}

	public Quaternion GetRotation (float _distance)
    {
		var posFrom		=	GetPosition(_distance - 0.01f);
		var posTo		=	GetPosition(_distance + 0.01f);

		return Quaternion.LookRotation(posTo - posFrom);
	}

	void OnDrawGizmos()
    {
		if (stations_.Count < 2)
			return;

		var	segmentsCount	=	1000f;
		var	positionOld		=	GetPosition(0);

		for (var i=1; i<=segmentsCount; i++) 
		{
			var	k			=	(float)i / segmentsCount;
			var	pos			=	k * length_;

			var	position	=	GetPosition(pos);

			Gizmos.color	=	new Color(1, 1, 1, k);
			Gizmos.DrawLine(positionOld, position);

			positionOld		=	position;
        }

		Gizmos.color	=	new Color(1, 1, 0, 0.1f);

		DrawGizmosLine(points1_);
		DrawGizmosLine(points2_);

		for (var i=0; i<points1_.Count; i++)
			Gizmos.DrawLine(points1_[i], points2_[i]);
		
	}

	void OnDrawGizmosSelected()
    {

	}

	void DrawGizmosLine (List<Vector3> _points) 
	{
		var	positionOld		=	_points[0];

		for (var i=1; i<_points.Count; i++) 
		{
			Gizmos.DrawLine(positionOld, _points[i]);

			positionOld		=	_points[i];
        }
	}


	void DrawMesh () 
	{
		var	segmentsCount	=	1000f;

		var	count			=	2 * 2 * segmentsCount * 3;

		var positions		=	new List<Vector3>();
		var normals			=	new List<Vector3>();
		var uvs				=	new List<Vector2>();
		var triangles		=	new List<int>();
		
		var uvPos			=	1;
		var indexes			=	0;

		var	pointLeft		=	new Vector3(-bottom/2f, 0, 0);
		var	pointRight		=	new Vector3(bottom/2f, 0,  0);

		points1_.Clear();
		points2_.Clear();

        for (var i=0; i<=segmentsCount; i++) 
		{
			var	k			=	(float)i / segmentsCount;
			var	distance	=	k * length_;

			var pos			=	GetPosition(distance);
			var rot			=	GetRotation(distance);

			var point2		=	pos + (rot * pointLeft);
			var point1		=	pos + (rot * pointRight);

			points1_.Add(point1);
			points2_.Add(point2);

			positions.Add(point1);
			positions.Add(point2);

			normals.Add(Vector3.up);
			normals.Add(Vector3.up);

			uvs.Add(new Vector2(uvPos, 0));
			uvs.Add(new Vector2(uvPos, 1));

			uvPos = uvPos == 0 ? 1 : 0;
			
			//	3, 2, 0,
			//	0, 1, 3

			//	6 7
			//	4 5
			//	2 3
			//	0 1

			if (i > 0)
			{
				//var p	=	i * 2 + 1;	//	3
				var p	=	indexes * 2 + 1;	//	3

				triangles.Add(p-0);		//	3
				triangles.Add(p-1);		//	2
				triangles.Add(p-2);		//	1

				
				triangles.Add(p-2);		//	1
				triangles.Add(p-1);		//	2
				triangles.Add(p-3);		//	0
			}
			
			i++;
			indexes++;
        }


		mesh.Clear();

		mesh.vertices		=	positions.ToArray();
		mesh.normals		=	normals.ToArray();
		mesh.uv				=	uvs.ToArray();

		mesh.triangles		=	triangles.ToArray();

		mesh.RecalculateBounds();
		mesh.RecalculateTangents();
    }

	//	end
}
