using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MeEngine.Events;
using HexasphereGrid;

namespace HotJupiter {
    public enum HexDirection { //Relative to current facing
        
        Forward = 0,
        ForwardRight = 1,
        BackwardRight = 2,
        Backward = 3,
        BackwardLeft = 4,
        ForwardLeft = 5,
        
    }

    public enum PentaDirection { //Relative to current facing
        Forward = 0,
        ForwardRight = 1,
        BackwardRight = 2,
        BackwardLeft = 3,
        ForwardLeft = 4,
        
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
            return $"TileCoords: {index}";
        }
    }

    public enum TileShape {
        Hexagon,
        Pentagon
    }

    [System.Serializable]
    public struct TileWithFacing {
        public TileCoords position;
        public TileCoords facing;
        public TileLevel level;

        public TileWithFacing(TileCoords position, TileCoords facing, TileLevel level = new TileLevel()){
            this.position = position;
            this.facing = facing;
            this.level = level;
        }

        public static explicit operator Tile(TileWithFacing tile) {
            return new Tile(tile.position, tile.level);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TileWithFacing))
            return false;

            TileWithFacing otherStruct = (TileWithFacing)obj;

            return this.level == otherStruct.level && this.position == otherStruct.position && this.facing == otherStruct.facing;
        }

        public override int GetHashCode()
        {
            return facing.GetHashCode().WrapShift(4) ^ position.GetHashCode().WrapShift(2) ^ level.GetHashCode();
        }

        public override string ToString()
        {
            return $"TileWithFacing - Position:{position}, Facing:{facing}, Level:{level}";
        }
    }

    [System.Serializable]
    public struct TileLevel {
        public byte level;

        public const int MAX = 5;
        public const int MIN = 0;

        public TileLevel(int levelIndex = 0){
            this.level = (byte)levelIndex;
        }

        public static implicit operator int(TileLevel tileLevel){
            return (int)tileLevel.level;
        }

        public static implicit operator uint(TileLevel tileLevel){
            return tileLevel.level;
        }

        public static implicit operator TileLevel(int levelIndex){
            return new TileLevel(Mathf.Clamp(levelIndex, MIN, MAX));
        }

        public static implicit operator TileLevel(uint levelIndex){
            return new TileLevel(Mathf.Clamp((int)levelIndex, MIN, MAX));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TileLevel))
            return false;

            TileLevel otherStruct = (TileLevel)obj;

            return this.level == otherStruct.level;
        }

        public override int GetHashCode()
        {
            return level.GetHashCode();
        }

        public override string ToString()
        {
            return $"TileLevel: {level}";
        }
    }

    [System.Serializable]
    public struct Tile {
        public TileCoords position;
        public TileLevel level;

        public Tile(TileCoords position, TileLevel level = new TileLevel()) {
            this.position = position;
            this.level = level;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tile))
            return false;

            Tile otherStruct = (Tile)obj;

            return this.level == otherStruct.level && this.position == otherStruct.position;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode().WrapShift(2) ^ level.GetHashCode();
        }

        public override string ToString()
        {
            return $"Tile - Position:{position}, Level:{level.level}";
        }
    }

    public class HexMapHelper : MonoBehaviour
    {
        private static HexMapHelper instance;

        public const float gridFirstAltitudeOffset = 0.225f;
        public const float gridAltitudeOffsets = 0.45f;

        public Hexasphere baseHexasphere;
        public HexGridSphere baseHexGrid;
        //public PlanetSizer planetSizer;

        public const float HexWidth = 1.0f; //The incircle diameter of a hexagon

        void Awake(){
            instance = this;
        }

        public static TileCoords GetTileFromWorldPoint(Vector3 worldPos){
            return new TileCoords(){ index = instance.baseHexasphere.GetTileAtPosition(worldPos) };
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
            HexasphereGrid.Tile tile = instance.baseHexasphere.tiles[startTile.index];
            if(tile.neighbours.Length == 6){
                //count tiles until we find the current facing
                for(int neighborIndex = 0; neighborIndex < tile.neighbours.Length; neighborIndex++){
                    HexasphereGrid.Tile neighborTile = tile.neighbours[neighborIndex];
                    if (neighborTile.index == startFacing.index) {
                        return new TileCoords { index = tile.neighbours[(neighborIndex + (int)direction) % 6].index};
                    }
                }
                throw new Exception("Failed to Find Neighbor");
            }else{
                throw new Exception("The startTile is not a Hexagon. Use GetTileInPentaDirection instead.");
            }
        }

        public static TileCoords GetTileInPentaDirection(TileCoords startTile, TileCoords startFacing, PentaDirection direction){
            HexasphereGrid.Tile tile = instance.baseHexasphere.tiles[startTile.index];
            if(tile.neighbours.Length == 5){
                //count tiles until we find the current facing
                for(int neighborIndex = 0; neighborIndex < tile.neighbours.Length; neighborIndex++){
                    HexasphereGrid.Tile neighborTile = tile.neighbours[neighborIndex];
                    if (neighborTile.index == startFacing.index) {
                        return new TileCoords { index = tile.neighbours[(neighborIndex + (int)direction) % 5].index};
                    }
                }
                throw new Exception("Failed to Find Neighbor");
            }else{
                throw new Exception("The startTile is not a Pentagon. Use GetTileInHexDirection instead.");
            }
        }

        public static Vector3 GetTileNormal(TileCoords tileCoords){
            return instance.baseHexasphere.GetTileCenter(tileCoords.index).normalized;
        }

        public static Vector3 GetWorldPointFromTile(TileCoords tileCoords, TileLevel level = new TileLevel()){
            HexasphereGrid.Tile tile = instance.baseHexasphere.tiles[tileCoords.index];
            if(level == 0){
                return instance.baseHexasphere.GetTileCenter(tileCoords.index);
            }else{
                return instance.baseHexasphere.GetTileCenter(tileCoords.index) + (GetTileNormal(tileCoords) * GetAltitudeFromLevel(level));
            }
        }

        public static Vector3 GetWorldPointFromTile(Tile tile){
            return GetWorldPointFromTile(tile.position, tile.level);
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

        

        /*public static HexDirection GetHexDirectionFromNeighborVector(Vector3 forwardVector, Vector3 upVector, Vector3 neighborVector){
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
        }*/

        public static TileLevel GetLevelFromAltitude(float altitude){
            return Mathf.Clamp(Mathf.RoundToInt((altitude + gridFirstAltitudeOffset) / gridAltitudeOffsets), TileLevel.MIN, TileLevel.MAX);
        }

        public static float GetAltitudeFromLevel(TileLevel level) {
            return Mathf.Max(0, gridFirstAltitudeOffset + ((level - 1) * gridAltitudeOffsets));
        }

        public static float GetRadialOffsetFromLevel(TileLevel level){
            return instance.baseHexGrid.Radius + GetAltitudeFromLevel(level);
        }

        public static float CrowFlyDistance(Tile hex1, Tile hex2){
            Vector3 pos1 = GetWorldPointFromTile(hex1);
            Vector3 pos2 = GetWorldPointFromTile(hex2);
            return Vector3.Distance(pos1, pos2);
        }

        public static HexasphereGrid.Tile GetHexasphereTile(Tile tile){
            if(instance.baseHexasphere.tiles != null){ 
                return instance.baseHexasphere.tiles[tile.position.index];
            }else{
                return null;
            }
        }
    }
}


