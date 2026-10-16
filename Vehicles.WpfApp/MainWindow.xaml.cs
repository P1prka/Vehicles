
using System;
using System.Windows;
using System.Windows.Controls;
using Vehicles.Core;

namespace Vehicles.WpfApp;

public partial class MainWindow : Window
{
    private readonly Garage<Car> cars = new("Cars");
    private readonly Garage<Boat> boats = new("Boats");

    public MainWindow()
    {
        InitializeComponent();

        CarsListBox.ItemsSource = cars.Items;
        BoatsListBox.ItemsSource = boats.Items;

        cars.MessageCreated += GarageMessageCreated;
        boats.MessageCreated += GarageMessageCreated;

        UpdateSelectedVehicle();
    }

    private void GarageMessageCreated(
        object? sender,
        LogMessageEventArgs e)
    {
        LogListBox.Items.Add(e.Message);
    }

    private void AddVehicle_Click(
        object sender,
        RoutedEventArgs e)
    {
        string make = MakeTextBox.Text.Trim();
        string model = ModelTextBox.Text.Trim();

        if (VehicleTypeComboBox.SelectedItem
            is not ComboBoxItem selectedItem)
        {
            MessageBox.Show("Select vehicle type.");
            return;
        }

        string type = selectedItem.Content?.ToString() ?? "";

        try
        {
            Vehicle vehicle;

            if (type == "Car")
            {
                vehicle = new Car(make, model);
            }
            else if (type == "Boat")
            {
                vehicle = new Boat(make, model);
            }
            else if (type == "AmphibiousCar")
            {
                vehicle = new AmphibiousCar(make, model);
            }
            else
            {
                MessageBox.Show("Unknown vehicle type.");
                return;
            }

            if (vehicle is Boat boat)
            {
                boats.Add(boat);
                BoatsListBox.Items.Refresh();
                BoatsListBox.SelectedItem = boat;
            }
            else if (vehicle is Car car)
            {
                cars.Add(car);
                CarsListBox.Items.Refresh();
                CarsListBox.SelectedItem = car;
            }

            LogListBox.Items.Add($"Added: {vehicle}");

            MakeTextBox.Clear();
            ModelTextBox.Clear();
            VehicleTypeComboBox.SelectedIndex = -1;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void GarageSelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (sender == CarsListBox &&
            CarsListBox.SelectedItem != null)
        {
            BoatsListBox.SelectedItem = null;
        }
        else if (sender == BoatsListBox &&
                 BoatsListBox.SelectedItem != null)
        {
            CarsListBox.SelectedItem = null;
        }

        UpdateSelectedVehicle();
    }

    private Vehicle? GetSelectedVehicle()
    {
        if (CarsListBox.SelectedItem is Vehicle car)
        {
            return car;
        }

        return BoatsListBox.SelectedItem as Vehicle;
    }

    private void RemoveVehicle_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (CarsListBox.SelectedItem is Car car)
        {
            if (cars.Remove(car))
            {
                CarsListBox.Items.Refresh();
                LogListBox.Items.Add($"Removed: {car}");
            }
        }
        else if (BoatsListBox.SelectedItem is Boat boat)
        {
            if (boats.Remove(boat))
            {
                BoatsListBox.Items.Refresh();
                LogListBox.Items.Add($"Removed: {boat}");
            }
        }
        else
        {
            MessageBox.Show("Select a vehicle.");
            return;
        }

        UpdateSelectedVehicle();
    }

    private void Move_Click(
        object sender,
        RoutedEventArgs e)
    {
        Vehicle? vehicle = GetSelectedVehicle();

        if (vehicle == null)
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

    private void Drive_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (GetSelectedVehicle() is IDriveable vehicle)
        {
            try
            {
                LogListBox.Items.Add(vehicle.Drive(10));
                UpdateSelectedVehicle();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    private void Swim_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (GetSelectedVehicle() is ISwimmable vehicle)
        {
            try
            {
                LogListBox.Items.Add(vehicle.Swim(10));
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
        Vehicle? vehicle = GetSelectedVehicle();

        if (vehicle == null)
        {
            SelectedVehicleTextBlock.Text =
                "No vehicle selected.";

            OdometerTextBlock.Text =
                "Odometer: 0 km";
        }
        else
        {
            SelectedVehicleTextBlock.Text =
                vehicle.ToString();

            OdometerTextBlock.Text =
                $"Odometer: {vehicle.Odometer:0.##} km";
        }

        MoveButton.IsEnabled = vehicle != null;
        RemoveButton.IsEnabled = vehicle != null;
        DriveButton.IsEnabled = vehicle is IDriveable;
        SwimButton.IsEnabled = vehicle is ISwimmable;
    }
}





