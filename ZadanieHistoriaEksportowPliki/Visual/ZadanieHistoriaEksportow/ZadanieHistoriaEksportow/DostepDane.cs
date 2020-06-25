using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace ZadanieHistoriaEksportow
{
    class DostepDane
    {
        public string[] DajLokale()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Polaczenie.PolaczenieWartosc("ZadanieDB")))
            {
                return connection.Query<string>("select distinct NazwaLokalu from HistoriaEksportow").ToArray();
            }
        }

        public List<Eksport> DajEksporty(string lokal)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Polaczenie.PolaczenieWartosc("ZadanieDB")))
            {
                return connection.Query<Eksport>($"select * from HistoriaEksportow where NazwaLokalu = '{ lokal }'").ToList();
            }
        }

        public List<Eksport> DajEksporty(string lokal, string czasOd, string czasDo)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Polaczenie.PolaczenieWartosc("ZadanieDB")))
            {
                return connection.Query<Eksport>($"select * from HistoriaEksportow where NazwaLokalu = '{ lokal }'" +
                    $"and DataEksportu >= '{ czasOd }' and DataEksportu <= '{ czasDo }'").ToList();
            }
        }
    }
}
