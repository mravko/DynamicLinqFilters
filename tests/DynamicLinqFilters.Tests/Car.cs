using System;

namespace Tests
{
    public class Car
    {
        public string ModelName { get; set; }
        public int NumberOfDoors { get; set; }
        public CarEngine CarEngine { get; set; }
        public DateTime MadeOn { get; set; }
        public DateTime? FirstServiceOn { get; set; }
    }
}