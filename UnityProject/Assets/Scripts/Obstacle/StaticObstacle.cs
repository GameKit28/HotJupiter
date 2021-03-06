﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MediocreEntertainment.BoardGameToolkit;

public class StaticObstacle : MonoBehaviour, IHaveTilePosition, IHaveTileFootprint
{
    // Start is called before the first frame update
    public ObstacleTemplate template;
    public Transform modelHolder;

    public TileCoords pivotPosition;
    public int pivotLevel;

    StaticFootprint footprint;

    void Start()
    {
        //Randomly Generate Model
        GameObject prefab = template.modelPrefabs.RandomItem();
        GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, modelHolder);

        if(pivotPosition.index == 0){
            pivotPosition = HexMapHelper.GetTileFromWorldPoint(transform.position);
        }
        TileCoords pivotFacing = HexMapHelper.GetNeighborTiles(pivotPosition).RandomItem();
        transform.position = HexMapHelper.GetWorldPointFromTile(pivotPosition, pivotLevel);
        modelHolder.transform.rotation = HexMapHelper.GetRotationFromFacing(pivotPosition, pivotFacing);

        footprint = new StaticFootprint(template.footprint, pivotPosition, pivotFacing, pivotLevel);
    }

    public FootprintBase GetFootprint(){
        return footprint;
    }
    public TileCoords GetPivotTilePosition(){
        return pivotPosition;
    }
    public int GetPivotTileLevel(){
        return pivotLevel;
    }
}
