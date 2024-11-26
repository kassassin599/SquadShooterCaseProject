using UnityEngine;

namespace Watermelon
{
    [RegisterModule("Control Manager")]
    public class ControlInitModule : InitModule
    {
        public override string ModuleName => "Control Manager";

        [SerializeField] bool selectAutomatically = true;

        [HideIf("selectAutomatically")]
        [SerializeField] InputType inputType;

        [HideIf("IsJoystickCondition")]
        [SerializeField] GamepadData gamepadData;

        public override void CreateComponent()
        {
            if (selectAutomatically)
                inputType = ControlUtils.GetCurrentInputType();

            Control.Init(inputType, gamepadData);

            if(inputType == InputType.Keyboard)
            {
                KeyboardControl keyboardControl = Initializer.GameObject.AddComponent<KeyboardControl>();
                keyboardControl.Init();
            } 
            else if(inputType == InputType.Gamepad)
            {
                GamepadControl gamepadControl = Initializer.GameObject.AddComponent<GamepadControl>();
                gamepadControl.Init();
            }
        }

        private bool IsJoystickCondition()
        {
            return selectAutomatically ? ControlUtils.GetCurrentInputType() == InputType.UIJoystick : inputType == InputType.UIJoystick;
        }
    }
}