using System;
using System.IO;

namespace AppListaDeCompras.Model
{
    public static class Configuracao
    {
        /// <summary>
        /// Caminho do banco de dados
        /// </summary>
        public static string CaminhoBD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ListaDeCompras.db3");


    }
}
