using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public static class PlayerInputHelper {
    /// <summary>
    /// PlayerInput用のイベント登録用のExtension
    /// </summary>
    /// <param name="playerInput">対象PlayerInput</param>
    /// <param name="actionName">ProjectSettings > Input Settings Package > Action Maps > Actionsの名前</param>
    /// <param name="callback">入力された値のCallback</param>
    /// <param name="condition">入力された値の条件</param>
    /// <typeparam name="T">入力されるstruct型, bool, float, Vector2など</typeparam>
    /// <returns>イベント登録解除用にActionを返す</returns>
    public static Action<InputAction.CallbackContext> RegisterInputAction<T>(
        this PlayerInput playerInput, string actionName, Action<T> callback, Func<InputAction.CallbackContext, bool> condition = null) where T : struct {
        var action = CreateInputAction<T>(actionName, callback, condition);
        playerInput.onActionTriggered += action;
        return action;
    }

    /// <summary>
    /// PlayerInput用のイベント作成Helper
    /// </summary>
    /// <param name="actionName">ProjectSettings > Input Settings Package > Action Maps > Actionsの名前</param>
    /// <param name="callback">入力された値のCallback</param>
    /// <param name="condition">入力された値の条件</param>
    /// <typeparam name="T">入力されるstruct型, bool, float, Vector2など</typeparam>
    /// <returns>イベント登録解除用にActionを返す</returns>
    public static Action<InputAction.CallbackContext> CreateInputAction<T>(string actionName, Action<T> callback, Func<InputAction.CallbackContext, bool> condition = null)
        where T : struct {

        return new Action<InputAction.CallbackContext>(context => {
            if (context.action.name != actionName) {
                return;
            }

            if (condition != null && !condition.Invoke(context)) {
                return;
            }
            var v = context.ReadValue<T>();
            callback?.Invoke(v);
        });

    }

    /// <summary>
    /// PlayerInputに登録した複数イベント解除
    /// </summary>
    /// <param name="playerInput"></param>
    /// <param name="actions"></param>
    /// <param name="shouldClear"></param>
    public static void UnregisterInputActions(this PlayerInput playerInput,
        List<Action<InputAction.CallbackContext>> actions, bool shouldClear = true) {
        foreach (var cb in actions) {
            playerInput.onActionTriggered -= cb;
        }

        if (shouldClear) {
            actions.Clear();
        }
    }
}