using Runtime.Infrastructures.Template;
using Runtime.Models;
using Runtime.Signals.Score;
using UniRx;

namespace Runtime.Panels.ScorePanel
{
    public class ScorePanelController : SignalListener
    {
        private readonly ScorePanelMediator _scorePanelMediator;
        private readonly IScoreModel _scoreModel;
        private readonly IGameProgressModel _gameProgressModel;
        
        public ScorePanelController(ScorePanelMediator scorePanelMediator, IScoreModel scoreModel, IGameProgressModel gameProgressModel)
        {
            _scorePanelMediator = scorePanelMediator;
            _scoreModel = scoreModel;
            _gameProgressModel = gameProgressModel;
        }

        private void OnUpdateScorePanel(UpdateScorePanelSignal signal)
        {
            _scorePanelMediator.UpdateScore(_scoreModel.Score , _gameProgressModel.RequirementScore);
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<UpdateScorePanelSignal>()
                .Subscribe(OnUpdateScorePanel)
                .AddTo(_disposables);
        }
    }
}