using Runtime.Infrastructures.Template;
using Runtime.Models;
using Runtime.Signals;
using Runtime.Signals.Score;
using UniRx;

namespace Runtime.Score
{
    public class ScoreController : SignalListener
    {
        private readonly IScoreModel _scoreModel;

        public ScoreController(IScoreModel scoreModel)
        {
            _scoreModel = scoreModel;
        }

        private void OnIncreaseScore(IncreaseScoreSignal signal)
        {
            _scoreModel.IncreaseScore(signal.IncreaseScore);
            
            _signalBus.Fire(new UpdateScorePanelSignal());
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<IncreaseScoreSignal>()
                .Subscribe(OnIncreaseScore)
                .AddTo(_disposables);
        }
    }
}