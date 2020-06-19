using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public GameObject hexObj;
    public Transform parentObj;

    private List<List<GameObject>> hexagons = new List<List<GameObject>>();
    private float[,] fallOffMap;

    public Color[] generate_map()
    {
        float[,] noiseMap = Noise.generate_noise_map(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                noiseMap[x, y] = noiseMap[x, y] - fallOffMap[x, y];

                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        return colorMap;
    }

    void OnValidate()
    {
        if (mapWidth < 1)
            mapWidth = 1;
        if (mapHeight < 1)
            mapHeight = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }

    public void Start()
    {
        fallOffMap = FalloffGenerator.generate_falloff_map(mapWidth);

        seed = Random.Range(0, 1000);

        generate_hexagons();
    }

    public void generate_hexagons()
    {
        float yOffset = 1.5f;
        float xOffset = 0.866f;

        Color[] colorMap = generate_map();

        for (int x = 0; x < mapWidth; x++)
        {
            List<GameObject> tempHexagons = new List<GameObject>();

            for (int y = 0; y < mapHeight; y++)
            {
                GameObject createdHexagon = Instantiate(hexObj, parentObj);

                createdHexagon.transform.localScale = new Vector3(1f, 1f, 1f);
                createdHexagon.transform.localRotation = Quaternion.identity;

                createdHexagon.GetComponent<Renderer>().material.color = colorMap[y * mapWidth + x];

                if (y%2==0)
                    createdHexagon.transform.localPosition = new Vector3((2 * x) * xOffset, y * yOffset, 0f);
                else
                    createdHexagon.transform.localPosition = new Vector3((2 * x + 1) * xOffset, y * yOffset, 0f);

                tempHexagons.Add(createdHexagon);
            }

            hexagons.Add(tempHexagons);
        }

        parentObj.localPosition = hexagons[mapWidth / 2][mapHeight / 2].transform.position * -1;

    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
