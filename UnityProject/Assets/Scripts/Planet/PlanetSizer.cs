﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexasphereGrid;

[ExecuteAlways]
public class PlanetSizer : MonoBehaviour
{
    public Hexasphere basePlanetSphere;
    public List<Hexasphere> sphereHexGrids;

    const float unitHexArea = 0.866f; //A hexagon with an incircle diamater of one, has this area.

    const float fourPI = 4 * Mathf.PI;
    const float spherePrimitiveRadiusMultiplier = 2f; //A Unity sphere with a scale one has a radius of 1/2;

    private int lastTileCount = 0;

    [SerializeField]
    public float planetRadius = 20f;

    // Start is called before the first frame update
    void Start()
    {
        //Size the sphere to give us hexagons of the desired size.
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Application.isPlaying){
            if(basePlanetSphere.tiles.Length != lastTileCount)
            {
                SetPlanetSize();
                lastTileCount = basePlanetSphere.tiles.Length;
            }
        }
    }

    private void SetPlanetSize()
    {
        //Desired Size of Hexagon Tile = 1 Unit Height (DH = 1.0)
        //Desired Surface Area of Sphere = Number of Tiles * Area of Hexagon
        //R = sqrt(SA / (4*PI))
        //Kit - This will be a little off, because 12 of those tiles are pentagons which may have a different surface area than a hexagon
        int tileCount = basePlanetSphere.tiles.Length;
        planetRadius = Mathf.Sqrt((tileCount * (unitHexArea * HexMapHelper.HexWidth)) / fourPI);
        float sphereSize = planetRadius * spherePrimitiveRadiusMultiplier;

        basePlanetSphere.transform.localScale = Vector3.one * sphereSize;

        //Planet maximum mountain peak altitude
        float peakAltitude = HexMapHelper.gridAltitudeOffsets * sphereHexGrids.Count;
        basePlanetSphere.extrudeMultiplier = peakAltitude / sphereSize; //This is relative to sphere scale

        for(int gridIndex = 1; gridIndex < sphereHexGrids.Count; gridIndex++) //Ignore the first grid, this is the base planet
        {
            sphereHexGrids[gridIndex].transform.localScale = Vector3.one * (planetRadius + HexMapHelper.gridFirstAltitudeOffset + ((gridIndex - 1) * HexMapHelper.gridAltitudeOffsets)) * spherePrimitiveRadiusMultiplier;
        }

        for(int tileIndex = 0; tileIndex < tileCount; tileIndex++){
            basePlanetSphere.SetTileExtrudeAmount(tileIndex, Random.Range(0, sphereHexGrids.Count) * (sphereHexGrids.Count - 1));
        }
    }
}