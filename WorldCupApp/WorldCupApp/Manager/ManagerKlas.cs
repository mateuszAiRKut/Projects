using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;
using WorldCupApp.Interfejsy;
using WorldCupApp.Widok;

namespace WorldCupApp.Manager
{
    public sealed class ManagerKlas
    {
        private static ManagerKlas managerKlas;
        private ServiceContainer kontener;
        private static string nazwa = "WorldCupApp";
        private static string token = "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";

        private ManagerKlas()
        {
            kontener = new ServiceContainer();

            kontener.Register<IWyborKoloru, WyborKoloru>();
            kontener.Register<string, string, IRepozytorium>((factory, n, t) => new ManagerRepozytorium(n, t));
            kontener.Register<ISerwis, ManagerSerwis>();
        }

        public static ManagerKlas DajObiektManagera()
        {
            if (managerKlas == null)
            {
                managerKlas = new ManagerKlas();
            }

            return managerKlas;
        }

        public static IWyborKoloru InstancjaWyborKoloru => DajObiektManagera().kontener.GetInstance<IWyborKoloru>();

        public static IRepozytorium InstancjaManagerRepozytorium => DajObiektManagera().kontener.GetInstance<string, string, IRepozytorium>(nazwa, token);

        public static ISerwis InstancjaManagerSerwis => DajObiektManagera().kontener.GetInstance<ISerwis>();
    }
}
