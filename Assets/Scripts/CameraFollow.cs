using UnityEngine;

namespace Assets.Scripts
{
   public class CameraFollow : MonoBehaviour
   {
      [SerializeField] private Transform _target;

      void LateUpdate()
      {
         if (_target)
         {
            transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
         }
      }
   }
}