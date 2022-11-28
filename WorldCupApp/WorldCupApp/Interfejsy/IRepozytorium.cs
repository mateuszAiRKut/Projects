using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace WorldCupApp.Interfejsy
{
    public interface IRepozytorium
    {
        Task<User> PobierzUzytkownika(string nazwaUzytkownika);
        Task<string> PobierzZawartoscPliku(string owner, string nazwaRepo, string sciezkaPliku, string galaz);
        void AktualizujPlik(string owner, string nazwaRepo, string sciezkaPliku, string zawartoscPliku, string komentarz, string galaz);
    }
}
