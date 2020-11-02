using AppListaDeCompras.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppListaDeCompras.ModelDB
{
    public static class ConexaoBD
    {
        public static readonly SQLiteConnection Banco = new SQLiteConnection(Configuracao.CaminhoBD);

        public static T ExecutarComando<T>(string comando)
        {
            string sql = @"select MAX(Id) from Lista";

            var command = new SQLiteCommand(Banco);

            command.CommandText = comando;

            return command.ExecuteScalar<T>();
        }

        public static void AtualizarBanco()
        {
            Banco.CreateTable<Produto>();
            Banco.CreateTable<Lista>();
        }
    }
}
