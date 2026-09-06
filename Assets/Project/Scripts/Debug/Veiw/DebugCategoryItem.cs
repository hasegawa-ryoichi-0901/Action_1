using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class DebugCategoryItem : MonoBehaviour {
    [SerializeField] protected Button _button;
    [SerializeField] protected Text _text;

    protected DebugMenu.DebugMenuCategory _debugMenuCategory;
    public Observable<DebugMenu.DebugMenuCategory> OnClickAsObservable() => _button.OnClickAsObservable().Select(x => _debugMenuCategory);

    public async UniTask SetupAsync(DebugMenu.DebugMenuCategory debugMenuCategory) {
        _debugMenuCategory = debugMenuCategory;
        this.SetDisplayName();
    }

    private void SetDisplayName() {
        _text.text = _debugMenuCategory.DisplayName;
    }
}
