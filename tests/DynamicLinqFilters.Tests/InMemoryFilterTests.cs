using DynamicLinqFilters.Models;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using DynamicLinqFilters.Extensions;
using System.Linq;
using Newtonsoft.Json.Linq;
using System;

namespace Tests
{
    public class InMemoryFilterTests
    {
        private List<Car> cars = new List<Car>();

        [SetUp]
        public void Setup()
        {
            cars.Add(new Car
            {
                Id = 1,
                ModelName = "Citroen",
                NumberOfDoors = 5,
                CarEngine = new CarEngine { Id = 1, NumberOfCylinder = 6, Series = "C1" },
                MadeOn = new DateTime(2010, 1, 1)
            });

            cars.Add(new Car
            {
                Id = 2,
                ModelName = "Toyota",
                NumberOfDoors = 4,
                CarEngine = new CarEngine { Id = 2, NumberOfCylinder = 4, Series = "T1" },
                MadeOn = new DateTime(2013, 1, 1),
                FirstServiceOn = new DateTime(2014, 1, 1)
            });
        }

        [TearDown]
        public void TearDown()
        {
            cars.Clear();
        }

        #region StringTests
        [Test]
        public void FilterCarsByString_SendsModelToyo_ReturnsToyota()
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



            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        [Test]
        public void FilterCarsByString_SendsModelToyoAndCit_ReturnsToyotaAndCitr()
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



            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(results.First().ModelName, "Citroen");
            Assert.AreEqual(results.Last().ModelName, "Toyota");

        }

        [Test]
        public void FilterCarsByStringWithFilterGroupHolder_SendsModelToyo_ReturnsToyota()
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
        public void FilterCarsByStringWithFilterGroupHolder_SendsModelToyoAndCit_ReturnsToyotaAndCitr()
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
        #endregion

        #region DateTests
        [Test]
        public void FilterCarsByDate_SendsGreaterThan2011_ReturnsToyota()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = nameof(Car.MadeOn),
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = new DateTime(2011, 1, 1),
                            Operator = ">="
                        }
                    }
                }
            };



            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        [Test]
        public void FilterCarsByNullableDate_AsDate_SendsGreaterThan2011_ReturnsToyota()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = nameof(Car.FirstServiceOn),
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = new DateTime(2011, 1, 1),
                            Operator = ">="
                        }
                    }
                }
            };

            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        [Test]
        public void FilterCarsByNullableDate_AsString_SendsGreaterThan2011_ReturnsToyota()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = nameof(Car.FirstServiceOn),
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = "2011-01-01",
                            Operator = ">="
                        }
                    }
                }
            };

            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        [Test]
        public void FilterCarsByDateAsString_SendsGreaterThan2011_ReturnsToyota()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = nameof(Car.MadeOn),
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = "2011-01-01",
                            Operator = ">="
                        }
                    }
                }
            };



            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        [Test]
        public void FilterCarsByDate_SendsGreaterThan2009_ReturnsToyotaAndCitroen()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = nameof(Car.MadeOn),
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = new DateTime(2009, 1, 1),
                            Operator = ">="
                        }
                    }
                }
            };



            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(results.Last().ModelName, "Toyota");
            Assert.AreEqual(results.First().ModelName, "Citroen");
        }
        #endregion

        #region ProjectionTests

        [Test]
        public void FilterCarsByProjectionDate_SendsGreaterThan2011_ReturnsToyota()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = "ProjectedMadeOn",
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = new DateTime(2011, 1, 1),
                            Operator = ">="
                        }
                    }
                }
            };

            //act
            var results = cars.Select(x => new { x.ModelName, ProjectedMadeOn = x.MadeOn }).Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Toyota");
        }

        #endregion

        #region ContainsTests

        [Test]
        public void FilterCarsWithContains_SendsId1_ReturnsCitroen()
        {
            //arrange
            var filters = new List<Filter>
            {
                new Filter
                {
                    PropertyName = "CarEngine.Id",
                    Value = new List<FilterValue>
                    {
                        new FilterValue
                        {
                            Value = new List<string> { "1" },
                            Operator = "in"
                        }
                    }
                }
            };

            //act
            var results = cars.Filter(filters);

            //assert
            Assert.NotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(results.First().ModelName, "Citroen");
        }
        #endregion
    }
}