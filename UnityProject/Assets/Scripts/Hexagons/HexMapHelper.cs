﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MeEngine.Events;
using HexasphereGrid;

public enum HexDirection {
    
    Forward = 0,
    ForwardRight = 1,
    BackwardRight = 2,
    Backward = 3,
    BackwardLeft = 4,
    ForwardLeft = 5,
    
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

public enum TileShape {
    Hexagon,
    Pentagon
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

    public static TileShape GetTileShape(TileCoords tile) {
        return instance.baseHexasphere.tiles[tile.index].neighbours.Length == 6 ? TileShape.Hexagon : TileShape.Pentagon;
    }

    public static TileCoords GetTileInHexDirection(TileCoords startTile, TileCoords startFacing, HexDirection direction){
        Tile tile = instance.baseHexasphere.tiles[startTile.index];
        if(tile.neighbours.Length == 6){
            //count tiles until we find the current facing
            for(int neighborIndex = 0; neighborIndex < tile.neighbours.Length; neighborIndex++){
                Tile neighborTile = tile.neighbours[neighborIndex];
                if (neighborTile.index == startFacing.index) {
                    return new TileCoords { index = tile.neighbours[(neighborIndex + (int)direction) % 6].index};
                }
            }
            throw new Exception("Failed to Find Neighbor");
        }else{
            throw new Exception("The startTile is not a Hexagon. Use GetTileInPentaDirection instead.");
        }
    }

    /*public static TileCoords GetTileInHexDirection(TileCoords startTile, Vector3 forwardVector, HexDirection direction){
        Tile tile = instance.baseHexasphere.tiles[startTile.index];
        if(tile.neighbours.Length == 6){
            Vector3 startTileCenter = instance.baseHexasphere.GetTileCenter(tile.index, true);
            Vector3 upVector = GetTileNormal(startTile);
            int neighborIndex = -1;
            foreach(Tile tileNeighbor in instance.baseHexasphere.tiles[startTile.index].neighbours) {
                neighborIndex++;
                Vector3 neighborVector = instance.baseHexasphere.GetTileCenter(tileNeighbor.index, true) - startTileCenter;
                HexDirection neighborDirection = GetHexDirectionFromNeighborVector(forwardVector, upVector, neighborVector);
                Debug.DrawRay(startTileCenter, neighborVector, Color.Lerp(Color.red, Color.green, neighborIndex / 5f), 10f);
                if(neighborDirection == direction)
                    return new TileCoords { index = tileNeighbor.index };
            }
            throw new Exception("Failed to Find Neighbor");
        }else{
            throw new Exception("The startTile is not a Hexagon. Use GetTileInPentaDirection instead.");
        }
    }*/

    public static TileCoords GetTileInPentaDirection(TileCoords startTile, Vector3 forwardVector, PentaDirection direction){
        Tile tile = instance.baseHexasphere.tiles[startTile.index];
        if(tile.neighbours.Length == 5){
            Vector3 startTileCenter = instance.baseHexasphere.GetTileCenter(tile.index, true);
            Vector3 upVector = GetTileNormal(startTile);
            foreach(Tile tileNeighbor in instance.baseHexasphere.tiles[startTile.index].neighbours) {
                Vector3 neighborVector = instance.baseHexasphere.GetTileCenter(tileNeighbor.index, true) - startTileCenter;
                PentaDirection neighborDirection = GetPentaDirectionFromNeighborVector(forwardVector, upVector, neighborVector);
                if(neighborDirection == direction)
                    return new TileCoords { index = tileNeighbor.index };
            }
            throw new Exception("Failed to Find Neighbor");
        }else{
            throw new Exception("The startTile is not a Pentagon. Use GetTileInHexDirection instead.");
        }
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


    private const float Cos30 = 0.86602f;
    private const float Cos72 = 0.30901699437f;
    private const float Cos90 = 0f;

    private const float Cos144 = -0.80901699437f;
    private const float Cos150 = -0.86602f;

    

    public static HexDirection GetHexDirectionFromNeighborVector(Vector3 forwardVector, Vector3 upVector, Vector3 neighborVector){
        //project both vectors onto flat plane
        forwardVector = Vector3.ProjectOnPlane(forwardVector, upVector);
        neighborVector = Vector3.ProjectOnPlane(neighborVector, upVector);

        float forwardDot = Vector3.Dot(forwardVector, neighborVector);
        
        if(forwardDot >= Cos30){
            return HexDirection.Forward;
        }else if(forwardDot < Cos30 && forwardDot >= Cos150){
            Vector3 leftVector = Vector3.Cross(forwardVector, upVector);
            float leftDot = Vector3.Dot(leftVector, neighborVector);

            if(leftDot >= Cos90) {
                if(forwardDot >= Cos90){
                    return HexDirection.ForwardLeft;
                }else{
                    return HexDirection.BackwardLeft;
                }
            }else{
                if(forwardDot >= Cos90){
                    return HexDirection.ForwardRight;
                }else{
                    return HexDirection.BackwardRight;
                }
            }
        }else{
            return HexDirection.Backward;
        }
    }

    public static PentaDirection GetPentaDirectionFromNeighborVector(Vector3 forwardVector, Vector3 upVector, Vector3 neighborVector){
        float forwardDot = Vector3.Dot(forwardVector, neighborVector);
        
        if(forwardDot < Cos144){
            return PentaDirection.Backward;
        }else{
            Vector3 leftVector = Vector3.Cross(forwardVector, upVector);
            float leftDot = Vector3.Dot(leftVector, neighborVector);

            if(leftDot >= Cos90) {
                if(forwardDot < Cos72){
                    return PentaDirection.BackwardLeft;
                }else{
                    return PentaDirection.ForwardLeft;
                }
            }else{
                if(forwardDot < Cos72){
                    return PentaDirection.BackwardRight;
                }else{
                    return PentaDirection.ForwardRight;
                }
            }
        }
    }

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



