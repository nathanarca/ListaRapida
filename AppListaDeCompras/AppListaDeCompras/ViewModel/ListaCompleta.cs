using AppListaDeCompras.Model;
using AppListaDeCompras.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppListaDeCompras.ViewModel
{
    public class ListaCompleta
    {
        public Lista Lista { get; set; }
        public List<Produto> Produtos { get; set; }


        public ListaCompleta Get(long id)
        {
            try
            {
                var _lista = new ListaBD().GetIdLista(id);

                var _produtos = new ProdutoBD().GetIdLista(id);

                return new ListaCompleta()
                {
                    Lista = _lista,
                    Produtos = _produtos
                };

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

    }
}
