using System.Collections.Generic;
namespace KanKikuchi.AudioManager{

/// <summary>
/// SEファイルへのパスを定数で管理するクラス
/// </summary>
public static class SEPath{

public enum SEPathType{

NONE = -1,

_00_BUTTON_N,
_01_BUTTON_Y,

}
	public const string _00_BUTTON_N = "SE/_00_button_n";
	public const string _01_BUTTON_Y = "SE/_01_button_y";
public static Dictionary<SEPathType, string> Dic = new() {
{SEPathType._00_BUTTON_N,"SE/_00_button_n"},
{SEPathType._01_BUTTON_Y,"SE/_01_button_y"},
};

}

}
