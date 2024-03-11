using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ElosBlock.UI
{
    public class Text_ClearAnim : MonoBehaviour
    {
        private TextMeshProUGUI mText;

        private readonly string TextOne = "Single";
        private readonly string TextTwo = "Double";
        private readonly string TextThree = "Triplet";
        private readonly string TextFour = "Quadruple";
        private readonly string TextFive = "Quintuple";
        private readonly string TextSixOrMore = "Miraculous";

        private void Start()
        {
            mText = GetComponent<TextMeshProUGUI>();
            mText.enabled = false;
        }

        private void ResetText()
        {
            mText.enabled = true;
            mText.characterSpacing = 0;
            mText.alpha = 1;
        }

        public void TriggerAnim(int data)
        {
            mText.text = data switch
            {
                1 => TextOne,
                2 => TextTwo,
                3 => TextThree,
                4 => TextFour,
                5 => TextFive,
                _ => TextSixOrMore
            };

            ResetText();

            mText.DOKill();
            DOTween.To(() => mText.characterSpacing, x => mText.characterSpacing = x,
                    20, 2f)
                   .SetTarget(mText)
                   .SetEase(Ease.OutCirc);
            mText.DOFade(0, 1.2f)
                 .SetDelay(1.5f)
                 .OnComplete(() => mText.enabled = false);
        }
    }
}