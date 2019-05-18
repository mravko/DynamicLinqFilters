using DynamicLinqFilters.Models;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using DynamicLinqFilters.Extensions;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Tests
{
    public class FilterTests
    {
        private List<Car> cars = new List<Car>();

        [SetUp]
        public void Setup()
        {
            cars.Add(new Car
            {
                ModelName = "Citroen",
                NumberOfDoors = 5,
                CarEngine = new CarEngine { NumberOfCylinder = 6, Series = "C1" }
            });

            cars.Add(new Car
            {
                ModelName = "Toyota",
                NumberOfDoors = 4,
                CarEngine = new CarEngine { NumberOfCylinder = 4, Series = "T1" }
            });
        }

        [TearDown]
        public void TearDown()
        {
            cars.Clear();
        }

        [Test]
        public void FilterCars_SendsModelToyo_ReturnsToyota()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = nameof(Car.ModelName),
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = "Toyo",
                            Operator = "StartsWith"
                        }
                    }
                }
            };

            var json = JArray.FromObject(filters).ToString();

            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        [Test]
        public void FilterCars_SendsModelToyoAndCit_ReturnsToyotaAndCitr()
        {
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = nameof(Car.ModelName),
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = "Toyo",
                            Operator = "StartsWith"
                        },
                        new FilterValue
                        {
                            Value = "Cit",
                            Operator = "StartsWith"
                        }
                    },
                    FilterValueJoinType = FilterJoinType.Or
                }
            };

            var json = JArray.FromObject(filters).ToString();

            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(results.First().ModelName, "Citroen");
            Assert.AreEqual(results.Last().ModelName, "Toyota");

        }

        [Test]
        public void FilterCarsWithFilterGroupHolder_SendsModelToyo_ReturnsToyota()
        {
            //arrange
            var filterGroupHolder = new FilterGroupsHolder
            {
                FilterGroups = new List<FilterGroup>
                {
                     new FilterGroup
                     {
                        Filters = new List<Filter>
                        {
                            new Filter
                            {
                                PropertyName = nameof(Car.ModelName),
                                Value = new List<FilterValue>
                                {
                                    new FilterValue
                                    {
                                        Value = "Toyo",
                                        Operator = "StartsWith"
                                    }
                                }
                            }
                        }
                     }
                 }
            };

            var json = JObject.FromObject(filterGroupHolder).ToString();

            //act
            var results = cars.Filter(filterGroupHolder);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        [Test]
        public void FilterCarsWithFilterGroupHolder_SendsModelToyoAndCit_ReturnsToyotaAndCitr()
        {
            var filterGroupHolder = new FilterGroupsHolder
            {
                FilterGroups = new List<FilterGroup>
                {
                     new FilterGroup
                     {
                        Filters = new List<Filter>
                        {
                            new Filter
                            {
                                PropertyName = nameof(Car.ModelName),
                                Value = new List<FilterValue>
                                {
                                    new FilterValue
                                    {
                                        Value = "Toyo",
                                        Operator = "StartsWith"
                                    },
                                    new FilterValue
                                    {
                                        Value = "Cit",
                                        Operator = "StartsWith"
                                    }
                                },
                                FilterValueJoinType = FilterJoinType.Or
                            }
                        }
                     }
                 }
            };

            var json = JObject.FromObject(filterGroupHolder).ToString();

            //act
            var results = cars.Filter(filterGroupHolder);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(results.First().ModelName, "Citroen");
            Assert.AreEqual(results.Last().ModelName, "Toyota");

        }
    }

    public class Car
    {
        public string ModelName { get; set; }
        public int NumberOfDoors { get; set; }
        public CarEngine CarEngine { get; set; }
    }

    public class CarEngine
    {
        public string Series { get; set; }
        public int NumberOfCylinder { get; set; }
    }
}