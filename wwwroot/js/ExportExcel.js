

var isExportTableExist;

$(function () {

    $(document).on('click', '.btn-export', function () {
        const data = $(this).closest('tr').find('td:eq(0)').text();
        var dataArray = data.split(' ');
        var selectedmonth = dataArray[0];
        var year = dataArray[2];
        var monthID;
        var montharray = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");

        montharray.forEach((month, index) => {
            if (month == selectedmonth)
                monthID = index+1;
        });

        console.log(monthID, year);
        excelDownload(selectedmonth, monthID, year);

    });


    $.ajax({
        url: "../Export/GetMonths",
        type: "GET",
        dataType: 'json',
        success: function (response) {

            console.log(response);

            if (isExportTableExist != null || isExportTableExist != undefined) {
                isExportTableExist.clear().rows.add(response).draw();
            }
            else {
                isExportTableExist = $('#grAdmin').DataTable({
                    "data": response,
                    "paginate": false,
                    "bInfo": false,
                    "bFilter": false,
                    "searching": false,
                    "ordering": false,
                    "lengthMenu": false,
                    "initComplete": function (settings, json) {
                        $("#divLoading").hide();
                    },
                    columnDefs: [{
                        className : 'no-width',
                    }],
                    columns: [
                        {
                            data: null, className: 'no-width',
                        },
                        {
                            data: null, className: 'no-width',
                            render: function (data, type, row, meta) {
                                return '<button type="button" class="btn-export"></button>';
                            }
                        },
                    ]
                });
            }
        },

        error: function (response) {

        }
    });
});

function excelDownload(month, monthID, year) {
    var param = {
        Month: monthID,
        Year: year,
    }

    $.ajax({
        url: "../Export/ExcelExport",
        type: "POST",
        data: param,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

            var url = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
            link.href = url;
            link.download = month+' '+year+' .xlsx';
            link.click();

            window.URL.revokeObjectURL(url);

            console.log("Download Completed..");
        },

        error: function (response) {

        }
    });
}