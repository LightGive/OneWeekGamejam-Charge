using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneWeekGamejam.Charge
{
    public class SpriteFlusher : MonoBehaviour
    {
        private const string ShaderParam = "_Flush";

        [SerializeField] SpriteRenderer _spRenderer = null;
        [SerializeField] float _flushTime = 0.05f;
        [SerializeField] float _noFlushTime = 0.2f;
        Coroutine _flushCoroutine = null;

		public void ResetFlush()
		{
            if (_flushCoroutine != null)
            {
                StopCoroutine(_flushCoroutine);
            }
            _spRenderer.material.SetFloat(ShaderParam, 0.0f);
        }

        public void StartFlush(int _flushCount)
        {
            ResetFlush();
            _flushCoroutine = StartCoroutine(FlushCoroutine(_flushCount));
        }

        IEnumerator FlushCoroutine(int _flushCount)
        {
            for (var i = 0; i < _flushCount; i++)
            {
                _spRenderer.material.SetFloat(ShaderParam, 1.0f);
                yield return new WaitForSeconds(_flushTime);
                _spRenderer.material.SetFloat(ShaderParam, 0.0f);
                yield return new WaitForSeconds(_noFlushTime);
            }
            _flushCoroutine = null;
        }
    }
}