using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class DebugView : MonoBehaviour {
    [SerializeField] private Button _btnTest;
    [SerializeField] private Button _btnReload;
    [SerializeField] private ScrollRect _scrollView;
    public RectTransform ScrollViewContent => _scrollView.content;
    public async UniTask SetupAsync() {
        _btnTest?
            .OnClickAsObservable()
            .Subscribe(_ => {
                Debug.Log("Button clicked by name [Test].");
            })
            .AddTo(this);
        _btnReload?
            .OnClickAsObservable()
            .Subscribe(_ => {
                Debug.Log("Button clicked by name [Reload].");
                Main.Instance.ReloadAsync().Forget();
            })
            .AddTo(this);
    }
}
