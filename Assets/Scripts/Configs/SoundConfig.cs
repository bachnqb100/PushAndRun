using System;
using System.Collections.Generic;
using DefaultNamespace.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Configs
{
    [CreateAssetMenu(menuName = "Configs/Sound Config", fileName = "Sound Config")]
    public class SoundConfig : ScriptableObject
    {
        private Dictionary<TypeSound, AudioClip> _dictAudioClip;

        [SerializeField]
        [TableList(DrawScrollView = false, ShowPaging = true, NumberOfItemsPerPage = 25, ShowIndexLabels = true)] //(AlwaysExpanded = true)]
        public List<Bgm> listBgm = new List<Bgm>();

        [SerializeField]
        [TableList(DrawScrollView = false, ShowPaging = true, NumberOfItemsPerPage = 25, ShowIndexLabels = true)] //(AlwaysExpanded = true)]
        public List<Sfx> listSfx = new List<Sfx>();

        [Title("Stats")]
        public float bgMusicVolume = 1f;
        public float sfxVolume = 1f;

        public void ResetCache()
        {
            if (!_dictAudioClip.CheckIsNullOrEmpty())
                _dictAudioClip.Clear();
        }

        public AudioClip GetAudio(TypeSound typeSound)
        {
            if (_dictAudioClip == null)
            {
                _dictAudioClip = new Dictionary<TypeSound, AudioClip>(listSfx.Count);
                for (var i = 0; i < listSfx.Count; i++)
                    _dictAudioClip.Add(listSfx[i].typeSound, listSfx[i].Audioclip);
            }

            if (_dictAudioClip.ContainsKey(typeSound))
                return _dictAudioClip[typeSound];
            return _dictAudioClip[TypeSound.Button];
        }

        [Title("SFX Add")]
        [Button(Expanded = true)]
        public void AddSfx(string nameSfx, AudioClip audioClip)
        {
            if (nameSfx == null || !audioClip)
            {
                BHDebug.LogError("Name audio clip or audio clip must be specified!!!");
                return;
            }
            
            if (!BHUtils.AddElementEnum<TypeSound>(nameSfx)) return;

            var newSfx = new Sfx();
            newSfx.typeSound =
                (TypeSound) Enum.GetValues(typeof(TypeSound)).Length;
            newSfx.Audioclip = audioClip;
            
            listSfx.Add(newSfx);
        }
    }
    
    
    [Serializable]
    public class Bgm
    {
        public string name;
        public AudioClip track;
    }
    
    
    [Serializable]
    public class Sfx
    {
        [SerializeField]
        [TableColumnWidth(150)]
        public TypeSound typeSound;
        
        [SerializeField]
        [TableColumnWidth(250, Resizable = false)]
        private AudioClip audioClip;

        public AudioClip Audioclip
        {
            get => audioClip;
            set => audioClip = value;
        }
    }
}

