using AppListaDeCompras.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppListaDeCompras.ModelDB
{
    class ListaBD
    {
        public int Count()
        {
            return ConexaoBD.Banco.Table<Lista>().Count();
        }

        public List<Lista> Get()
        {
            return ConexaoBD.Banco.Table<Lista>().ToList();
        }

        public Lista GetIdLista(long id)
        {
            return ConexaoBD.Banco.Table<Lista>().FirstOrDefault(n => n.Id == id);
        }

        public Lista Salvar(Lista objeto)
        {
            if (objeto.Id != 0)
            {
                Atualizar(objeto);
            }
            else
            {
                Inserir(objeto);
            }

            return objeto;
        }
        public int Inserir(Lista objeto)
        {
            var retorno = ConexaoBD.Banco.Insert(objeto);

            objeto.Id = LastInsertId();

            return retorno;
        }

        public long LastInsertId()
        {
            string sql = @"select MAX(Id) from Lista";

            return ConexaoBD.ExecutarComando<long>(sql);
        }

        public int Inserir(List<Lista> objeto)
        {
            return ConexaoBD.Banco.InsertAll(objeto);
        }

        public int Atualizar(Lista objeto)
        {
            return ConexaoBD.Banco.Update(objeto);
        }

        public int Deletar(Lista objeto)
        {
            return ConexaoBD.Banco.Delete(objeto);
        }
    }
}
