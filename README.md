# DynamicLinqFilters
Dynamic specification of filters that are translated to LINQ queries using Expressions trees. Can be executed on EFCore and In-Memory collections.

The basic idea of this project is to provide simple filtering structure that can be easily applied on top of EfCore providers and In-Memory Linq collections.
## Install the latest release
`dotnet add package DynamicLinqFilters.Extensions`

## Structure description

The current filter structure that can be sent to the extension methods has the following parts:

`FilterGroupHolder`
  - `FilterGroupHolder` acts as a placeholder for filter groups. 
  - *Can be ommited if filter groups are not used.*
  - Can contain one or more `FilterGroups`.
  - Can define how are the `FilterGroups` concatenated *(with `AND` or `OR`)*
  
`FilterGroups`
  - Groups of filters. 
  - *Can be ommited if filter groups are not used.*
  - Can contain one or more `Filters`.
  - Can define how are the `Filters` concatenated *(with `AND` or `OR`)*
  
 `Filter`
  - The filter that we want to apply. Here we specify the `propery name` on which we want to filter. 
  - Can contain one or more `FilterValues`.
  - Can define how are the `FilterValues` concatenated *(with `AND` or `OR`)*
  
 `FilterValues`
  - The filter values that we want to apply on given filter.
  - Defines the `operator` to be applied *(`Equals`, `StartsWith`, `Contains`, `GreatherThen` etc.)*.
 
## Examples 
 Given the following model `Car` and `CarEngine`, on which we want to filter, we can construct the following filtering examples:
 
#### Models
```c#
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
```

#### Collection setup
```c#
List<Car> cars = new List<Car>();

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
```

## Example without FilterGroups

#### Filters in c#
```c#
//Example 1
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
// Returns one Toyota car.
var results = cars.Filter(filters);


//Example 2
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
// Returns two cars. Citroen and Toyota
var results = cars.Filter(filters);

```
  
## Example with FilterGroups

#### Filters in c#
```c#
//Example 1
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
// Returns one Toyota car.
var results = cars.Filter(filterGroupHolder);


//Example 2
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
// Returns two cars. Citroen and Toyota
var results = cars.Filter(filterGroupHolder);

```

**More info and examples can be found as test cases inside the test project.**
