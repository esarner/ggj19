using UnityEngine;

namespace Assets.Scripts.Movement
{
   public class CharacterMovement
   {
      public Vector2 LookVector { get; private set; }

      private float _speed;
      private Vector2 _moveVelocity;
      private readonly Rigidbody2D _rigidbody2D;
      private readonly Animator _animator;
      private readonly IDirectionalInput _directionalInput;
      private static readonly int AnimationSpeed = Animator.StringToHash("moveSpeed");
      private static readonly int IsMoving = Animator.StringToHash("isMoving");

      public CharacterMovement(Rigidbody2D rigidbody2D, Animator animator, IDirectionalInput directionalInput, float speed)
      {
         _rigidbody2D = rigidbody2D;
         _animator = animator;
         _directionalInput = directionalInput;
         _moveVelocity = new Vector2(0, 0);

         SetSpeed(speed);
      }

      public void HandleMoveInput()
      {
         var input = _directionalInput.GetMovementInput();
         _moveVelocity = input * _speed;

         var isMoving = IsValidInput(input);
         _animator.SetBool(IsMoving, isMoving);

         if (isMoving)
         {
            _rigidbody2D.transform.up = _rigidbody2D.velocity.normalized;
         }
      }

      public void MoveCharacter()
      {
         if (_moveVelocity.sqrMagnitude > Vector2.kEpsilonNormalSqrt)
         {
            _rigidbody2D.velocity = _moveVelocity;
         }
         else
         {
            _rigidbody2D.velocity = Vector2.zero;
         }
      }

      public Transform GetTransform()
      {
         return _rigidbody2D.transform;
      }

      private static bool IsValidInput(Vector2 input)
      {
         return input != Vector2.zero;
      }

      public void SetSpeed(float speed)
      {
         _speed = speed;
         _animator.SetFloat(AnimationSpeed, _speed / 2);
      }
   }
}