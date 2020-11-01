using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroTransfer : MonoBehaviour
{
	public MetroStation[]	stations;

	public MetroLine[]		lines;

	public int ContainsLine (MetroLine _line)
    {
        for (var i=0; i<lines.Length; i++)
		{
			if (lines[i] == _line)
				return i;
		}

		return -1;
    }

    //	
    void Update()
    {
        
    }

	//	
}
