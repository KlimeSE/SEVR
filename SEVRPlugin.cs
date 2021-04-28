using System;
using VRage.Plugins;
using HarmonyLib;
using VRage.GameServices;
using Sandbox.Game.Entities;
using VRage.Input;
using VRage.Utils;
using Sandbox.Game;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRageMath;
using Sandbox.Game.Entities.Character;

namespace SEVRPlugin
{
    public class SEVRPlugin : IPlugin, IDisposable
    {
        public void Dispose()
        {

        }

        public void Init(object gameInstance)
        {
            var harmony = new Harmony("SEVRPlugin");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(MyControllerHelper))]
        [HarmonyPatch("IsControl")]
        class Patch01
        {
            static void Postfix(MyStringId context, MyStringId stringId, ref bool __result, bool joystickOnly = false, bool useXinput = false,
                MyControlStateType type = MyControlStateType.NEW_PRESSED)
            {
                if (stringId == MyControlsSpace.LOOKAROUND && type == MyControlStateType.PRESSED)
                {
                    __result = true;
                }
            }
        }

        public void Update()
        {
            if (MySession.Static != null && MySession.Static.Ready)
            {
                if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity is MyShipController)
                {
                    var controller = MySession.Static.ControlledEntity as MyShipController;
                    bool something_pressed = false;
                    if (MyAPIGateway.Input.IsKeyPress(MyKeys.Left))
                    {
                        something_pressed = true;
                        controller.MoveAndRotate(Vector3.Zero, new Vector2(0f, -10f), 0f);
                    }

                    if (MyAPIGateway.Input.IsKeyPress(MyKeys.Right))
                    {
                        something_pressed = true;
                        controller.MoveAndRotate(Vector3.Zero, new Vector2(0f, 10f), 0f);
                    }

                    if (MyAPIGateway.Input.IsKeyPress(MyKeys.Up))
                    {
                        something_pressed = true;
                        controller.MoveAndRotate(Vector3.Zero, new Vector2(-10f, 0f), 0f);
                    }

                    if (MyAPIGateway.Input.IsKeyPress(MyKeys.Down))
                    {
                        something_pressed = true;
                        controller.MoveAndRotate(Vector3.Zero, new Vector2(10f, 0f), 0f);
                    }

                    if (something_pressed)
                    {
                        controller.MoveAndRotate();
                    }
                }
            }
        }
    }
}
