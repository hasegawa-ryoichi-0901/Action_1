using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using ZLinq;

namespace KanKikuchi.AudioManager {
    using UnityEngine;

    /// <summary>
    /// BGM関連の管理をするクラス
    /// </summary>
    public class BGMManager : AudioManager<BGMManager> {
        public const float FADE_TIME = 1.5f;

        //AudioPlayerの数(同時再生可能数)
        protected override int _audioPlayerNum => AudioManagerSetting.Entity.BGMAudioPlayerNum;

        //再生に使ってるプレイヤークラス
        private AudioPlayer _audioPlayer => _audioPlayerList[0];

        //オーディオファイルが入ってるディレクトリへのパス
        public static readonly string AUDIO_DIRECTORY_PATH = "BGM";

        //=================================================================================
        //初期化
        //=================================================================================

        //起動時に実行される
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize() {
            if (AudioManagerSetting.Entity.IsAutoGenerateBGMManager) {
                Create();
            }
        }

        public static void Create() {
            new GameObject("BGMManager", typeof(BGMManager));
        }

        protected override void Init() {
            base.Init();
            var setting = AudioManagerSetting.Entity;

            LoadAudioClip(AUDIO_DIRECTORY_PATH, setting.BGMCacheType, setting.IsReleaseBGMCache);

            ChangeBaseVolume(setting.BGMBaseVolume);
            if (setting.BGMMute) {
                this.Mute();
            }
            else {
                this.UnMute();
            }

            if (!setting.IsDestroyBGMManager) {
                DontDestroyOnLoad(gameObject);
            }
        }

        //=================================================================================
        //再生
        //=================================================================================
        public AudioPlayer Play(BGMPath.BGMPathType bgmType, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = false, Action callback = null) {
            if (!BGMPath.Dic.TryGetValue(bgmType, out var v)) {
                return null;
            }
            return RunPlayer(v, volumeRate, delay, pitch, isLoop, callback);
        }

        /// <summary>
        /// 再生
        /// </summary>
        public void Play(AudioClip audioClip, float volumeRate = 1, float delay = 0, float pitch = 1,
            bool isLoop = true, bool allowsDuplicate = false, Action callback = null) {
            //重複が許可されてない場合は、既に再生しているものを止める
            if (!allowsDuplicate) {
                Stop();
            }

            RunPlayer(audioClip, volumeRate, delay, pitch, isLoop, callback);
        }

        /// <summary>
        /// 再生
        /// </summary>
        public void Play(string audioPath, float volumeRate = 1, float delay = 0, float pitch = 1, bool isLoop = true,
            bool allowsDuplicate = false, Action callback = null) {
            //重複が許可されてない場合は、既に再生しているものを止める
            if (!allowsDuplicate) {
                Stop();
            }

            RunPlayer(audioPath, volumeRate, delay, pitch, isLoop, callback);
        }

        public void PlayFade(AudioClip audioClip, bool isLoop = true, Action fadeCallback = null,
            Action callback = null) {
            var currentPlayers = this._audioPlayerList
                .AsValueEnumerable()
                .Where(x => x.CurrentState != AudioPlayer.State.Wait);
            foreach (var currentPlayer in currentPlayers) {
                currentPlayer.Fade(FADE_TIME, 1, 0);
            }

            var player = GetNextAudioPlayer();
            player.Stop();
            player.ChangeState(AudioPlayer.State.Fading);
            player.Fade(audioClip, BaseVolume, 1, 1, isLoop, FADE_TIME, 0, 1, callback, fadeCallback);
        }
    }
}