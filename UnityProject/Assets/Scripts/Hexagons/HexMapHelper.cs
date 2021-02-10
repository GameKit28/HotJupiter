﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MeEngine.Events;
using HexasphereGrid;

public enum HexDirection {
    Backward = 0,
    BackwardLeft = 1,
    ForwardLeft = 2,
    Forward = 3,
    ForwardRight = 4,
    BackwardRight = 5
}

public enum PentaDirection {
    Backward = 0,
    BackwardLeft = 1,
    ForwardLeft = 2,
    ForwardRight = 3,
    BackwardRight = 4
}

[Serializable]
public struct TileCoords {
    public int index;

    public bool Equals(TileCoords other)
    {
        return Equals(other, this);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var objectToCompareWith = (TileCoords) obj;

        return index == objectToCompareWith.index;
    }

    public override int GetHashCode()
    {
        return index.GetHashCode();
    }

    public static bool operator ==(TileCoords o1, TileCoords o2) 
    {
        return o1.Equals(o2);
    }

    public static bool operator !=(TileCoords o1, TileCoords o2) 
    {
        return !o1.Equals(o2);
    }

    public override string ToString()
    {
        return "Tile: " + index.ToString();
    }
}

public struct TileWithFacing {
    public TileCoords position;
    public TileCoords facing;
}

public class HexMapHelper : MonoBehaviour
{
    private static HexMapHelper instance;

    public const float gridFirstAltitudeOffset = 0.25f;
    public const float gridAltitudeOffsets = 0.5f;

    public Hexasphere baseHexasphere;
    public PlanetSizer planetSizer;

    public const float HexWidth = 1.0f; //The incircle diameter of a hexagon

    void Awake(){
        instance = this;
    }

    public static TileCoords GetTileFromWorldPoint(Vector3 worldPos){
        return new TileCoords(){ index = instance.baseHexasphere.GetTileAtPos(worldPos) };
    }

    public static List<TileCoords> GetNeighborTiles(TileCoords tile){
        List<TileCoords> neighbors = new List<TileCoords>();
        foreach(var neighbor in instance.baseHexasphere.tiles[tile.index].neighbours){
            neighbors.Add(new TileCoords() {index = neighbor.index });
        }
        return neighbors;
    }

    public static TileCoords GetTileInDirection(TileCoords startTile, Vector3 forwardVector, HexDirection direction){
        Tile tile = instance.baseHexasphere.tiles[startTile.index];
        Vector3 startTileCenter = instance.baseHexasphere.GetTileCenter(tile.index, true);
        Vector3 upVector = GetTileNormal(startTile);
        Tile[] neighbors = instance.baseHexasphere.tiles[startTile.index].neighbours;
        Color[] colorsToUse = new Color[] {Color.red, Color.yellow, Color.green, Color.blue, Color.magenta, Color.white};
        for (int neighborIndex = 0; neighborIndex < neighbors.Length; neighborIndex++) {
            Tile tileNeighbor = neighbors[neighborIndex];
            Debug.Log("Neighbor" + tileNeighbor.index);
            Vector3 neighborVector = instance.baseHexasphere.GetTileCenter(tileNeighbor.index, true) - startTileCenter;
            //Debug.DrawRay(startTileCenter, neighborVector, colorsToUse[neighborIndex], 100, false);
            HexDirection neighborDirection = GetHexDirectionFromNeighborVector(forwardVector, upVector, neighborVector);
            Debug.Log("Neighbor Dirction" + neighborDirection);
            if(neighborDirection == direction)
                return new TileCoords { index = tileNeighbor.index };
        }
        throw new Exception("Failed to Find Neighbor");
    }

    public static Vector3 GetTileNormal(TileCoords tileCoords){
        return instance.baseHexasphere.GetTileCenter(tileCoords.index).normalized;
    }

    public static Vector3 GetWorldPointFromTile(TileCoords tileCoords, int level = 0){
        Tile tile = instance.baseHexasphere.tiles[tileCoords.index];
        if(level == 0){
            return instance.baseHexasphere.GetTileCenter(tileCoords.index, true);
        }else{
            return instance.baseHexasphere.GetTileCenter(tileCoords.index) + (GetTileNormal(tileCoords) * GetAltitudeFromLevel(level));
        }
    }

    public static Quaternion GetRotationFromFacing(TileCoords startingTile, TileCoords tileFacing){
        return Quaternion.LookRotation(GetFacingVector(startingTile, tileFacing), GetTileNormal(startingTile));
    }

    public static Vector3 GetFacingVector(TileCoords startTile, TileCoords tileFacing){
        return (GetWorldPointFromTile(tileFacing) - GetWorldPointFromTile(startTile)).normalized;
    }

    public static List<Vector3> NeighborVectors(TileCoords startTile) {
        Vector3 startPos = instance.baseHexasphere.GetTileCenter(startTile.index);
        List<Vector3> neighborVectors = new List<Vector3>();
        foreach(var tile in instance.baseHexasphere.tiles[startTile.index].neighbours){
            neighborVectors.Add((instance.baseHexasphere.GetTileCenter(tile.index) - startPos).normalized);
        }
        return neighborVectors;
    }

    /*public static TileCoords FindTileInDirection(TileCoords startingTile, TileCoords currentTileFacing, HexDirection direction){
        Vector3 facingVector = GetFacingVector(startingTile, currentTileFacing);
        if(direction == HexDirection.Forward) return currentTileFacing;
        Tile[] neighbors = instance.baseHexasphere.tiles[startingTile.index].neighbours;
        for(int neighborIndex = 0; neighborIndex < neighbors.Length; neighborIndex++){
            Tile neighbor = neighbors[neighborIndex];
            Vector3 neighborVector = instance.baseHexasphere.GetTileCenter(neighbor.index) - instance.baseHexasphere.GetTileCenter(startingTile.index);
            if(direction == GetHexDirectionFromNeighborVector(facingVector, GetTileNormal(startingTile), neighborVector)) {
                return new TileCoords() {index = neighbor.index};
            }
        }
        return currentTileFacing;//Should not encounter this
    }*/

    private const float DotAngle150 = -0.86602f;
    private const float DotAngle30 = 0.86602f;
    private const float DotAngle90 = 0f;

    public static HexDirection GetHexDirectionFromNeighborVector(Vector3 forwardVector, Vector3 upVector, Vector3 neighborVector){
        float forwardAngle = Vector3.Dot(forwardVector, neighborVector);
        
        if(forwardAngle >= DotAngle30){
            return HexDirection.Forward;
        }else if(forwardAngle < DotAngle30 && forwardAngle >= DotAngle150){
            Vector3 leftVector = Vector3.Cross(forwardVector, upVector);
            float leftAngle = Vector3.Dot(leftVector, neighborVector);

            if(leftAngle >= DotAngle90) {
                if(forwardAngle >= DotAngle90){
                    return HexDirection.ForwardLeft;
                }else{
                    return HexDirection.BackwardLeft;
                }
            }else{
                if(forwardAngle >= DotAngle90){
                    return HexDirection.ForwardRight;
                }else{
                    return HexDirection.BackwardRight;
                }
            }
        }else{
            return HexDirection.Backward;
        }
    }

    /*public static Vector3 GetVectorFromDirection(HexDirection direction){
        switch(direction){
            case HexDirection.NorthEast:
                return new Vector3(0.5f, 0, 0.87f);
            case HexDirection.East:
                return new Vector3(1f, 0, 0);
            case HexDirection.SouthEast:
                return new Vector3(0.5f, 0, -0.87f);
            case HexDirection.SouthWest:
                return new Vector3(-0.5f, 0, -0.87f);
            case HexDirection.West:
                return new Vector3(-1f, 0, 0);
            case HexDirection.NorthWest:
                return new Vector3(-0.5f, 0, 0.87f);
            default:
                return Vector3.up;
        }
    }*/

    public static int GetLevelFromAltitude(float altitude){
        return Mathf.Clamp(Mathf.RoundToInt((altitude + gridFirstAltitudeOffset) / gridAltitudeOffsets), 0, 6);
    }

    public static float GetAltitudeFromLevel(int level) {
        return Mathf.Max(0, gridFirstAltitudeOffset + ((level - 1) * gridAltitudeOffsets));
    }

    public static float GetRadialOffsetFromLevel(int level){
        return instance.planetSizer.planetRadius + GetAltitudeFromLevel(level);
    }

    public static Color GetLevelColor(int level) {
        switch(level) {
            case 0: 
                return Color.white;
            
            case 1:
                return Color.red;

            case 2:
                return new Color(1, 0.5f, 0, 1); //Orange

            case 3:
                return Color.yellow;

            case 4:
                return Color.green;

            case 5:
                return Color.blue;

            case 6:
                return new Color(1, 0, 1, 1); //Purple

            default:
                return Color.white;
        }
    }

    public static float CrowFlyDistance(TileCoords hex1Tile, int hex1Height, TileCoords hex2Tile, int hex2Height){
        Vector3 pos1 = GetWorldPointFromTile(hex1Tile, hex1Height);
        Vector3 pos2 = GetWorldPointFromTile(hex2Tile, hex2Height);
        return Vector3.Distance(pos1, pos2);
    }
}



