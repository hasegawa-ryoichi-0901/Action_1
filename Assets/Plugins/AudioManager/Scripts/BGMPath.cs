using System.Collections.Generic;
namespace KanKikuchi.AudioManager{

/// <summary>
/// BGMファイルへのパスを定数で管理するクラス
/// </summary>
public static class BGMPath{

public enum BGMPathType{

NONE = -1,

BGM,

}
	public const string BGM = "BGM/bgm";
public static Dictionary<BGMPathType, string> Dic = new() {
{BGMPathType.BGM,"BGM/bgm"},
};

}

}
