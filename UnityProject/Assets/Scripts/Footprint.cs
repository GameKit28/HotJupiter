using System.Collections.Generic;

public class DynamicFootprint : FootprintBase {
    RelativeFootprintTemplate footprintTemplate;

    public DynamicFootprint(IHaveTileFootprint owner, RelativeFootprintTemplate template)
        : base(owner){
        footprintTemplate = template;
    }

    public void SetPivotTile(TileWithFacing pivotVec, int pivotLevel) {
        footprintParts = CalculateTilesFromTemplate(footprintTemplate, pivotVec.position, pivotVec.facing, pivotLevel);
        PlayfieldManager.RemoveAllClaims(footprintOwner);
        if(!TryClaimFootprintTiles()) UnityEngine.Debug.LogWarning("Unable to claim footprint tiles.");
        base.FireFootprintUpdatedEvent();
    }
}

public class StaticFootprint : FootprintBase {
    public StaticFootprint(IHaveTileFootprint owner, List<TileWithLevel> footprintParts)
        : base(owner){
        this.footprintParts = footprintParts;
        if(!TryClaimFootprintTiles()) UnityEngine.Debug.LogWarning("Unable to claim footprint tiles.");
    }

    public StaticFootprint(IHaveTileFootprint owner, RelativeFootprintTemplate footprint, TileCoords pivotTile, TileCoords pivotFacing, int pivotLevel)
        : base(owner) {
        footprintParts = CalculateTilesFromTemplate(footprint, pivotTile, pivotFacing, pivotLevel);
        if(!TryClaimFootprintTiles()) UnityEngine.Debug.LogWarning("Unable to claim footprint tiles.");
    }
}

public abstract class FootprintBase {

    protected List<TileWithLevel> footprintParts = new List<TileWithLevel>();

    protected IHaveTileFootprint footprintOwner;

    public event FootprintUpdatedDel FootprintUpdatedEvent;

    protected FootprintBase(IHaveTileFootprint footprintOwner){
        this.footprintOwner = footprintOwner;
    }

    public List<TileWithLevel> GetAllTilesInFootprint(){
        return footprintParts;
    }

    protected void FireFootprintUpdatedEvent(){
        FootprintUpdatedEvent?.Invoke();
    }

    protected List<TileWithLevel> CalculateTilesFromTemplate(RelativeFootprintTemplate footprint, TileCoords pivotTile, TileCoords pivotFacing, int pivotLevel){
        List<TileWithLevel> footprintList = new List<TileWithLevel>();
        foreach (var part in footprint.footprintParts)
        {
            TileWithFacing newVec = new TileWithFacing(){position = pivotTile, facing = pivotFacing};
            if(part.relativePosStep1.step > 0){
                newVec = newVec.Traverse(part.relativePosStep1.direction, part.relativePosStep1.step);
            }
            if(part.relativePosStep2.step > 0){
                newVec = newVec.Traverse(part.relativePosStep2.direction, part.relativePosStep2.step);
            }
            footprintList.Add(new TileWithLevel(){position = newVec.position, level = pivotLevel + part.relativeLevel} );
        }
        return footprintList;
    }

    protected bool TryClaimFootprintTiles(){
        bool canClaim = true;
        foreach(var part in footprintParts){
            if((int)PlayfieldManager.GetTileObstacleType(part) > (int)TileObstacleType.Empty)
                canClaim = false;
                break;
        }

        if(canClaim){
            foreach(var part in footprintParts){
                if(!PlayfieldManager.TryStakeTileClaim(part, footprintOwner, TileObstacleType.Solid)){
                    UnityEngine.Debug.LogError($"Tile was occupied despite our initial check.");
                };
            }
        }

        return canClaim;
    }
}

public delegate void FootprintUpdatedDel();