// Write your JavaScript code.

$(document).ready(function () {
    initMap();
    $.ajax({
        url: "House/HouseList",
        method: "GET"
    }).then(response => {
        console.log(response)

    })
});



function initMap() {
    var uluru = { lat: 41.3345876, lng: -73.06000929999999 };
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                center: uluru
            });
            var marker = new google.maps.Marker({
                position: uluru,
                map: map
            });
}


//get geocode
$("#houseCreate").click(evt => {
    evt.preventDefault()
    //ajax request to get location
    const address = $("#createAddress").val()
    const city = $("#createCity").val()
    const state = $("#createState").val()
    const zip = $("#createZip").val()
    $.ajax({
        method: "GET",
        url: `https://maps.googleapis.com/maps/api/geocode/json?address=${address}+${city}+${state}+${zip}&key=${googleAPI.key}`
    }).then(res => {
        console.log(res.results["0"].geometry.location)
        })
    //submit form
})