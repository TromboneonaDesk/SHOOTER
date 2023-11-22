using System;
using System.Globalization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class InputManager
{
    private static Controls _ctrls;

    private static Vector3 _mousePos;


    private static Camera cam;
    public static Ray GetCameraRay()
    {
        return cam.ScreenPointToRay(_mousePos);
    }

    public static void Init(Player p)
    {
        _ctrls = new();
        cam = Camera.main;
        _ctrls.Permenanet.Enable();

        _ctrls.InGame.Shoot.performed += _ =>
        {
            p.Shoot();
        };
        _ctrls.Permenanet.MousePos.performed += ctx =>
        {
            _mousePos = ctx.ReadValue<Vector2>();
        };

        _ctrls.InGame.SwitchWeapon.performed += ctx =>
        {
            string keyString = ctx.control.ToString();           
            p.SwitchWeapon(int.Parse(keyString.Substring(keyString.Length - 1)) - 1);
        };


        _ctrls.InGame.SwitchAmmo.performed += ctx =>
        {
            string keyString = ctx.control.ToString();
            p.SwitchAmmo(int.Parse(keyString.Substring(keyString.Length - 1)) - 5);
        };
    }

    public static void EnableInGame()
    {
        _ctrls.InGame.Enable();
    }
}
