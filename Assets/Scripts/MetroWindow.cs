using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class MetroWindow : MonoBehaviour
{
	public static MetroWindow _;
	
	public MetroStation		item;

	public	Text		title;
	public	Text		title2;
	public	Text		description;

	public	GameObject		windowBottom;
	public	GameObject		windowMain;
	public	GameObject		background;

	int status	=	0;

	public void ButtonClose()
	{
		item	=	null;
		status	=	0;

		windowMain.SetActive(false);
		windowBottom.SetActive(false);
		background.SetActive(false);
	}

	public void ButtoOpen ()
	{
		background.SetActive(true);
		windowMain.SetActive(true);
		windowBottom.SetActive(false);

		status	=	1;
	}

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
	void Start()
	{
		//SetHelp();
	}

	//	
	void Update()
    {
		if (_ == null)
			_ = this;

		windowBottom.SetActive(false);
		windowMain.SetActive(false);
		background.SetActive(false);

		if (MetroPathFinder._.stationFrom != null && MetroPathFinder._.stationTo != null)
		{
			title.text			=	MetroPathFinder._.stationFrom.name + " -> " + MetroPathFinder._.stationTo.name;
			title2.text			=	title.text;

			description.text	=	"Время в пути: " + (MetroPathFinder._.stationsNum * 2) + " минуты";

			windowBottom.SetActive(status != 1);
			windowMain.SetActive(status == 1);
			background.SetActive(status == 1);
		}
		else
		if (MetroPathFinder._.stationFrom != null) 
		{
			title.text			=	MetroPathFinder._.stationFrom.name;
			title2.text			=	title.text;

			description.text	=	MetroPathFinder._.stationFrom.description;

			windowBottom.SetActive(status != 1);
			windowMain.SetActive(status == 1);
			background.SetActive(status == 1);
		}

		

    }

	//	
	public void SetItem (MetroStation _station)
	{
		item	=	_station;

		if (item == null) {
			ButtonClose();
			return;
		}

		title.text			=	item.name;
		title2.text			=	item.name;
		description.text	=	item.description;

		gameObject.SetActive(true);

		windowMain.SetActive(false);
		windowBottom.SetActive(true);
		background.SetActive(false);

		//ButtoBackground();
	}

	//	end
}
