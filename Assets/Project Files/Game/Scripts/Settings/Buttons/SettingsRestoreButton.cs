using UnityEngine.EventSystems;

namespace Watermelon
{
    public class SettingsRestoreButton : SettingsButtonBase
    {
        public override void Init()
        {
#if MODULE_MONETIZATION
            gameObject.SetActive(Monetization.IsActive);
#else
            gameObject.SetActive(false);
#endif
        }

        public override void OnClick()
        {
#if MODULE_MONETIZATION
            IAPManager.RestorePurchases();
#endif

            // Play button sound
            AudioController.PlaySound(AudioController.AudioClips.buttonSound);
        }

        public override void Select()
        {
            IsSelected = true;

            Button.Select();

            EventSystem.current.SetSelectedGameObject(null); //clear any previous selection (best practice)
            EventSystem.current.SetSelectedGameObject(Button.gameObject, new BaseEventData(EventSystem.current));
        }

        public override void Deselect()
        {
            IsSelected = false;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}