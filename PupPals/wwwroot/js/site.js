//only runs function when on the landing page

if (window.location.pathname == "/") {

    $(document).ready(function () {
        $.ajax({
            url: "House/HouseList",
            method: "GET"
        }).then(response => {
            let myHouse = response.filter(h => h.isResidence === true)[0]
            console.log(myHouse)
            //create map and center around the user's house
            let homeMap = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                //position is stored as a string so it must be parsed
                center: JSON.parse(myHouse.position)
            });

            //creates markers for all of the houses associated with that user
            response.forEach(h => {
                let marker = new google.maps.Marker({
                    //position is stored as a string so it must be parsed
                    position: JSON.parse(h.position),
                    //put markers on map created above
                    map: homeMap
                });
            })
            

        var input = document.getElementById('pac-input');
        var searchBox = new google.maps.places.SearchBox(input);

        // Bias the SearchBox results towards current map's viewport.
        homeMap.addListener('bounds_changed', function () {
            searchBox.setBounds(homeMap.getBounds());
        });
        // Listen for the event fired when the user selects a prediction and retrieve
        // more details for that place.
        searchBox.addListener('places_changed', function () {
            var places = searchBox.getPlaces();

            if (places.length == 0) {
                return;
            }

            // For each place, get the icon, name and location.
            var bounds = new google.maps.LatLngBounds();
            places.forEach(function (place) {
                if (!place.geometry) {
                    console.log("Returned place contains no geometry");
                    return;
                }


                if (place.geometry.viewport) {
                    // Only geocodes have viewport.
                    bounds.union(place.geometry.viewport);
                } else {
                    bounds.extend(place.geometry.location);
                }
            });
            homeMap.fitBounds(bounds);
        });


            })//end of .then
    }); //end of doc.ready

}//end of path

