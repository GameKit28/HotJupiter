using System.Collections.Generic;

namespace HotJupiter{
    public class DynamicFootprint : FootprintBase {
        RelativeFootprintTemplate footprintTemplate;

        public DynamicFootprint(IHaveTileFootprint owner, RelativeFootprintTemplate template)
            : base(owner){
            footprintTemplate = template;
        }

        public void SetPivotTile(TileWithFacing pivotVec) {
            footprintParts = CalculateTilesFromTemplate(footprintTemplate, pivotVec);
            PlayfieldManager.RemoveAllReservations(footprintOwner);
            //if(!TryClaimFootprintTiles()) UnityEngine.Debug.LogWarning("Unable to claim footprint tiles.");
            base.FireFootprintUpdatedEvent();
        }
    }

    public class StaticFootprint : FootprintBase {
        public StaticFootprint(IHaveTileFootprint owner, List<FootprintTile> footprintParts)
            : base(owner){
            this.footprintParts = footprintParts;
            //if(!TryClaimFootprintTiles()) UnityEngine.Debug.LogWarning($"Owner {owner} unable to claim footprint tiles.");
        }

        public StaticFootprint(IHaveTileFootprint owner, RelativeFootprintTemplate footprint, TileWithFacing pivotVec)
            : base(owner) {
            footprintParts = CalculateTilesFromTemplate(footprint, pivotVec);
            //if(!TryClaimFootprintTiles()) UnityEngine.Debug.LogWarning($"Owner {owner} unable to claim footprint tiles.");
        }
    }

    public struct FootprintTile {
        public Tile tile;
        public TileObstacleType obstacleType;

        public FootprintTile(TileCoords position, TileLevel level, TileObstacleType obstacleType){
            tile = new Tile(position, level);
            this.obstacleType = obstacleType;
        }

        public override string ToString()
        {
            return $"FootprintTile - Tile:{tile}, Obstacle:{obstacleType}";
        }
    }

    public abstract class FootprintBase {

        protected List<FootprintTile> footprintParts = new List<FootprintTile>();

        protected IHaveTileFootprint footprintOwner;

        public event FootprintUpdatedDel FootprintUpdatedEvent;

        protected FootprintBase(IHaveTileFootprint footprintOwner){
            this.footprintOwner = footprintOwner;
        }

        public List<FootprintTile> GetAllTilesInFootprint(){
            return footprintParts;
        }

        protected void FireFootprintUpdatedEvent(){
            FootprintUpdatedEvent?.Invoke();
        }

        protected List<FootprintTile> CalculateTilesFromTemplate(RelativeFootprintTemplate footprint, TileWithFacing pivotVec){
            List<FootprintTile> footprintList = new List<FootprintTile>();
            foreach (var part in footprint.footprintParts)
            {
                TileWithFacing newVec = pivotVec;
                if(part.relativePosStep1.step > 0){
                    newVec = newVec.TraversePlanar(part.relativePosStep1.direction, part.relativePosStep1.step);
                }
                if(part.relativePosStep2.step > 0){
                    newVec = newVec.TraversePlanar(part.relativePosStep2.direction, part.relativePosStep2.step);
                }
                newVec.TraverseVertical(part.relativeLevel);
                footprintList.Add(new FootprintTile(newVec.position, newVec.level, part.obstacleType));
            }
            return footprintList;
        }

        /*protected bool TryClaimFootprintTiles(){
            bool canClaim = true;
            foreach(var part in footprintParts){
                if((int)PlayfieldManager.GetTileObstacleType(part.tile) > (int)TileObstacleType.Empty)
                    canClaim = false;
                    break;
            }

            if(canClaim){
                foreach(var part in footprintParts){
                    if(!PlayfieldManager.TryReserveTile(part, footprintOwner)){
                        UnityEngine.Debug.LogError($"Tile was occupied despite our initial check.");
                    };
                }
            }

            return canClaim;
        }*/
    }

    public delegate void FootprintUpdatedDel();
}