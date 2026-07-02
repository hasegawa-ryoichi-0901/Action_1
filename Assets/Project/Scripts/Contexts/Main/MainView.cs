using Cysharp.Threading.Tasks;
using UnityEngine;

public class MainView : MonoBehaviour {
    private IMainAction _mainAction;
    public async UniTask SetupAsync(IMainAction mainAction) {
        _mainAction = mainAction;
    }
}
