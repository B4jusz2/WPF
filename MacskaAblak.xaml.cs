using System.Linq;
using System.Windows;

namespace Macska
{
    public partial class MacskaAblak : Window
    {
        private readonly int? szerkesztettId;

        public MacskaAblak(int? catId = null)
        {
            InitializeComponent();
            szerkesztettId = catId;
            FajtakBetoltese();

            if (szerkesztettId.HasValue)
            {
                CimezesModositasra();
                AdatokBetoltese();
            }
        }

        private void CimezesModositasra()
        {
            Title = "Macska módosítása";
            txtCatId.IsEnabled = false;
        }

        private void FajtakBetoltese()
        {
            using (var ctx = new MacskaContext())
            {
                cbBreed.ItemsSource = ctx.Fajtak.OrderBy(f => f.BreedName).ToList();
            }
        }

        private void AdatokBetoltese()
        {
            using (var ctx = new MacskaContext())
            {
                var macska = ctx.Macskak.FirstOrDefault(m => m.CatId == szerkesztettId.Value);
                if (macska == null)
                {
                    MessageBox.Show("A kiválasztott macska nem található.");
                    DialogResult = false;
                    Close();
                    return;
                }

                txtCatId.Text = macska.CatId.ToString();
                txtName.Text = macska.Name;
                txtAge.Text = macska.Age.ToString();
                txtGender.Text = macska.Gender;
                cbBreed.SelectedValue = macska.BreedId;
            }
        }

        private void BtnMentes_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCatId.Text, out int catId) && !szerkesztettId.HasValue)
            {
                MessageBox.Show("Az azonosító szám legyen.");
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("A kor szám legyen.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtGender.Text) ||
                cbBreed.SelectedValue == null)
            {
                MessageBox.Show("Minden mezőt tölts ki.");
                return;
            }

            using (var ctx = new MacskaContext())
            {
                if (szerkesztettId.HasValue)
                {
                    var macska = ctx.Macskak.FirstOrDefault(m => m.CatId == szerkesztettId.Value);
                    if (macska == null)
                    {
                        MessageBox.Show("A macska nem található.");
                        return;
                    }

                    macska.Name = txtName.Text.Trim();
                    macska.Age = age;
                    macska.Gender = txtGender.Text.Trim();
                    macska.BreedId = (int)cbBreed.SelectedValue;
                }
                else
                {
                    bool letezik = ctx.Macskak.Any(m => m.CatId == catId);
                    if (letezik)
                    {
                        MessageBox.Show("Ez az azonosító már létezik.");
                        return;
                    }

                    var ujMacska = new Macska
                    {
                        CatId = catId,
                        Name = txtName.Text.Trim(),
                        Age = age,
                        Gender = txtGender.Text.Trim(),
                        BreedId = (int)cbBreed.SelectedValue
                    };

                    ctx.Macskak.Add(ujMacska);
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