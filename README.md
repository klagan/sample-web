# Sample webapi techniques

## Sample.WebApi.Shaping

Example of shaping the returned data.  The example allows the caller to submit a csv query string `fields`.  The csv is a list of the properties being requested.

> The following example calls assume the solution is being hosted on `https://localhost:5001/`

### Call for a list of the weather forecasts

This call will retrieve a list of weather forecasts in the `full shape`

```
https://localhost:5001/weatherforecast
```

### Call for a list of shaped weather forecasts 

This call will retrieve a list of weather forecasts where the weather forecast object only returns the `date` and `temperatureC` properties

```
https://localhost:5001/weatherforecast?fields=date,temperaturec
```

### Call for a single weather forecast

This call will retrieve a single weather forecast

```
https://localhost:5001/weatherforecast/getforecast
```

### Call for a single shaped weather forecast

This call will retrieve a single weather forecast where the weather forecast object only returns the `date` and `temperatureC` properties

```
https://localhost:5001/weatherforecast/getforecast?fields=date,temperaturec
```
