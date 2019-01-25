using UnityEditor;
using UnityEngine;

namespace Assets.GridStuff.Brushes.Editor
{
   [CustomEditor(typeof(BuildingBrush))]
   public class BuildingBrushEditor : GridBrushEditorBase
   {
      //public override void PaintPreview(GridLayout grid, GameObject brushTarget, Vector3Int position)
      //{
      //   base.PaintPreview(grid, brushTarget, position);
      //}

      //public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
      //{
      //   base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);
      //}

      public override void RegisterUndo(GameObject layer, GridBrushBase.Tool tool)
      {
         Undo.RegisterCompleteObjectUndo(BuildingBrush.GetWall(), "Paint");
         Undo.RegisterCompleteObjectUndo(BuildingBrush.GetFloor(), "Paint");
      }
   }
}
