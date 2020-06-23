﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] generate_falloff_map(int width, int height)
    {
        float[,] map = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i / (float)width * 2 - 1;
                float y = j / (float)height * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = fx(value);
            }
        }

        return map;
    }


    static float fx(float value)
    {
        float
            a = 3,
            b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
        
    }
}
