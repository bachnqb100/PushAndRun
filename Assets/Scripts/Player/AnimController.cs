using DefaultNamespace;
using DG.Tweening;
using RootMotion.Demos;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class AnimController : CharacterAnimationThirdPerson
    {
        [Space]
        [SerializeField] private float changeAnimLookDuration = 0.5f;

        [SerializeField] private float changeAnimPlayDuration = 1f;
        [Space] 
        [SerializeField] private float changeAnimDefeatDuration = 1f;
        [SerializeField] private float changeAnimVictoryDuration = 1f;

        [SerializeField] private SerializedDictionary<GameStatus, float> blendAnimByGameStatusMap;
        [SerializeField] private SerializedDictionary<DefeatReason, float> blendAnimByDefeatMap;
        [SerializeField] private SerializedDictionary<VictoryAnimType, float> blendAnimByVictoryMap;
        
        
        public void SetLookAt()
        {
            DOVirtual.Float(0f, 1f, changeAnimLookDuration, x => animator.SetFloat("Detect", x));
        }

        public void SetLookAround()
        {
            DOVirtual.Float(1f, 0f, changeAnimLookDuration, x => animator.SetFloat("Detect", x));
        }
        
        public void UpdateAnim()
        {
            DOVirtual.Float(animator.GetFloat("Play"), blendAnimByGameStatusMap[GameController.Instance.GameStatus],
                changeAnimPlayDuration, x => animator.SetFloat("Play", x));

            if (GameController.Instance.GameStatus == GameStatus.Defeat)
            {
                DOVirtual.Float(animator.GetFloat("Defeat"), blendAnimByDefeatMap[GameController.Instance.DefeatReason],
                    changeAnimDefeatDuration, x => animator.SetFloat("Defeat", x));
            }
            
            if (GameController.Instance.GameStatus == GameStatus.Victory)
            {
                DOVirtual.Float(animator.GetFloat("Victory"), blendAnimByVictoryMap.GetRandomElement().Value,
                    changeAnimVictoryDuration, x => animator.SetFloat("Victory", x));
            }
            
        }
    }
}