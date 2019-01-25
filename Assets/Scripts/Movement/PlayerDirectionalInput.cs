using UnityEngine;

namespace Assets.Scripts.Movement
{
   public class PlayerDirectionalInput : IDirectionalInput
   {
      public Vector2 GetMovementInput()
      {
         var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
         
         return Vector2.ClampMagnitude(input, 1.0f);
      }
   }
}