function getCoordinatesFromAddress(address) {
    return $.ajax({
        url: `https://nominatim.openstreetmap.org/search?q=${address}&format=json`,
        type: 'post',
        datatype: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: response => {
            return response;
        },
        error: err => {
            return err;
        }
    });
}

function getAddressForAllPharmacies() {
    return $.ajax({
        url: '/Location/GetAddressForAllPharmacies',
        type: 'get',
        datatype: 'json',
        contentType: 'application/json',
        processData: false,
        async: false,
        success: response => {
            return response;
        },
        error: err => {
            return err;
        }
    });
}