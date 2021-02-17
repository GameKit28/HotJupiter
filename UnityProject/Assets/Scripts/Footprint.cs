using System.Collections.Generic;

public class DynamicFootprint : FootprintBase {
    RelativeFootprintTemplate footprintTemplate;

    public DynamicFootprint(RelativeFootprintTemplate template){
        footprintTemplate = template;
    }

    public void SetPivotTile(TileWithFacing pivotVec, int pivotLevel) {
        footprintParts = CalculateTilesFromTemplate(footprintTemplate, pivotVec.position, pivotVec.facing, pivotLevel);
        base.FireFootprintUpdatedEvent();
    }
}

public class StaticFootprint : FootprintBase {
    public StaticFootprint(List<TileWithLevel> footprintParts){
        this.footprintParts = footprintParts;
    }

    public StaticFootprint(RelativeFootprintTemplate footprint, TileCoords pivotTile, TileCoords pivotFacing, int pivotLevel) {
        footprintParts = CalculateTilesFromTemplate(footprint, pivotTile, pivotFacing, pivotLevel);
    }
}

public abstract class FootprintBase {

    protected List<TileWithLevel> footprintParts = new List<TileWithLevel>();

    public event FootprintUpdatedEvent FootprintUpdatedEvent;

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
}

public delegate void FootprintUpdatedEvent();