using AppListaDeCompras.ModelDB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppListaDeCompras.Model
{
    public class Lista
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }

        public void Salvar()
        {
            new ListaBD().Salvar(this);
        }

        internal int Apagar()
        {
            return  new ListaBD().Deletar(this);
        }
    }
}
