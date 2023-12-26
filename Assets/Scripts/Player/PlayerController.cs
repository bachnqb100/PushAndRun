using System;
using DefaultNamespace;
using DG.Tweening;
using RootMotion.Demos;
using RootMotion.Dynamics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IFallable
    {
        [Title("Player")]
        [SerializeField] private GameObject controller;
        [SerializeField] private int layerPlayer;
        [SerializeField] private int layerCharacterController;
        [SerializeField] private UserControlThirdPerson userControlThirdPerson;
        [SerializeField] private CharacterPuppet character;
        [SerializeField] private float delaySpawnPlayerDuration = 0.5f;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private PuppetMaster puppetMaster;
        [SerializeField] private AnimController animController;

        [Title("Invisible")] 
        [Space]
        [SerializeField] private float changeAlphaDuration = 1f;
        
        [Header("Body")]
        [SerializeField] private Material bodyMaterial;
        [SerializeField] private Color invisibleColorBody = Color.gray;
        
        [Header("Clothes")]
        [SerializeField] private Material[] clothesMaterial;
        [SerializeField, Range(0f, 1f)] private float alphaValue = 0.6f;
        
        [Title("Effects")]
        [SerializeField] private PlayerEffectController effectController;

        [Title("Gameplay")] 
        [SerializeField, Range(0f, 1f)] private float sprintRate = 0.2f;
        [SerializeField, Range(0f, 1f)] private float jogRate = 0.2f;
        [SerializeField] private float changeSpeedSprintDuration = 0.5f;
        
        [Space]
        [SerializeField] private float fitness = 5f;
        [SerializeField] private float fitnessDecreaseRate = 1f;
        
        
        
        private bool _isInvisible;
        private float _initSpeed;
        private float _currentSpeed;
        private CharacterRunStatus _currentCharacterRunStatus;

        private float _maxFitness;
        private float _currentFitness;
        private float _currentFitnessDecrease;
        private float _currentFitnessIncrease;
        private bool _isSprint;
        private bool _isJog;

        private bool _hasShield;
        private bool _hasInvisible;
        private bool _hasJog;
        
        private float CurrentFitness
        {
            get => _currentFitness;
            set
            {
                _currentFitness = value;
                _currentFitness = Mathf.Clamp(_currentFitness, 0f, _maxFitness);
                EventGlobalManager.Instance.OnUpdateFitness.Dispatch(_currentFitness/_maxFitness);
            }
        }
        
        private void Start()
        {
            effectController.DisableRunTrail();
            effectController.SetStatusEffectSprint(false);
            effectController.SetStatusEffectJog(false);
            
            effectController.DisableShield();
        }

        private void OnEnable()
        {
            character.OnStartJump.AddListener(effectController.DisableRunTrail);
            character.OnCheckOnGround.AddListener(effectController.EnableRunTrail);

            EventGlobalManager.Instance.OnPlayerStartSprint.AddListener(StartSprint);
            EventGlobalManager.Instance.OnPlayerEndSprint.AddListener(EndSprint);

            EventGlobalManager.Instance.OnPlayerCollectShield.AddListener(StartShield);

            EventGlobalManager.Instance.OnPlayerCollectRecoveryFitness.AddListener(RecoveryFitness);
            EventGlobalManager.Instance.OnPlayerCollectConsumeFitness.AddListener(ConsumeFitness);

            EventGlobalManager.Instance.OnPlayerExhausted.AddListener(StartJog);
        }

        private void OnDisable()
        {
            character.OnStartJump.RemoveListener(effectController.DisableRunTrail);
            character.OnCheckOnGround.RemoveListener(effectController.EnableRunTrail);
            
            EventGlobalManager.Instance.OnPlayerStartSprint.RemoveListener(StartSprint);
            EventGlobalManager.Instance.OnPlayerEndSprint.RemoveListener(EndSprint);
            
            EventGlobalManager.Instance.OnPlayerCollectShield.RemoveListener(StartShield);

            EventGlobalManager.Instance.OnPlayerCollectRecoveryFitness.RemoveListener(RecoveryFitness);
            EventGlobalManager.Instance.OnPlayerCollectConsumeFitness.RemoveListener(ConsumeFitness);
            
            EventGlobalManager.Instance.OnPlayerExhausted.RemoveListener(StartJog);

        }

        private void Update()
        {
            Sprint();
        }

        public void InitPlayer(Transform transform)
        {
            puppetMaster.SwitchToActiveMode();
            animController.UpdateAnim();
            userControlThirdPerson.enabled = false;
            character.enabled = false;

            DOVirtual.DelayedCall(delaySpawnPlayerDuration, () =>
            {
                this.transform.localPosition = transform.position + Vector3.up * 10;
                userControlThirdPerson.enabled = true;
                character.enabled = true;
                
                effectController.EnableRunTrail();
            });

            _initSpeed = GameManager.Instance.GameData.userData.currentSpeed;
            SetSpeed(_initSpeed);
            SetCharacterStatus(CharacterRunStatus.Normal);
            
            effectController.SetStatusEffectSprint(false);
            effectController.SetStatusEffectJog(false);
            _maxFitness = GameManager.Instance.GameData.userData.fitness;
            _currentFitness = _maxFitness;
            EventGlobalManager.Instance.OnUpdateFitness.Dispatch(1f);
            _currentFitnessDecrease = GameManager.Instance.GameData.userData.fitnessDecreaseRate;
            _currentFitnessIncrease = GameManager.Instance.GameData.userData.fitnessIncreaseRate;
            
            //sprint and jog
            _isSprint = false;
            _isJog = false;
            
            //invisible
            ChangeLayer(false);
            
            //shield
            _hasShield = false;
            EndShield();
        } 


        #region Defeat

        public void Fall()
        {
            //TODO: Logic player fall
            Debug.Log("Player falling");
            
            CameraManager.CameraController.Instance.EnableFallCamera();
                
            GameController.Instance.DefeatByPlayerFall();

            puppetMaster.state = PuppetMaster.State.Dead;
            
            animController.UpdateAnim();
            
            effectController.DisableRunTrail();
        }

        [Button]
        public void TimeOut()
        {
            //TODO: Logic timeout
            Debug.Log("Time out");
            
            animController.UpdateAnim();
            puppetMaster.SwitchToKinematicMode();
            
            effectController.DisableRunTrail();
        }
        
        [Button]
        public void Detected()
        {
            //TODO: Logic timeout
            Debug.Log("Time out");
            
            animController.UpdateAnim();
            puppetMaster.SwitchToKinematicMode();
            
            effectController.DisableRunTrail();
        }
        
        #endregion

        #region Victory

        public void Victory()
        {
            Debug.Log("Player victory");
            
            animController.UpdateAnim();
            
            puppetMaster.SwitchToKinematicMode();
            
            effectController.DisableRunTrail();
        }

        #endregion


        public void PlayerUpdateAnim()
        {
            animController.UpdateAnim();
        }

        public void PlayerOnlyUseAnimation()
        {
            puppetMaster.SwitchToKinematicMode();
        }
        
        public void ResetPlayer()
        {
            puppetMaster.state = PuppetMaster.State.Alive;
        }

        public void ResetPlayerMainScreen(Transform pos)
        {
            ResetPlayer();
            
            puppetMaster.SwitchToKinematicMode();
            userControlThirdPerson.enabled = false;
            character.enabled = false;
            

            DOVirtual.DelayedCall(delaySpawnPlayerDuration, () =>
            {
                transform.localPosition = pos.position;
            });

            _initSpeed = GameManager.Instance.GameData.userData.currentSpeed;
            SetSpeed(_initSpeed);
        }

        public void SetSpeed(float speed)
        {
            animController.AnimSpeedMultiplier = speed;
            _currentSpeed = speed;
        }

        #region Gameplay
       
        public void SetCharacterStatus(CharacterRunStatus runStatus) => _currentCharacterRunStatus = runStatus;

        
        #region Sprint

        [Button]
        public void StartSprint()
        {
            if (_currentCharacterRunStatus == CharacterRunStatus.Sprint) return;
            _currentCharacterRunStatus = CharacterRunStatus.Sprint;
            var targetSpeed = _initSpeed * (1f + sprintRate);
            DOVirtual.Float(_currentSpeed, targetSpeed, changeSpeedSprintDuration, x =>
            {
                SetSpeed(x);
            });
            
            effectController.SetStatusEffectSprint(true);
            effectController.SetStatusEffectJog(false);

            _isSprint = true;
        }

        void Sprint()
        {
            if (!GameController.Instance.IsPlaying) return;
            
            if (_isSprint)
            {
                ConsumeFitness(_currentFitnessDecrease * Time.deltaTime);
                
                if (_currentFitness <= 0f)
                {
                    EndSprint();
                }
            }
            else
            {
                if (_currentFitness <= _maxFitness && !_isJog)
                {
                    RecoveryFitness(_currentFitnessIncrease * Time.deltaTime);
                }
            }
        }

        void EndSprint()
        {
            ResetSpeed();
            _isSprint = false;
        }

        #endregion
        
        
        #region Item Invisible

        private Tween invisibleTween;
        
        [Button]
        void StartInvisible(float duration)
        {
            if (_hasInvisible)
                EndShield();
            
            _hasInvisible = true;
            
            ChangeLayer(true);
            SetInvisible(true);

            invisibleTween = DOVirtual.DelayedCall(duration, EndInvisible);

        }

        [Button]
        void EndInvisible()
        {
            if (_hasInvisible) invisibleTween.Kill();
            _hasInvisible = false;
            ChangeLayer(false);
            SetInvisible(false);
        }
        
        void ChangeLayer(bool invisible)
        {
            if (invisible) controller.layer = layerCharacterController;
            else
            {
                controller.layer = layerPlayer;
            }
        }
        
        private void SetInvisible(bool invisible)
        {
            if (_isInvisible == invisible) return;

            bodyMaterial.DOKill();
            _isInvisible = invisible;
            
            if (!invisible)
            {
                var currentColor = bodyMaterial.color;
                DOVirtual.Color(currentColor, Color.white, changeAlphaDuration, x => bodyMaterial.color = x).OnComplete(
                    () =>
                    {
                        bodyMaterial.SetFloat("_RenderingMode", (float)RenderingMode.Opaque);
                    }).SetTarget(bodyMaterial);
                
                foreach (var material in clothesMaterial)
                {
                    var color = material.color;
                    DOVirtual.Float(color.a, 1f, changeAlphaDuration, x =>
                        {
                            color.a = x;
                            material.color = color;
                        }).OnComplete(() => material.SetFloat("_RenderingMode", (float)RenderingMode.Opaque))
                        .SetTarget(bodyMaterial);
                }
            }
            else
            {
                bodyMaterial.SetFloat("_RenderingMode", (float) RenderingMode.Transparent);
                var currentColor = bodyMaterial.color;
                DOVirtual.Color(currentColor, invisibleColorBody, changeAlphaDuration, x => bodyMaterial.color = x)
                    .SetTarget(bodyMaterial);
                
                foreach (var material in clothesMaterial)
                {
                    material.SetFloat("_RenderingMode", (float) RenderingMode.Transparent);
                    
                    var color = material.color;
                    DOVirtual.Float(color.a, alphaValue, changeAlphaDuration, x =>
                    {
                        color.a = x;
                        material.color = color;
                    }).SetTarget(bodyMaterial);
                }
            }
        }

        #endregion


        #region Jog

        private Tween jogTween;

        [Button]
        public void StartJog(float duration)
        {
            effectController.StopEffectSprintJog();
            
            if (_currentCharacterRunStatus == CharacterRunStatus.Jog) return;
            _currentCharacterRunStatus = CharacterRunStatus.Jog;
            _isJog = true;
            var targetSpeed = _initSpeed * (1f - sprintRate);
            DOVirtual.Float(_currentSpeed, targetSpeed, changeSpeedSprintDuration, x =>
            {
                SetSpeed(x);
            });
            
            effectController.SetStatusEffectSprint(false);
            effectController.SetStatusEffectJog(true);

            DOVirtual.DelayedCall(duration, EndJog);
        }


        void EndJog()
        {
            _isJog = false;
            ResetSpeed();
        }

        
        #endregion


        #region Fitness

        void RecoveryFitness(float valueRecovery)
        {
            CurrentFitness += valueRecovery;
        }

        void ConsumeFitness(float valueConsume)
        {
            CurrentFitness -= valueConsume;
        }

        #endregion


        #region Shield

        private Tween shieldTween;
        
        [Button]
        void StartShield(float duration)
        {
            if (_hasShield)
            {
                EndShield();
            }

            _hasShield = true;
            effectController.EnableShield();
            puppetMaster.SwitchToKinematicMode();

            shieldTween = DOVirtual.DelayedCall(duration, EndShield);
        }

        void EndShield()
        {
            if (shieldTween != null) shieldTween.Kill();
            _hasShield = false;
            effectController.DisableShield();
            puppetMaster.SwitchToActiveMode();
        }

        #endregion
        
        [Button]
        public void ResetSpeed()
        {
            if (_currentCharacterRunStatus == CharacterRunStatus.Normal) return;
            _currentCharacterRunStatus = CharacterRunStatus.Normal;
            var targetSpeed = _initSpeed;
            DOVirtual.Float(_currentSpeed, targetSpeed, changeSpeedSprintDuration, x =>
            {
                SetSpeed(x);
            });
            
            effectController.SetStatusEffectSprint(false);
            effectController.SetStatusEffectJog(false);
        }

        #endregion

        enum RenderingMode
        {
            Opaque = 0,
            Fade = 1,
            Transparent = 2,
        }
    }
} 