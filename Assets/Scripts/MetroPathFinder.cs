using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MetroPathFinder : MonoBehaviour
{
	public static MetroPathFinder _;

	public static void StationClick (MetroStation _station)
    {
		if (_ != null) 
		{
			if (_.stationFrom == _station) {
				_.stationFrom	=	null;
				return;
			} else
			if (_.stationTo == _station) {
				_.stationTo		=	null;
				return;
			} else 
			if (_.stationFrom == null) {
				_.stationFrom	=	_station;
				return;
			}
			else 
			if (_.stationTo == null) {
				_.stationTo		=	_station;
				return;
			} 
			else
			if (_.stationFrom != null && _station != null)
			{
				_.stationTo		=	_station;
				return;
			}
		}
	}

	public MetroStation		stationFrom;
	public MetroStation		stationTo;

	public SpriteRenderer	selectionFrom;
	public SpriteRenderer	selectionTo;

    public MetroLine[]		lines;
	public MetroTransfer[]	transfers;

	public GameObject		linePrefab;
	public GameObject		stationPrefab;

	public int				stationsNum	=	0;


	MetroStation	stationFrom_;
	MetroStation	stationTo_;

	void OnEnable()
    {
		_ = this;
	}

	void OnDisable()
    {
		_ = null;
	}

    void Start()
    {
        
    }

    //	
    void Update()
    {
		if (stationFrom == null && stationTo != null)
		{
			stationFrom	=	stationTo;
			stationTo	=	null;
		}


        if (stationFrom_ != stationFrom || stationTo_ != stationTo)
		{
			stationsNum		=	0;

			stationFrom_	=	stationFrom;
			stationTo_		=	stationTo;

			for (var i=transform.childCount-1; i>=0; i--)
				Destroy(transform.GetChild(i).gameObject);

			selectionFrom.gameObject.SetActive(stationFrom != null);
			selectionTo.gameObject.SetActive(stationTo != null);

			if (stationFrom != null)
			{
				selectionFrom.transform.position	=	stationFrom.position;

				var	clr		=	stationFrom.GetColor();
					clr.a	=	1;

				selectionFrom.color					=	clr;
			}

			if (stationTo != null)
			{
				selectionTo.transform.position		=	stationTo.position;

				var	clr		=	stationTo.GetColor();
					clr.a	=	1;

				selectionTo.color					=	clr;
			}

			//	если станции не пустые
			if (stationFrom_ != null && stationTo_ != null)
			{
				var lineFrom		=	stationFrom_.transform.parent.GetComponent<MetroLine>();
				var lineTo			=	stationTo_.transform.parent.GetComponent<MetroLine>();

				if (lineFrom == lineTo)
				{
					ShowHideLines(false);

					DrawLine(lineFrom, lineFrom.GetStationIndex(stationFrom_), lineFrom.GetStationIndex(stationTo_));
				}
				else
				{
					for (var i=0; i<transfers.Length; i++)
					{
						var containsFrom	=	transfers[i].ContainsLine(lineFrom);
						var containsTo		=	transfers[i].ContainsLine(lineTo);

						if (containsFrom > -1 && containsTo > -1)
						{
							ShowHideLines(false);

							DrawLine(lineFrom, lineFrom.GetStationIndex(stationFrom_), lineFrom.GetStationIndex(transfers[i].stations[containsFrom]));
							DrawLine(lineTo, lineTo.GetStationIndex(stationTo_), lineTo.GetStationIndex(transfers[i].stations[containsTo]));
						}
					}
				}
			}
			else
				ShowHideLines(true);
		}
    }

	void ShowHideLines (bool _show)
    {
		for (var i=0; i<lines.Length; i++)
		{
			var	clr		=	lines[i].GetColor();
				clr.a	=	_show ? 1f : 0.2f;

			lines[i].SetColor(clr);
		}
			
	}

	void DrawLine (MetroLine _line, int _from, int _to)
    {
		

		var linePath			=	Instantiate(linePrefab, transform).GetComponent<MetroLine>();
		var	colorPath			=	_line.color;
			colorPath.a			=	1f;

		linePath.SetColor(colorPath);

		var	min		=	Mathf.Min(_from, _to);
		var	max		=	Mathf.Max(_from, _to);

		stationsNum				+=	(max - min);

		for (var i=min; i<=max; i++)
		{
			var	station			=	_line.transform.GetChild(i);

			var stationPath				=	Instantiate(stationPrefab, linePath.transform).GetComponent<MetroStation>();
				stationPath.position	=	station.position;
				stationPath.gameObject.SetActive(false);
		}
	}

	//	end
}
