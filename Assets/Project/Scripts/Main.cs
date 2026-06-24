using Cysharp.Threading.Tasks;
using KanKikuchi.AudioManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Main : SingletonMonoBehaviour<Main> {
    protected override async UniTask Awake() {
        await base.Awake();
        InitializeAudioSettings();

#if true
        var dctx = await Resources.LoadAsync<GameObject>("Debug/#DebugContext");
        Instantiate(dctx);
        await UniTask.Yield();
#endif
        await ContextManager.Instance.SetupAsync();
    }

    private void InitializeAudioSettings() {
        var setting = AudioManagerSetting.Entity;
        BGMManager.Create();
        SEManager.Create();
    }

    public async UniTask ReloadAsync() {
        await UniTask.SwitchToMainThread();
        await ContextManager.ReloadAsync();
        SceneManager.LoadScene(GameConstants.SceneReload);
    }
}
