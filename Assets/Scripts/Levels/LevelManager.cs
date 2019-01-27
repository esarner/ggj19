using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameHUD _gameHUD;
        [SerializeField] private List<DailyObjective> _dailyObjectives;
        [SerializeField] private Transform _playerStartPosition;
        [SerializeField] private Transform _player;
        [SerializeField] private int _dayLength;
        [SerializeField] private DropZone _dropZone;
        [SerializeField] private AudioClip _song;
        private AudioSource _audioSource;

        private float _dailyTimer;
        private float _endOfDayTime;
        private int _currentObjectiveIndex;

        private bool _isGameRunning = false;

        private void RestartSong()
        {
            _audioSource.clip = _song;
            _audioSource.Play();
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _dailyTimer = 0;
            _endOfDayTime = _dailyTimer + _dayLength * 5;
            _currentObjectiveIndex = -1;
        }

        private void Start()
        {
            LoadNextLevel();
        }

        private void Update()
        {
            _dailyTimer += Time.deltaTime;

            UpdateTimePiece();

            if (_dailyTimer >= _endOfDayTime)
            {
                StartCoroutine(EndOfDay());
            }
        }

        private void UpdateTimePiece()
        {
            if (_isGameRunning)
                _gameHUD.TimePiece.SetTime(_dailyTimer * 12f);
        }

        private void LoadNextLevel()
        {
            if (_currentObjectiveIndex < _dailyObjectives.Count - 1)
            {
                _currentObjectiveIndex += 1;

                StartCoroutine(LoadNextLevelRoutine());
                //DisplayDailyObjective

            }
            else
            {
                // Finished all levels!
            }
        }

        public IEnumerator LoadNextLevelRoutine()
        {
            var dailyObjective = _dailyObjectives.ElementAt(_currentObjectiveIndex);

            _gameHUD.SetMissionBriefing(dailyObjective.name, dailyObjective.ObjectiveDescription);
            _gameHUD.DisplayBriefing(true);
            _gameHUD.DisplayTimePiece(false);
            RestartSong();

            yield return new WaitForSecondsRealtime(4f);

            ResetPlayerStartPosition();
            ResetDailyTimer();
            _gameHUD.TimePiece.SetLimit(_dayLength);
            _gameHUD.DisplayBriefing(false);
            _gameHUD.DisplayTimePiece(true);
            _isGameRunning = true;
        }

        public void DisplayMissionBriefing()
        {

        }

        private void ResetPlayerStartPosition()
        {
            _player.position = _playerStartPosition.position;
        }

        private void ResetDailyTimer()
        {
            _dailyTimer = 0;
        }

        private IEnumerator EndOfDay()
        {
            var dailyObjective = _dailyObjectives.ElementAt(_currentObjectiveIndex);
            
            _isGameRunning = false;
            var points = CalculatePointsForObjective(dailyObjective);
            _gameHUD.SetScoreScreen(dailyObjective.ObjectiveDescription, points);
            _gameHUD.DisplayScore(true);
            _gameHUD.TimePiece.SetTime(60 * _dayLength);

            yield return new WaitForSecondsRealtime(3f);

            _gameHUD.DisplayScore(false);
            // Display end level screen
            LoadNextLevel();
        }

        private IEnumerable<(PickupType Type, int Points)> CalculatePointsForObjective(DailyObjective objective)
        {
            var objectsInDropZone = _dropZone.GetObjectsInDropZone();
            
            var allPickupClassifications = objectsInDropZone
                .SelectMany(o => o.Classifications);
            
            var pointsByType = allPickupClassifications
                .GroupBy(pc => pc.Type, pc => pc.Points)
                .Select(g => new {Type = g.Key, Points = g.Sum()});

            var totalPointsByType = pointsByType.Join(objective.PickupTypesWithMultiplier, arg => arg.Type, arg => arg.PickupType,
                (p, d) => (p.Type, p.Points * d.Multiplier));

            return totalPointsByType;
        }
    }
}