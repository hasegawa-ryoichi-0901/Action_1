using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugMenu", menuName = "Debug/DebugMenu", order = 0)]
[Serializable]
public class DebugMenu : ScriptableObject {

    [SerializeField] public List<DebugMenuCategory> DebugMenuCategories;

    [SerializeField] public DebugCategoryItem DebugCategoryItemPrefab;
    [Serializable]
    public class DebugMenuCategory {
        public enum EDebugMenuNodeType {
            Category,
            Command,
        }
        [SerializeField] public string NodeId = "None";
        [SerializeField] public string ParentNodeId = "None";
        [SerializeField] public string DisplayName = "None";
        [SerializeField] public EDebugMenuNodeType NodeType;
        [SerializeField] public BaseDebugMenuWindow DebugMenuWindowPrefab;

        public DebugMenuCategory() { }
    }
}

