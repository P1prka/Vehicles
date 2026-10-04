using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Vehicles.Core;

namespace Vehicles.WpfApp;

public partial class MainWindow : Window
{
    private ObservableCollection<Vehicle> vehicles = new();

    public MainWindow()
    {
        InitializeComponent();

        VehicleListBox.ItemsSource = vehicles;

        DriveButton.IsEnabled = false;
        SwimButton.IsEnabled = false;
    }

    private void AddVehicle_Click(object sender, RoutedEventArgs e)
    {
        string make = MakeTextBox.Text.Trim();
        string model = ModelTextBox.Text.Trim();

        if (VehicleTypeComboBox.SelectedItem is not ComboBoxItem selectedItem)
        {
            MessageBox.Show("Select vehicle type.");
            return;
        }

        try
        {
            string type = selectedItem.Content.ToString()!;

            Vehicle vehicle;

            if (type == "Car")
            {
                vehicle = new Car(make, model);
            }
            else if (type == "Boat")
            {
                vehicle = new Boat(make, model);
            }
            else
            {
                vehicle = new AmphibiousCar(make, model);
            }

            vehicles.Add(vehicle);

            MakeTextBox.Clear();
            ModelTextBox.Clear();
            VehicleTypeComboBox.SelectedIndex = -1;

            LogListBox.Items.Add(
                $"Added: {vehicle}");
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void VehicleListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (VehicleListBox.SelectedItem is not Vehicle vehicle)
        {
            SelectedVehicleTextBlock.Text =
                "No vehicle selected.";

            OdometerTextBlock.Text =
                "Odometer: 0 km";

            DriveButton.IsEnabled = false;
            SwimButton.IsEnabled = false;

            return;
        }

        SelectedVehicleTextBlock.Text =
            $"{vehicle.Make} {vehicle.Model}";

        OdometerTextBlock.Text =
            $"Odometer: {vehicle.Odometer:0.##} km";

        DriveButton.IsEnabled =
            vehicle is IDriveable;

        SwimButton.IsEnabled =
            vehicle is ISwimmable;
    }

    private void Move_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleListBox.SelectedItem is not Vehicle vehicle)
        {
            MessageBox.Show("Select a vehicle.");
            return;
        }

        try
        {
            string message = vehicle.Move(10);

            LogListBox.Items.Add(message);

            UpdateSelectedVehicle();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void Drive_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleListBox.SelectedItem is IDriveable driveable)
        {
            try
            {
                string message = driveable.Drive(10);

                LogListBox.Items.Add(message);

                UpdateSelectedVehicle();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    private void Swim_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleListBox.SelectedItem is ISwimmable swimmable)
        {
            try
            {
                string message = swimmable.Swim(10);

                LogListBox.Items.Add(message);

                UpdateSelectedVehicle();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    private void UpdateSelectedVehicle()
    {
        if (VehicleListBox.SelectedItem is Vehicle vehicle)
        {
            OdometerTextBlock.Text =
                $"Odometer: {vehicle.Odometer:0.##} km";
        }
    }
}