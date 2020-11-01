using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MetroDebugCube : MonoBehaviour
{
	[Range(0f, 1f)]
	public float position;

	public MetroLine	line;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (line == null)
			return;

        var current	=	position * line.length;

		transform.position	=	line.GetPosition(current);
		transform.rotation	=	line.GetRotation(current);
    }
}
