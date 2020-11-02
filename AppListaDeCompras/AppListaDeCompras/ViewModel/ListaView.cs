using AppListaDeCompras.Model;
using AppListaDeCompras.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppListaDeCompras.ViewModel
{
    public class ListaView : Lista
    {
        public decimal ValorTotal { get; set; }
        public decimal QtdTotal { get; set; }

        public List<ListaView> Get()
        {
            try
            {

                var listas = new ListaBD().Get();

                if(listas?.Count > 0)
                {

                    var retorno = new List<ListaView>();

                    foreach (var l in listas)
                    {

                        var totais = new ProdutoBD().GetTotaisIdLista(l.Id);

                        retorno.Add(new ListaView()
                        {
                            Data = l.Data,
                            Id = l.Id,
                            Descricao = l.Descricao,
                            QtdTotal = totais[0].Qtd,
                            ValorTotal = totais[0].Valor
                        });
                    }

                    return retorno;
                }

                return null;

            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

    }
}
