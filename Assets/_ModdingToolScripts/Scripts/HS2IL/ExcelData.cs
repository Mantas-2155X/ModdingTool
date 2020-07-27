using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExcelData", menuName = "Honey Select 2: Libido/Create ExcelData", order = 2)]
public class ExcelData : ScriptableObject
{
	public List<ExcelData.Param> list = new List<ExcelData.Param>();

	[Serializable]
	public class Param
	{
		public List<string> list = new List<string>();
	}
}