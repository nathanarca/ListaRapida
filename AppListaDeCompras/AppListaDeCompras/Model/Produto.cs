using AppListaDeCompras.ModelDB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppListaDeCompras.Model
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long IdLista { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal Quantidade { get; set; }
        public bool Peso { get; set; }

        [Ignore]
        public decimal PrecoTotal { get { return Preco * Quantidade; } }

        [Ignore]
        public string QuantidadeFormatada
        {
            get
            {
                return Peso ? $"{Quantidade:N3}" : $"{Quantidade:N0}";
            }
        }

        public DateTime Data { get; set; }

        internal void Salvar()
        {
            new ProdutoBD().Salvar(this);
        }

        internal void Apagar()
        {
            new ProdutoBD().Deletar(this);
        }
    }
}
