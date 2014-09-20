function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function readHiddenImageURL(input) {
    if (input.files && input.files[0]) {
        $('#divImg').show();
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#hiddenImage').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#imgInp").change(function () {
    readURL(this);
});

$("#imgInp").change(function () {
    readHiddenImageURL(this);
});