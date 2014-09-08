function GetParentDistrict(_districtTypeId) {
    if ($('#ddlDistrictType').val() != 0) {
        var processMessage = "<option value='0'> Please wait...</option>";
        $('#ddlParentDistrict').html(processMessage).show();
        var url = "/District/PopulateParentDistricts";

        $.ajax({
            url: url,
            data: { districtTypeId: _districtTypeId },
            cache: false,
            type: "POST",
            success: function (data) {
                var option = "<option value=''></option>";
                for (var x = 0; x < data.length; x++) {
                    if ($('#ParentDistrictId').val() != data[x].Value)
                        option += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                    else
                        option += "<option selected=selected value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
                $('#ddlParentDistrict').html(option).show();
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    } else {
        var option = "";
        $('#ddlParentDistrict').html(option).show();
    }
}

function SetParentDistrictId(_parentDistrictId) {
    $('#ParentDistrictId').val(_parentDistrictId);
};

if ($('#ddlDistrictType').is(':disabled') != true) {
    if ($('#ddlDistrictType').val() != 0)
        body.onLoad = GetParentDistrict($('#ddlDistrictType').val());
};