using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class DebugView : MonoBehaviour {
    [SerializeField] private Button _btnTest;

    public async UniTask SetupAsync() {
        _btnTest
            .OnClickAsObservable()
            .Subscribe(_ => {
                Debug.Log("Button clicked by name [Test].");
            })
            .AddTo(this);
    }
}
