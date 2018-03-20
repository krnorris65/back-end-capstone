//only runs function when on the landing page

if (window.location.pathname == "/") {

    $(document).ready(function () {
        $.ajax({
            url: "House/HouseList",
            method: "GET"
        }).then(response => {
            let myHouse = response.filter(h => h.isResidence === true)[0]
            console.log(myHouse)
            let homeMap
            let prev_infowindow

            let icons = {
                false:  'https://maps.google.com/mapfiles/ms/micons/blue-dot.png',
                true: 'http://maps.google.com/mapfiles/ms/micons/red-dot.png',
                best: 'https://maps.google.com/mapfiles/ms/micons/ltblue-dot.png',
                myHouse: 'https://maps.google.com/mapfiles/ms/micons/purple-dot.png',
                search:'https://maps.google.com/mapfiles/kml/pal4/icon57.png'
            }

            function createMap() {
                //create map and center around the user's house
                homeMap = new google.maps.Map(document.getElementById('map'), {
                    zoom: 16,
                    //position is stored as a string so it must be parsed
                    center: JSON.parse(myHouse.position)
                });

                //creates markers and detail windows for all of the houses associated with that user
                response.forEach(h => {
                    let iconColor
                    if (h.isResidence) {
                        iconColor = icons["myHouse"]
                    } else if (!h.avoid && h.petList.filter(p => p.bestFriend).length > 0) {
                        iconColor = icons["best"]
                    } else {
                        iconColor = icons[h.avoid]
                    }

                    let marker = new google.maps.Marker({
                        //position is stored as a string so it must be parsed
                        position: JSON.parse(h.position),
                        //put markers on map created above
                        map: homeMap,
                        icon: {
                            url: iconColor
                        }
                    });
                    

                    //details when click on marker
                    let markerContent = `<div class="markerDetails"><h4>`

                    //if the house is the user's residence, add the house icon before the address
                    if(h.isResidence) {
                        markerContent += `<span class="glyphicon glyphicon-home"></span> `
                    }
                    //if the house is marked to avoid, add the avoid icon before the address
                    if (h.avoid) {
                        markerContent += `<span class="glyphicon glyphicon-ban-circle" style="color:red"></span> `
                    }

                    markerContent += `${h.address}</h4><hr style="margin:5px 0px"/>`
                    if (h.notes != null) {
                        markerContent += `<p>${h.notes}</p>`
                    }
                    if (h.ownerList.length > 0) {
                        markerContent += `<p><b>Owners</b></p>
                            <ul>`
                        h.ownerList.forEach(o => {
                            markerContent += `<li>${o.firstName} ${o.lastName}</li>`
                        })
                        markerContent += `</ul>`
                    }
                    if (h.petList.length > 0) {
                        markerContent += `<p><b>Pets</b></p>
                            <ul>`
                        h.petList.forEach(p => {
                            markerContent += `<li>`
                            if (p.name != null){
                                markerContent += `${p.name} (${p.type})`
                            } else {
                                markerContent += `${p.type}`
                            }
                            if (p.bestFriend) {
                                markerContent += `<span class="glyphicon glyphicon-star" style="color:darkgoldenrod"></span>`
                            }
                            markerContent += `</li>`
                        })
                        markerContent += `</ul>`
                    }
                    //brings user to house detail page for more information
                    markerContent += `<a href="/House/Details/${h.id}">More Info</a>
                        </div>`

                    //creates the info window for each marker
                    var infowindow = new google.maps.InfoWindow({
                        content: markerContent
                    });

                    //event listener for click on marker
                    marker.addListener('click', function () {
                        //if a detail window is open, close it before opening a new one
                        if (prev_infowindow) {
                            prev_infowindow.close();
                        }
                        //set prev_infowindow as the current detail window open
                        prev_infowindow = infowindow;
                        //open the detail window
                        infowindow.open(homeMap, marker);
                    });
                })
            }
            //invoke function to create the map
            createMap();


            //allows users to search different parts of the map
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

                    let newMarker = new google.maps.Marker({
                        map: homeMap,
                        icon: icons["search"],
                        position: place.geometry.location
                    })

                    console.log(newMarker)

                    if (place.geometry.viewport) {
                        // Only geocodes have viewport.
                        bounds.union(place.geometry.viewport);
                    } else {
                        bounds.extend(place.geometry.location);
                    }
                });
                homeMap.fitBounds(bounds);
            });

            //listens for click on search map button
            $("#mapButton").click(b => {
                $("#mapInput").html(
                    `<p>Type an address below and see the houses you've tracked in other parts of town</p>
                    <input id="pac-input" class="controls" type="text" placeholder="Search">
                        <button id="myNeighborhood">Go to My Neighborhood</button>`)
            })

            //resets map to original view (focused around user's neighborhood)
            $("#myNeighborhood").click(m => {
                createMap();
            })

            

        })//end of .then
    }); //end of doc.ready

}//end of path

