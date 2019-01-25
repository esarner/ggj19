using System;
using Assets.GridStuff.Brushes;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.GridStuff.Tiles.Tile_Scripts
{
	[Serializable]
	public class TerrainTile : TileBase
	{
		[SerializeField]
		public Sprite[] m_Sprites;
      
	   private GridInformation _gridInformation;
	   private int _emptyTileIndex = 15;
	   private readonly Matrix4x4 _upTransform = Matrix4x4.identity;
	   private readonly Matrix4x4 _rightTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one);
	   private readonly Matrix4x4 _downTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -180f), Vector3.one);
	   private readonly Matrix4x4 _leftTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -270f), Vector3.one);

	   public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
	   {
	      _gridInformation = BrushUtility.GetRootGridInformation();
         return base.StartUp(location, tilemap, go);
	   }


      public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
		   //if (CurrentTileHasDoor(location) == false)
		   {
		      for (int yd = -1; yd <= 1; yd++)
		      for (int xd = -1; xd <= 1; xd++)
		      {
		         Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
		         if (TileValue(tileMap, position))
		            tileMap.RefreshTile(position);
		      }
		   }
		}

	   private bool CurrentTileHasDoor(Vector3Int location)
	   {
	      return _gridInformation.GetPositionProperty(location, GridInformation.HasDoor, false);
	   }

	   public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			UpdateTile(location, tileMap, ref tileData);
		}

		private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;

			var mask = GetMask(location, tileMap);

		   var tileInfo = GetTileInfo((byte)mask, location);
		   if (tileInfo.Index >= 0 && tileInfo.Index < m_Sprites.Length && TileValue(tileMap, location))
			{
				tileData.sprite = m_Sprites[tileInfo.Index];
			   tileData.transform = tileInfo.TileTransform;
				tileData.color = Color.white;
				tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
				tileData.colliderType = Tile.ColliderType.Sprite;
			}
		}

	   private int GetMask(Vector3Int location, ITilemap tileMap)
	   {
	      int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
	      mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;
	      mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;
	      mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;
	      mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;
	      mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;
	      mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;
	      mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;

	      byte original = (byte) mask;
	      if ((original | 254) < 255) { mask = mask & 125; }
	      if ((original | 251) < 255) { mask = mask & 245; }
	      if ((original | 239) < 255) { mask = mask & 215; }
	      if ((original | 191) < 255) { mask = mask & 95; }

	      return mask;
	   }

	   private bool TileValue(ITilemap tileMap, Vector3Int position)
		{
			TileBase tile = tileMap.GetTile(position);
			return (tile != null && tile == this);
		}

		private TileInfo GetTileInfo(byte mask, Vector3Int location)
		{
		   var tileInfo = new TileInfo
		   {
		      Index = -1,
		      TileTransform = GetTransform(mask)
		   };

		   Vector3Int neighouringTileLocation;

		   switch (mask)
		   {
		      case 0:
		         tileInfo.Index = 0;
		         break;
		      case 1:
		      case 4:
		      case 16:
		      case 64:
		         tileInfo.Index = 1;
		         break;
		      case 5:
		      case 20:
		      case 80:
		      case 65:
		         tileInfo.Index = 2;
		         break;
		      case 7:
		      case 28:
		      case 112:
		      case 193:
		         var quarterRotationAngle = Mathf.RoundToInt(tileInfo.TileTransform.rotation.eulerAngles.z - 90) % 270;
               var tileDataTransformRotated = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, quarterRotationAngle), Vector3.one);
		         neighouringTileLocation = GetLocalRotationDownTile(location, tileInfo.TileTransform);
		         var neighouringRotatedTileLocation = GetLocalRotationDownTile(location, tileDataTransformRotated);

               tileInfo.Index = HasDoor(neighouringTileLocation) || HasDoor(neighouringRotatedTileLocation) ? 14 : 3;
		         if (HasDoor(neighouringRotatedTileLocation))
		         {
		            var halfRotationAngle = Mathf.RoundToInt(tileDataTransformRotated.rotation.eulerAngles.z - 180) % 270;
                  var tileDataTransformRotated180 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, halfRotationAngle), Vector3.one);
                  tileInfo.TileTransform = Matrix4x4.TRS(Vector3.zero, tileDataTransformRotated180.rotation, new Vector3(1, -1, 1));
		         }
		         break;
		      case 17:
		      case 68:
		         tileInfo.Index = 4;
		         break;
		      case 21:
		      case 84:
		      case 81:
		      case 69:
		         tileInfo.Index = 5;
		         break;
		      case 23:
		      case 92:
		      case 113:
		      case 197:
		         tileInfo.Index = 6;
		         break;
		      case 29:
		      case 116:
		      case 209:
		      case 71:
		         tileInfo.Index = 7;
		         break;
		      case 31:
		      case 124:
		      case 241:
		      case 199:
		         neighouringTileLocation = GetLocalRotationLeftTile(location, tileInfo.TileTransform);
		         tileInfo.Index = HasDoor(neighouringTileLocation) ? _emptyTileIndex : 8;
		         break;
		      case 85:
		         tileInfo.Index = 9;
		         break;
		      case 87:
		      case 93:
		      case 117:
		      case 213:
		         tileInfo.Index = 10;
		         break;
		      case 95:
		      case 125:
		      case 245:
		      case 215:
		         tileInfo.Index = 11;
		         break;
		      case 119:
		      case 221:
		         tileInfo.Index = 12;
		         break;
		      case 127:
		      case 253:
		      case 247:
		      case 223:
		         neighouringTileLocation = GetLocalRotationUpLeftTile(location, tileInfo.TileTransform);
		         tileInfo.Index = HasDoor(neighouringTileLocation) ? _emptyTileIndex : 13;
		         break;
		      case 255:
		         tileInfo.Index = _emptyTileIndex;
		         break;
		   }

		   if (tileInfo.Index != _emptyTileIndex)
		   {
		      _gridInformation.SetPositionProperty(location, GridInformation.HasDoor, false);
		   }
		   _gridInformation.SetPositionProperty(location, GridInformation.CanHaveDoor, tileInfo.Index == 8);

         return tileInfo;
		}

	   private Vector3Int GetLocalRotationDownTile(Vector3Int location, Matrix4x4 tileDataTransform)
	   {
         var xDirection = 0;
	      var yDirection = 0;

	      if (tileDataTransform == _upTransform)
	      {
	         xDirection = 0;
	         yDirection = -1;
	      }
         else if (tileDataTransform == _rightTransform)
	      {
	         xDirection = -1;
	         yDirection = 0;
	      }
         else if (tileDataTransform == _downTransform)
	      {
	         xDirection = 0;
	         yDirection = 1;
	      }
         else if (tileDataTransform == _leftTransform)
	      {
	         xDirection = 1;
	         yDirection = 0;
	      }

         return new Vector3Int(location.x + xDirection, location.y + yDirection, 0);
	   }

	   private Vector3Int GetLocalRotationLeftTile(Vector3Int location, Matrix4x4 tileDataTransform)
	   {
         var xDirection = 0;
	      var yDirection = 0;

	      if (tileDataTransform == _upTransform)
	      {
	         xDirection = -1;
	         yDirection = 0;
	      }
         else if (tileDataTransform == _rightTransform)
	      {
	         xDirection = 0;
	         yDirection = 1;
	      }
         else if (tileDataTransform == _downTransform)
	      {
	         xDirection = 1;
	         yDirection = 0;
	      }
         else if (tileDataTransform == _leftTransform)
	      {
	         xDirection = 0;
	         yDirection = -1;
	      }

         return new Vector3Int(location.x + xDirection, location.y + yDirection, 0);
	   }

	   private Vector3Int GetLocalRotationUpLeftTile(Vector3Int location, Matrix4x4 tileDataTransform)
	   {
         var xDirection = 0;
	      var yDirection = 0;

	      if (tileDataTransform == _upTransform)
	      {
	         xDirection = -1;
	         yDirection = 1;
	      }
         else if (tileDataTransform == _rightTransform)
	      {
	         xDirection = 1;
	         yDirection = 1;
	      }
         else if (tileDataTransform == _downTransform)
	      {
	         xDirection = 1;
	         yDirection = -1;
	      }
         else if (tileDataTransform == _leftTransform)
	      {
	         xDirection = -1;
	         yDirection = -1;
	      }

         return new Vector3Int(location.x + xDirection, location.y + yDirection, 0);
	   }

	   private Matrix4x4 GetTransform(byte mask)
		{
			switch (mask)
			{
				case 4:
				case 20:
				case 28:
				case 68:
				case 84:
				case 92:
				case 116:
				case 124:
				case 93:
				case 125:
				case 221:
				case 253:
				   return _rightTransform;
				case 16:
				case 80:
				case 112:
				case 81:
				case 113:
				case 209:
				case 241:
				case 117:
				case 245:
				case 247:
					return _downTransform;
				case 64:
				case 65:
				case 193:
				case 69:
				case 197:
				case 71:
				case 199:
				case 213:
				case 215:
				case 223:
					return _leftTransform;
			}
			return _upTransform;
		}

      public bool HasDoor(Vector3Int position)
	   {
	      return _gridInformation.GetPositionProperty(position, GridInformation.HasDoor, false);
	   }

      public class TileInfo
	   {
	      public int Index { get; set; }
	      public Matrix4x4 TileTransform { get; set; }
	      //public bool CanHaveDoor { get; set; }
	   }

#if UNITY_EDITOR
		[MenuItem("Assets/Create/Terrain Tile")]
		public static void CreateTerrainTile()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Terrain Tile", "New Terrain Tile", "asset", "Save Terrain Tile", "Assets");

			if (path == "")
				return;

			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TerrainTile>(), path);
		}
#endif
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(TerrainTile))]
	public class TerrainTileEditor : Editor
	{
	   private int _numberOfTiles = 16;
	   private TerrainTile tile { get { return (target as TerrainTile); } }

		public void OnEnable()
		{
		   if (tile.m_Sprites == null || tile.m_Sprites.Length != _numberOfTiles)
			{
				tile.m_Sprites = new Sprite[_numberOfTiles];
				EditorUtility.SetDirty(tile);
			}
		}


		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField("Place sprites shown based on the contents of the sprite.");
			EditorGUILayout.Space();

			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 210;

			EditorGUI.BeginChangeCheck();
			tile.m_Sprites[0] = (Sprite) EditorGUILayout.ObjectField("Filled", tile.m_Sprites[0], typeof(Sprite), false, null);
			tile.m_Sprites[1] = (Sprite) EditorGUILayout.ObjectField("Three Sides", tile.m_Sprites[1], typeof(Sprite), false, null);
			tile.m_Sprites[2] = (Sprite) EditorGUILayout.ObjectField("Two Sides and One Corner", tile.m_Sprites[2], typeof(Sprite), false, null);
			tile.m_Sprites[3] = (Sprite) EditorGUILayout.ObjectField("Two Adjacent Sides", tile.m_Sprites[3], typeof(Sprite), false, null);
			tile.m_Sprites[4] = (Sprite) EditorGUILayout.ObjectField("Two Opposite Sides", tile.m_Sprites[4], typeof(Sprite), false, null);
			tile.m_Sprites[5] = (Sprite) EditorGUILayout.ObjectField("One Side and Two Corners", tile.m_Sprites[5], typeof(Sprite), false, null);
			tile.m_Sprites[6] = (Sprite) EditorGUILayout.ObjectField("One Side and One Lower Corner", tile.m_Sprites[6], typeof(Sprite), false, null);
			tile.m_Sprites[7] = (Sprite) EditorGUILayout.ObjectField("One Side and One Upper Corner", tile.m_Sprites[7], typeof(Sprite), false, null);
			tile.m_Sprites[8] = (Sprite) EditorGUILayout.ObjectField("One Side", tile.m_Sprites[8], typeof(Sprite), false, null);
			tile.m_Sprites[9] = (Sprite) EditorGUILayout.ObjectField("Four Corners", tile.m_Sprites[9], typeof(Sprite), false, null);
			tile.m_Sprites[10] = (Sprite) EditorGUILayout.ObjectField("Three Corners", tile.m_Sprites[10], typeof(Sprite), false, null);
			tile.m_Sprites[11] = (Sprite) EditorGUILayout.ObjectField("Two Adjacent Corners", tile.m_Sprites[11], typeof(Sprite), false, null);
			tile.m_Sprites[12] = (Sprite) EditorGUILayout.ObjectField("Two Opposite Corners", tile.m_Sprites[12], typeof(Sprite), false, null);
			tile.m_Sprites[13] = (Sprite) EditorGUILayout.ObjectField("One Corner", tile.m_Sprites[13], typeof(Sprite), false, null);
			tile.m_Sprites[14] = (Sprite) EditorGUILayout.ObjectField("One Side Closed", tile.m_Sprites[14], typeof(Sprite), false, null);
			tile.m_Sprites[15] = (Sprite) EditorGUILayout.ObjectField("Empty", tile.m_Sprites[15], typeof(Sprite), false, null);
			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(tile);

			EditorGUIUtility.labelWidth = oldLabelWidth;
		}
	}
#endif
}