# DynamicLinqFilters
Dynamic specification of filters that are translated to LINQ queries using Expressions trees. Can be executed on EFCore and In-Memory collections.

The basic idea of this project is to provide simple filtering structure that can be easily applied on top of EfCore providers and In-Memory Linq collections.

## Structure description

The current filter structure that can be sent to the extension methods has the following parts:

`FilterGroupHolder`
  - `FilterGroupHolder` acts as a placeholder for filter groups. 
  - Can contain one or more `FilterGroups`.
  - Can define how are the `FilterGroups` concatenated *(with `AND` or `OR`)*
  
`FilterGroups`
  - Groups of filters. 
  - Can contain one or more `Filters`.
  - Can define how are the `Filters` concatenated *(with `AND` or `OR`)*
  
 `Filter`
  - The filter that we want to apply. Here we specify the `propery name` on which we want to filter. 
  - Can contain one or more `FilterValues`
  - Can define how are the `FilterValues` concatenated *(with `AND` or `OR`)*
  
 `FilterValues`
  - The filter values that we want to apply on given filter.
  - Defines the `operator` to be applied *(`Equals`, `StartsWith`, `Contains`, `GreatherThen` etc.)*
