using System.Globalization;
using System.Linq;
using System.Windows;

namespace Macska
{
    public partial class FajtaAblak : Window
    {
        private readonly int? szerkesztettId;

        public FajtaAblak(int? breedId = null)
        {
            InitializeComponent();
            szerkesztettId = breedId;
            OrszagokBetoltese();

            if (szerkesztettId.HasValue)
            {
                Title = "Fajta módosítása";
                txtBreedId.IsEnabled = false;
                AdatokBetoltese();
            }
        }

        private void OrszagokBetoltese()
        {
            using (var ctx = new MacskaContext())
            {
                cbCountry.ItemsSource = ctx.Szarmazas.OrderBy(x => x.CountryName).ToList();
            }
        }

        private void AdatokBetoltese()
        {
            using (var ctx = new MacskaContext())
            {
                var fajta = ctx.Fajtak.FirstOrDefault(f => f.BreedId == szerkesztettId.Value);
                if (fajta == null)
                {
                    MessageBox.Show("A kiválasztott fajta nem található.");
                    DialogResult = false;
                    Close();
                    return;
                }

                txtBreedId.Text = fajta.BreedId.ToString();
                txtBreedName.Text = fajta.BreedName;
                txtAvWeight.Text = fajta.AvWeight.ToString(CultureInfo.InvariantCulture);
                txtLifeSpan.Text = fajta.LifeSpan;
                txtDescription.Text = fajta.Description;
                txtFurLenght.Text = fajta.FurLenght;
                txtPersonality.Text = fajta.Personality;
                txtImgPath.Text = fajta.ImgPath;
                cbCountry.SelectedValue = fajta.CountryId;
            }
        }

        private void BtnMentes_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtBreedId.Text, out int breedId) && !szerkesztettId.HasValue)
            {
                MessageBox.Show("Az azonosító szám legyen.");
                return;
            }

            if (!decimal.TryParse(txtAvWeight.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal avWeight))
            {
                MessageBox.Show("Az átlag súly szám legyen.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBreedName.Text) ||
                string.IsNullOrWhiteSpace(txtLifeSpan.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtFurLenght.Text) ||
                string.IsNullOrWhiteSpace(txtPersonality.Text) ||
                string.IsNullOrWhiteSpace(txtImgPath.Text) ||
                cbCountry.SelectedValue == null)
            {
                MessageBox.Show("Minden mezőt tölts ki.");
                return;
            }

            using (var ctx = new MacskaContext())
            {
                if (szerkesztettId.HasValue)
                {
                    var fajta = ctx.Fajtak.FirstOrDefault(f => f.BreedId == szerkesztettId.Value);
                    if (fajta == null)
                    {
                        MessageBox.Show("A fajta nem található.");
                        return;
                    }

                    fajta.BreedName = txtBreedName.Text.Trim();
                    fajta.AvWeight = avWeight;
                    fajta.LifeSpan = txtLifeSpan.Text.Trim();
                    fajta.Description = txtDescription.Text.Trim();
                    fajta.FurLenght = txtFurLenght.Text.Trim();
                    fajta.Personality = txtPersonality.Text.Trim();
                    fajta.ImgPath = txtImgPath.Text.Trim();
                    fajta.CountryId = (int)cbCountry.SelectedValue;
                }
                else
                {
                    bool letezik = ctx.Fajtak.Any(f => f.BreedId == breedId);
                    if (letezik)
                    {
                        MessageBox.Show("Ez az azonosító már létezik.");
                        return;
                    }

                    var ujFajta = new Fajtak
                    {
                        BreedId = breedId,
                        BreedName = txtBreedName.Text.Trim(),
                        AvWeight = avWeight,
                        LifeSpan = txtLifeSpan.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        FurLenght = txtFurLenght.Text.Trim(),
                        Personality = txtPersonality.Text.Trim(),
                        ImgPath = txtImgPath.Text.Trim(),
                        CountryId = (int)cbCountry.SelectedValue
                    };

                    ctx.Fajtak.Add(ujFajta);
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