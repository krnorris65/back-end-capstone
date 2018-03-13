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
        })
    });
}

//get geocode of address
$(".houseCreate").click(evt => {
    const address = $(".formAddress").val()
    const city = $(".formCity").val()
    const state = $(".formState").val()
    const zip = $(".formZip").val()
    //if the required fields are not null then do an ajax request to get geolocation before submitting form
    if (address != "" && city != "" && state != "") {
        //prevent form from sending before geolocation is captured
        evt.preventDefault()

        $.ajax({
            method: "GET",
            url: `https://maps.googleapis.com/maps/api/geocode/json?address=${address}+${city}+${state}+${zip}&key=${googleAPI.key}`
        }).then(res => {
            //if the results only return one result, set that as the geolocation
            if (res.results.length == 1) {
                //geolocation of the address entered
                let geoLocation = res.results["0"].geometry.location

                //assign to position input
                let stringGeo = JSON.stringify(geoLocation)
                $(".formPosition").val(stringGeo)

                //submit form
                $('form').submit()
            } else {
                //if the results have more than one result or no results alert the user of the address not being found
                alert("Address not found")
            }
        })
    }
})