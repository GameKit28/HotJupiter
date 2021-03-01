using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfieldManager : MonoBehaviour
{

    public struct TileReservation{
        public float reservationTime;
        public IHaveTileFootprint reservingObject;
    }

    public struct TileClaim{
        public IHaveTileFootprint claimHolder;
        public TileObstacleType obstacleType;

        public override bool Equals(object obj)
        {
            if (!(obj is TileClaim))
            return false;

            TileClaim otherStruct = (TileClaim)obj;

            return this.claimHolder == otherStruct.claimHolder && this.obstacleType == otherStruct.obstacleType;
        }

        public override int GetHashCode()
        {
            return claimHolder.GetHashCode().WrapShift(2) ^ obstacleType.GetHashCode();
        }
    }

    public static PlayfieldManager instance;

    private Dictionary<Tile, HashSet<TileClaim>> tileContents = new Dictionary<Tile, HashSet<TileClaim>>();
    private Dictionary<IHaveTileFootprint, HashSet<HashSet<TileClaim>>> claimMap = new Dictionary<IHaveTileFootprint, HashSet<HashSet<TileClaim>>>();

    private void Awake() {
        instance = this;
    }

    public static TileObstacleType GetTileObstacleType(Tile tile){
        HashSet<TileClaim> tileClaims;
        TileObstacleType foundObstacle = TileObstacleType.Empty;
        if(instance.tileContents.TryGetValue(tile, out tileClaims)){
            Debug.Log($"Tile Claim Found {tile}");
            foreach(TileClaim claim in tileClaims){
                if((int)claim.obstacleType > (int)foundObstacle){
                    foundObstacle = claim.obstacleType;
                }
            }
        }
        Debug.Log($"Obstacle Type: {tile}, {foundObstacle}");
        return foundObstacle;
    }

    public static List<IHaveTileFootprint> GetTileOccupants(Tile tile){
        HashSet<TileClaim> tileClaims;
        List<IHaveTileFootprint> tileOccupants = new List<IHaveTileFootprint>();
        if(instance.tileContents.TryGetValue(tile, out tileClaims)){
            foreach(TileClaim claim in tileClaims){
                tileOccupants.Add(claim.claimHolder);
            }
        }
        return tileOccupants;
    }

    public static bool TryStakeTileClaim(FootprintTile tile, IHaveTileFootprint claimant){
        Debug.Log(tile);
        Debug.Log(claimant);
        HashSet<TileClaim> tileClaims;
        if(instance.tileContents.TryGetValue(tile.tile, out tileClaims)){
            if(tileClaims == null) tileClaims = new HashSet<TileClaim>();
            if(tile.obstacleType != TileObstacleType.Empty){
                foreach(TileClaim claim in tileClaims){
                    if((int)claim.obstacleType > 0){ //Not Empty
                        return false;
                    }
                }
            }
        }else{
            tileClaims = new HashSet<TileClaim>();
            instance.tileContents.Add(tile.tile, tileClaims);
        }
        tileClaims.Add(new TileClaim() {claimHolder = claimant, obstacleType = tile.obstacleType});
        
        HashSet<HashSet<TileClaim>> claimSets;
        if(instance.claimMap.TryGetValue(claimant, out claimSets)){
            claimSets.Add(tileClaims);
        }else{
            claimSets = new HashSet<HashSet<TileClaim>>();
            claimSets.Add(tileClaims);
            instance.claimMap.Add(claimant, claimSets);
        }

        return true;
    }

    public static void RemoveTileClaim(Tile tile, IHaveTileFootprint claimant){
        HashSet<TileClaim> tileClaims;
        if(instance.tileContents.TryGetValue(tile, out tileClaims)){
            int result = tileClaims.RemoveWhere((x) => x.claimHolder == claimant);
            if(result == 0) Debug.LogWarning($"Claimant {claimant} has no stake on tile {tile}.");

            HashSet<HashSet<TileClaim>> claimSets;
            if(instance.claimMap.TryGetValue(claimant, out claimSets)){
                bool setResult = claimSets.Remove(tileClaims);
                if(setResult == false) Debug.LogWarning($"Claimant {claimant} is missing stake on tile {tile}.");
                if(claimSets.Count == 0){
                    instance.claimMap.Remove(claimant);
                }
            }else{
                Debug.LogWarning($"Claimant {claimant} has no stakes.");
            }
        }else{
            Debug.LogWarning($"Tile {tile} has no active claims.");
        }
    }

    public static void RemoveAllClaims(IHaveTileFootprint claimHolder){
        HashSet<HashSet<TileClaim>> tileClaimSets;
        if(instance.claimMap.TryGetValue(claimHolder, out tileClaimSets)){
            foreach(HashSet<TileClaim> set in tileClaimSets){
                int result = set.RemoveWhere((x) => x.claimHolder == claimHolder);
                if(result == 0) Debug.LogWarning($"Claim Holder {claimHolder} is missing a stake.");
            }
            tileClaimSets.Clear();
            instance.claimMap.Remove(claimHolder);
        }else{
            Debug.LogWarning($"Claim Holder {claimHolder} has no active claims.");
        }
    }
}
