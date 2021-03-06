﻿using Actor.Tests.Domain.Model;

namespace Actor.Tests.Domain.Memoization
{
    public class VehicleMemento : IVehicleMemento
    {
        public string Vin { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}