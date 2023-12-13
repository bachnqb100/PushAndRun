using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject controller;
        [SerializeField] private int layerPlayer;
        [SerializeField] private int layerCharacterController;

        [Title("Invisible")] 
        [Space]
        [SerializeField] private float changeAlphaDuration = 1f;
        
        [Header("Body")]
        [SerializeField] private Material bodyMaterial;
        [SerializeField] private Color invisibleColorBody = Color.gray;
        
        [Header("Clothes")]
        [SerializeField] private Material[] clothesMaterial;
        [SerializeField, Range(0f, 1f)] private float alphaValue = 0.6f;
        
        
        private bool _isInvisible;

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

        enum RenderingMode
        {
            Opaque = 0,
            Fade = 1,
            Transparent = 2,
        }
    }
} 