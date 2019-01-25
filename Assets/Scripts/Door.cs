using System;
using UnityEngine;

namespace Assets.Scripts
{
   public class Door : MonoBehaviour
   {
      [SerializeField] private Transform _leftDoor;
      [SerializeField] private Transform _rightDoor;

      private enum DoorState
      {
         Stopped,
         Opening,
         AwaitingClosing,
         Closing
      }

      private DoorState _state = DoorState.Stopped;
      private Vector3 _leftStartPosition;
      private Vector3 _leftEndPosition;
      private Vector3 _rightStartPosition;
      private Vector3 _rightEndPosition;
      private Vector3 _leftStartLocalPosition;
      private float _moveDistance = 0.5f;
      private float _lerpTime = 0.5f;
      private float _currentLerpTime = 0;
      private int _numberOfTransformsInVicinity = 0;
      private float _timeToCloseDoor = 0;
      private float _waitingTimeAfterLeavingVicinity = 2f;

      void Awake()
      {
         _leftStartPosition = _leftDoor.position;
         _leftEndPosition = _leftStartPosition + _leftDoor.up * _moveDistance;
         _rightStartPosition = _rightDoor.position;
         _rightEndPosition = _rightStartPosition - _rightDoor.up * _moveDistance;
         _leftStartLocalPosition = _leftDoor.localPosition;
      }

      void Update()
      {
         switch (_state)
         {
            case DoorState.Stopped:
               break;
            case DoorState.Opening:
               OpenDoor();
               break;
            case DoorState.AwaitingClosing:
               AwaitClosing();
               break;
            case DoorState.Closing:
               CloseDoor();
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }

      private void OpenDoor()
      {
         var lerp = UpdateLerp();
         _leftDoor.position = Vector3.Lerp(_leftStartPosition, _leftEndPosition, lerp);
         _rightDoor.position = Vector3.Lerp(_rightStartPosition, _rightEndPosition, lerp);
         
         if (_leftDoor.position == _leftEndPosition && _numberOfTransformsInVicinity == 0)
         {
            _state = DoorState.AwaitingClosing;
         }
      }

      private void AwaitClosing()
      {
         if (Time.timeSinceLevelLoad > _timeToCloseDoor)
         {
            _currentLerpTime = 0;
            _state = DoorState.Closing;
         }
      }

      private void CloseDoor()
      {
         var lerp = UpdateLerp();
         _leftDoor.position = Vector3.Lerp(_leftEndPosition, _leftStartPosition, lerp);
         _rightDoor.position = Vector3.Lerp(_rightEndPosition, _rightStartPosition, lerp);
      }

      private float UpdateLerp()
      {
         _currentLerpTime += Time.deltaTime;
         if (_currentLerpTime > _lerpTime)
         {
            _currentLerpTime = _lerpTime;
         }

         var t = _currentLerpTime / _lerpTime;

         //return t * t * (3f - 2f * t); // smoothstep
         return t * t * t * (t * (6f * t - 15f) + 10f); // smootherstep
      }

      void OnTriggerEnter2D(Collider2D other)
      {
         _numberOfTransformsInVicinity++;
         if (_state != DoorState.Opening)
         {
            _currentLerpTime = (_leftDoor.localPosition.y - _leftStartLocalPosition.y) / _moveDistance;
            _state = DoorState.Opening;
         }
      }

      void OnTriggerExit2D(Collider2D other)
      {
         _numberOfTransformsInVicinity = Mathf.Max(_numberOfTransformsInVicinity - 1, 0);

         if (_numberOfTransformsInVicinity == 0)
         {
            _timeToCloseDoor = Time.timeSinceLevelLoad + _waitingTimeAfterLeavingVicinity;
            if (_state != DoorState.Opening)
            {
               _state = DoorState.AwaitingClosing;
            }
         }
      }
   }
}