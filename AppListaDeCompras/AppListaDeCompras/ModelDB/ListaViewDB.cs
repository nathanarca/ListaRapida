using AppListaDeCompras.Model;
using AppListaDeCompras.ViewModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppListaDeCompras.ModelDB
{
    class ListaViewBD
    {
        public List<ListaView> Get()
        {
            
            var sql = $"SELECT ";
            sql += " Lista.Id,";
            sql += " Lista.Descricao,";
            sql += " Lista.Data,";
            sql += " sum(Produto.Preco * Produto.Quantidade) as ValorTotal";
            //sql += " count(Produto.Id) as QtdTotal ";
            sql += " FROM Lista";
            sql += " INNER JOIN Produto on Produto.IdLista = Lista.Id";
            sql += " GROUP BY Lista.Id";

            return ConexaoBD.Banco.Query<ListaView>(sql);
        }

        
    }
}
