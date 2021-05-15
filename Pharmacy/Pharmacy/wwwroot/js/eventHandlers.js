function onDelete(id, isDeleteClicked) {
    const deleteSpan = `deleteSpan_${id}`;
    const confirmDeleteSpan = `confirmDeleteSpan_${id}`;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

function onSearch() {
    const searchInputValue = $('#searchInput').val();

    if (searchInputValue && searchInputValue.length > 0) {
        $('#searchButton').attr("disabled", false);
    } else {
        $('#searchButton').attr("disabled", true)
    }
}

function onImport() {
    const fileUpload = $("#postedFile").get(0);
    const files = fileUpload.files;
    const fileData = new FormData();  

    fileData.append(files[0].name, files[0]); 

    $.ajax({
        url: '/Products/Import',
        type: 'post',
        datatype: 'json',
        contentType: false,
        processData: false,
        async: false,
        data: fileData,
        success: response => {
            toastr.success('Success!', response);
        },
        error: err => {
            toastr.error('Error.', err.responseJSON.detail);
        }
    });
}