using UnityEngine;

namespace HexasphereGrid {

	partial class Hexasphere : MonoBehaviour {

		#region Events

		/* Event definitions */
		public delegate float PathFindingEvent(Hexasphere hexasphere, int toTileIndex, int fromTileIndex);
		public delegate void TileEvent(Hexasphere hexasphere, int tileIndex);
		public delegate void HexasphereEvent(Hexasphere hexasphere);

		/// <summary>
		/// Fired when path finding algorithmn evaluates a tile. Return the increased cost for tile.
		/// </summary>
		public event PathFindingEvent OnPathFindingCrossTile;

		/// <summary>
		/// Fired when path finding algorithmn evaluates a tile. Return the increased cost for tile.
		/// </summary>
		public event TileEvent OnTileClick;

		/// <summary>
		/// Fired when cursor is on a tile
		/// </summary>
		public event TileEvent OnTileMouseOver;

		/// <summary>
		/// Triggered when a FlyTo() operation starts
		/// </summary>
		public event HexasphereEvent OnFlyStart;

		/// <summary>
		/// Triggered when a FlyTo() operation reaches destination
		/// </summary>
		public event HexasphereEvent OnFlyEnd;

		/// <summary>
		/// Triggered when a drag gesture starts
		/// </summary>
		public event HexasphereEvent OnDragStart;

		/// <summary>
		/// Triggered when a drag gesture ends
		/// </summary>
		public event HexasphereEvent OnDragEnd;

		/// <summary>
		/// Triggered when a user zooms in/out
		/// </summary>
		public event HexasphereEvent OnZoom;

		/// <summary>
		/// Triggered when grid is regenerated
		/// </summary>
		public event HexasphereEvent OnGeneration;

		#endregion

	}

}