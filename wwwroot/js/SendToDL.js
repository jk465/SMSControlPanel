$(function () {
    console.log("smsToDL", getUser());

    LoadDL();
});

function LoadDL() {
    $.ajax({
        url: "../DistList/LoadDL",
        type: "POST",
        success: function (_response) {
            PopulateDL(_response);
        }
    });
}

function PopulateDL(data) {
    for (var i = 0; i < data.length; i++) {
        $('#DistList').append($('<option/>', {
            value: data[i],
            text: data[i]
        }));
    }
}

async function SendMessageToDL(event) {
    event.preventDefault();

    var isvalid = DLClientValidation();

    console.log(isvalid);

    if (isvalid == false) {
        return;
    }
    var SMSMessage = '';
    var teamId = $('#ddrTeam').val();

  //Pranav Change  if (['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13'].includes(teamId)) {
    if (teamId != '' && teamId > 0) {
        if ($('#hdnsubmit').val() == "submit1") {
            SMSMessage = $('#hdnsmsmessage').val();
        }
        else {
            SMSMessage = $('#txtMessage').val();
        }
    }
    else {
        SMSMessage = $('#txtnopopup').val();
    }

    var param = {
        message: SMSMessage,
        distlist: $('#DistList').val(),
        team: teamId,
        messagetitle: $('#ddrMsgTitle').val(),
        template: $('#drdptemplate').val(),
        originator: $('#TxtOriginator').val(),
        radcommunication: $('#radcommunication').val(),
    }

    var data = await $.ajax({
        url: "../DistList/SendSms",
        type: "POST",
        data: param,
        success: function (_response) {
            console.log(_response);
        }
    });

    $('#msg-response').text(data.response);
    $('#msg-response').show();
    if (data.success == true) {
        $('#txtnopopup').val('');
        $('#txtMessage').val('');
        $('#ddrMsgTitle').val(0);
        $('#ddrTeam').val(0);
        $('#DistList').val('');
    }

}

function DLClientValidation() {
    //var $disabledFields = $('#SMSForm').find(':disabled');
    //$disabledFields.prop('disabled', false);


    $.validator.addMethod('Default_value', function (value, element, param) {
        return value !== param;
    });

    $('#smsToDL').validate({
        errorClass: 'error-Label',
        //ignore:[],
        rules: {
            TxtOriginator: {
                required: true
            },
            DistList: {
                required: true,
            },
            ddrTeam: {
                required: true,
                Default_value: '0'
            },
            ddrMsgTitle: {
                required: true,
                Default_value: '0'
            },
            txtMessage: {
                required: true
            },
            txtnopopup: {
                required: true
            }
        },
        messages: {
            TxtOriginator: {
                required: 'Please enter Originator name'
            },
            DistList: {
                required: 'Please select Distribution List',
            },
            ddrTeam: {
                required: 'Select a Team',
                Default_value: 'Select a Team'
            },
            ddrMsgTitle: {
                required: 'Select a Message Ttile',
                Default_value: 'Select a Message Ttile'
            },
            txtMessage: {
                required: 'No message provided'
            },
            txtnopopup: {
                required: 'Please enter the message'
            }
        }
    });

    return $('#smsToDL').valid();

}