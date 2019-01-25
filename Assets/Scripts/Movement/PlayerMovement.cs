using UnityEngine;

namespace Assets.Scripts.Movement
{
   public class PlayerMovement : MonoBehaviour
   {
      [SerializeField] private float _speed;

      public CharacterMovement CharacterMovement { get; private set; }

      void Awake()
      {
         CharacterMovement = new CharacterMovement(GetComponent<Rigidbody2D>(), GetComponent<Animator>(), new PlayerDirectionalInput(), _speed);
      }

      void Update()
      {
         CharacterMovement.HandleMoveInput();
      }

      void FixedUpdate()
      {
         CharacterMovement.MoveCharacter();
      }
   }
}
