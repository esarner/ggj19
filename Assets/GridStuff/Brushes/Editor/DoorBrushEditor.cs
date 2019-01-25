using Assets.Scripts;
using UnityEditor;

namespace Assets.GridStuff.Brushes.Editor
{
   [CustomEditor(typeof(DoorBrush))]
   public class DoorBrushEditor : LayerObjectBrushEditor<Door>
   {
      //public override void PaintPreview(GridLayout grid, GameObject brushTarget, Vector3Int position)
      //{
      //   base.PaintPreview(grid, brushTarget, position);
      //}

      //public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
      //{
      //   base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);
      //}

      //public override void RegisterUndo(GameObject layer, GridBrushBase.Tool tool)
      //{
      //   Undo.RegisterCompleteObjectUndo(DoorBrush.GetWall(), "Paint");
      //}
   }
}
