using System.Threading.Tasks;
using R3;
using ZLinq;

namespace KanKikuchi.AudioManager {

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//オーディオのキャッシュの種類
public enum AudioCacheType {
	None, All, Used
}

public static class AudioManager {
	public static float GetAudioLength(AudioPlayer player) {
		if (player == null || player.CurrentClip == null) {
			return 0;
		}

		return player.CurrentClip.length;
	}
}
/// <summary>
/// オーディオを管理するマネージャクラスの親クラス
/// </summary>
public abstract class AudioManager<T> : SingletonMonoBehaviour<T> where T : MonoBehaviourWithInit {
	
	//キャッシュの種類
	private AudioCacheType _cacheType;
	
	//キャッシュしているAudioClip
	private Dictionary<string, AudioClip> _audioClipDict = new Dictionary<string, AudioClip>();

	//実際にオーディオを再生するクラス
	protected readonly List<AudioPlayer> _audioPlayerList = new List<AudioPlayer>();

	//次に再生するプレイヤーの番号
	private int _nextAudioPlayerNo = 0;

	//AudioPlayerの数(同時再生可能数)
	protected abstract int _audioPlayerNum { get; }
	public int AudioPlayerNum => _audioPlayerNum;
	public bool IsMute = false;

	//ボリュームの基準と倍率
	protected float BaseVolume {
		get => this._baseVolume.Value;
		set => this._baseVolume.Value = value;
	}

	private ReactiveProperty<float> _baseVolume = new ReactiveProperty<float>(1.0f);
	public Observable<float> BaseVolumeAsObservable() => this._baseVolume.AsObservable();

	//=================================================================================
	//初期化、破棄
	//=================================================================================

	protected override void Init() {
		base.Init();

		//AudioSourceとAudioPlayer生成
		for (int i = 0; i < _audioPlayerNum; i++) {
			_audioPlayerList.Add(new AudioPlayer(gameObject.AddComponent<AudioSource>()));
		}
	}

	//指定したディレクトリにあるAudioClipをロードし、キャッシュ
	protected void LoadAudioClip(string directoryPath, AudioCacheType cacheType, bool isReleaseBGMCache) {
		_cacheType = cacheType;
		if (_cacheType == AudioCacheType.All) {
			_audioClipDict = Resources.LoadAll<AudioClip>(directoryPath).ToDictionary(clip => clip.name, clip => clip);
		}
		if (_cacheType == AudioCacheType.Used && isReleaseBGMCache) {
			SceneManager.sceneUnloaded += (scene) => {
				_audioClipDict.Clear();
			};
		}
	}

	public void ClearAudioCache() {
		this._audioClipDict.Clear();
	}

	//=================================================================================
	//更新
	//=================================================================================

	private void Update() {
		foreach (var audioPlayer in _audioPlayerList) {
			if (audioPlayer.CurrentState != AudioPlayer.State.Wait) {
				audioPlayer.Update();
			}
		}
	}

	//=================================================================================
	//設定、変更
	//=================================================================================

	/// <summary>
	/// ボリュームの基準を変更する(再生中のボリュームも変更する)
	/// </summary>
	public void ChangeBaseVolume(float baseVolume) {
		BaseVolume = baseVolume;
		_audioPlayerList
			.AsValueEnumerable()
			.Where(player => player.CurrentState != AudioPlayer.State.Wait).ToList()
			.ForEach(player => player.ChangeVolume(BaseVolume));
	}
	
	//=================================================================================
	//取得、判定
	//=================================================================================

	public string GetPlayingAudioName() {
		return _audioPlayerList
			.AsValueEnumerable()
			.FirstOrDefault(player => player.CurrentState == AudioPlayer.State.Playing)?.CurrentAudioName;
	}
	/// <summary>
	/// 再生中のオーディオの名前を全て取得
	/// </summary>
	public List<string> GetCurrentAudioNames() {
		return _audioPlayerList
			.AsValueEnumerable()
			.Where(player => player.CurrentState != AudioPlayer.State.Wait).Select(player => player.CurrentAudioName).ToList();
	}

	/// <summary>
	/// 再生しているものがあるか
	/// </summary>
	public bool IsPlaying() {
		return GetCurrentAudioNames().Count > 0;
	}
	
	//=================================================================================
	//再生開始
	//=================================================================================

	/// <summary>
	/// 再生開始
	/// </summary>
	protected AudioPlayer RunPlayer(AudioClip audioClip, float volumeRate, float delay, float pitch, bool isLoop, Action callback = null) {
		var player = GetNextAudioPlayer();
		player.Play(audioClip, BaseVolume, volumeRate, delay, pitch, isLoop, callback);
		return player;
	}
	
	/// <summary>
	/// 再生開始
	/// </summary>
	protected AudioPlayer RunPlayer(string audioPath, float volumeRate, float delay, float pitch, bool isLoop, Action callback = null) {
		return RunPlayer(GetAudioClip(audioPath), volumeRate, delay, pitch, isLoop, callback);
	}
	
	//オーディオのパスを名前に変換
	protected string PathToName(string audioPath) {
		return Path.GetFileNameWithoutExtension(audioPath);
	}

	//指定したパスのAudioClipを取得
	private AudioClip GetAudioClip(string audioPath) {
		string audioName = PathToName(audioPath);
		
		if (_audioClipDict.ContainsKey(audioName)) {
			return _audioClipDict[audioName];
		}

		var audioClip = Resources.Load<AudioClip>(audioPath);
		if (audioClip == null) {
			Debug.LogError(audioPath + " not found");
		}

		if (_cacheType == AudioCacheType.Used) {
			_audioClipDict[audioName] = audioClip;
		}

		return audioClip;
	}

	//次に再生するAudioPlayerを取得
	protected AudioPlayer GetNextAudioPlayer() {
		var audioPlayer = _audioPlayerList[_nextAudioPlayerNo];

		_nextAudioPlayerNo++;
		if (_nextAudioPlayerNo >= _audioPlayerList.Count) {
			_nextAudioPlayerNo = 0;
		}

		return audioPlayer;
	}

	//=================================================================================
	//再生終了
	//=================================================================================

	/// <summary>
	/// 指定されたパスまたは名前のものが再生されていたら停止
	/// </summary>
	public void Stop(string audioPathOrName) {
		var audioName = PathToName(audioPathOrName);
		_audioPlayerList.ForEach(player => player.Stop(audioName));
	}

	/// <summary>
	/// 全ての再生を停止する
	/// </summary>
	public void Stop() {
		_audioPlayerList.ForEach(player => player.Stop());
	}
	
	//=================================================================================
	//フェード
	//=================================================================================

	/// <summary>
	/// 指定されたパスまたは名前のものが再生されていたらフェードする
	/// </summary>
	public void Fade(string audioPathOrName, float duration, float from, float to, Action callback) {
		var audioName = PathToName(audioPathOrName);
		_audioPlayerList.ForEach(player => player.Fade(audioName, duration, from, to, callback));
	}
	
	/// <summary>
	/// 指定されたパスまたは名前のものが再生されていたらフェードアウトする
	/// </summary>
	public void FadeOut(string audioPathOrName, float duration = 1f, Action callback = null) {
		var audioName = PathToName(audioPathOrName);
		Fade(audioName, duration, 1, 0, callback);
	}
	
	/// <summary>
	/// 指定されたパスまたは名前のものが再生されていたらフェードインする
	/// </summary>
	public void FadeIn(string audioPathOrName, float duration = 1f, Action callback = null) {
		var audioName = PathToName(audioPathOrName);
		Fade(audioName, duration, 0, 1, callback);
	}

	public async Task FadeInAsync(string audioPathOrName, float duration = 1f) {
		var audioName = PathToName(audioPathOrName);
		var t = new TaskCompletionSource<AudioPlayer>();
		
		FadeIn(audioPathOrName, duration);
		await Task.Delay(TimeSpan.FromSeconds(duration));
	} 
	
	/// <summary>
	/// 再生しているものをフェードする
	/// </summary>
	public void Fade(float duration, float from, float to, Action callback) {
		_audioPlayerList.ForEach(player => player.Fade(duration, from, to, callback));
	}
	
	/// <summary>
	/// 再生しているものをフェードアウトする
	/// </summary>
	public void FadeOut(float duration = 1f, Action callback = null) {
		Fade(duration, 1, 0, callback);
	}
	
	/// <summary>
	/// 再生しているものをフェードインする
	/// </summary>
	public void FadeIn(float duration = 1f, Action callback = null) {
		Fade(duration, 0, 1, callback);
	}

	//=================================================================================
	//一時停止、再開
	//=================================================================================

	/// <summary>
	/// 指定されたパスまたは名前のものが再生されていたら一時停止
	/// </summary>
	public void Pause(string audioPathOrName) {
		var audioName = PathToName(audioPathOrName);
		_audioPlayerList.ForEach(player => player.Pause(audioName));
	}

	/// <summary>
	/// 全ての再生を一時停止
	/// </summary>
	public void Pause() {
		_audioPlayerList.ForEach(player => player.Pause());
	}

	/// <summary>
	/// 指定されたパスまたは名前のものが一時停止されていたら再開
	/// </summary>
	public void UnPause(string audioPathOrName) {
		var audioName = PathToName(audioPathOrName);
		_audioPlayerList.ForEach(player => player.UnPause(audioName));
	}

	/// <summary>
	/// 一時停止しているものを全て再開
	/// </summary>
	public void UnPause() {
		_audioPlayerList.ForEach(player => player.UnPause());
	}

	//=================================================================================
	//ミュート
	//=================================================================================

	/// <summary>
	/// ミュート
	/// </summary>
	public void Mute() {
		IsMute = true;
		_audioPlayerList.ForEach(player => player.Mute());
	}

	/// <summary>
	/// ミュート解除
	/// </summary>
	public void UnMute()
	{
		_audioPlayerList.ForEach(player => player.UnMute());
		IsMute = false;
	}

}
}