using System.Linq;
using System.Windows;

namespace Macska
{
    public partial class SzarmazasAblak : Window
    {
        private readonly int? szerkesztettId;

        public SzarmazasAblak(int? countryId = null)
        {
            InitializeComponent();
            szerkesztettId = countryId;

            if (szerkesztettId.HasValue)
            {
                Title = "Származás módosítása";
                txtCountryId.IsEnabled = false;
                AdatokBetoltese();
            }
        }

        private void AdatokBetoltese()
        {
            using (var ctx = new MacskaContext())
            {
                var adat = ctx.Szarmazas.FirstOrDefault(x => x.CountryId == szerkesztettId.Value);
                if (adat == null)
                {
                    MessageBox.Show("A kiválasztott származás nem található.");
                    DialogResult = false;
                    Close();
                    return;
                }

                txtCountryId.Text = adat.CountryId.ToString();
                txtCountryName.Text = adat.CountryName;
                txtContinent.Text = adat.Continent;
            }
        }

        private void BtnMentes_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCountryId.Text, out int countryId) && !szerkesztettId.HasValue)
            {
                MessageBox.Show("Az azonosító szám legyen.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCountryName.Text) ||
                string.IsNullOrWhiteSpace(txtContinent.Text))
            {
                MessageBox.Show("Minden mezőt tölts ki.");
                return;
            }

            using (var ctx = new MacskaContext())
            {
                if (szerkesztettId.HasValue)
                {
                    var adat = ctx.Szarmazas.FirstOrDefault(x => x.CountryId == szerkesztettId.Value);
                    if (adat == null)
                    {
                        MessageBox.Show("A származási adat nem található.");
                        return;
                    }

                    adat.CountryName = txtCountryName.Text.Trim();
                    adat.Continent = txtContinent.Text.Trim();
                }
                else
                {
                    bool letezik = ctx.Szarmazas.Any(x => x.CountryId == countryId);
                    if (letezik)
                    {
                        MessageBox.Show("Ez az azonosító már létezik.");
                        return;
                    }

                    var uj = new Szarmazas
                    {
                        CountryId = countryId,
                        CountryName = txtCountryName.Text.Trim(),
                        Continent = txtContinent.Text.Trim()
                    };

                    ctx.Szarmazas.Add(uj);
                }

                ctx.SaveChanges();
            }

            DialogResult = true;
            Close();
        }

        private void BtnMegse_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}