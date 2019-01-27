using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public enum GameState
    {
        StartScreen,
        MissionBriefing,
        Running,
        EndScreen
    }
    
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

        private GameState _gameState;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _dailyTimer = 0;
            _endOfDayTime = _dailyTimer + _dayLength * 5;
        }

        private void Start()
        {
            LoadStartScreen();
        }

        private void Update()
        {
            switch (_gameState)
            {
                case GameState.StartScreen:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        LoadNextLevel();
                    }
                    break;
                case GameState.MissionBriefing:
                    break;
                case GameState.Running:
                    break;
                case GameState.EndScreen:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (_gameState == GameState.Running)
            {
                _dailyTimer += Time.deltaTime;

                UpdateTimePiece();

                if (_dailyTimer >= _endOfDayTime)
                {
                    StartCoroutine(EndOfDay());
                }
            }
        }

        private void LoadStartScreen()
        {
            _gameState = GameState.StartScreen;
            _currentObjectiveIndex = -1;
            
            _gameHUD.DisplayStartScreen(true);
            _gameHUD.DisplayBriefing(false);
            _gameHUD.DisplayScore(false);
            _gameHUD.DisplayTimePiece(false);
        }

        private void UpdateTimePiece()
        {
            if (_gameState == GameState.Running)
                _gameHUD.TimePiece.SetTime(_dailyTimer * 12f);
        }

        private void LoadNextLevel()
        {
            if (_currentObjectiveIndex < _dailyObjectives.Count - 1)
            {
                _gameState = GameState.MissionBriefing;
                _currentObjectiveIndex += 1;

                StartCoroutine(LoadNextLevelRoutine());
            }
            else
            {
                // Finished all levels!
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public IEnumerator LoadNextLevelRoutine()
        {
            var dailyObjective = _dailyObjectives.ElementAt(_currentObjectiveIndex);

            _gameHUD.SetMissionBriefing(dailyObjective.name, dailyObjective.ObjectiveDescription);

            _gameHUD.DisplayStartScreen(false);
            _gameHUD.DisplayBriefing(true);
            _gameHUD.DisplayScore(false);
            _gameHUD.DisplayTimePiece(false);
            
            RestartSong();

            yield return new WaitForSecondsRealtime(4f);

            ResetPlayerStartPosition();
            ResetDailyTimer();
            _gameHUD.TimePiece.SetLimit(_dayLength);
            _gameHUD.DisplayBriefing(false);
            _gameHUD.DisplayTimePiece(true);
            _gameState = GameState.Running;
        }

        private void RestartSong()
        {
            _audioSource.clip = _song;
            _audioSource.Play();
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

            _gameState = GameState.EndScreen;
            var points = CalculatePointsForObjective(dailyObjective);
            _gameHUD.SetScoreScreen(dailyObjective.ObjectiveDescription, points);
            _gameHUD.DisplayScore(true);
            _gameHUD.TimePiece.SetTime(60 * _dayLength);

            yield return new WaitForSecondsRealtime(3f);

            _gameHUD.DisplayScore(false);
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