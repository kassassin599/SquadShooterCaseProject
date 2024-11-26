using System;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [RequireComponent(typeof(UISettings))]
    public class GamepadUISettings : MonoBehaviour
    {
        private int selectedButtonId;
        private IGamepadButton SelectedButton => gamepadButtons[selectedButtonId];

        private List<IGamepadButton> gamepadButtons;

        private UISettings settingsUI;

        private void Awake()
        {
            settingsUI = GetComponent<UISettings>();

            UIController.PageOpened += OnPageOpened;
            UIController.PageClosed += OnPageClosed;
        }

        private void Start()
        {
            gamepadButtons = new List<IGamepadButton>();

            RectTransform contentTransform = settingsUI.ContentRectTransform;
            int childCount = contentTransform.childCount;

            for(int i = 0; i < childCount; i++)
            {
                Transform child = contentTransform.GetChild(i);
                if (!child.gameObject.activeSelf) continue;

                SettingsButtonBase settingsButtonBase = child.GetComponent<SettingsButtonBase>();
                if(settingsButtonBase != null)
                {
                    gamepadButtons.Add(new GamepadButton(settingsButtonBase));
                }
                else
                {
                    SettingsElementsGroup settingsElementsGroup = child.GetComponent<SettingsElementsGroup>();
                    if(settingsElementsGroup != null)
                    {
                        gamepadButtons.Add(new GamepadGroupButtons(settingsElementsGroup));
                    }
                }
            }
        }

        private void OnDestroy()
        {
            UIController.PageOpened -= OnPageOpened;
            UIController.PageClosed -= OnPageClosed;
        }

        private void OnPageOpened(UIPage page, Type pageType)
        {
            if(page == settingsUI)
            {
                if (Control.IsInitialized && Control.InputType == InputType.Gamepad)
                {
                    selectedButtonId = 0;
                    SelectedButton.Select();
                }

                UIGamepadButton.EnableTag(UIGamepadButtonTag.Settings);
            }
        }

        private void OnPageClosed(UIPage page, Type pageType)
        {
            if (page == settingsUI)
            {
                if (Control.IsInitialized && Control.InputType == InputType.Gamepad)
                {
                    SelectedButton.Deselect();
                    selectedButtonId = 0;
                }

                UIGamepadButton.DisableTag(UIGamepadButtonTag.Settings);
            }
        }

        private void Update()
        {
            if (!settingsUI.IsPageDisplayed) return;
            if (!Control.IsInitialized) return;

            if (Control.InputType == InputType.Gamepad)
            {
                if (GamepadControl.WasButtonPressedThisFrame(GamepadButtonType.DDown))
                {
                    if (selectedButtonId < gamepadButtons.Count - 1)
                    {
                        SelectedButton.Deselect();

                        selectedButtonId++;
                        SelectedButton.Select();
                    }
                }
                else if (GamepadControl.WasButtonPressedThisFrame(GamepadButtonType.DUp))
                {
                    if (selectedButtonId > 0)
                    {
                        SelectedButton.Deselect();

                        selectedButtonId--;
                        SelectedButton.Select();
                    }
                }
                else if (GamepadControl.WasButtonPressedThisFrame(GamepadButtonType.DLeft))
                {
                    SelectedButton.OnLeftButtonPressed();
                }
                else if (GamepadControl.WasButtonPressedThisFrame(GamepadButtonType.DRight))
                {
                    SelectedButton.OnRightButtonPressed();
                }
                else if (GamepadControl.WasButtonPressedThisFrame(GamepadButtonType.A))
                {
                    SelectedButton.OnClick();
                }
                else if (GamepadControl.WasButtonPressedThisFrame(GamepadButtonType.B))
                {
                    settingsUI.OnCloseButtonClicked();
                }
            }
        }

        private interface IGamepadButton
        {
            public void Select();
            public void Deselect();

            public void OnLeftButtonPressed();
            public void OnRightButtonPressed();

            public void OnClick();
        }

        private class GamepadButton : IGamepadButton
        {
            private SettingsButtonBase button;

            public GamepadButton(SettingsButtonBase button)
            {
                this.button = button;
            }

            public void Select()
            {
                button.Select();
            }

            public void Deselect()
            {
                button.Deselect();
            }

            public void OnLeftButtonPressed() { }
            public void OnRightButtonPressed() { }

            public void OnClick()
            {
                button.OnClick();
            }
        }

        private class GamepadGroupButtons : IGamepadButton
        {
            private SettingsButtonBase[] buttons;

            private int buttonIndex = 0;

            public GamepadGroupButtons(SettingsElementsGroup elementsGroup)
            {
                buttons = Array.FindAll(elementsGroup.GetComponentsInChildren<SettingsButtonBase>(), x => x.gameObject.activeSelf);
                buttonIndex = 0;
            }

            public void Select()
            {
                buttonIndex = 0;
                buttons[buttonIndex].Select();
            }

            public void Deselect()
            {
                buttons[buttonIndex].Deselect();
                buttonIndex = 0;
            }

            public void OnLeftButtonPressed()
            {
                int tempIndex = buttonIndex;

                tempIndex--;
                if (tempIndex < 0)
                    tempIndex = 0;

                if (buttonIndex != tempIndex)
                {
                    buttons[buttonIndex].Deselect();
                    buttons[tempIndex].Select();

                    buttonIndex = tempIndex;
                }
            }

            public void OnRightButtonPressed()
            {
                int tempIndex = buttonIndex;

                tempIndex++;
                if (tempIndex >= buttons.Length)
                    tempIndex = buttons.Length - 1;

                if (buttonIndex != tempIndex)
                {
                    buttons[buttonIndex].Deselect();
                    buttons[tempIndex].Select();

                    buttonIndex = tempIndex;
                }
            }

            public void OnClick()
            {
                buttons[buttonIndex].OnClick();
            }
        }
    }
}
