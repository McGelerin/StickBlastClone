using Runtime.Panels.GameOverPanel;
using Runtime.Panels.Header;
using Runtime.Panels.Settings;
using Zenject;

namespace Runtime.Installers.GameAreaScene
{
    public class GameplayPanelsInstaller : MonoInstaller<GameplayPanelsInstaller>
    {
        public override void InstallBindings()
        {
            BindSettings();
            BindHeader();
            BindGameOverPanel();
        }
        
        private void BindHeader()
        {
            Container.BindInterfacesAndSelfTo<HeaderMediator>().AsSingle();
            Container.Bind<HeaderView>().FromComponentInHierarchy().AsSingle();
        }
        private void BindSettings()
        {
            Container.BindInterfacesAndSelfTo<SettingsController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SettingsMediator>().AsSingle();
            Container.Bind<SettingsView>().FromComponentInHierarchy().AsSingle();
        }
        
        private void BindGameOverPanel()
        {
            Container.BindInterfacesAndSelfTo<GameOverPanelController>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverPanelMediator>().AsSingle();
            Container.Bind<GameOverPanelView>().FromComponentInHierarchy().AsSingle();
        }
    }
}