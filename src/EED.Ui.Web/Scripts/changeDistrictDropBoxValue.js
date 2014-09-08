function GetDistrict(_officeId) {
    if ($('#ddlOffice').val() != 0) {
        var processMessage = "<option value='0'> Please wait...</option>";
        $('#ddlDistrict').html(processMessage).show();
        var url = "/Contest/PopulateDistricts";

        $.ajax({
            url: url,
            data: { officeId: _officeId },
            cache: false,
            type: "POST",
            success: function (data) {
                var option = "<option value=''></option>";
                for (var x = 0; x < data.length; x++) {
                    if ($('#DistrictId').val() != data[x].Value)
                        option += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                    else
                        option += "<option selected=selected value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $('#ddlDistrict').html(option).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    } else {
        var option = "";
        $('#ddlDistrict').html(option).show();
    }
}

function SetDistrictId(_districtId) {
    $('#DistrictId').val(_districtId);
};

if ($('#ddlOffice').is(':disabled') != true) {
    if ($('#ddlOffice').val() != 0)
        body.onLoad = GetDistrict($('#ddlOffice').val());
};