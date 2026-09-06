
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IDebugViewModel {
    List<BaseDebugMenuWindow> ActiveWindows { get; }
    List<GameObject> ListViews { get; }
    UniTask SetupAsync();
}
