using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MyBox;
using UnityEditor;
using UnityEngine;

public class HPointList : MonoBehaviour
{
    #if UNITY_EDITOR
        private struct HPointType
        {
            public string Name;
            public string KeyName;
            public string Comment;
            public int[] NoForeplay;
            public int[] NoJobs;
            public int[] NoInsert;
            public int[] NoSpecial;
            public int[] NoLesbo;
            public int[] NoVarious;
            public bool HasLimit;
            public int ID;
        }
        
        private static readonly List<HPointType> hPointTypes = new List<HPointType>()
        {
            new HPointType
            {
                ID = 0,
                Name = "Floor",
                KeyName = "yuka"
            },
            new HPointType
            {
                ID = 1,
                Name = "Standing",
                KeyName = "tachi"
            },
            new HPointType
            {
                ID = 2,
                Name = "Wall",
                KeyName = "kabe"
            },
            new HPointType
            {
                ID = 3,
                Name = "Chair",
                KeyName = "isu"
            },
            new HPointType
            {
                ID = 4,
                NoSpecial = new int[]
                {
                    5
                },
                Name = "Desk",
                KeyName = "tsukue",
                Comment = "角ｵﾅ"
            },
            new HPointType
            {
                ID = 5,
                Name = "Armchair",
                KeyName = "tesuri_isu",
                Comment = "椅子と同じ"
            },
            new HPointType
            {
                ID = 6,
                Name = "Long Chair",
                KeyName = "naga_isu",
                Comment = "椅子と同じ"
            },
            new HPointType
            {
                ID = 7,
                Name = "Sofa Bed",
                KeyName = "sofa_bed"
            },
            new HPointType
            {
                ID = 8,
                Name = "Sofa Table",
                KeyName = "sofa_table",
                Comment = "そんなものはない"
            },
            new HPointType
            {
                ID = 9,
                Name = "Counter",
                KeyName = "counter"
            },
            new HPointType
            {
                ID = 10,
                Name = "Stairs",
                KeyName = "kaidan"
            },
            new HPointType
            {
                ID = 11,
                Name = "Bath",
                KeyName = "ofuro"
            },
            new HPointType
            {
                ID = 12,
                Name = "Shower",
                KeyName = "shower"
            },
            new HPointType
            {
                ID = 13,
                Name = "Western-style Toilet",
                KeyName = "yoshiki_toire"
            },
            new HPointType
            {
                ID = 14,
                Name = "Japanese-style Toilet",
                KeyName = "washiki_toire"
            },
            new HPointType
            {
                ID = 15,
                Name = "Glory Hole",
                KeyName = "glory_hole"
            },
            new HPointType
            {
                ID = 16,
                Name = "Soap Mattress",
                KeyName = "sopu_matto",
                Comment = "未実装"
            },
            new HPointType
            {
                ID = 17,
                Name = "Triangle Horse",
                KeyName = "mokuba"
            },
            new HPointType
            {
                ID = 18,
                Name = "Inspection Chair",
                KeyName = "bundendai"
            },
            new HPointType
            {
                ID = 19,
                Name = "Restraint",
                KeyName = "kosokudai"
            },
            new HPointType
            {
                ID = 20,
                Name = "Guillotine",
                KeyName = "guillotine"
            },
            new HPointType
            {
                ID = 21,
                Name = "Dildo",
                KeyName = "dildo"
            },
            new HPointType
            {
                ID = 22,
                Name = "Restraint Chair",
                KeyName = "kosokuisu"
            },
        };
        
        [ButtonMethod]
        public void CreateDefaultHPoints()
        {
            var parent = GameObject.Find("Map");
            DestroyImmediate(parent.transform.Find("hpoint_map").gameObject);
            
            var hPointGroup = new GameObject("hpoint_map");
            hPointGroup.transform.parent = parent.transform;
            
            var list = hPointGroup.AddComponent<HPointList>();
            foreach (var hpointType in hPointTypes)
            {
                var hPointLocationGroup = new GameObject{name = $"hpoint_{hpointType.KeyName}_gp"};
                hPointLocationGroup.transform.parent = hPointGroup.transform;

                for (var i = 0; i < 2; i++)
                {
                    var startPoint = new GameObject{name = i == 0 ? "hpoint_start" : $"hpoint_{hpointType.KeyName}"};
                    startPoint.transform.parent = hPointLocationGroup.transform;
                    var hPointComponent = startPoint.AddComponent<HPoint>();
                    hPointComponent.id = hpointType.ID;
                    hPointComponent.Data = new HPoint.HpointData();
                    if (hpointType.NoForeplay != null) {
                        hPointComponent.Data.notMotion[0].motionID = hpointType.NoForeplay.ToList();
                    }
                    if (hpointType.NoJobs != null) {
                        hPointComponent.Data.notMotion[1].motionID = hpointType.NoJobs.ToList();
                    }
                    if (hpointType.NoInsert != null) {
                        hPointComponent.Data.notMotion[2].motionID = hpointType.NoInsert.ToList();
                    }
                    if (hpointType.NoSpecial != null) {
                        hPointComponent.Data.notMotion[3].motionID = hpointType.NoSpecial.ToList();
                    }
                    if (hpointType.NoLesbo != null) {
                        hPointComponent.Data.notMotion[4].motionID = hpointType.NoLesbo.ToList();
                    }
                    if (hpointType.NoVarious != null) {
                        hPointComponent.Data.notMotion[5].motionID = hpointType.NoVarious.ToList();
                    }
                }
            }
            
            list.AssignHPointsToGroup();
        }
        
        [ButtonMethod]
        private void AssignHPointsToGroup()
        {
            HpointGroup = this.GetComponentsInChildren<HPoint>()
                .Where(x => x.name == "hpoint_start")
                .Select(x => new PlaceInfo
                {
                    HPoints = x.transform.parent.gameObject,
                    Place = x.id,
                    Start = x.gameObject
                }).ToArray();
        }
    #endif
    
    [SerializeField] private PlaceInfo[] HpointGroup;
    private HPoint[] HPoints;
    public Dictionary<int, List<HPoint>> lst;

    [Serializable]
    private class PlaceInfo
    {
        [Header("Hポイントグループ")] public GameObject HPoints;
        [Header("場所")] public int Place;
        [Header("開始ポイント")] public GameObject Start;
    }

    [Serializable]
    public class LoadInfo
    {
        public string Manifest;
        public string Name;
        public string Path;
    }
}