using AppListaDeCompras.Model;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppListaDeCompras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageProduto : Rg.Plugins.Popup.Pages.PopupPage
    {
        Produto _prod;
        private bool ajustandoNumero;
        private bool carregando = true;
        private bool Peso;
        public PageProduto(Produto prod, long idLista)
        {
            InitializeComponent();

            _prod = prod;

            CarregarTela(prod, idLista);

            carregando = false;
        }

        private void CarregarTela(Produto prod, long idLista)
        {
            try
            {
                panelApagar.IsVisible = (prod.Id != 0);
                
                textBoxDescricao.Text = $"{prod.Descricao}";

                ajustandoNumero = true;
                textBoxValor.Text = $"{prod.Preco:N2}";

                ajustandoNumero = true;
                textBoxQuantidade.Text = $"{prod.QuantidadeFormatada}";

                Peso = prod.Peso;
                AplicarEstiloUnidade();

                buttonLimparValor.IsVisible = !string.IsNullOrEmpty(textBoxValor.Text.Trim());
                buttonLimparQtd.IsVisible = !string.IsNullOrEmpty(textBoxQuantidade.Text.Trim());

                buttonSalvar.Clicked += (ss, ee) =>
                {
                    if (string.IsNullOrEmpty(textBoxDescricao.Text.Trim()))
                    {
                        textBoxDescricao.Focus();
                        return;
                    }

                    prod.Descricao = textBoxDescricao.Text.Trim();

                    prod.IdLista = idLista;

                    prod.Preco = string.IsNullOrEmpty(textBoxValor.Text?.Trim()) ? 0 : decimal.Parse(textBoxValor.Text.Trim());

                    var qtd = string.IsNullOrEmpty(textBoxQuantidade.Text?.Trim()) ? 0 : decimal.Parse(textBoxQuantidade.Text.Trim());

                    prod.Quantidade = decimal.Round(qtd, 3);

                    prod.Peso = Peso;

                    prod.Salvar();

                    PopupNavigation.Instance.PopAsync(true);

                };

                buttonApagar.Clicked += async (ss, ee) =>
                {

                    var apagar = await DisplayAlert("Opa!","Deseja realmente remover este produto?","Sim","Não");

                    if (apagar)
                    {
                        prod.Apagar();

                        await PopupNavigation.Instance.PopAsync(true);
                    }
                };
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (_prod.Id != 0)
            {
                if (_prod.Quantidade > 0)
                {
                    textBoxValor.Focus();
                }
                else
                {
                    textBoxQuantidade.Focus();
                }

            }
            else
            {
                textBoxDescricao.Focus();
            }


        }

        private void textBoxDescricao_Completed(object sender, EventArgs e)
        {
            textBoxQuantidade.Focus();
        }

        private void textBoxQuantidade_Completed(object sender, EventArgs e)
        {
            textBoxValor.Focus();
        }

        private void buttonLimparQtd_Clicked(object sender, EventArgs e)
        {
            textBoxQuantidade.Text = string.Empty;
            textBoxQuantidade.Focus();
        }

        private void buttonLimparValor_Clicked(object sender, EventArgs e)
        {
            textBoxValor.Text = string.Empty;
            textBoxValor.Focus();
        }

        private void textBoxQuantidade_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (carregando) return;
            buttonLimparQtd.IsVisible = !string.IsNullOrEmpty(textBoxQuantidade.Text.Trim());

            if (Peso)
            {
                if (!ajustandoNumero)
                {
                    ajustandoNumero = true;
                    return;
                };

                if (!string.IsNullOrEmpty(textBoxQuantidade.Text.Trim()))
                {

                    ajustandoNumero = false;

                    var oldText = textBoxQuantidade.Text.Trim();
                    var number = DumbParse(oldText);

                    var newText = $"{number}";


                    if (newText.Length == 1)
                    {
                        newText = $"0,00{newText}";
                    }
                    else if (newText.Length == 2)
                    {
                        newText = $"0,0{newText}";
                    }
                    else if (newText.Length == 3)
                    {
                        newText = $"0,{newText}";
                    }
                    else
                    {
                        var ultimos = newText.Substring(newText.Length - 3, 3);
                        var primeiros = newText.Substring(0, newText.Length - 3);

                        newText = $"{primeiros},{ultimos}";
                    }

                    textBoxQuantidade.Text = newText;

                }
            }
        }

        private void textBoxValor_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (carregando) return;

            if (!ajustandoNumero)
            {
                ajustandoNumero = true;
                return;
            };

            buttonLimparValor.IsVisible = !string.IsNullOrEmpty(textBoxValor.Text.Trim());

            if (!string.IsNullOrEmpty(textBoxValor.Text.Trim()))
            {

                ajustandoNumero = false;

                var oldText = textBoxValor.Text.Trim();
                var number = DumbParse(oldText);

                var newText = $"{number}";


                if (newText.Length == 1)
                {
                    newText = $"0,0{newText}";
                }
                else if (newText.Length == 2)
                {
                    newText = $"0,{newText}";
                }
                else
                {
                    var ultimos = newText.Substring(newText.Length - 2, 2);
                    var primeiros = newText.Substring(0, newText.Length - 2);

                    newText = $"{primeiros},{ultimos}";
                }

                textBoxValor.Text = newText;

            }
        }

        public int DumbParse(string input)
        {
            var number = 0;
            int multiply = 1;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (Char.IsDigit(input[i]))
                {
                    number += (input[i] - '0') * (multiply);
                    multiply *= 10;
                }
            }
            return number;
        }

        private async void buttonFechar_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private void buttonTipoPesometro_Clicked(object sender, EventArgs e)
        {
            Peso = true;
            AplicarEstiloUnidade();
            buttonLimparQtd_Clicked(null, null);
        }

        private void buttonTipoUnidade_Clicked(object sender, EventArgs e)
        {
            Peso = false;
            AplicarEstiloUnidade();
            buttonLimparQtd_Clicked(null, null);
        }

        private void AplicarEstiloUnidade()
        {
            try
            {
                if (Peso)
                {
                    textBoxQuantidade.Placeholder = "0,000";

                    buttonTipoPesometro.BackgroundColor = Color.FromHex("#8e44ad");
                    buttonTipoPesometro.TextColor = Color.White;
                    buttonTipoPesometro.BorderWidth = 0;
                    
                    buttonTipoUnidade.BackgroundColor = Color.Transparent;
                    buttonTipoUnidade.TextColor = Color.FromHex("#8e44ad");
                    buttonTipoUnidade.BorderWidth = 1;
                    buttonTipoUnidade.BorderColor = Color.FromHex("#8e44ad"); ;

                }
                else
                {

                    textBoxQuantidade.Placeholder = "0";

                    buttonTipoUnidade.BackgroundColor = Color.FromHex("#8e44ad");
                    buttonTipoUnidade.TextColor = Color.White;
                    buttonTipoUnidade.BorderWidth = 0;

                    buttonTipoPesometro.BackgroundColor = Color.Transparent;
                    buttonTipoPesometro.TextColor = Color.FromHex("#8e44ad");
                    buttonTipoPesometro.BorderWidth = 1;
                    buttonTipoPesometro.BorderColor = Color.FromHex("#8e44ad"); ;
                }
            }
            catch (Exception erro)
            {

                throw erro;
            }
        }

        private void textBoxQuantidade_Focused(object sender, FocusEventArgs e)
        {
            decimal qtd = decimal.TryParse(textBoxQuantidade?.Text.Trim(), out qtd) ? qtd : 0;

            if (qtd == 0) textBoxQuantidade.Text = string.Empty;
        }

        private void textBoxValor_Focused(object sender, FocusEventArgs e)
        {
            decimal qtd = decimal.TryParse(textBoxValor?.Text.Trim(), out qtd) ? qtd : 0;

            if (qtd == 0) textBoxValor.Text = string.Empty;
        }
    }
}