using System.Collections.Generic;
using System;
using UnityEngine;

public class TilePath {
    List<TilePathNode> pathNodes = new List<TilePathNode>();

    private class TilePathNode {
        public TileWithFacing tile;

        public TilePathNode(TileWithFacing tile){
            this.tile = tile;
        }
    }

    public TileWithFacing originTile { get {
        return pathNodes[0].tile;
    }}

    public TilePath(TileWithFacing originTile, List<TileWithFacing> traversalSteps){
        pathNodes.Add(new TilePathNode(originTile));
        foreach(var tile in traversalSteps){
            pathNodes.Add(new TilePathNode(tile));
        }
    }

    public TilePath(TileWithFacing originTile){
        pathNodes.Add(new TilePathNode(originTile));
    }

    public int NodeCount { get {
        return pathNodes.Count;
    }}

    private TilePathNode GetNodeAtTime(float turnTime) {
        float normalizedTurnTime = turnTime / TimeManager.TurnDuration;
        int nodeIndex = Mathf.RoundToInt(normalizedTurnTime * (pathNodes.Count - 1));
        return pathNodes[nodeIndex];
    }

    private float GetNodeEnterTime(int nodeIndex){
        return Mathf.Lerp(0, TimeManager.TurnDuration, (nodeIndex - 0.5f) / (pathNodes.Count - 1));
    }

    private float GetNodeEnterTime(TilePathNode node){
        int nodeIndex = pathNodes.IndexOf(node);
        return GetNodeEnterTime(nodeIndex);
    }

    private float GetNodeExitTime(int nodeIndex){
        return Mathf.Lerp(0, TimeManager.TurnDuration, (nodeIndex - 0.5f) / (pathNodes.Count - 1));
    }

    private float GetNodeExitTime(TilePathNode node){
        int nodeIndex = pathNodes.IndexOf(node);
        return GetNodeExitTime(nodeIndex);
    }

    private bool IsInsideNodeAtTime(TilePathNode node, float turnTime){
        int nodeIndex = pathNodes.IndexOf(node);
        if(nodeIndex == -1) return false;
        return GetNodeExitTime(nodeIndex) > turnTime && turnTime > GetNodeEnterTime(nodeIndex);
    }

    public List<TileWithFacing> GetTilesInPath(){
        List<TileWithFacing> tiles = new List<TileWithFacing>();
        foreach(var node in pathNodes){
            tiles.Add(node.tile);
        }
        return tiles;
    }

    public TileWithFacing GetEndTile(){
        return pathNodes[pathNodes.Count - 1].tile;
    }

    public void AppendTile(TileWithFacing tile){
        pathNodes.Add(new TilePathNode(tile));
    }
}

