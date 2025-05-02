using Lofelt.NiceVibrations;
using Runtime.Haptic.Signal;
using Runtime.Identifiers;
using Runtime.Infrastructures.Template;
using Runtime.Models;
using Runtime.Signals;
using UniRx;

namespace Runtime.Panels.GameOverPanel
{
	public class GameOverPanelController : SignalListener
	{
		private readonly GameOverPanelMediator _mediator;
		private readonly IGameProgressModel _gameProgressModel;

		public GameOverPanelController(GameOverPanelMediator mediator, IGameProgressModel gameProgressModel)
		{
			_mediator = mediator;
			_gameProgressModel = gameProgressModel;
		}
		
		protected override void SubscribeToSignals()
		{
			_signalBus.GetStream<GameWinLoseSignal>()
				.Subscribe(GameWinLose)
				.AddTo(_disposables);
			
			_mediator.OnContinueButtonClicked += ContinueButtonClickedHandler;
		}
		
		private void ContinueButtonClickedHandler()
		{
			_signalBus.Fire(new VibrateSignal(HapticPatterns.PresetType.MediumImpact));
			
			
			
			_signalBus.Fire(new LoadSceneSignal(SceneID.GameScene));
			_mediator.DisableContinueButton();
		}

		private void GameWinLose(GameWinLoseSignal signal)
		{
			_mediator.ActivateGameOverPanel(signal.IsLevelSuccess, _gameProgressModel.Level);
			HapticPatterns.PresetType hapticPreset = signal.IsLevelSuccess ? HapticPatterns.PresetType.Success : HapticPatterns.PresetType.Failure;
			_signalBus.Fire(new VibrateSignal(hapticPreset));
		}

		public override void Dispose()
		{
			base.Dispose();
			_mediator.OnContinueButtonClicked -= ContinueButtonClickedHandler;
		}
	}
}