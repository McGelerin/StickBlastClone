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
    }
}