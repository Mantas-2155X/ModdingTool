using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventContentInfoData", menuName = "Honey Select 2: Libido/Create EventContentInfoData", order = 1)]
public class EventContentInfoData : ScriptableObject
{
    public List<Param> param = new List<Param>();

    [Serializable]
    public class Param
    {
        public int id;
        public int[] meetingLocationMaps;
        public int[] goToFemaleMaps;
        public int draw;
        public int call;
        public string manifestEventSprite;
        public string bundleEventSprite;
        public string fileEventSprite;
        public string[] eventNames;
    }
}