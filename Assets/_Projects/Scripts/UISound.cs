using System.Collections;
using System.Collections.Generic;
using KanKikuchi.AudioManager;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class UISound : MonoBehaviour
    {
        public void OnPointerEnterCursor()
		{
            SEManager.Instance.Play(SEPath.CURSOR);
		}

        public void OnPointerDownSound()
		{
            SEManager.Instance.Play(SEPath.UIENTER);
		}
    }
}