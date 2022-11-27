using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightInject;
using AppWorldCup.Interfejsy;
using AppWorldCup.Widok;

namespace AppWorldCup.Manager
{
    public sealed class ManagerKlas
    {
        private static ManagerKlas managerKlas;
        private ServiceContainer kontener;
        private static string nazwa = "AppWorldCup";
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
