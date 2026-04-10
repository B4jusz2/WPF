using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Macska
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Betoltes();
        }

        private void Betoltes()
        {
            using (var ctx = new MacskaContext())
            {
                dgMacskak.ItemsSource = ctx.Macskak
                    .Include(m => m.Fajta)
                    .OrderBy(m => m.CatId)
                    .ToList();

                dgFajtak.ItemsSource = ctx.Fajtak
                    .Include(f => f.Szarmazas)
                    .OrderBy(f => f.BreedId)
                    .ToList();

                dgSzarmazas.ItemsSource = ctx.Szarmazas
                    .OrderBy(sz => sz.CountryId)
                    .ToList();
            }
        }

        private void BtnUjMacska_Click(object sender, RoutedEventArgs e)
        {
            var ablak = new MacskaAblak();
            if (ablak.ShowDialog() == true)
                Betoltes();
        }

        private void BtnMacskaMod_Click(object sender, RoutedEventArgs e)
        {
            var kijelolt = dgMacskak.SelectedItem as Macska;
            if (kijelolt == null)
            {
                MessageBox.Show("Válassz ki egy macskát!");
                return;
            }

            var ablak = new MacskaAblak(kijelolt.CatId);
            if (ablak.ShowDialog() == true)
                Betoltes();
        }

        private void BtnUjFajta_Click(object sender, RoutedEventArgs e)
        {
            var ablak = new FajtaAblak();
            if (ablak.ShowDialog() == true)
                Betoltes();
        }

        private void BtnFajtaMod_Click(object sender, RoutedEventArgs e)
        {
            var kijelolt = dgFajtak.SelectedItem as Fajtak;
            if (kijelolt == null)
            {
                MessageBox.Show("Válassz ki egy fajtát!");
                return;
            }

            var ablak = new FajtaAblak(kijelolt.BreedId);
            if (ablak.ShowDialog() == true)
                Betoltes();
        }

        private void BtnUjSzarmazas_Click(object sender, RoutedEventArgs e)
        {
            var ablak = new SzarmazasAblak();
            if (ablak.ShowDialog() == true)
                Betoltes();
        }

        private void BtnSzarmazasMod_Click(object sender, RoutedEventArgs e)
        {
            var kijelolt = dgSzarmazas.SelectedItem as Szarmazas;
            if (kijelolt == null)
            {
                MessageBox.Show("Válassz ki egy származási adatot!");
                return;
            }

            var ablak = new SzarmazasAblak(kijelolt.CountryId);
            if (ablak.ShowDialog() == true)
                Betoltes();
        }
        private void dgFajtak_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var fajta = dgFajtak.SelectedItem as Fajtak;

            if (fajta == null || string.IsNullOrEmpty(fajta.ImgPath))
            {
                imgFajta.Source = null;
                return;
            }

            try
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fajta.ImgPath);

                if (File.Exists(fullPath))
                {
                    imgFajta.Source = new BitmapImage(new Uri(fullPath));
                }
                else
                {
                    imgFajta.Source = null;
                }
            }
            catch
            {
                imgFajta.Source = null;
            }
        }
    }
}