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