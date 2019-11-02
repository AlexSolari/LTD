using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Buttons
{
    public class ToggleBuildMenuButton : GeneralButton
    {
        public override void OnClick()
        {
            EventHandler.Current.Dispatch(EventHandler.Events.OnBuildMenuToggle);

            base.OnClick();
        }
    }
}
