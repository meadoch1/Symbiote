﻿using System;
using Actor.Tests.Domain.Model;

namespace Actor.Tests.Domain
{
    public class DriverFactory
    {
        public Driver CreateNewDriver(string ssn, string firstName, string lastName, DateTime dateOfBirth)
        {
            return new Driver(ssn, firstName, lastName, dateOfBirth);
        }
    }
}