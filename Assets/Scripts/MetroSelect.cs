using UnityEngine;
using UnityEngine.UI;

public class MetroSelect : MonoBehaviour
{
	public static MetroSelect _;

	public	MetroStation		target;

	public	bool				selected;

	public	RectTransform		rectTrnsform;
	public	GameObject			view;
	public	Image				image;
	public	Text				textfield;

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
    void LateUpdate()
    {
		if (MetroStation.rollOver != null)
		{
			target		=	MetroStation.rollOver;
			selected	=	false;
		}
		else
		{
			target		=	null;
		}

		view.SetActive(target != null);

		if (target != null)
		{
			textfield.text			=	target.name;

			rectTrnsform.position	=	Camera.main.WorldToScreenPoint(target.position);
			image.color				=	selected  ? new Color(1, 1, 0.3f) : Color.white;
		}

		/*
		if (MdcUiWindow._ != null && MdcUiWindow._.item != null)
		{
			target		=	MdcUiWindow._.item;
			selected	=	true;
		}
		else 
		else
		{
			target		=	null;
		}
		
		view.SetActive(target != null);


        if (target != null)
		{
			textfield.text			=	target.name;

			rectTrnsform.position	=	Camera.main.WorldToScreenPoint(target.markPoint.position);
			image.color				=	selected  ? new Color(1, 1, 0.3f) : Color.white;
		}
			
		*/
    }

	//	end
}
