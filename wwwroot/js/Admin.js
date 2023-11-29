var SelectedRow;
var isDataTableExist;
var UpdateIsValid = true;
var InsertIsValid = true;
var currentPage = 1;

$(document).ready(function () {
    // Load user data on page load
    // loadUsersData();
    getPagination();

    // Search button click event
    $('#BtnSearch').click(function () {
        var first = $('#TxtFirstname').val();
        var last = $('#TxtLastname').val();
        searchfristlastname(first, last);
    });

    $('#grAdmin').on("click", ".editBtn", function () {
        if (SelectedRow) {
            $('.cancelBtn').click();
        }
        var currentRow = $(this).closest('tr');
        // Clone the row
        var editableRow = currentRow.clone();
        var DeptId;
        // Replace cell content with editable elements
        editableRow.find('td').each(function () {
            var cellText = $(this).text();
            var cellIndex = $(this).index();
            var td = $(this);

            if (cellIndex == 5)
                DeptId = cellText;
            // For the second cell (index 1), replace with a select element
            if (cellIndex === 9) {
                var select = $('<select class="editRow">');
                td.html(select);
                $.when(GetTeamDetails(DeptId)).then(function (response) {
                    //console.log(response);
                    // var select = $('<select class="editRow">');
                    // Populate select with options from the response data
                    for (var i = 0; i < response.length; i++) {
                        select.append($('<option/>', {
                            value: response[i].id,
                            text: response[i].team_Name
                        }));
                    }
                });
            }
            else if (cellIndex === 10) {
                var select = $('<select class="editRow">');
                td.html(select);
                $.when(GetRole()).then(function (response) {

                    // var select = $('<select class="editRow">');

                    for (var i = 0; i < response.length; i++) {
                        select.append($('<option/>', {
                            value: response[i].rol_id,
                            text: response[i].rol_role
                        }));
                    }
                });
            }
            else if (cellIndex === 11) {
                $(this).html('<button class="updateBtn editRow">Update</button><br/><button class="cancelBtn editRow">Cancel</button>')
            }
            else if (cellIndex === 12) {
                return true;
            }
            else {
                $(this).html('<input class="editRow" type="text" value="' + cellText + '">');
            }
        });
        currentRow.replaceWith(editableRow);
        SelectedRow = currentRow;
    });

    $('#grAdmin').on("click", ".cancelBtn", function () {

        var currentRow = $(this).closest('tr');

        currentRow.replaceWith(SelectedRow);
        SelectedRow = undefined;

        UpdateIsValid = true;
        InsertIsValid = true;

        //SelectedRow.find('td').each(function () {
        //    var value = $(this);

        //   // console.log(value.text(), "edit");
        //})
    });

    $('#grAdmin').on("click", ".updateBtn", function () {
        var updatedValues = [];
        var invalidCount = 0;
        var row = $(this).closest('tr');

        var inputs = row.find('input');
        var selects = row.find('select');


        selects.each(function () {
            var currentValue = $(this).val();
            if (currentValue == 0) {
                if (UpdateIsValid) {
                    $(this).after('<span class="td-error">Please select</span>');
                }
                invalidCount++;
            }
        });


        if (invalidCount != 0)
            UpdateIsValid = false;
        else
            UpdateIsValid = true;

        if (UpdateIsValid) {
            $('.td-error').remove();
            inputs.each(function () {
                var currentValue = $(this).val();
                updatedValues.push(currentValue);
            });
            selects.each(function () {
                var currentText = $(this).find('option:selected').text();
                updatedValues.push(currentText);
            });

            var param = {
                ID: updatedValues[0],
                logonid: updatedValues[1],
                firstname: updatedValues[2],
                lastname: updatedValues[3],
                costcentre: updatedValues[4],
                departmentid: updatedValues[5],
                department: updatedValues[6],
                r_role: updatedValues[7],
                phonenumber: updatedValues[8],
                Team_Name: updatedValues[9],
                rol_role: updatedValues[10]
            }

            $.ajax({
                url: '../Admin/UpdateUser',
                method: 'POST',
                data: param,
                dataType: 'json',
                success: function (response) {
                    //console.log(response.success);
                    if (response.success) {
                        selects.each(function () {
                            var currentText = $(this).find('option:selected').text();
                            $(this).replaceWith(currentText);
                        });
                        inputs.each(function () {
                            var currentValue = $(this).val();
                            $(this).replaceWith(currentValue);
                        });
                        row.find('td').each(function () {
                            var cellIndex = $(this).index();

                            if (cellIndex === 11) {
                                $(this).html("<button class='editBtn'>Edit</button>");
                            }
                        });
                        alert('User Updated!');
                    }
                    else {
                        alert('Update failed!')
                    }
                }
            });


            // UpdateValueToDb(updatedValues);
        }
    });

    $('#grAdmin').on("click", ".saveBtn", function () {
        var invalidCount = 0;
        var row = $(this).closest('tr');

        var inputs = row.find('input');
        var selects = row.find('select');

        selects.each(function () {
            var currentValue = $(this).val();
            if (currentValue == 0 || currentValue == undefined) {
                if (InsertIsValid) {
                    $(this).after('<span class="td-error">Please select</span>');
                }
                invalidCount++;
            }
        });

        if (invalidCount != 0)
            InsertIsValid = false;
        else
            InsertIsValid = true;

        if (InsertIsValid) {

            var param = {
                ID: '',
                logonid: $('#insert-logon').val(),
                firstname: $('#insert-fname').val(),
                lastname: $('#insert-lname').val(),
                costcentre: $('#insert-costcentre').val(),
                departmentid: $('#insert-departmentid').val(),
                department: $('#insert-department').val(),
                r_role: $('#insert-role').val(),
                phonenumber: $('#insert-phonenumber').val(),
                Team_Name: $('#insert-team').val(),
                rol_role: ''  //not used in SP
            }

            //if ($('#insert-team').val() != 'Others') {
            //    nontemplateusers();
            //}

            $.ajax({
                url: '../Admin/InsertUser',
                method: 'POST',
                data: param,
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        alert('User Inserted');
                        loadUsersData();
                    }
                    else {
                        alert('Login Exists, Failed to Insert!')
                    }
                }
            });

            //$.ajax({
            //    url: '../Admin/CheckExist',
            //    method: 'POST',
            //    data: param,
            //    dataType: 'json',
            //    success: function (response) {
            //        if (response.success) {

            //            $.ajax({
            //                url: '../Admin/InsertUser',
            //                method: 'POST',
            //                data: param,
            //                dataType: 'json',
            //                success: function (response) {
            //                    if (response.success) {
            //                        alert('User Inserted');
            //                        loadUsersData();
            //                    }
            //                    else {
            //                        alert('User insertion failed!')
            //                    }
            //                }
            //            });
            //        }
            //        else {
            //            $('.saveBtn').after('<span class="td-error">Login Exists</span>')
            //        }
            //    }
            //});
        }
    });

    function nontemplateusers() {
        alert("Team must be selected as others for non template users");
        return false;
    }

    $('#btnInsert').click(function () {
        // Create an empty row with input fields

        insertRow = $('#insertRow').length;

        if (insertRow == 0) {
            var emptyRow = "<tr id='insertRow'>" +
                "<td hidden ></td>" +
                "<td>" +
                "<input id='insert-logon' type='text' class='FindUser editRow' />" +
                "<button class='btnFindUser editRow'>Find User</button>" +
                "</td>" +
                "<td><input id='insert-fname' type='text' class='find-user-input editRow' /></td>" +
                "<td><input id='insert-lname' type='text' class='find-user-input editRow' /></td>" +
                "<td><input id='insert-costcentre' type='text' class='find-user-input editRow' /></td>" +
                "<td><input id='insert-departmentid' type='text' class='find-user-input editRow' /></td>" +
                "<td><input id='insert-department' type='text' class='find-user-input editRow' /></td>" +
                "<td><input id='insert-phonenumber' type='text' class='find-user-input editRow' /></td>" +
                "<td><select id='insert-team' class='find-user-input editRow'></select></td>" +
                "<td><select id='insert-role' class='find-user-input editRow'></select></td>" +
                "<td><button class='saveBtn btn btn-primary'>Save</button></td>" +
                "<td><button class='cancelBtn btn btn-secondary'>Cancel</button></td>" +
                "</tr>";

            // Replace the insert button row with the empty row
            $('#grAdminBody tr:last').after(emptyRow);
        }
    });

    $(document).on("click", ".deleteBtn", function () {
        var row = $(this).closest("tr");
        var empid = row.find("td:eq(1)").text();
        if (empid == '') {
            empid = row.find("td:eq(1) :input:first").val();
            $('.cancelBtn').click();
        }
        // var update_by = "SYSTEMS\K7905713"; // Replace with the appropriate value
        var update_by = getUser().EmpID;
        // Display a confirmation dialog
        if (confirm("Are you sure you want to delete?")) {
            // Perform delete operation
            DeleteUser(empid, update_by);
        }
    });

    $(document).on('click', '.btnFindUser', function () {
        $('#txtfname').val('');
        $('#txtlname').val('');
        $('.error-label').hide();
        $('.warning-label').hide();
        $('#colleage-content').hide();
        $('#Findser').dialog({
            modal: true,
            position: { my: "left top", at: "left top", of: window },
            title: "Select a User",
            Height: 350,
            width: 450,
            minHeight: 350,
            open: function (event, ui) {
                // Add CSS styles to the dialog container
                $(this).parent().css({
                    "max-height": "350px", // Set the maximum height
                    "overflow-y": "auto" // Enable vertical scrollbar
                });
            }
        })
    });

    $(document).on('click', '.name-link', function () {
        var row = $(this).closest('tr');

        var logon = row.find('td:eq(1)').text();
        var fname = row.find('td:eq(2)').text();
        var lname = row.find('td:eq(3)').text();
        var costcentre = row.find('td:eq(4)').text();
        var departmentid = row.find('td:eq(5)').text();
        var department = row.find('td:eq(6)').text();
        var phonenumber = row.find('td:eq(7)').text();

        $('#insert-logon').val(logon);
        $('#insert-fname').val(fname);
        $('#insert-lname').val(lname);
        $('#insert-costcentre').val(costcentre);
        $('#insert-departmentid').val(departmentid);
        $('#insert-department').val(department);
        $('#insert-phonenumber').val(phonenumber);

        $.when(GetTeamDetails(departmentid)).then(function (response) {
            var select = $('#insert-team');
            select.empty();
            // Populate select with options from the response data
            for (var i = 0; i < response.length; i++) {
                select.append($('<option/>', {
                    value: response[i].id,
                    text: response[i].team_Name
                }));
            }
        });

        $.when(GetRole()).then(function (response) {
            var select = $('#insert-role');
            select.empty();
            for (var i = 0; i < response.length; i++) {
                select.append($('<option/>', {
                    value: response[i].rol_id,
                    text: response[i].rol_role
                }));
            }
        });

        $('#Findser').dialog("close").dialog('destroy');
    });

    $(document).on('click', '.cancelBtn', function () {
        $('#insertRow').remove();
    })
});


function SearchUser() {
    fname = $('#txtfname').val();
    lname = $('#txtlname').val();

    if (fname == '' && lname == '') {
        $('.error-label').show();
        return;
    }
    $('.error-label').hide();
    

    var param = {
        firstname: fname,
        lastname: lname
    }

    $.ajax({
        url: '../Admin/GetColleagues',
        method: 'POST',
        data: param,
        dataType: 'json',
        success: function (response) {

            if (response.length <= 0) {
                $('.warning-label').show();
                $('#colleage-content').hide();
                return;
            }
            $('.warning-label').hide();
            $('#colleage-content').show();
            var tbody = $('#colleague-tbody');
            tbody.empty();

            $.each(response, function (index, item) {
                var row = $('<tr>');
                row.append($('<td class="name-link">').text(item.name));
                row.append($('<td>').text(item.login));
                row.append($('<td hidden>').text(item.fname));
                row.append($('<td hidden>').text(item.lname));
                row.append($('<td hidden>').text(item.costcentre));
                row.append($('<td hidden>').text(item.departmentid));
                row.append($('<td hidden>').text(item.department));
                row.append($('<td hidden>').text(item.phonenumber));

                tbody.append(row);
            });
        }

    });
}

function GetTeamDetails(deptId) {
    var DeptID = {
        DeptID: deptId
    }

    var response = $.ajax({
        url: '../Admin/GetTeamDetails',
        method: 'POST',
        data: DeptID,
        dataType: 'json',
    });

    return response;
}

function GetRole() {
    var response = $.ajax({
        url: '../Admin/GetRoleDetails',
        method: 'GET',
        dataType: 'json',
    });

    return response;
}

// Function to load users data
function loadUsersData() {
    $.ajax({
        url: "../Admin/LoadUsersData",
        type: "GET",
        dataType: "json",
        success: function (response) {
            updateTable(response);
        },
        error: function (response) {
            // Handle error
        }
    });

    //var tableBody = $("#grAdminBody");
    //if (isDataTableExist != null || isDataTableExist != undefined) {
    //    isDataTableExist.clear();
    //    $('#grAdmin').DataTable().destroy();
    //}
    // Clear existing table rows
    // tableBody.empty();

    //isDataTableExist = $('#grAdmin').DataTable({
    //    serverSide: true,
    //    paging: true, // Enable pagination
    //    pageLength: 10, // Set number of rows per page
    //    sorting: false,
    //    searching: false,
    //    ajax: {
    //        url: "../Admin/LoadUsersData",
    //        type: "POST",
    //        dataType: "json",
    //        contentType: 'application/json',
    //        data: function (d) {
    //            //d.draw = d.Draw;
    //            //d.length = d.Length;
    //            //d.start = d.Start;
    //            var data = {
    //                Draw: d.draw,
    //                Length: d.length,
    //                Start: d.start
    //            };
    //            return JSON.stringify(data);
    //        },
    //        dataSrc:'data',
    //    },
    //    columnDefs: [
    //        {
    //            targets: [0, 7],
    //            visible: false,
    //        }
    //    ],
    //    columns: [
    //        {
    //            data: 'id', className: 'center',
    //        },
    //        {
    //            data: 'logonid', className: 'center',
    //        },
    //        {
    //            data: 'firstname', className: 'center',
    //        },
    //        {
    //            data: 'lastname', className: 'center',
    //        },
    //        {
    //            data: 'costcentre', className: 'center',
    //        },
    //        {
    //            data: 'departmentid', className: 'center',
    //        },
    //        {
    //            data: 'department', className: 'center',
    //        },
    //        {
    //            data: 'r_role', className: 'center',
    //        },
    //        {
    //            data: 'phonenumber', className: 'center',
    //        },
    //        {
    //            data: 'team_Name', className: 'center',
    //        },
    //        {
    //            data: 'rol_role', className: 'center',
    //        },
    //        {
    //            data: null, className: 'center',
    //            render: function (data, type, row, meta) {
    //                return '<button class="editBtn">Edit</button>';
    //            }
    //        },
    //        {
    //            data: null, className: 'center',
    //            render: function (data, type, row, meta) {
    //                return '<button class="deleteBtn">Delete</button>';
    //            }
    //        },
    //    ],
    //    language: {
    //        infoFiltered:""
    //    }
    //});

    //if (isDataTableExist) {
    //    $('#grAdmin').removeClass('dataTable');
    //}
}

// Function to search by first name and last name
function searchfristlastname(first, last) {

    if (first == '' & last == '') {
        loadUsersData();
        return;
    }


    $.ajax({
        url: "../Admin/SearchFirstLastName",
        type: "GET",
        data: { first: first, last: last },
        dataType: "json",
        success: function (response) {
            if (response.length > 0) {
                // User(s) found, update the table
                updateTable(response);
            } else {
                // User not found, show alert
                alert("User Not Found Please try using some other Keyword");
            }
        },
        error: function (response) {
            // Handle error
        }
    });
}

function DeleteUser(empid, update_by) {
    $.ajax({
        url: "../Admin/DeleteUser",
        type: "POST",
        data: { empid: empid, update_by: update_by },
        dataType: "json",
        success: function (response) {
            //updateTable(response);
            loadUsersData();
            // User deleted successfully
            alert("User has been deleted");
        },
        error: function (response) {
            // Handle error
        }
    });
}

// Function to update the table with data
function updateTable(data) {
    var tableBody = $("#grAdminBody");
    //if (isDataTableExist != null || isDataTableExist != undefined) {
    //    isDataTableExist.clear();
    //    $('#grAdmin').DataTable().destroy();
    //}
    // Clear existing table rows
    tableBody.empty();

    for (var i = 0; i < data.length; i++) {
        var row = "<tr>" +
            "<td hidden>" + data[i].id + "</td>" +
            "<td>" + data[i].logonid + "</td>" +
            "<td>" + data[i].firstname + "</td>" +
            "<td>" + data[i].lastname + "</td>" +
            "<td>" + data[i].costcentre + "</td>" +
            "<td>" + data[i].departmentid + "</td>" +
            "<td>" + data[i].department + "</td>" +
            "<td hidden>" + data[i].r_role + "</td>" +
            "<td>" + data[i].phonenumber + "</td>" +
            "<td>" + data[i].team_Name + "</td>" +
            "<td>" + data[i].rol_role + "</td>" +
            "<td><button class='editBtn'>Edit</button></td>" +
            "<td><button class='deleteBtn'>Delete</button></td>" +
            "</tr>";

        tableBody.append(row);
    }

    getPagination();

    //isDataTableExist = $('#grAdmin').DataTable({
    //    data: data,
    //    paging: true, // Enable pagination
    //    pageLength: 10, // Set number of rows per page
    //    sorting: false,
    //    searching: false,
    //    columnDefs: [
    //        {
    //            targets: [0, 7],
    //            visible: false,
    //        }
    //    ],
    //    columns: [
    //        {
    //            data: 'id', className: 'center',
    //        },
    //        {
    //            data: 'logonid', className: 'center',
    //        },
    //        {
    //            data: 'firstname', className: 'center',
    //        },
    //        {
    //            data: 'lastname', className: 'center',
    //        },
    //        {
    //            data: 'costcentre', className: 'center',
    //        },
    //        {
    //            data: 'departmentid', className: 'center',
    //        },
    //        {
    //            data: 'department', className: 'center',
    //        },
    //        {
    //            data: 'r_role', className: 'center',
    //        },
    //        {
    //            data: 'phonenumber', className: 'center',
    //        },
    //        {
    //            data: 'team_Name', className: 'center',
    //        },
    //        {
    //            data: 'rol_role', className: 'center',
    //        },
    //        {
    //            data: null, className: 'center',
    //            render: function (data, type, row, meta) {
    //                return '<button class="editBtn">Edit</button>';
    //            }
    //        },
    //        {
    //            data: null, className: 'center',
    //            render: function (data, type, row, meta) {
    //                return '<button class="deleteBtn">Delete</button>';
    //            }
    //        },
    //    ]
    //});

    //if (isDataTableExist) {
    //    $('#grAdmin').removeClass('dataTable');
    //}
}

function getPagination() {

    var maxRows = 10; //parseInt($('#maxRows').val()); // get Max Rows from select option
    var totalRows = $('#grAdmin' + ' tbody tr').length; // numbers of rows
    var totalpages = Math.ceil(totalRows / maxRows);

    //console.log(totalpages);
    createPagination(totalpages, currentPage);

}

function createPagination(totalpages, page) {
    const ulTag = document.querySelector('.pagination');

    if (page > 0 && page <= totalpages) {
        currentPage = page;
    }

    let liTag = '';
    let beforepages = currentPage - 2;
    let afterpages = currentPage + 1;
    let activeLi = '';
    let disablePreviousButton = 'disabled';
    let disableNextButton = 'disabled';
    if (currentPage > 1) {
        disablePreviousButton = '';
    }
    liTag += `<li class="page-item ${disablePreviousButton}" onclick="createPagination(${totalpages},${currentPage - 1})"><span class="page-link">Prev</span></li>`;

    if (currentPage >= 1) {
        if (currentPage == 1) {
            activeLi = "active";
        }
        liTag += `<li class="page-item" onclick="createPagination(${totalpages},1)"><span class="page-link  ${activeLi}">1</span></li>`;
    }

    for (let pageLength = beforepages; pageLength <= afterpages; pageLength++) {
        if (pageLength > totalpages) {
            continue;
        }
        if (pageLength <= 1) {
           // pageLength = 2;
            continue;
        }
        if (currentPage == pageLength) {
            activeLi = "active"
        } else {
            activeLi = '';
        }
        liTag += `<li class="page-item" onclick="createPagination(${totalpages},${pageLength})"><span class="page-link  ${activeLi}">${pageLength}</span></li>`;
    }

    if (currentPage < totalpages - 1) {
        if (currentPage < totalpages - 2) {
            liTag += `<li class="page-item dots"><span class="page-link">...</span></li>`;
        }
        liTag += `<li class="page-item" onclick="createPagination(${totalpages},${totalpages})"><span class="page-link ${activeLi}">${totalpages}</span></li>`;
    }

    if (currentPage < totalpages) {
        disableNextButton = '';
    }

    liTag += `<li class="page-item ${disableNextButton}" onclick="createPagination(${totalpages},${currentPage + 1})"><span class="page-link">Next</span></li>`;

    ulTag.innerHTML = liTag;

    var maxRows = 10; // parseInt($('#maxRows').val());
    var trnum = 0; // reset tr counter
    console.log(currentPage);
    $('#grAdmin' + ' tr:gt(1)').each(function () {
        // each TR in  table and not the header
        trnum++; // Start Counter
        if (trnum > maxRows * currentPage || trnum <= maxRows * currentPage - maxRows) {
            // if tr number gt maxRows
            $(this).hide(); // fade it out
        }
        else {
            $(this).show();
        } // else fade in Important in case if it ..
    });

}
