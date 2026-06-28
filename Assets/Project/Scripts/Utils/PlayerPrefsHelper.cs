using UnityEngine;

public static class PlayerPrefsHelper {
    public static void SetBool(string _key, bool _value) {
        PlayerPrefs.SetInt(_key, _value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetBool(string _key) {
        int value = PlayerPrefs.GetInt(_key, 1);
        return value == 1;
    }

    public static void Create() {   }

    public static void ResetTimeNotRecive() {   }

    public static void Delete() {
        PlayerPrefs.DeleteAll();
    }
}
