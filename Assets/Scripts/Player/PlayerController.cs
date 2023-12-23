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
        
        
        private bool _isInvisible;

        private void Start()
        {
            effectController.DisableRunTrail();
        }

        private void OnEnable()
        {
            character.OnStartJump.AddListener(effectController.DisableRunTrail);
            character.OnCheckOnGround.AddListener(effectController.EnableRunTrail);
        }

        private void OnDisable()
        {
            character.OnStartJump.RemoveListener(effectController.DisableRunTrail);
            character.OnCheckOnGround.RemoveListener(effectController.EnableRunTrail);
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

        enum RenderingMode
        {
            Opaque = 0,
            Fade = 1,
            Transparent = 2,
        }
    }
} 