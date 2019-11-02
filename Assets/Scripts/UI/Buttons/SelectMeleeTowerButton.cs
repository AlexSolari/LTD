using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Buttons
{
    public class SelectMeleeTowerButton : GeneralButton
    {
        private void Awake()
        {
            EventHandler.Current.SubscribeToEvent(EventHandler.Events.OnBuildMenuToggle, () => gameObject.SetActive(!gameObject.activeSelf));
            gameObject.SetActive(false);
        }

        public override void OnClick()
        {
            EventHandler.Current.Dispatch(EventHandler.Events.ChooseTower_Melee);

            base.OnClick();
        }
    }
}
