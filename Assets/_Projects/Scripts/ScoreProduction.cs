using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace OneWeekGamejam.Charge
{
    public class ScoreProduction : MonoBehaviour
    {
        [SerializeField] TextMeshPro _text = null;
        [SerializeField] Animation _anim = null;

        Coroutine _popupCoroutine = null;

        public void Popup(int score,Vector2 pos)
		{
			if (_anim.isPlaying)
			{
                _anim.Stop();
			}
            transform.position = new Vector3(pos.x, pos.y, -0.5f);
            _text.text = score.ToString("0");
            _anim.Play();

            if(_popupCoroutine != null)
			{
                StopCoroutine(_popupCoroutine);
			}
            _popupCoroutine = StartCoroutine(_Popup());
		}

        IEnumerator _Popup()
		{
            _text.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _text.gameObject.SetActive(false);
            _popupCoroutine = null;
		}
    }
}