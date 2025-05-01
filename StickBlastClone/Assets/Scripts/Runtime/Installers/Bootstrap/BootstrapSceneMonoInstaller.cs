using Runtime.BootStrap;
using Runtime.Infrastructures;
using Zenject;

namespace Runtime.Installers.Bootstrap
{
    public class BootstrapSceneMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ApplicationSettings>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BootstrapSceneInitializer>().AsSingle();
        }
    }
}