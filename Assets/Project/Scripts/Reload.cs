using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour {
    protected async UniTask Start() {
        Logger.Magenta("Reload -> Main...");

        await UniTask.DelayFrame(1);

        await Resources.UnloadUnusedAssets();

        await SceneManager.LoadSceneAsync(GameConstants.SceneMain);
        await UniTask.DelayFrame(1);
        await Resources.UnloadUnusedAssets();
        Logger.Magenta("Reload -> Main");
    }
}
