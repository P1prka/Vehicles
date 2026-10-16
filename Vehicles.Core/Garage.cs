using System;
using System.Collections.Generic;

namespace Vehicles.Core;

public class Garage<T> where T : Vehicle
{
    private readonly List<T> items = new List<T>();

    public string Name { get; }

    public IReadOnlyList<T> Items => items.AsReadOnly();
    public event EventHandler<VehicleEventArgs>? VehicleAdded;

    public event EventHandler<VehicleEventArgs>? VehicleRemoved;




    public event EventHandler<LogMessageEventArgs>? MessageCreated;
    public Garage(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Garage name is required.",
                nameof(name));
        }

        Name = name.Trim();
    }

    public void Add(T vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (items.Contains(vehicle))
        {
            throw new ArgumentException(
                "This vehicle is already in the garage.",
                nameof(vehicle));
        }

        items.Add(vehicle);

        vehicle.MessageCreated += OnVehicleMessage;

        VehicleAdded += vehicle.OnVehicleAdded;
        VehicleRemoved += vehicle.OnVehicleRemoved;

        VehicleAdded?.Invoke(
            this,
            new VehicleEventArgs(vehicle));

    }



    public bool Remove(T vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        bool removed = items.Remove(vehicle);

        if (!removed)
        {
            return false;
        }

        VehicleAdded -= vehicle.OnVehicleAdded;
        VehicleRemoved -= vehicle.OnVehicleRemoved;

        vehicle.MessageCreated -= OnVehicleMessage;

        VehicleRemoved?.Invoke(
            this,
            new VehicleEventArgs(vehicle));

        return true;
    }


    private void OnVehicleMessage(object? sender, LogMessageEventArgs e)
    {
        string message = $"[{Name}] {e.Message}";

        MessageCreated?.Invoke(
            this,
            new LogMessageEventArgs(message));
    }



}











