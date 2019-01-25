using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.GridStuff.Brushes
{

   [CreateAssetMenu]
   [CustomGridBrush(false, true, false, "Door")]
   public class DoorBrush : LayerObjectBrush<Door>
   {
      //public const string k_WallLayerName = "Walls";

      public TileBase m_Wall;
      //public TileBase m_Door;
      //private GridInformation _gridInformation;

      //void OnEnable()
      //{
      //   _gridInformation = BrushUtility.GetRootGridInformation();
      //}

      public override void Paint(GridLayout grid, GameObject layer, Vector3Int position)
      {
         BrushUtility.Select(BrushUtility.GetRootGrid().gameObject);

         Tilemap walls = GetTilemap();

         //var wall = walls != null 
         //   ? walls.GetTile<TerrainTile>(position)
         //   : null;

         var gridInformation = BrushUtility.GetRootGridInformation();
         var canHaveDoor = gridInformation.GetPositionProperty(position, GridInformation.CanHaveDoor, false);

         //if (wall != null && wall.CanHaveDoor(position))
         if (walls != null && canHaveDoor)
         {
            gridInformation.SetPositionProperty(position, GridInformation.HasDoor, true);
            var transformMatrix = walls.GetTransformMatrix(position);
            walls.SetTile(position, null);
            base.Paint(grid, layer, position);
            activeObject.transform.rotation = transformMatrix.rotation;
            //PaintInternal(position, walls);
         }
      }

      //private void PaintInternal(Vector3Int position, Tilemap walls)
      //{
      //   //var transformMatrix = walls.GetTransformMatrix(position);
      //   //walls.SetTile(position, null);
      //   walls.SetTile(position, m_Door);
      //   //walls.SetTransformMatrix(position, transformMatrix);
      //}

      public override void Erase(GridLayout grid, GameObject layer, Vector3Int position)
      {
         Tilemap walls = GetTilemap();

         var gridInformation = BrushUtility.GetRootGridInformation();
         var hasDoor = gridInformation.GetPositionProperty(position, GridInformation.HasDoor, false);
         var door = allObjects.SingleOrDefault(d => grid.WorldToCell(d.transform.position) == position);

         if (walls != null && hasDoor && door != null)
         {
            DestroyImmediate(door.gameObject);
            BrushUtility.Select(BrushUtility.GetRootGrid().gameObject);
            
            gridInformation.SetPositionProperty(position, GridInformation.HasDoor, false);
            walls.SetTile(position, m_Wall);
            //EraseInternal(position, walls);
         }
      }

      //private void EraseInternal(Vector3Int position, Tilemap walls)
      //{
      //   if (_gridInformation.GetPositionProperty(position, GridInformation.HasDoor, false))
      //   {
      //      _gridInformation.SetPositionProperty(position, GridInformation.HasDoor, false);
      //      walls.SetTile(position, m_Wall);
      //   }
      //}

      //public static Tilemap GetWall()
      //{
      //   GameObject go = GameObject.Find(k_WallLayerName);
      //   return go != null ? go.GetComponent<Tilemap>() : null;
      //}
   }
}