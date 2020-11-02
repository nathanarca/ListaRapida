using Acr.UserDialogs;
using AppListaDeCompras.ModelDB;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppListaDeCompras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLista : ContentPage
    {

        private ViewModel.ListaCompleta ListaAtual;
        private bool _editando { get; set; }
        private bool naoPesquisar { get; set; }
        public PageLista(ViewModel.ListaCompleta listaCompleta)
        {
            InitializeComponent();

            //HOMOLOGACAO
            //if (Device.RuntimePlatform == Device.Android) adMobView.AdUnitId = "ca-app-pub-3940256099942544/6300978111";

            //PRODUCAO
            if (Device.RuntimePlatform == Device.Android) adMobView.AdUnitId = "ca-app-pub-5541916824987072/8177768884";

            labelNomeLista.Text = listaCompleta.Lista.Descricao;

            ListaAtual = listaCompleta;

            if (ListaAtual?.Produtos?.Count > 0)
            {
                panelGrid.IsVisible = true;
                panelNenhumProduto.IsVisible = false;
                panelPesquisa.IsVisible = true;
                listViewProdutos.ItemsSource = ListaAtual.Produtos;

                labelValorTotal.Text = ListaAtual?.Produtos?.Sum(n => n.Quantidade * n.Preco).ToString("C");
            }

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

        public PageLista(long idLista)
        {
            InitializeComponent();

            ListaAtual = new ViewModel.ListaCompleta().Get(idLista);

            if (ListaAtual?.Produtos?.Count > 0)
            {
                panelGrid.IsVisible = true;
                panelNenhumProduto.IsVisible = false;
                panelPesquisa.IsVisible = true;
                listViewProdutos.ItemsSource = ListaAtual.Produtos;
            }
        }

        private async void listViewProdutos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {

                if (PopupNavigation.Instance.PopupStack.Count == 0)
                {

                    var pageValorBoleto = new PageProduto((Model.Produto)e.Item, ListaAtual.Lista.Id);

                    pageValorBoleto.Disappearing += (ss, ee) =>
                    {
                        RecarregarProdutos();
                    };

                    await PopupNavigation.Instance.PushAsync(pageValorBoleto);
                }

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private async void buttonIncluirProduto_Clicked(object sender, EventArgs e)
        {
            try
            {

                if (PopupNavigation.Instance.PopupStack.Count == 0)
                {
                    var prod = new Model.Produto();

                    prod.Descricao = textBoxBusca?.Text?.Trim();

                    var pageValorBoleto = new PageProduto(prod, ListaAtual.Lista.Id);

                    pageValorBoleto.Disappearing += (ss, ee) =>
                    {
                        RecarregarProdutos();
                        _editando = false;
                    };

                    await PopupNavigation.Instance.PushAsync(pageValorBoleto);

                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void RecarregarProdutos()
        {
            try
            {
                var listaDeProdutos = new ProdutoBD().GetIdLista(ListaAtual.Lista.Id);

                buttonNadaEncontrado.IsVisible = false;
                naoPesquisar = true;
                textBoxBusca.Text = string.Empty;

                ListaAtual.Produtos = listaDeProdutos;

                if (listaDeProdutos?.Count > 0)
                {
                    panelGrid.IsVisible = true;
                    panelNenhumProduto.IsVisible = false;
                    panelPesquisa.IsVisible = true;
                    listViewProdutos.ItemsSource = listaDeProdutos;
                    labelValorTotal.Text = listaDeProdutos.Sum(n => n.Quantidade * n.Preco).ToString("C");
                }
                else
                {
                    labelValorTotal.Text = "R$0,00";
                    panelGrid.IsVisible = false;
                    panelNenhumProduto.IsVisible = true;
                    panelPesquisa.IsVisible = false;

                }

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private async Task buttonOpcoes_ClickedAsync(object sender, EventArgs e)
        {
            try
            {

                string[] opcoesComProduto = { "Zerar a quantidade", "Zerar os preços", "Zerar quantidade e preços", "Apagar os produtos", "Renomear", "Copiar", "Apagar esta lista" };

                string[] opcoesSemProduto = { "Renomear", "Copiar", "Apagar esta lista" };

                string[] opcoes = (ListaAtual?.Produtos?.Count > 0) ? opcoesComProduto : opcoesSemProduto;

                var acao = await DisplayActionSheet(ListaAtual.Lista.Descricao, "Cancelar", null, opcoes);

                if (!string.IsNullOrEmpty(acao))
                {



                    if (acao == "Renomear")
                    {

                        var pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                        {
                            InputType = InputType.Name,
                            OkText = "Ok",
                            CancelText = "Cancelar",
                            Text = ListaAtual.Lista.Descricao,
                            Title = "Renomear lista"
                        });

                        if (pResult.Ok && !string.IsNullOrWhiteSpace(pResult.Text))
                        {
                            ListaAtual.Lista.Descricao = pResult.Text;
                            ListaAtual.Lista.Salvar();
                            labelNomeLista.Text = ListaAtual.Lista.Descricao;
                        }
                    }
                    else if (acao == "Copiar")
                    {
                        var pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                        {
                            InputType = InputType.Name,
                            OkText = "Ok",
                            CancelText = "Cancelar",
                            Text = $"{ListaAtual.Lista.Descricao} - Copia",
                            Title = "Copiar lista"
                        });

                        if (pResult.Ok && !string.IsNullOrWhiteSpace(pResult.Text))
                        {
                            ListaAtual.Lista.Descricao = pResult.Text;
                            ListaAtual.Lista.Data = DateTime.Now;
                            ListaAtual.Lista.Id = 0;
                            ListaAtual.Lista.Salvar();

                            foreach (var p in ListaAtual.Produtos)
                            {
                                p.IdLista = ListaAtual.Lista.Id;
                                p.Id = 0;
                                p.Salvar();
                            }

                            labelNomeLista.Text = ListaAtual.Lista.Descricao;

                        }


                    }
                    else
                    {

                        if (acao != "Cancelar")
                        {



                            var pergunta = acao == "Apagar esta lista" ? "Deseja realmente apagar esta lista?" : $"Deseja realmente {acao.ToLower()} desta lista?";

                            var sim = await DisplayAlert("Atenção!", pergunta, "Sim", "Não");

                            if (sim)
                            {

                                switch (acao)
                                {
                                    case "Zerar a quantidade":


                                        ZerarQuantidade();
                                        RecarregarProdutos();

                                        break;
                                    case "Zerar os preços":

                                        ZerarPrecos();
                                        RecarregarProdutos();

                                        break;
                                    case "Zerar quantidade e preços":

                                        ZerarQuantidade();
                                        ZerarPrecos();
                                        RecarregarProdutos();

                                        break;
                                    case "Apagar os produtos":

                                        ApagarProdutos();
                                        RecarregarProdutos();

                                        break;
                                    case "Apagar esta lista":

                                        ApagarProdutos();
                                        ListaAtual.Lista.Apagar();

                                        await Navigation.PopModalAsync(false);

                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void ApagarProdutos()
        {
            foreach (var p in ListaAtual?.Produtos)
            {
                p.Apagar();
            }
        }

        private void ZerarPrecos()
        {
            foreach (var p in ListaAtual?.Produtos)
            {
                p.Preco = 0;
                p.Salvar();
            }
        }

        private void ZerarQuantidade()
        {
            foreach (var p in ListaAtual?.Produtos)
            {
                p.Quantidade = 0;
                p.Salvar();
            }
        }

        private void textBoxBusca_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (naoPesquisar)
                {
                    naoPesquisar = false;

                    if (ListaAtual?.Produtos?.Count > 0)
                    {
                        listViewProdutos.ItemsSource = ListaAtual?.Produtos;
                        panelGrid.IsVisible = true;
                        buttonNadaEncontrado.IsVisible = false;
                    }
                    return;
                }

                var listaFiltrada = ListaAtual?.Produtos?.Where(n => (n?.Descricao?.ToLower()?.Contains(textBoxBusca.Text?.Trim()?.ToLower()) ?? false))?.ToList();

                if (listaFiltrada?.Count > 0)
                {
                    listViewProdutos.ItemsSource = listaFiltrada;
                    panelGrid.IsVisible = true;
                    buttonNadaEncontrado.IsVisible = false;
                    panelNenhumProduto.IsVisible = false;
                }
                else
                {
                    buttonNadaEncontrado.Text = $"Incluir \"{textBoxBusca.Text?.Trim()}\"";
                    buttonNadaEncontrado.IsVisible = true;
                    panelGrid.IsVisible = false;
                    buttonNadaEncontrado.IsEnabled = true;
                    panelNenhumProduto.IsVisible = false;

                }

            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void textBoxBusca_Completed(object sender, EventArgs e)
        {
            try
            {
                if (buttonNadaEncontrado.IsVisible)
                {
                    buttonIncluirProduto_Clicked(sender, e);
                }
                else
                {
                    listViewProdutos.Focus();
                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }
    }
}