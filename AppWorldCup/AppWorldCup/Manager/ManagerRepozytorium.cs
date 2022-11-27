using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using AppWorldCup.Interfejsy;
using System.IO;

namespace AppWorldCup.Manager
{
    public class ManagerRepozytorium : IRepozytorium
    {
        private GitHubClient githubKlient;

        public ManagerRepozytorium(string nazwa, string token)
        {
            githubKlient = new GitHubClient(new ProductHeaderValue(nazwa));
            githubKlient.Credentials = new Credentials(token);
        }

        public async Task<User> PobierzUzytkownika(string nazwaUzytkownika) => await githubKlient.User.Get(nazwaUzytkownika);

        public async void AktualizujPlik(string owner, string nazwaRepo, string sciezkaPliku, string zawartoscPliku, string komentarz, string galaz)
        {
            var szczegolyPliku = await githubKlient.Repository.Content.GetAllContentsByRef(owner, nazwaRepo, sciezkaPliku, galaz);

            if (szczegolyPliku != null)
            {
                await githubKlient.Repository.Content.UpdateFile(owner, nazwaRepo, sciezkaPliku, new UpdateFileRequest(komentarz, zawartoscPliku, szczegolyPliku.First().Sha));
            }

            string sciezkaPlikuLokalna = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\", sciezkaPliku.Split('/').Last()));
            await File.WriteAllTextAsync(sciezkaPlikuLokalna, zawartoscPliku);
        }

        public async Task<string> PobierzZawartoscPliku(string owner, string nazwaRepo, string sciezkaPliku, string galaz)
        {
            var szczegolyPliku = await githubKlient.Repository.Content.GetAllContentsByRef(owner, nazwaRepo, sciezkaPliku, galaz);
            return szczegolyPliku.First().Content;
        }
    }
}
