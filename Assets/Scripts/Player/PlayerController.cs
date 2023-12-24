using System;
using DefaultNamespace;
using DG.Tweening;
using RootMotion.Demos;
using RootMotion.Dynamics;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
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
        
        private void Start()
        {
            effectController.DisableRunTrail();
            effectController.SetStatusEffectSprint(false);
            effectController.SetStatusEffectJog(false);
        }

        private void OnEnable()
        {
            character.OnStartJump.AddListener(effectController.DisableRunTrail);
            character.OnCheckOnGround.AddListener(effectController.EnableRunTrail);

            EventGlobalManager.Instance.OnPlayerStartSprint.AddListener(StartSprint);
            EventGlobalManager.Instance.OnPlayerEndSprint.AddListener(EndSprint);
        }

        private void OnDisable()
        {
            character.OnStartJump.RemoveListener(effectController.DisableRunTrail);
            character.OnCheckOnGround.RemoveListener(effectController.EnableRunTrail);
            
            EventGlobalManager.Instance.OnPlayerStartSprint.RemoveListener(StartSprint);
            EventGlobalManager.Instance.OnPlayerEndSprint.RemoveListener(EndSprint);
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
        } 

        [Button]
        void ChangeLayer()
        {
            if (controller.layer == layerPlayer) controller.layer = layerCharacterController;
            else
            {
                controller.layer = layerPlayer;
            }
        }

        [Button]
        private void SetInvisible(bool invisible)
        {
            if (_isInvisible == invisible) return;

            this.DOKill();
            _isInvisible = invisible;
            
            if (!invisible)
            {
                var currentColor = bodyMaterial.color;
                DOVirtual.Color(currentColor, Color.white, changeAlphaDuration, x => bodyMaterial.color = x).OnComplete(
                    () =>
                    {
                        bodyMaterial.SetFloat("_RenderingMode", (float)RenderingMode.Opaque);
                    }).SetTarget(this);
                
                foreach (var material in clothesMaterial)
                {
                    var color = material.color;
                    DOVirtual.Float(color.a, 1f, changeAlphaDuration, x =>
                        {
                            color.a = x;
                            material.color = color;
                        }).OnComplete(() => material.SetFloat("_RenderingMode", (float)RenderingMode.Opaque))
                        .SetTarget(this);
                }
            }
            else
            {
                bodyMaterial.SetFloat("_RenderingMode", (float) RenderingMode.Transparent);
                var currentColor = bodyMaterial.color;
                DOVirtual.Color(currentColor, invisibleColorBody, changeAlphaDuration, x => bodyMaterial.color = x)
                    .SetTarget(this);
                
                foreach (var material in clothesMaterial)
                {
                    material.SetFloat("_RenderingMode", (float) RenderingMode.Transparent);
                    
                    var color = material.color;
                    DOVirtual.Float(color.a, alphaValue, changeAlphaDuration, x =>
                    {
                        color.a = x;
                        material.color = color;
                    }).SetTarget(this);
                }
            }
        }

        void SetInvisibleClothes(bool invisible)
        {
            if (invisible)
            {
                foreach (var material in clothesMaterial)
                {
                    material.SetFloat("_RenderingMode", (float) RenderingMode.Transparent);
                    
                    var color = material.color;
                    DOVirtual.Float(color.a, alphaValue, changeAlphaDuration, x =>
                    {
                        color.a = x;
                        material.color = color;
                    }).SetTarget(this);
                }
            }
            else
            {
                foreach (var material in clothesMaterial)
                {
                    var color = material.color;
                    DOVirtual.Float(color.a, 1f, changeAlphaDuration, x =>
                        {
                            color.a = x;
                            material.color = color;
                        }).OnComplete(() => material.SetFloat("_RenderingMode", (float)RenderingMode.Transparent))
                        .SetTarget(this);
                }
            }
            
            
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

        public void SetSpeed(float speed)
        {
            animController.AnimSpeedMultiplier = speed;
            _currentSpeed = speed;
        }

        #region Gameplay

        public void SetCharacterStatus(CharacterRunStatus runStatus) => _currentCharacterRunStatus = runStatus;
        
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
                _currentFitness -= _currentFitnessDecrease * Time.deltaTime;
                EventGlobalManager.Instance.OnUpdateFitness.Dispatch(_currentFitness/_maxFitness);
                
                if (_currentFitness <= 0f)
                {
                    EndSprint();
                }
            }
            else
            {
                if (_currentFitness <= _maxFitness)
                {
                    _currentFitness += _currentFitnessIncrease * Time.deltaTime;
                    EventGlobalManager.Instance.OnUpdateFitness.Dispatch(_currentFitness/_maxFitness);
                }
            }
        }

        void EndSprint()
        {
            ResetSpeed();
            _isSprint = false;
        }

        [Button]
        public void StartJog()
        {
            if (_currentCharacterRunStatus == CharacterRunStatus.Jog) return;
            _currentCharacterRunStatus = CharacterRunStatus.Jog;
            var targetSpeed = _initSpeed * (1f - sprintRate);
            DOVirtual.Float(_currentSpeed, targetSpeed, changeSpeedSprintDuration, x =>
            {
                SetSpeed(x);
            });
            
            effectController.SetStatusEffectSprint(false);
            effectController.SetStatusEffectJog(true);
        }

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