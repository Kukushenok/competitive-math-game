using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;
namespace Assets.Errors
{
    public class CoolUserMessageLogger : MonoBehaviour, IUserMessageLogger
    {
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Transform messageParent;
        public void Log(IUserMessageLogger.LogLevel level, string message)
        {
            Transform t = Instantiate(textPrefab, messageParent).transform;
            t.GetComponentInChildren<TextMeshProUGUI>().text = message;
            t.gameObject.SetActive(true);
            Sequence theSequence = DOTween.Sequence();
            t.transform.localScale = Vector3.zero;
            theSequence.Append(t.DOScale(1.0f, 0.5f))
                .Append(t.DOShakeScale(1.5f, 0.1f))
                .Append(t.DOScale(0.0f, 1.0f).SetEase(Ease.InCubic))
                .AppendCallback(() => Destroy(t.gameObject));
            theSequence.Play();
        }
    }
}
