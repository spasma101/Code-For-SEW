$( document ).ready(function() {

    getCurrentLocation();

    let rw = $('button.refreshWeather');
    rw.hide();
  
    let listMarkup = $('ul.List');
    let weatherTodayMarkup = $('ul.weatherToday');

    $(".getWeather").on('click', function() {
        console.log('Firing location');
        
        let input = document.getElementById("locationName");
        let location = encodeURIComponent(input.value);

        rw.show();
        rw.css('display', 'block')

        getLocationId(location);
    });

    rw.on('click', function() {
        console.log('Firing Refresh');
        let input = document.getElementById("locationName");
        let location = encodeURIComponent(input.value);
        getLocationId(location);
    })

    function getCurrentLocation() {
        if ("geolocation" in navigator){ //check geolocation available 
            //try to get user current location using getCurrentPosition() method
            navigator.geolocation.getCurrentPosition(function(position){ 
                    $("#result").html("Found your location <br />Lat : "+position.coords.latitude+" </br>Lang :"+ position.coords.longitude);

                    getLocationIdByLatLong(position.coords.latitude, position.coords.longitude);
                },
                function(error) {
                    if (error.code == error.PERMISSION_DENIED)
                      console.log("you denied me :-(");
                        let defaultLocationName = 'Belfast';
                        insertLocationName(defaultLocationName);
                        let defaultLocationId = '44544';
                        getWeatherByLocation(defaultLocationId);
                  });
        }
        else
        {
            console.log("Browser doesn't support geolocation!");
            
        }
    }

    function insertLocationName(locationName) {
        $('.weatherLocation').html(
            '<span class="weatherLocation">' + locationName + '</span>'
        )
    }
    function insertLocationerror(location) {
        $('.locationLookupError').html(
            '<span class="locationLookupError">Error: Please enter a valid location!'+ location +'</span>'
        )
        rw.hide();
        rw.css('display', 'none')
    }
    function deleteErrorMessage() {
        $('.locationLookupError').html(
            '<span class="locationLookupError"></span>'
        )
    }

    function getLocationIdByLatLong(latt, long) {
        
        let latLongLocationQuery = 'https://www.metaweather.com/api/location/search/?lattlong=';
        let latLongLocationQueryString = (latt.toString()) +','+ (long.toString());

        console.log(latLongLocationQuery+latLongLocationQueryString);

        insertLocationName(location);

        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function() 
        {
            if (this.readyState == 4 && this.status == 200) 
            {
                //Use parse() method to convert JSON string to JSON object
                var jsonResponse = JSON.parse(this.responseText);

                switch(jsonResponse.length) {
                    case 0:
                      // code block class = locationLookupError
                        insertLocationerror(location);
                        $('.weatherToday').empty();
                        $('.List').empty();
                      break;
                      
                    default:
                      // code block
                      // code block
                      console.log(jsonResponse)
                      deleteErrorMessage();
                      console.log(jsonResponse[0].woeid);
                      insertLocationName(jsonResponse[0].title)
                      getWeatherByLocation(jsonResponse[0].woeid);

                      break;
                  }

                
            }
        }

        xmlhttp.open("GET", latLongLocationQuery + latLongLocationQueryString, true);
        xmlhttp.send();
    }

    function getLocationId(location) {
        
        let locationQuery = 'https://www.metaweather.com/api/location/search/?query=';
        let locationQueryString = location;

        insertLocationName(location);

        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function() 
        {
            if (this.readyState == 4 && this.status == 200) 
            {
                //Use parse() method to convert JSON string to JSON object
                var jsonResponse = JSON.parse(this.responseText);

                switch(jsonResponse.length) {
                    case 0:
                      // code block class = locationLookupError
                        insertLocationerror(location);
                        $('.weatherToday').empty();
                        $('.List').empty();
                      break;
                      
                    default:
                      // code block
                      // code block
                      console.log(jsonResponse)
                      deleteErrorMessage();
                        $.each(jsonResponse, function(i, item){
                            console.log("the Location ID for item " + i + " is " + item.woeid)
                            
                            getWeatherByLocation(item.woeid);
                        })
                      break;
                  }

                
            }
        }

        xmlhttp.open("GET", locationQuery + locationQueryString, true);
        xmlhttp.send();
    }

    function getWeatherByLocation(locationId) {
        console.log('getWeatherByLocation function has been called');

        console.log(locationId);

        $('.weatherToday').empty();
        $('.List').empty();

        let location = 'https://www.metaweather.com/api/location/';
        let query = locationId;
        let items = [];
        let todaysWeather = [];

        var xmlhttp = new XMLHttpRequest();

        xmlhttp.onreadystatechange = function() 
        {
            if (this.readyState == 4 && this.status == 200) 
            {
                //Use parse() method to convert JSON string to JSON object
                var responseJsonObj = JSON.parse(this.responseText);
                
                console.log(responseJsonObj);
                console.log(responseJsonObj.timezone);

                if(responseJsonObj.timezone == 'Europe/London') {
                    var consolidated_weather = responseJsonObj.consolidated_weather;
                    console.log(consolidated_weather);
    
                    var slicedWeather = consolidated_weather.slice(1, 6);
                    var weatherToday = consolidated_weather[0];
                    console.log(weatherToday);

                    //Collect todays weather and push to array
                    todaysWeather.push(
                        '<li class="weatherToday__Item">'
                        + '<h3>' + weatherToday.applicable_date + '</h3>'
                        + '<p>' + weatherToday.weather_state_name + '</p>'
                        + '<img src="https://www.metaweather.com/static/img/weather/' + weatherToday.weather_state_abbr + '.svg" />'
                        + '</li>'
                    )
                    
                    $.each(slicedWeather, function(i, item){
                    //Collect all items and add to array to deliver to dom as one block
                        items.push(
                            '<li class="List__Item">'
                            + '<h3>' + item.applicable_date + '</h3>'
                            + '<p>' + item.weather_state_name + '</p>'
                            + '<img src="https://www.metaweather.com/static/img/weather/' + item.weather_state_abbr + '.svg" />'
                            + '</li>'
                        )
                    })

                    
                } else {
                    return;
                }
            listMarkup.append(items);
            weatherTodayMarkup.append(todaysWeather);
            }
        };
        
        xmlhttp.open("GET", location + query, true);
        xmlhttp.send();
    }

  });