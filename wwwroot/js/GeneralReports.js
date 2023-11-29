var isDataTableExist;
var isFailedMsgDataTableExist;
var isSmsDetailExist;

$(function () {
    console.log("Reports", getUser());

    HideErrorLabels();
    RenderJqueryDates();
    CheckRole(getUser().Role);

    $('#chkboxSelectAll').on('click', function () {

        var isChecked = $(this).prop('checked');

        $('#GridDeptView tr input[type="checkbox"]').prop('checked', isChecked);
        if (isChecked) {
            var companyIDs = [];
            $('#GridDeptView tr').each(function () {
                var companyId = $(this).find('td:nth-child(2)').text();
                companyIDs.push(companyId);
            });
            LoadAllDeptUser(companyIDs);
        }
        else {
            $('#GridDeptView tr').each(function () {
                var companyId = $(this).find('td:nth-child(2)').text();
                UnLoadDeptUser(companyId);
            });
        }
    });

    $('#chkboxSelectAllUser').on('click', function () {

        var isChecked = $(this).prop('checked');

        $('#GridDeptUsersView tr input[type="checkbox"]').prop('checked', isChecked);
        if (isChecked) {
            $('#GridDeptUsersView tr').each(function () {
                $('#DeptUserCheckbox').prop('checked', true);
            });
        }
    });

    $(document).on('click', '.DeptCheckbox', function () {
        if ($(this).prop('checked')) {
            const CompanyId = $(this).closest('tr').find('td:eq(1)').text();
            LoadDeptUser(CompanyId);
        }
        else {
            $('#chkboxSelectAll').prop('checked', false);
            const CompanyId = $(this).closest('tr').find('td:eq(1)').text();
            UnLoadDeptUser(CompanyId);
        }
        var allChecked = $('.DeptCheckbox:checked').length === $('.DeptCheckbox').length;
        $('#chkboxSelectAll').prop('checked', allChecked);
    });

    $(document).on('click', '.DeptUserCheckbox', function () {
        var isChecked = $(this).prop('checked');
        if (!isChecked) {
            $('#chkboxSelectAllUser').prop('checked', false);
        }
        var allChecked = $('.DeptUserCheckbox:checked').length === $('.DeptUserCheckbox').length;
        $('#chkboxSelectAllUser').prop('checked', allChecked);
    });

    $(document).on('click', '.smsusagemodel', function (event) {
        var row = $(this).parent().parent();
        var EmpId = row.find('td:first-child').text();
        event.preventDefault();
        FailedMsgPopup(EmpId);
    });

    $(document).on('click', '.btn-view', function (event) {
        var smsId = event.target.value;
        event.preventDefault();
        MessageDetailPopup(smsId);
    });

});

function RenderJqueryDates() {
    $('#txtFromDate').datepicker({
        dateFormat: "dd-mm-yy",
        showOn: 'none',
        changeMonth: true,
        changeYear: true,
        maxDate: 0,
        onSelect: function (selectedDate) {
            var fromDate = $(this).datepicker("getDate");

            $('#txtToDate').datepicker('option', 'minDate', fromDate);
        }
    });
    $('#txtToDate').datepicker({
        dateFormat: "dd-mm-yy",
        showOn: 'none',
        changeMonth: true,
        changeYear: true,
        maxDate: 0,
    });

    $('#imgFromdate').on('click', function () {
        $('#txtFromDate').datepicker('show');
    });
    $('#imgToDate').on('click', function () {
        $('#txtToDate').datepicker('show');
    });

}

function HideErrorLabels() {
    $('#errlblDept').hide();
    $('#errlblDeptuser').hide();
    $('#errlblFromdate').hide();
    $('#errlblTodate').hide();
    $('#NoRecordFound').hide();
    $('.Reportresult').hide();
}

function CheckRole(Role) {
    if (Role == 2) {
        $('#tblUsersDept').show();
        $('#chkboxSelectAll').hide();
        //$('#pnlContainerDeptUsersView').hide();
        LoadDepartments();
        LoadDeptUser(getUser().DEPARTMENTID);
    }
    if (Role == 3) {
        $('#tblUsersDept').show();
        $('#pnlContainerDeptUsersView').hide();
        LoadDepartments();
    }
}

function LoadDepartments() {

    var Role = getUser().Role;
    var DeptId = {
        DeptId: getUser().DEPARTMENTID
    }
    var EmpId = {
        EmpId: getUser().EmpID
    }

    if (Role == 3) {
        $.ajax({
            url: "../GeneralReport/GetDepartments",
            type: "POST",
            data: DeptId,
            success: function (_response) {
                PopulateDepartments(_response);
            }
        });
    }
    else if (Role == 2) {
        $.ajax({
            url: "../GeneralReport/GetDepartmentsForAdmin",
            type: "POST",
            data: EmpId,
            success: function (_response) {
                PopulateDepartments(_response);
                $('#DepartmentTablebody').find('.DeptCheckbox').prop('checked', true);
            }
        });
    }
    else {
        $.ajax({
            url: "../GeneralReport/GetDepartments",
            type: "POST",
            data: DeptId,
            success: function (_response) {
                PopulateDepartments(_response);
            }
        });
    }
}

function PopulateDepartments(Department) {

    var tableBody = $('#DepartmentTablebody');

    tableBody.empty();

    $.each(Department, function (index, item) {
        var row = $('<tr>');
        row.append($('<td>').html('<input type="checkbox" class="DeptCheckbox">'));
        row.append($('<td>').text(item.departmentid));
        row.append($('<td>').text(item.department));

        tableBody.append(row);
    });

}

function LoadDeptUser(CompanyId) {

    var companyId = {
        DeptId: CompanyId
    }

    $.ajax({
        url: "../GeneralReport/GetDeptUsers",
        type: "POST",
        data: companyId,
        success: function (_response) {
            PopulateDeptUsers(_response);
        }
    });
}

function PopulateDeptUsers(users) {

    $('#pnlContainerDeptUsersView').show();
    $('#ErrlblDeptUser').hide();

    var tableBody = $('#DeptUserTablebody');
    //  tableBody.empty();

    $.each(users, function (index, item) {
        var row = $('<tr>');
        row.append($('<td>').addClass('hidden').text(item.departmentid));
        row.append($('<td>').html('<input type="checkbox" class="DeptUserCheckbox">'));
        row.append($('<td>').text(item.empID));
        row.append($('<td>').text(item.firstname));
        row.append($('<td>').text(item.department));
        tableBody.append(row);
    });
}

function UnLoadDeptUser(companyId) {

    var row = $('#DeptUserTablebody tr').filter(function () {
        return $(this).find('td.hidden').text() == companyId;
    });

    row.remove();

    var tableRow = $('#DeptUserTablebody tr').length;
    //console.log('table', tableRow);

    $('#GridDeptView tr').each(function () {
        var DeptId = $(this).find('td:nth-child(2)').text();
        var checkbox = $(this).find('.DeptCheckbox');
        if (DeptId == companyId) {
            checkbox.prop('checked', false);
        }
    });

    if (tableRow == 0) {
        $('#pnlContainerDeptUsersView').hide();
        $('#ErrlblDeptUser').show();
        $('#chkboxSelectAllUser').prop('checked', false);
    }
}

function LoadAllDeptUser(companyIDs) {

    console.log(JSON.stringify({ 'DeptIds': companyIDs }));

    $.ajax({
        url: "../GeneralReport/GetAllDeptUsers",
        type: "POST",
        traditional: true,
        dataType: 'json',
        data: JSON.stringify({ 'DeptIds': companyIDs }),
        contentType: 'application/json; charset=utf-8',
        success: function (_response) {
            PopulateDeptUsers(_response);
        }
    });
}

function SubmitReport() {
    HideErrorLabels();

    var IsValid = ClientValidation();

    if (!IsValid)
        return;

    var param = {
        FromDate: $('#txtFromDate').val(),
        ToDate: $('#txtToDate').val(),
        EmpIds: getEmpIds(),
        DeptIds: getDeptIds()
    }

    LoadSMSUsageData(param);

    LoadSMSUsageDetails(param);

}

function LoadSMSUsageData(param) {
    $.ajax({
        url: "../GeneralReport/GenerateReport",
        type: "POST",
        data: JSON.stringify(param),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
           // console.log(response);
            $('.Reportresult').show();
            //if (response.length > 0)
            //    $('.Reportresult').show();
            //else
            //    $('#NoRecordFound').show();

            var IsPaginate = response.length <= 5 ? false : true;

            if (isDataTableExist != null || isDataTableExist != undefined) {
                //isDataTableExist.clear().rows.add(response).draw();
                $('#GridViewReportResult').DataTable().destroy();
            }
            isDataTableExist = $('#GridViewReportResult').DataTable({
                "data": response,
                "paginate": IsPaginate,
                "bInfo": IsPaginate,
                "bFilter": false,
                "searching": false,
                "ordering": false,
                "lengthMenu": false,
                "iDisplayLength": 5,
                "initComplete": function (settings, json) {
                    $("#divLoading").hide();
                },
                //ajax:
                //{
                //    url: "/GeneralReport/GenerateReport",
                //    type: "POST",
                //    data: function (JsonData) {
                //        JsonData.FromDate = $('#txtFromDate').val();
                //        JsonData.ToDate = $('#txtToDate').val();
                //        JsonData.EmpIds = getEmpIds();
                //        JsonData.DeptIds = getDeptIds();
                //    },
                //    //dataType: 'json',
                //    //contentType: 'application/json; charset=utf-8',
                //    dataSrc: "",

                //},
                columnDefs: [
                    {
                        className: 'expand-column',
                    }],
                columns: [
                    {
                        data: 'empID', className: 'expand-column',
                    },
                    {
                        data: 'userName', className: 'expand-column',
                    },
                    {
                        data: 'totalMessages', className: 'expand-column',
                    },
                    {
                        data: 'failedMessages', className: 'expand-column',
                        render: function (data, type, row, meta) {
                            return '<input type="button" class="smsusagemodel" value="' + data + '" />';
                        }
                    },
                ]
            });


        },

        error: function (response) {

        }
    });
}

function LoadSMSUsageDetails(param) {
    $.ajax({
        url: "../GeneralReport/GenerateSmsDetailReport",
        type: "POST",
        data: JSON.stringify(param),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            
            //if (response.length > 0)
            //    $('.Reportresult').show();
            //else
            //    $('#NoRecordFound').show()


            var IsPaginate = response.length <= 5 ? false : true;

            if (isSmsDetailExist != null || isSmsDetailExist != undefined) {
                $('#GridViewSmsDetail').DataTable().destroy();
            }

            isSmsDetailExist = $('#GridViewSmsDetail').DataTable({
                "data": response,
                "paginate": IsPaginate,
                "bInfo": IsPaginate,
                "bFilter": false,
                "searching": false,
                "ordering": false,
                "lengthMenu": false,
                "iDisplayLength": 5,
                "initComplete": function (settings, json) {
                    $("#divLoading").hide();
                },
                columnDefs: [
                    {
                        className: 'expand-column',
                    }],
                columns: [
                    {
                        data: 'sentBy', className: 'expand-column',
                    },
                    {
                        data: 'messages', className: 'expand-column',
                    },
                    {
                        data: 'dateTimeSent', className: 'expand-column',
                    },
                    {
                        data: 'smsID', className: 'expand-column',
                        render: function (data, type, row, meta) {
                            return '<button type="button" class="btn-view" value="' + data + '">View</button>';
                        }
                    },
                ]
            });


        },

        error: function (response) {

        }
    });
}

function ClientValidation() {
    var isValid = true;
    var DeptIds = getDeptIds();
    var EmpIds = getEmpIds();
    var role = getUser().Role;

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();

    if (fromDate == '') {
        $('#errlblFromdate').show();
        isValid = false;
    }
    if (toDate == '') {
        $('#errlblTodate').show();
        isValid = false;
    }

    if (role != 1) {
        if (DeptIds.length == 0) {
            $('#errlblDept').show();
            isValid = false;
        }

        if (EmpIds.length == 0) {
            $('#errlblDeptuser').show();
            isValid = false;
        }
    }

    return isValid;
}

function getDeptIds() {
    var DeptIds = [];

    $('#GridDeptView input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {
            var id = $(this).closest('tr').find('td:eq(1)').text();
            DeptIds.push(id);
        }
    });

    return DeptIds;
}

function getEmpIds() {
    var EmpIds = [];
    $('#GridDeptUsersView input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {
            var id = $(this).closest('tr').find('td:eq(2)').text();
            EmpIds.push(id);
        }
    });
    return EmpIds;
}

function FailedMsgPopup(empId) {
    var param = {
        FromDate: $('#txtFromDate').val(),
        ToDate: $('#txtToDate').val(),
        EmpId: empId,
    }

    $("#div_popup").dialog({
        "modal": true,
        position: { my: 'top', at: 'top', of: window },
        width: 900,
        height: 650,
        open: function (event, ui) {
            var titleBar = $(this).closest('.ui-dialog').find('.ui-dialog-titlebar');

            var img = $('#img-title');

            titleBar.append(img);
        }
    });

    $.ajax({
        url: "../GeneralReport/GetFailedMsgDetails",
        type: "POST",
        data: param,
        success: function (response) {

            var IsPaginate = response.length <= 10 ? false : true;

            if (isFailedMsgDataTableExist != null || isFailedMsgDataTableExist != undefined) {
                //isFailedMsgDataTableExist.clear().rows.add(response).draw();
                $('#GridViewFailedReport').DataTable().destroy();
            }

            isFailedMsgDataTableExist = $('#GridViewFailedReport').DataTable({
                "data": response,
                "paginate": IsPaginate,
                "bInfo": IsPaginate,
                "bFilter": false,
                "searching": false,
                "ordering": false,
                "lengthMenu": false,
                "iDisplayLength": 5,
                "initComplete": function (settings, json) {
                    $("#divLoading").hide();
                },
                columnDefs: [
                    {
                        className: 'expand-column',
                    }],
                columns: [
                    {
                        data: 'msg_Title',
                    },
                    {
                        data: 'phoneNr', className: 'expand-column',
                    },
                    {
                        data: 'msgstatus', className: 'expand-column',
                    },
                    {
                        data: 'dateTimeResp', className: 'expand-column',
                    },
                    {
                        data: 'messageText',
                    },
                ]
            });

        },

        error: function (response) {

        }
    });


}

function closeModal() {
    $("#div_popup").dialog('close');
}

function MessageDetailPopup(smsId) {
    console.log(smsId);

    var param = {
        smsID: smsId
    }

    $("#div_popup").dialog({
        "modal": true,
        position: { my: 'top', at: 'top', of: window },
        width: 900,
        height: 750,
        open: function (event, ui) {
            var titleBar = $(this).closest('.ui-dialog').find('.ui-dialog-titlebar');

            var img = $('#img-title');

            titleBar.append(img);
        }
    });

    $.ajax({
        url: "../GeneralReport/GetMessageDetails",
        type: "POST",
        data: param,
        success: function (response) {

            var IsPaginate = response.length <= 10 ? false : true;

            if (isFailedMsgDataTableExist != null || isFailedMsgDataTableExist != undefined) {
                $('#GridViewFailedReport').DataTable().destroy();
            }

            isFailedMsgDataTableExist = $('#GridViewFailedReport').DataTable({
                "data": response,
                "paginate": IsPaginate,
                "bInfo": IsPaginate,
                "bFilter": false,
                "searching": false,
                "ordering": false,
                "lengthMenu": false,
                "iDisplayLength": 5,
                "initComplete": function (settings, json) {
                    $("#divLoading").hide();
                },
                columnDefs: [
                    {
                        className: 'expand-column',
                    }],
                columns: [
                    {
                        data: 'msg_Title',
                    },
                    {
                        data: 'phoneNr', className: 'expand-column',
                    },
                    {
                        data: 'msgstatus', className: 'expand-column',
                    },
                    {
                        data: 'dateTimeResp', className: 'expand-column',
                    },
                    {
                        data: 'msgContent',
                    },
                ]
            });

        },

        error: function (response) {

        }
    });

}

function exportsmsReport() {
    var param = {
        FromDate: $('#txtFromDate').val(),
        ToDate: $('#txtToDate').val(),
        EmpIds: getEmpIds(),
        DeptIds: getDeptIds()
    }

    $.ajax({
        url: "../GeneralReport/ExportSmsReport",
        type: "POST",
        data: JSON.stringify(param),
        contentType: 'application/json; charset=utf-8',
        xhrFields: {
            responseType:'blob'
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

            var url = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
            link.href = url;
            link.download = 'SMSUsage Report.xlsx';
            link.click();

            window.URL.revokeObjectURL(url);

            console.log("Download Completed..");
        },

        error: function (response) {

        }
    });
}

function exportsmsDetailReport() {
    var param = {
        FromDate: $('#txtFromDate').val(),
        ToDate: $('#txtToDate').val(),
        EmpIds: getEmpIds(),
        DeptIds: getDeptIds()
    }

    $.ajax({
        url: "../GeneralReport/ExportSmsDetailReport",
        type: "POST",
        data: JSON.stringify(param),
        contentType: 'application/json; charset=utf-8',
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

            var url = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
            link.href = url;
            link.download = 'SMSUsage Detail Report.xlsx';
            link.click();

            window.URL.revokeObjectURL(url);

            console.log("Download Completed..");
        },

        error: function (response) {

        }
    });
}

function reset() {
    HideErrorLabels();
    $('#txtFromDate').val('');
    $('#txtToDate').val('');
    $('td input[type="checkbox"]').prop('checked', false);

}