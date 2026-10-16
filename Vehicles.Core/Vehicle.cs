using System;   
namespace Vehicles.Core;

public abstract class Vehicle
{
    public string Make { get; }
    public string Model { get; }
    public double Odometer { get; private set; }

    protected Vehicle(string make, string model)
    {
        if (string.IsNullOrWhiteSpace(make))
        {
            throw new ArgumentException(
                "Make is required.");
        }

        if (string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException(
                "Model is required.");
        }

        Make = make.Trim();
        Model = model.Trim();
        Odometer = 0;
    }

    public abstract string Move(double km);

    protected void AddKm(double km)
    {
        if (km <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(km),
                "Distance must be greater than zero.");
        }

        Odometer += km;
    }


    internal event EventHandler<LogMessageEventArgs>? MessageCreated;

    internal void OnVehicleAdded(object? sender, VehicleEventArgs e)
    {
        if (ReferenceEquals(this, e.Vehicle))
        {
            return;
        }

        string message =
            $"{Make} {Model} honks to welcome " +
            $"{e.Vehicle.Make} {e.Vehicle.Model}.";

        MessageCreated?.Invoke(
            this,
            new LogMessageEventArgs(message));
    }

    internal void OnVehicleRemoved(object? sender, VehicleEventArgs e)
    {
        if (ReferenceEquals(this, e.Vehicle))
        {
            return;
        }

        string message =
            $"{Make} {Model} flashes its lights after " +
            $"{e.Vehicle.Make} {e.Vehicle.Model} leaves.";

        MessageCreated?.Invoke(
            this,
            new LogMessageEventArgs(message));
    }


    public override string ToString()
    {
        return $"{GetType().Name}: {Make} {Model}";
    }
}