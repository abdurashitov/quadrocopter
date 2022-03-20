using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globalparametr : MonoBehaviour
{
	private static Globalparametr instance;
	public List<Vector3> route = new List<Vector3> { 

		new Vector3(40, 10, -10), 
		new Vector3(40, 10, 20),
		new Vector3(0, 10, 40),
		new Vector3(40, 10,40),
		new Vector3(0, 10,0),

	};

	public List<Comand> route1 = new List<Comand> {

		new Comand(Comand.comands.up, new Vector3(0,10,0), 0),
		new Comand(Comand.comands.goTo, new Vector3(40,10,-10), 8),
		new Comand(Comand.comands.goTo, new Vector3(40,10,20), 8),
		new Comand(Comand.comands.goTo, new Vector3(0,10,40), 8),
		new Comand(Comand.comands.goTo, new Vector3(0,10,0), 8),
		//new Comand(Comand.comands.hover, new Vector3(0,10,0), 0),
		new Comand(Comand.comands.down, new Vector3(0,0,0), 0)

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
