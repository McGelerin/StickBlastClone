using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Runtime.Panels.ScorePanel
{
    public class ScorePanelMediator
    {
        private readonly ScorePanelView _scorePanelView;

        public ScorePanelMediator(ScorePanelView scorePanelView)
        {
            _scorePanelView = scorePanelView;
        }

        public void UpdateScore(int currentScore , int requirementScore)
        {
            _scorePanelView.ScoreText.SetText(currentScore + "/" + requirementScore);

            float targetFill  = Mathf.Clamp((float)currentScore / requirementScore, 0f, 1f);
            
            _scorePanelView.FillImage.DOFillAmount(targetFill, 0.5f).SetEase(Ease.OutQuad); ;
        }
    }
}