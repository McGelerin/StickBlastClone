using System;
using DG.Tweening;
using Zenject;

namespace Runtime.Panels.GameOverPanel
{
	public class GameOverPanelMediator : IInitializable, IDisposable
	{
		private readonly GameOverPanelView _view;
		public event Action OnContinueButtonClicked;
		
		public void Initialize()
		{
			_view.continueButton.onClick.AddListener(() => OnContinueButtonClicked?.Invoke());
		}

		public GameOverPanelMediator(GameOverPanelView view)
		{
			_view = view;
		}

		public void ActivateGameOverPanel(bool isGameWon, int levelIndex)
		{
			_view.gameObject.SetActive(true);

			SetVisuals(isGameWon, levelIndex);
		}

		private void SetVisuals(bool isGameWon, int levelIndex)
		{
			_view.gameObject.SetActive(true);
			_view.timeline.Play();
			
			_view.gameResultImage.sprite = isGameWon
				                               ? _view.gameWonSprite
				                               : _view.gameLostSprite;
				
			SetTexts(isGameWon, levelIndex);
			ChangeObjectActivation(isGameWon);
		}

		private void ChangeObjectActivation(bool isGameWon)
		{
			_view.gameWonSecondaryImage.SetActive(isGameWon);
			_view.gameLostSecondaryImage.SetActive(!isGameWon);
			_view.particle.SetActive(isGameWon);

		}

		private void SetTexts(bool isGameWon, int levelIndex)
		{
			string gameOverText = isGameWon ? "Completed!" : "Failed!";
			_view.gameResultText.SetText(gameOverText);
			
			_view.buttonText.text = isGameWon
				? "Continue"
				: "Try Again";
			
			string levelText = $"Level {levelIndex}";
			_view.wonLevelText.SetText(levelText);
			_view.lostLevelText.SetText(levelText);
		}

		public void Dispose()
		{
			_view.continueButton.onClick.RemoveAllListeners();
		}

		public void DisableContinueButton()
		{
			_view.continueButton.interactable = false;
		}
	}
}