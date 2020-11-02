using AppListaDeCompras.Model;
using AppListaDeCompras.ViewModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppListaDeCompras.ModelDB
{
    public class ProdutoBD
    {
        public int Count()
        {
            return ConexaoBD.Banco.Table<Produto>().Count();
        }

        public List<Produto> Get()
        {
            return ConexaoBD.Banco.Table<Produto>().ToList();
        }

        public List<ProdutoTotal> GetTotaisIdLista(long id)
        {
            return ConexaoBD.Banco.Query<ProdutoTotal>($"SELECT sum(Preco * Quantidade) as Valor,sum(Quantidade) as Qtd FROM [Produto] WHERE [IdLista] = {id}");
        }

        public List<Produto> GetId(int id)
        {
            return ConexaoBD.Banco.Query<Produto>($"SELECT * FROM [Produto] WHERE [Id] = {id}");
        }

        public List<Produto> GetIdLista(long id)
        {
            return ConexaoBD.Banco.Table<Produto>().Where(n => n.IdLista == id).ToList();
        }

        public int Salvar(Produto objeto)
        {
            if (objeto.Id != 0)
            {
                return Atualizar(objeto);
            }
            else
            {
                objeto.Data = DateTime.Now;
                return Inserir(objeto);
            }
        }
        public int Inserir(Produto objeto)
        {
            var retorno = ConexaoBD.Banco.Insert(objeto);
            objeto.Id = LastInsertId();
            return retorno;
        }

        public long LastInsertId()
        {
            string sql = @"select MAX(Id) from Produto";

            return ConexaoBD.ExecutarComando<long>(sql);
        }

        public int Inserir(List<Produto> objeto)
        {
            return ConexaoBD.Banco.InsertAll(objeto);
        }

        public int Atualizar(Produto objeto)
        {
            return ConexaoBD.Banco.Update(objeto);
        }

        public int Deletar(Produto objeto)
        {
            return ConexaoBD.Banco.Delete(objeto);
        }
    }
}
