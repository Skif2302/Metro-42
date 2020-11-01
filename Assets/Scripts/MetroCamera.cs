using UnityEngine;

[ExecuteInEditMode]
public class MetroCamera : MonoBehaviour
{
	public static MetroCamera _;

	public static void SetTarget (Transform _target)
	{
		if (_ != null)
			_.target	=	_target;
	}

	//
	public static float GetEasingK (ref float _time, float _k=0.1f, float _timeStep=0.001f)
	{
		var	time		=	_time + Time.deltaTime;
		var	timeNum		=	time / _timeStep;

		_time			=	time - Mathf.Floor(timeNum) * _timeStep;

		var	s			=	0f;
		var	m			=	_k;
		var	n			=	0;
		
		while (timeNum > 1) {
			s			+=	m;
			m			*=	_k;
			timeNum--;
			n++;
		}

		return s;
	}

	/// 
	public static Vector3 GetPointOnPlane (Vector3 _screenPosition) 
	{
		var	ray 			= 	Camera.main.ScreenPointToRay(_screenPosition);
		var plane			=	new Plane(Vector3.up, Vector3.zero);
		var	hitPoint		=	Vector3.zero;
		var	hitDist			=	0f;
		
		if (plane.Raycast(ray, out hitDist))
			hitPoint	= 	ray.GetPoint(hitDist);

		return new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
	}

	/// 
	public static Vector3 GetMousePoint () 
	{
		var plane			=	new Plane(Vector3.up, Vector3.zero);
		var	ray 			= 	Camera.main.ScreenPointToRay(Input.mousePosition);

		float distance		=	0;

		if (plane.Raycast(ray, out distance))
			return ray.GetPoint(distance);

		return Vector3.zero;
	}

	//
	public static Vector2 GetPointOnScreen (Vector3 _worldPosition)
	{
		return Camera.main.WorldToScreenPoint(_worldPosition);
	}


	//-----------------------------------------------------------

	public		Transform	target;
	public		Vector3		position;

	public      float		horisontal          =	-60;

	[Range(20f, 80f)]
	public      float       vertical            =   40;
	
	public      float		zoom;
	public      float		zoomMin				=	20;
	public      float		zoomMax				=	300;
	
	Vector3		speeds_				=	new Vector3 (90f, 30f, .9f);
	
	Transform	target_;
	Vector3		position_;


	float		verticalMin_			=	15;
	float		verticalMax_			=	80;
	float		horisontal_, vertical_;

	float		rotationTime_;

	float		
		zoom_;

	Vector3		positionDelta_;
	float		positionDeltaTime_;

	bool		buttonUp_		=	false;
	bool		buttonDn_		=	false;
	bool		buttonLt_		=	false;
	bool		buttonRt_		=	false;
	bool		buttonZoomIn_	=	false;
	bool		buttonZoomOut_	=	false;
	bool		buttonDrag_		=	false;

	Vector3		positionDragStart_;
	Vector3		positionDragDelta_;
	Vector3		positionTo_;

	// 
	public void ButtonUp (bool _enable)
	{
		buttonUp_		=	_enable;
	}

	// 
	public void ButtonDn (bool _enable)
	{
		buttonDn_		=	_enable;
	}
	
	// 
	public void ButtonLt (bool _enable)
	{
		buttonLt_		=	_enable;
	}

	// 
	public void ButtonRt (bool _enable)
	{
		buttonRt_		=	_enable;
	}

	// 
	public void ButtonZoomIn (bool _enable)
	{
		buttonZoomIn_	=	_enable;
	}

	// 
	public void ButtonZoomOut (bool _enable)
	{
		buttonZoomOut_	=	_enable;
	}

	// 
	public void ButtonDragPress ()
	{
		var pos			=	GetMousePoint();

		//Debug.Log("ButtonDragPress: TRUE / " + pos);

		buttonDrag_			=	true;
		positionDragStart_	=	pos;
	}

	//	------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	//	
    void OnEnable()
    {
        _ = this;
    }

	//	
    void OnDisable()
    {
        _ = null;
    }

	// 
	void Start ()
	{
		target_			=	target;
		horisontal_		=	horisontal;

		vertical		=	Mathf.Clamp(vertical, verticalMin_, verticalMax_);
		vertical_		=	vertical; 

		zoom			=	Mathf.Clamp(zoom, zoomMin, zoomMax);
		zoom_			=	zoom;

		positionTo_		=	position;
	}

	// 
	void LateUpdate ()
	{
		if (target != null)
			position		=   target.position;

		if (Application.isPlaying)
		{
			if (buttonDrag_)
			{
				if (Input.GetMouseButtonUp(0)) 
					buttonDrag_	=	false;
				
				if (buttonDrag_)
					positionTo_	=	position + (positionDragStart_ - GetMousePoint());
			}

			var deltaTo	=	positionTo_	-	position;

			if (deltaTo.magnitude > 0.1f)
				position	+=	deltaTo * 0.1f;


			if (buttonUp_ != buttonDn_)
			{
				vertical	+=	(Time.deltaTime * (buttonUp_ ? -1f : 1f)) * speeds_.y;
				vertical	=	Mathf.Clamp(vertical, verticalMin_, verticalMax_);
			}

			if (buttonLt_ != buttonRt_)
			{
				horisontal	+=	(Time.deltaTime * (buttonLt_ ? 1f : -1f)) * speeds_.x;
			}

			if (buttonZoomIn_ != buttonZoomOut_)
			{
				zoom		+=	zoom * (Time.deltaTime * (buttonZoomOut_ ? 1f : -1f)) * speeds_.z;
				zoom		=	Mathf.Clamp(zoom, zoomMin, zoomMax);
			}

			positionDelta_	+=	(Vector3.zero - positionDelta_) * GetEasingK(ref positionDeltaTime_, 0.1f, 0.001f);

			var k	=	0.3f;
		
			horisontal_		+=	(horisontal - horisontal_) * k;
			vertical_		+=	(vertical - vertical_) * k;

			zoom			=	Mathf.Clamp(zoom, zoomMin, zoomMax);
			zoom_			+=	(zoom - zoom_) * k;
		}
		else
		{
			positionDelta_	=	Vector3.zero;
			horisontal_		=	horisontal;
			vertical_		=	vertical;

			zoom			=	Mathf.Clamp(zoom, zoomMin, zoomMax);
			zoom_			=	zoom;
		}

		transform.rotation	=	Quaternion.Euler(vertical_, horisontal_, 0);
		transform.position	=	transform.rotation * new Vector3(0.0f, 0.0f, -zoom_) + position + positionDelta_;
    }
	
	//	end
}

