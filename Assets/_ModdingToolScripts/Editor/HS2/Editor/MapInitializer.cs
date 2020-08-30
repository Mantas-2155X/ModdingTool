using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using CameraEffector;
using Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Object = UnityEngine.Object;

public static partial class MapInitializer
{
    private static readonly string[] ppStrings = new[] {"default", "user", "color"};

    private static string mapName = "map";

    public static GameObject MakeNewWrapper(GameObject root, string name)
    {
        var gameObject = new GameObject {name = name};
        gameObject.transform.parent = root.transform;
        return gameObject;
    }
    
    [MenuItem("hooh Tools/Initialize HS2 Map", false)]
    public static void WrapObject()
    {
        var allMapObjects = Object.FindObjectsOfType<Transform>();
        
        var root = new GameObject();
        root.name = "Map";
        var mapVisibleList = root.AddComponent<MapVisibleList>();
        var sunLightInfo = root.AddComponent<SunLightInfo>();
        var lightmapPrefab = root.AddComponent<LightmapPrefab>();

        // Reflection Probe
        var lightProbes = MakeNewWrapper(root, "Light Probes");
        var reflectionProbes = MakeNewWrapper(root, "Reflection Probes");
        var ppVolume = MakeNewWrapper(root, $"PostVolume_{mapName}");
        var lightGroup = MakeNewWrapper(root, "Light Group");
        var mapObjectGroup = MakeNewWrapper(root, "Map Object Group");
        var hPointGroup = MakeNewWrapper(root, $"hpoint_{mapName}");
        var fluidCollisionGroup = MakeNewWrapper(root, $"fluidCollsionGroup_{mapName}");
        var collisionGroup = MakeNewWrapper(root, $"colGroup_{mapName}");
        var soundGroup = MakeNewWrapper(root, "env_00");

        // Post Processing Volume
        InitializePostProcessing(ppVolume);

        // HPoint Groups
        var hPointList = hPointGroup.AddComponent<HPointList>();

        // Sound Group Setting
        var mapEnvSetting = soundGroup.AddComponent<MapEnvSetting>();

        sunLightInfo.RootObjectMaps = new List<GameObject> {soundGroup, mapObjectGroup};

        var audioList = new List<AudioSource>();
        allMapObjects.ToList().ForEach(x =>
        {
            if (x.parent == null)
            {
                x.parent = mapObjectGroup.transform;
            }

            var audio = x.GetComponent<AudioSource>();
            if (audio != null)
            {
                audioList.Add(audio);
                x.parent = soundGroup.transform;
            }

            var light = x.GetComponent<Light>();
            if (light != null)
            {
                try { x.parent = lightGroup.transform; }
                catch (Exception e)
                {
                    // ignored
                }
            }

            var refProbe = x.GetComponent<ReflectionProbe>();
            if (refProbe != null)
            {
                x.parent = reflectionProbes.transform;
            }

            var lightProbe = x.GetComponent<LightProbeGroup>();
            if (lightProbe != null) x.parent = lightProbe.transform;
        });
        
        mapEnvSetting.AudioSources = audioList.ToArray();
    }
}