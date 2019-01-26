using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<DailyObjective> _dailyObjectives;
        [SerializeField] private Transform _playerStartPosition;
        [SerializeField] private Transform _player;
        [SerializeField] private int _dayLength;
        [SerializeField] private DropZone _dropZone;

        private float _dailyTimer;
        private float _endOfDayTime;
        private int _currentObjectiveIndex;

        private void Awake()
        {
            _dailyTimer = 0;
            _endOfDayTime = _dailyTimer + _dayLength;
            _currentObjectiveIndex = -1;
        }

        private void Start()
        {
            LoadNextLevel();
        }

        private void Update()
        {
            _dailyTimer += Time.deltaTime;

            if (_dailyTimer >= _endOfDayTime)
            {
                EndOfDay();
            }
        }

        private void LoadNextLevel()
        {
            if (_currentObjectiveIndex < _dailyObjectives.Count - 1)
            {
                _currentObjectiveIndex += 1;
                //DisplayDailyObjective
                ResetPlayerStartPosition();
                ResetDailyTimer();
            }
            else
            {
                // Finished all levels!
            }
        }

        private void ResetPlayerStartPosition()
        {
            _player.position = _playerStartPosition.position;
        }

        private void ResetDailyTimer()
        {
            _dailyTimer = 0;
        }

        private void EndOfDay()
        {
            var points = CalculatePointsForCurrentDay();
            // Display end level screen
            LoadNextLevel();
        }

        private int CalculatePointsForCurrentDay()
        {
            var dailyObjective = _dailyObjectives.ElementAt(_currentObjectiveIndex);

            var totalpoints = 0;
            
            var objectsInDropZone = _dropZone.GetObjectsInDropZone();
            foreach (var pickup in objectsInDropZone)
            {
                totalpoints += pickup.Classifications
                    .Join(dailyObjective.PickupTypesWithMultiplier, arg => arg.Type, arg => arg.PickupType, (p, d) => p.Points * d.Multiplier)
                    .Sum();
            }

            return totalpoints;
        }
    }
}