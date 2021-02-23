using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseBrain<TGamePiece> : MonoBehaviour
{
    public TGamePiece myGamePiece;

    public PieceController pieceController;

    public BaseGamePiece currentTarget;

    public virtual BaseGamePiece FindTarget() {
        return null;
    }

    public virtual CommandPointController SelectCommand(){
        return null;
    }
}
