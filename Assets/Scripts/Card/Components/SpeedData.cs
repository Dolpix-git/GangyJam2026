using UnityEngine;

namespace CardGame.Data
{
    /// <summary>
    /// Pure data component for speed values.
    /// No behavior, only data storage.
    /// </summary>
    public class SpeedData : MonoBehaviour
    {
        [SerializeField] private int baseSpeed;
        [SerializeField] private int currentSpeed;

        public int BaseSpeed
        {
            get => baseSpeed;
            set => baseSpeed = value;
        }

        public int CurrentSpeed
        {
            get => currentSpeed;
            set => currentSpeed = value;
        }

        public void Initialize(int speed)
        {
            baseSpeed = speed;
            currentSpeed = speed;
        }
    }
}
