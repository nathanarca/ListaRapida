using Acr.UserDialogs;
using AppListaDeCompras.Model;
using AppListaDeCompras.ModelDB;
using AppListaDeCompras.ViewModel;
using AppListaDeCompras.Views;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppListaDeCompras
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //HOMOLOGACAO
            //if (Device.RuntimePlatform == Device.Android) adMobView.AdUnitId = "ca-app-pub-3940256099942544/6300978111";
            //PRODUÇÃO
            if (Device.RuntimePlatform == Device.Android) adMobView.AdUnitId = "ca-app-pub-5541916824987072/4023224854";

            ConexaoBD.AtualizarBanco();

            CarregarListas();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            try
            {
                if (e.NetworkAccess != NetworkAccess.Internet)
                {
                    linhaAdMob.Height = 0;
                }
                else
                {
                    linhaAdMob.Height = 50;
                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private async void ListViewInfoInicial_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {

                if (Application.Current.MainPage.Navigation.ModalStack.Count == 0)
                {
                    var linha = e.Item as ListaView;

                    var listaCompleta = new ListaCompleta().Get(linha.Id);

                    var pageEditarLista = new PageLista(listaCompleta);

                    pageEditarLista.Disappearing += (ss, ee) => { CarregarListas(); };

                    await Navigation.PushModalAsync(pageEditarLista, false);
                }


            }
            catch (Exception erro)
            {

                throw erro;
            }


        }

        private async void buttonMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                var acao = await DisplayActionSheet("Lista Rápida", "Cancelar", null, "Compartilhar App","Sobre");

                switch (acao)
                {
                    case "Compartilhar App":

                        var message = new ShareMessage
                        {
                            Url = "https://goo.gl/UGkzBL",
                            Title = "Lista Rápida",
                            Text = "Lista de compras, Material Escolar, Estoque Atual tudo em um só lugar."
                        };

                        await CrossShare.Current.Share(message);

                        break;
                    case "Sobre":

                        await Navigation.PushModalAsync(new PageSobre(), false);

                        break;
                }

            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        private async void buttonNovaLista_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                {
                    InputType = InputType.Name,
                    OkText = "Criar",
                    CancelText = "Cancelar",
                    Title = "Criar lista",
                });

                if (pResult.Ok && !string.IsNullOrWhiteSpace(pResult.Text))
                {
                    var lista = new Lista()
                    {
                        Data = DateTime.Now,
                        Descricao = pResult.Text
                    };

                    lista.Salvar();

                    var listaCompleta = new ListaCompleta()
                    {
                        Lista = lista,
                        Produtos = new List<Produto>()
                    };

                    var pageEditarLista = new PageLista(listaCompleta);

                    pageEditarLista.Disappearing += (ss, ee) => { CarregarListas(); };

                    await Navigation.PushModalAsync(pageEditarLista, false);

                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void CarregarListas()
        {
            try
            {
                var listas = new ListaView().Get();

                if (listas?.Count > 0)
                {
                    panelNenhumaLista.IsVisible = false;
                    panelGrid.IsVisible = true;

                    ListViewInfoInicial.ItemsSource = listas;

                }
                else
                {
                    panelNenhumaLista.IsVisible = true;
                    panelGrid.IsVisible = false;
                }


            }
            catch (Exception erro)
            {

                throw erro;
            }
        }
    }
}
