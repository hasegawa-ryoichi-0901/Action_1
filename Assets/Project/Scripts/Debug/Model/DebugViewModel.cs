using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DebugViewModel : IDebugViewModel {
    public List<BaseDebugMenuWindow> ActiveWindows { get; set; }
    public List<GameObject> ListViews { get; set; }
    public async UniTask SetupAsync() {

    }
}
