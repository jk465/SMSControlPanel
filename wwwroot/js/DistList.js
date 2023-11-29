var isDLTableExist;
var isViewTableExist;

$(function () {
    console.log("Dl", getUser());

    $('#txtCompany').val(getUser().CompanyName);
    $('#txtUsername').val(getUser().EmpID);

    var path = window.location.pathname;

    if (path.includes('DistList')) {
        DistList_PageLoad(); 
    }
    if (path.includes('View')) {
        var listname = getListName();
        ViewDL(listname);
    }
});

function DistList_PageLoad() {
    LoadDistList();

    $('#chkSelectAllCheckboxes').on('change', function () {

        var isChecked = $(this).prop('checked');

        $('#gvdistributionlist tr input[type="checkbox"]').prop('checked', isChecked);
        if (isChecked) {
            $('#selectTODelete').show();
        } else {
            $('#selectTODelete').hide();
        }
    });

    $(document).on('change', '.dl-checkbox', function () {
        var allChecked = $('.dl-checkbox:checked').length === $('.dl-checkbox').length;
        $('#chkSelectAllCheckboxes').prop('checked', allChecked);
        $('#selectTODelete').show();

        var allUnchecked = $('.dl-checkbox').filter(':checked').length === 0;
        if (allUnchecked)
            $('#selectTODelete').hide();
    });

    $(document).on('click', '.btn-delete', function () {
        var ListName = $(this).closest('tr').find('td:eq(1)').text();
        DeleteDistList(ListName);
    });
    $(document).on('click', '.btn-update', function () {
        var ListName = $(this).closest('tr').find('td:eq(1)').text();
        UpdateDistList(ListName);
    });
    $(document).on('click', '.btn-view', function () {
        var ListName = $(this).closest('tr').find('td:eq(1)').text();
        ViewDistList(ListName);
    });
}

function LoadDistList() {
    $.ajax({
        url: "../MaintainDL/LoadDL",
        type: "GET",
        dataType: 'json',
        success: function (response) {

            if (response.length == 0) {
                $('#distListTable').hide();
                $('#imgexcelicon').hide();
                $('#selectTODelete').hide();
                return;
            }
                

            $('#imgexcelicon').show();

            var IsPaginate = response.length > 10 ? true : false;

            if (isDLTableExist != null || isDLTableExist != undefined) {
                $('#gvdistributionlist').DataTable().destroy();
            }
            isDLTableExist = $('#gvdistributionlist').DataTable({
                "data": response,
                "paginate": IsPaginate,
                "bInfo": IsPaginate,
                "bFilter": false,
                "searching": false,
                "ordering": false,
                "lengthMenu": false,
                "initComplete": function (settings, json) {
                    $("#divLoading").hide();
                },
                columnDefs: [{
                    className: 'center',
                }],
                columns: [
                    {
                        data: null, className: 'center',
                        render: function (data, type, row, meta) {
                            return '<input type="checkbox" class="dl-checkbox"></button>';
                        }
                    },
                    {
                        data: 'listname', className: 'center',
                    },
                    {
                        data: 'entries', className: 'center',
                    },
                    {
                        data: null, className: 'center',
                        render: function (data, type, row, meta) {
                            return '<button type="button" class="btn-delete">Delete</button>';
                        }
                    },
                    {
                        data: null, className: 'center',
                        render: function (data, type, row, meta) {
                            return '<button type="button" class="btn-update">Update</button>';
                        }
                    },
                    {
                        data: null, className: 'center',
                        render: function (data, type, row, meta) {
                            return '<button type="button" class="btn-view">View</button>';
                        }
                    },
                ]
            });

        },

        error: function (response) {

        }
    });
}

function DeleteDistList(listName) {

    var param = {
        ListName: listName,
    }
    $.ajax({
        url: "../MaintainDL/DeleteDL",
        type: "POST",
        data: param,
        success: function (reponse) {
            console.log(reponse);
            LoadDistList();
        }
    });
}

function openCreateDL() {
    window.location.href = '../MaintainDL/CreateDL';
}

function closeModel() {
    var path = window.location.pathname;

    if (path.includes('Create')) {
        window.location.href = '../MaintainDL/DistList';
    } else {
        window.location.href = '../../MaintainDL/DistList';
    }
    
}

function createDL() {
    $('#exist-err-label').hide();


    var isValid = FormValidation();

    if (isValid == false)
        return;


    checkExists().then(function (isExists) {
        if (isExists == false) {
            uploadFile('created');
        }
        else {
            $('#exist-err-label').show();
            return;
        }
    })
}

function FormValidation() {
    $('#dl-form').validate({
        errorClass: 'error-Label',
        rules: {
            FileUpload1: {
                required: true,
                extension: ['csv','txt'],
                filesize: 2097152
            },
            txtDistlistName: {
                required: true,
            },
        },
        messages: {
            FileUpload1: {
                required: 'Please choose a file',
                extension: 'Unsupported File Format.',
                filesize: 'File Size Exceeded'
            },
            txtDistlistName: {
                required: 'Please enter the Distribution List name',
            },
        }
    });

    $.validator.addMethod('filesize', function (value, element, param) {
        if (element.files && element.files[0])
            return element.files[0].size <= param;
        return true;
    });
    $.validator.addMethod('extension', function (value, element, param) {
        var extension = value.split('.').pop().toLowerCase();
        return this.optional(element) || $.inArray(extension, param) !== -1;
    });

    return $('#dl-form').valid();
}

async function checkExists() {

    var listName = {
        ListName: $('#txtDistlistName').val()
    }


    var response = await $.ajax({
        url: "../MaintainDL/CheckExists",
        type: "POST",
        data: listName,
        dataType: 'json',
    });

    var result = response.success;

    return result;

}

function uploadFile(action) {
    var URL;
    var path = window.location.pathname;
    if (path.includes('Create')) {
        URL = '../MaintainDL/UploadFile';
    } else {
        URL = '../../MaintainDL/UploadFile';
    }
    
    var fileInput = document.getElementById('FileUpload1');
    var file = fileInput.files[0];
    var listname = $('#txtDistlistName').val();

    var formData = new FormData();
    formData.append('file', file);
    formData.append('text', listname);

    $.ajax({
        url: URL,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log(response);
            $('#response').text(response+' Records '+action+' successfully');
            $('#FileUpload1').val('');
            $('#txtDistlistName').val('');
        }
    });
}

function UpdateDistList(ListName) {
    window.location.href = '../MaintainDL/UpdateDL/?ListName='+ListName;
}

function UpdateDL() {
    IsValid = FormValidation();
    if (IsValid) {
        uploadFile('appended');
    }
}

function ViewDistList(ListName){
    window.location.href = '../MaintainDL/ViewDL/?ListName=' + ListName;
}

function ViewDL(listname) {

    var param = {
        ListName: listname
    }

    $.ajax({
        url: "../../MaintainDL/GetPhoneNumbers",
        type: "POST",
        data: param,
        dataType: 'json',
        success: function (response) {

            console.log(response);

            if (isViewTableExist != null || isViewTableExist != undefined) {
                isViewTableExist.clear().rows.add(response).draw();
            }
            else {
                isViewTableExist = $('#tblphoneNumber').DataTable({
                    "data": response,
                    "paginate": true,
                    "bInfo": false,
                    "bFilter": false,
                    "searching": false,
                    "ordering": false,
                    "lengthMenu": false,
                    "initComplete": function (settings, json) {
                        $("#divLoading").hide();
                    },
                    columnDefs: [{
                        className: 'no-width',
                    }],
                    columns: [
                        {
                            data: null, className: 'no-width',
                        },
                    ]
                });
            }
        },

        error: function (response) {

        }
    });
}

function ExportDL() {

    $.ajax({
        url: '../MaintainDL/ExportDL',
        type: 'POST',
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

            var url = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
            link.href = url;
            link.download = 'ExistingList.xlsx';
            link.click();

            window.URL.revokeObjectURL(url);

            console.log("Download Completed..");
        },

        error: function (response) {

        }
    })
}

function DeleteSelected() {
    $('#selectTODelete').hide();
    $('.dl-checkbox:checked').each(function () {
        var Listname = $(this).closest('tr').find('td:eq(1)').text();

        DeleteDistList(Listname);
    })
}