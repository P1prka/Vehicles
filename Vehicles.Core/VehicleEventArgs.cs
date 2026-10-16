using System;

namespace Vehicles.Core;

public class VehicleEventArgs : EventArgs
{
    public Vehicle Vehicle { get; }

    public VehicleEventArgs(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        Vehicle = vehicle;
    }
}



