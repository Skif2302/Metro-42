using UnityEngine;
using UnityEditor;

[AddComponentMenu("Метро 2042/Станция")]
[ExecuteInEditMode]
public class MetroStation : MonoBehaviour
{
	static public MetroStation rollOver;

	public string	description;

	public string	imageURL;
    public string	videoURL;

	[Range(0f, 3f)]
	public float	curveRange	=	0;

    //public Vector3	direction;

	public float	distance;

	Vector3 position_	=	Vector3.zero;

	Color color_;

	public Vector3 position
	{
		get { return transform.position; }
		set { transform.position = value; }
	}

	public Color GetColor ()
    {
		return color_;
	}

	public void SetColor (Color _color)
    {
		color_	=	_color;

		if (Application.isPlaying)
		{
			var	meshRenderer	=	GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				var material		=	Application.isPlaying ? meshRenderer.material : meshRenderer.sharedMaterial;

				if (material != null)
					material.SetColor("_Color", _color);
			}
		}
	}


	void Update ()
	{
		if (transform.position.y != transform.parent.position.y)
		{
			var pos		=	transform.position;
				pos.y	=	transform.parent.position.y;

			transform.position	=	pos;
		}
		
		if (!position_.Equals(position)) {
			position_	=	position;

			var line	=	transform.parent.GetComponent<MetroLine>();
			if (line != null)
				line.recount	=	true;
		}
	}

	void OnMouseDown ()
	{
		MetroPathFinder.StationClick(this);

		MetroWindow._.SetItem(this);
	}

	//	
	void OnMouseEnter()
	{
		rollOver	=	this;
	}

	//	
	void OnMouseExit()
	{
		if (rollOver == this)
			rollOver	=	null;
	}

	//	end
}
