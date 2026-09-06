using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DebugViewController {
    public DebugViewController(DebugMenu debugMenu) {
        _debugMenu = debugMenu;
    }
    protected DebugMenu _debugMenu { get; }
    public DebugMenu DebugMenu => IsValid();

    protected DebugView _debugView;

    private DebugMenu IsValid() {
        if (!_debugMenu) {
            Logger.Warning($"{DebugMenu.GetType().Name}が割り当てられていません");
        }
        return _debugMenu;
    }

    public async UniTask InitializeAsync() {

        await RegisterListViewAsync();
    }

    protected async UniTask RegisterListViewAsync() {
        var obj = new GameObject("CategoryView");
        obj.transform.SetParent(_debugView.ScrollViewContent);
        var layoutGroup = obj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.UpperLeft;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = false;
        layoutGroup.childForceExpandHeight = true;
        layoutGroup.childForceExpandWidth = true;
        var sizeFitter = obj.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        var item = Object.Instantiate(DebugMenu.DebugCategoryItemPrefab, obj.transform);
        await item.SetupAsync(_debugMenu.DebugMenuCategories.First());
    }

    protected async UniTask RegisterNodesAsync() {
        var obj = new GameObject("");
    }

    protected async UniTask RegisterNodeAsync(DebugMenu.DebugMenuCategory category) {
        var obj = new GameObject($"{category.DisplayName}");

    }
}
