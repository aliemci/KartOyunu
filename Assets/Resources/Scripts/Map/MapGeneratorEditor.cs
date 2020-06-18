﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapgen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if(mapgen.autoUpdate)
                mapgen.generate_map();
        }


        if (GUILayout.Button("Generate"))
            mapgen.generate_map();
    }

}
