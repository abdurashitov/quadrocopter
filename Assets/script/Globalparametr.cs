using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globalparametr : MonoBehaviour
{
	private static Globalparametr instance;
	public List<Vector3> route = new List<Vector3> { 

		new Vector3(40, 10, 0), 
		//new Vector3(40, 10, 20),
		//new Vector3(0, 10, 40),
		//new Vector3(40, 10,40),
		//new Vector3(0, 10,0),

	};

	Globalparametr()
    {    }

	public static Globalparametr getInstance()
	{
		if (instance == null)
			instance = new Globalparametr();
		return instance;
	}

}
