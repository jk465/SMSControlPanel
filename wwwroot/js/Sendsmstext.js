var DeptName;
var OriginatorPrivilege;

$(function () {
    console.log("sms", getUser());
    RenderJqueryDates();
    $.when(GetDepartmentName())
        .then(function () {
            ValidateFields();
        });

    $.when(GetOriginatorPrivilege())
        .then(function () {
            CheckOriginator();
        });

    LoadTemplate();
    LoadTeamTemplate();

    $('#txtnopopup').on('keyup', function () {
        var text = $(this).val();

        var remainingLength = 160 - text.length;

        $('#txtcountremcharacters').prop('defaultValue', remainingLength);
    });

    $('#Txtpopupname').on('keyup', function () {
        var text = $(this).val();

        var remainingLength = 11 - text.length;

        $('#Txtnamecount').prop('defaultValue', remainingLength);
    });
    $('#Txtpopupreason').on('keyup', function () {
        var text = $(this).val();

        var remainingLength = 32 - text.length;

        $('#Txtreasoncount').prop('defaultValue', remainingLength);
    });

    $('.onlyalphabets').on('keypress', function (event) {
        var keyCode = event.which;
        var char = String.fromCharCode(keyCode);

        var isAlphabetic = /^[a-zA-Z]+$/.test(char);

        if (!isAlphabetic)
            event.preventDefault();

    });

    $('.onlynumberics').on('keypress', function (event) {
        var keyCode = event.which;
        var char = String.fromCharCode(keyCode);

        var isAlphabetic = /^[0-9s]+$/.test(char);

        if (!isAlphabetic)
            event.preventDefault();

    });
});

function RenderJqueryDates() {
    $('#Txtpopupdate').datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: 0
    })
}

var GetDepartmentName = function () {
    return $.ajax({
        url: "../SendSmsText/GetDept",
        type: "POST",
        data: getUser(),
        // async: false,  -- deprecated
        success: function (_response) {
            DeptName = _response;
        }
    });
}

var GetOriginatorPrivilege = function () {

    var OriginParam = {
        "DeptID": getUser().DeptID,
        "DeptName": DeptName,
        "CostCenter": getUser().COSTCENTRE
    }

    return $.ajax({
        url: "../SendSmsText/GetOriginatorPrivilege",
        type: "POST",
        data: OriginParam,
        //async: false,
        success: function (_response) {
            OriginatorPrivilege = _response;
        }
    });
}

function LoadTeamTemplate() {
    var TeamTemplateParam = {
        "RoleID": getUser().Role,
        "TeamID": getUser().Team
    }

    $.ajax({
        url: "../SendSmsText/LoadTeamTemplate",
        type: "POST",
        data: TeamTemplateParam,
        success: function (_response) {
            PopulateTeamTemplate(_response);
        }
    });
}

function LoadTemplate() {
   
    $.ajax({
        url: "../SendSmsText/LoadTemplate",
        type: "POST",
        success: function (_response) {
            PopulateTemplate(_response);
        }
    });
}

function PopulateTemplate(templates) {
    for (var i = 0; i < templates.length; i++) {
        $('#drdptemplate').append($('<option/>', {
            value: templates[i].id,
            text: templates[i].team_Name
        }));
    }
}

function PopulateTeamTemplate(templates) {
    for (var i = 0; i < templates.length; i++) {
        $('#ddrTeam').append($('<option/>', {
            value: templates[i].id,
            text: templates[i].team_Name
        }));
    }
}

function ValidateFields() {
    if (DeptName != null & getUser().Team != 6) {
        $('#table_content #ddrteamrow').show();
        $('#table_content #ddrtitle').show();
        $('#table_content #ddrmsg').show();
        $('#table_content #ddrmsgnopopup').hide();
        $('#table_content #charlength').hide();
        $('#txtMessage').prop('disabled', true);
    }
    else {
        $('#table_content #ddrteamrow').hide();
        $('#table_content #ddrtitle').hide();
        $('#table_content #ddrmsg').hide();
        $('#table_content #ddrmsgnopopup').show();
        $('#table_content #charlength').show();
        $('#txtMessage').prop('disabled', false);
    }
}

function CheckOriginator() {
    if (OriginatorPrivilege == '1') {
        $('#TxtOriginator').prop('readonly', false);
        $('#TxtOriginator').val('');
        $('#TxtOriginator').focus();
    }
    else if (OriginatorPrivilege == '0' &
        sessionStorage.getItem("DeptID") == 12695) {
        $('#TxtOriginator').prop('readonly', true);
        $('#TxtOriginator').prop('disabled', true);
        $('#TxtOriginator').val('BusDespatch');
    }
    else {
        $('#TxtOriginator').prop('readonly', true);
        $('#TxtOriginator').prop('disabled', true);
        $('#TxtOriginator').val('VirginMedia');
        $('#txtPhoneNumber').focus();
    }
}

function OnTeamchange() {
    var TeamID = $('#ddrTeam').val();

    if (TeamID == 0 || TeamID == 17) {
        $('#ddrMsgTitle').prop('disabled', true);
        return;
    }

    $('#ddrMsgTitle').prop('disabled', false);

    var params = {
        "TeamID": TeamID
    }

    $.ajax({
        url: "../SendSmsText/LoadMessageTitle",
        type: "POST",
        data: params,
        success: function (_response) {
            PopulateMessageTitle(_response);
        }
    });
}

function PopulateMessageTitle(messageTitles) {
    $('#ddrMsgTitle').empty();
    $('#ddrMsgTitle').append($('<option/>', {
        value: 0,
        text: '--Select Message Title--'
    }));
    for (var i = 0; i < messageTitles.length; i++) {
        $('#ddrMsgTitle').append($('<option/>', {
            value: messageTitles[i].id,
            text: messageTitles[i].msg_title
        }));
    }
}

function OnMsgTitleChange() {
    var MsgTitleID = $('#ddrMsgTitle').val();

    if (MsgTitleID == 0) {
        $('#txtMessage').prop('disabled', true);
        return;
    }

    
    if (MsgTitleID == 9) {
        $('#hidetxtlabl').css('display', 'block');
    } else if (MsgTitleID == 4) {
        $('#reasoncount').css('display', 'block');
        $('#lblMsg').show();
    }

   // $('#txtMessage').prop('disabled', false);

    var params = {
        "MsgTitleID": MsgTitleID
    }

    $.ajax({
        url: "../SendSmsText/LoadMsgTemplate",
        type: "POST",
        data: params,
        success: function (_response) {
            PopulateMsgTemplate(_response, MsgTitleID);
        }
    });
}

function ResetPopup() {
    $('#refno').hide();
    $('#date').hide();
    $('#name').hide();
    $('#time').hide();
    $('#number').hide();
    $('#reason').hide();
    $('#Trextn').hide();
    $('#tragentname').hide();
    $('#StoreNamelbl').hide();
    $('#Address1lbl').hide();
    $('#Address2lbl').hide();
    $('#PostCodelbl').hide();
    $('#Txtpopupreason').hide();
    $('#reasoncount').hide();
    $('#hidetxtlabl').hide();
    $('#Kellypopuptime').hide();

    $('#hdnname').val('0');
    $('#hdndate').val('0');
    $('#hdntime').val('0');
    $('#hdnphnumber').val('0');
    $('#hdnrefno').val('0');
    $('#hdnreason').val('0');
    $('#hdnkellytime').val('0');
    $('#hdneagentname').val('0');
    $('#hdnextn').val('0');
    $('#hdnstoretxt').val('0');
    $('#hdnaddress1text').val('0');
    $('#hdnaddress2text').val('0');
    $('#hdnpostcode').val('0');

    
	$('#Ddlpopupkellytime').val('AM-PM');
	$('#Txtpopupdate').val('');
	$('#Ddlpopuptime').val('AM-PM');
	$('#Txtpopupname').val('');
	$('#Txtpopupnumber').val('');
	$('#Txtpopuprefno').val('');
	$('#txtextn').val('');
	$('#txtagentname').val('');
	$('#Txtpopupreason').val('');
	$('#StoreNameText').val('');
	$('#Address1Text').val('');
	$('#Address2Text').val('');
	$('#PostCodeText').val('');

}

function PopulateMsgTemplate(Msgtemplate, MsgTitleID) {
    ResetPopup();
    var message = Msgtemplate;
    var dyncnt = 0;

    if (MsgTitleID == 16 | MsgTitleID == 6) {

        $('#hdnfromkelly').val('kelly');
        $('#hdnmessage').val(message);
        $('#Txtmsglabel').val(message);

        var teststring = '<time>';

        var exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnkellytime').val('1');
            $('#Kellypopuptime').css('display', 'block');
            $('#Ddlpopupkellytime').show();
            dyncnt = 2;
        }
        else {
            $('#Ddlpopupkellytime').val('AM-PM');
        }

        teststring = "<date>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdndate').val('1');
            $('#date').css('display', 'block');
            $('#Txtpopupdate').prop('disabled', false);
            //$('#Txtpopupdate').focus();
        }
        else {
            $('#Txtpopupdate').val('');
        }
    }
    else if (MsgTitleID != 16) {

        $('#hdnfromkelly').val('notkelly');
        $('#hdnmessage').val(message);
        $('#Txtmsglabel').val(message);

        var teststring = "<date>";

        var exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdndate').val('1');
            $('#date').css('display', 'block');
            $('#Txtpopupdate').prop('disabled', false);
            dyncnt = 2;
        }
        else {
            $('#Txtpopupdate').val('');
        }

        teststring = "<time>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdntime').val('1');
            dyncnt = 2;

            if (MsgTitleID != "16") {
                $('#time').css('display', 'block');
                $('#Kellypopuptime').hide();
            }

            else if (MsgTitleID == "16") {
                $('#Kellypopuptime').css('display', 'block');
            }
            $('#Ddlpopuptime').prop('disabled', false);
        }
        else {
            $('#Ddlpopuptime').val('AM-PM');
        }

        teststring = "<name>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnname').val('1');
            $('#name').css('display', 'block');
            $('#hidetxtlabl').css('display', 'block');
            dyncnt = 1;
            $('#Txtpopupname').prop('disabled', false);
        }
        else {
            $('#Txtpopupname').val('');
        }

        teststring = "<phnumber>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnphnumber').val('1');
            $('#number').css('display', 'block');
            dyncnt = 1;
            $('#Txtpopupnumber').prop('disabled', false);
        }
        else {
            $('#Txtpopupnumber').val('');
        }
        teststring = "<ref.no>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnrefno').val('1');
            $('#refno').css('display', 'block');
            dyncnt = 1;
            $('#Txtpopuprefno').prop('disabled', false);
        }
        else {
            $('#Txtpopuprefno').val('');
        }
        teststring = "<ext.>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnextn').val('1');
            $('#Trextn').css('display', 'block');
            dyncnt = 1;
            $('#txtextn').prop('disabled', false);
        }
        else {
            $('#txtextn').val('');
        }

        teststring = "<agentname>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdneagentname').val('1');
            $('#tragentname').css('display', 'block');
            $('#txtagentname').css('display', 'block');
            dyncnt = 8;
            $('#txtagentname').prop('disabled', false);
        }
        else {
            $('#txtagentname').val('');
        }

        teststring = "<reason>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnreason').val('1');
            $('#reason').css('display', 'block');
            $('#reasoncount').css('display', 'block');
            dyncnt = 8;
            $('#Txtpopupreason').css('display', 'block');
        }
        else {
            $('#Txtpopupreason').val('');
        }

        teststring = "<StoreName>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnstoretxt').val('1');
            $('#StoreNamelbl').css('display', 'block');
            dyncnt = 8;
            $('#StoreNameText').prop('disabled', false);
        }
        else {
            $('#StoreNameText').val('');
        }

        teststring = "<StoreAddress1>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnaddress1text').val('1');
            $('#Address1lbl').css('display', 'block');
            dyncnt = 8;
            $('#Address1Text').prop('disabled', false);
        }
        else {
            $('#Address1Text').val('');
        }

        teststring = "<StoreAddress2>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnaddress2text').val('1');
            $('#Address2lbl').css('display', 'block');
            dyncnt = 8;
            $('#Address2Text').prop('disabled', false);
        }
        else {
            $('#Address2Text').val('');
        }

        teststring = "<StorePostcode>";
        exists = message.indexOf(teststring);
        if (exists != -1) {
            $('#hdnpostcode').val('1');
            $('#PostCodelbl').css('display', 'block');
            dyncnt = 8;
            $('#PostCodeText').prop('disabled', false);
        }
        else {
            $('#PostCodeText').val('');
        }
    }

    if (dyncnt == 0) {
        $('#hdnmessage').val(message);
        $('#txtMessage').val(message);
    }

    else if (dyncnt > 0) {
        ShowDialog();
    }
    else {
        $('#ddrMsgTitle').prop('disabled', true);
    }
}

function ShowDialog() {
    $("#popupdiv").dialog({
        title: "Message",
        "modal": true,

        Height: 450,
        width: 330,
        minHeight: 450,
        "buttons": {
            "Cancel": function () {

                $('#ddrMsgTitle').prop('selectedIndex', 0);
                $('#hdnsubmit').val('cancel1');
                $('#txtMessage').prop('disabled', true);
                $(this).dialog("close").dialog('destroy');
            },
            "Submit": function () {

                $('#hdnsubmit').val('submit1');

                var strKelly = $('#hdnfromkelly').val();
                var result = validate(strKelly);
                if (result == true) {
                    $(this).dialog("close");
                }
            }
        }
    });
}

function validate(strKelly) {

    var name = $('#hdnname').val();
    var date = $('#hdndate').val();
    var time = $('#hdntime').val();
    var number = $('#hdnphnumber').val();
    var refno = $('#hdnrefno').val();
    var extn = $('#hdnextn').val();
    var agentname = $('#hdneagentname').val();
    var reason = $('#hdnreason').val();
    var Kellypopuptime = $('#hdnkellytime').val();
    var strname = $('#hdnstoretxt').val();
    var stradd1 = $('#hdnaddress1text').val();
    var stradd2 = $('#hdnaddress2text').val();
    var strpostcode = $('#hdnpostcode').val();
    var ddrMsg = $('#ddrMsgTitle').val();
    
    //reg for strname,stradd1,stradd2,strpostcode
    var regx = /^[0-9a-zA-Z_,.: \s]+$/;

    $('#txtMessage').prop('disabled', true);
    if (strKelly == "kelly") {

        if (Kellypopuptime == "1") {
            var pkellytime = $('#Ddlpopupkellytime').val();
            if (pkellytime == "") {
                alert("Enter time");
                return false;
            }
        }

        if (date == "1") {
            var pdate = $('#Txtpopupdate').val();
            if (pdate == "") {
                alert("Enter Date");
                return false;
            }
        }

        combinestringforKelly();
    }

    else if (strKelly != "kelly") {
        if (date == 1) {
            $('#charlength').hide();
        }
        if (name == "1") {
            var pname = $('#Txtpopupname').val();   
            if (pname == "") {
                alert("Enter Name");
                return false;
            }
            var refregex = /^[a-zA-Z]+$/;
            if (refregex.test(pname)) {
            }
            else {
                alert("Only alphabets are allowed");
                return false;
            }
        }
        if (agentname == "1") {
            var pagentname = $('#txtagentname').val(); 
            if (pagentname == "") {
                alert("Enter Name");
                return false;
            }
            var refregex = /^[a-zA-Z]+$/;
            if (refregex.test(pagentname)) {
            }
            else {
                alert("Only alphabets are allowed");
                return false;
            }
        }

        if (refno == "1") {
            var prefno = $('#Txtpopuprefno').val(); 
            if (prefno == "") {
                alert("Enter Reference Number");
                return false;
            }
            var refregex = /^\d{8}$/;
            if (refregex.test(prefno)) {
            }
            else {
                alert("Please enter valid reference number with 8 digits");
                return false;
            }
        }

        if (extn == "1") {
            var pextno = $('#txtextn').val(); 
            if (pextno == "") {
                alert("Enter extension Number");
                return false;
            }
            var refregex = /^\d{4}$/;
            if (refregex.test(pextno)) {
            }
            else {
                alert("Please enter valid extension number with 4 digits");
                return false;
            }
        }

        if (date == "1") {
            var pdate = $('#Txtpopupdate').val();
            if (pdate == "") {
                alert("Enter Date");
                return false;
            }
        }

        if (time == "1") {
            var ptime = $('#Ddlpopuptime').val(); 

            if (ptime == "") {
                alert("Enter time");
                return false;
            }
        }

        if (number == "1") {

            var pno = $('#Txtpopupnumber').val(); 

            if (pno == "") {
                alert("Enter Contact Number");
                return false;
            }
            var ph1 = /^(0)\d{10}$/;
            var ph2 = /^(44)\d{10}$/;
            if ((ph1.test(pno)) || (ph2.test(pno))) {
            }
            else {
                alert("Contact Number should be in the form 441234567890 or 01234567890" + "\n");
                return false;
            }
        }

        if (reason == "1") {
            var preason = $('#Txtpopupreason').val();
            if (preason == "") {
                alert("Enter Reason");
                return false;
            }
        }

        if(strname == "1") {
            var pstrname = $('#StoreNameText').val();
            if (pstrname == "") {
                alert("Please Enter Store Name");
                return false;
            }
            if (pstrname.length < 3) {
                alert("Store Name should be above 3 characters");
                return false;
            }

            if (regx.test(pstrname)) {
            }

            else {
                alert("Store name should contain only alphanumeric values");
                return false;
            }

        }

        if(stradd1 == "1") {
            var pstradd1 = $('#Address1Text').val(); 
            if (pstradd1 == "") {
                alert("Please Enter Address1");
                return false;
            }
            if (pstradd1.length < 4) {
                alert("Address1 should be above 4 characters");
                return false;
            }
            if (pstradd1.length > 130) {
                alert("Address1 should be below 130 characters");
                return false;
            }
            if (regx.test(pstradd1)) {
            }

            else {
                alert("Address1 should contain only alphanumeric values");
                return false;
            }
        }

        if (strpostcode == "1") {
            var pstrpostcode = $('#PostCodeText').val();
            if (pstrpostcode == "") {
                alert("Please Enter Store Post Code");
                return false;
            }
            if (pstrpostcode.length < 4) {
                alert("Store Post Code should be above 4 characters");
                return false;
            }

            if (regx.test(pstrpostcode)) {
            }

            else {
                alert("Store Post Code should contain only alphanumeric values");
                return false;
            }
        }

        if (stradd2 == "1") {
            var pstradd2 = $('#Address2Text').val();

            if (pstradd2.length > 130) {
                alert("Address2 should be below 130 characters");
                return false;
            }
            if (regx.test(pstrpostcode)) {
            }

            else {
                alert("Address2 should contain only alphanumeric values");
                return false;
            }

        }

        combinestring(pdate, pname, pno, ptime, prefno, preason, pextno, pagentname, ddrMsg, pstrname, pstradd1, pstradd2, pstrpostcode);

    }
}

function combinestring(pdate, pname, pno, ptime, prefno, preason, pextno, pagentname, ddrMsg, pstrname, pstradd1, pstradd2, pstrpostcode) {
    
    var initmsg = $('#hdnmessage').val();
    var finalmsg = initmsg.replace('<name>', pname);
    finalmsg = finalmsg.replace('<date>', pdate);
    finalmsg = finalmsg.replace("<time>", ptime);
    finalmsg = finalmsg.replace('<phnumber>', pno);
    finalmsg = finalmsg.replace('<ref.no>', prefno);
    finalmsg = finalmsg.replace('<reason>', preason);
    finalmsg = finalmsg.replace('<ext.>', pextno);
    finalmsg = finalmsg.replace('<agentname>', pagentname);
    finalmsg = finalmsg.replace('<StoreName>', pstrname);
    finalmsg = finalmsg.replace('<StoreAddress1>', pstradd1);
    finalmsg = finalmsg.replace('<StoreAddress2>', pstradd2);
    finalmsg = finalmsg.replace('<StorePostcode>', pstrpostcode);
    finalmsg = finalmsg.replace(',,', ',');
    finalmsg = finalmsg.replace(', ,', ',');

    $('#hdnsmsmessage').val(finalmsg);
    $('#txtMessage').val(finalmsg);
    $('#txtMessage').prop('selectedIndex', ddrMsg);

    $('#popupdiv').dialog("close");
}

function combinestringforKelly() {
    var lblMessage = $('#Txtmsglabel').val();
    var pdate = $('#Txtpopupdate').val();
    var pkellytime = $('#Ddlpopupkellytime').val();
    var finalmsg = lblMessage;
    finalmsg = finalmsg.replace('<date>', pdate);
    finalmsg = finalmsg.replace('<time>', pkellytime);
    $('#hdnsmsmessage').val(finalmsg);
    $('#hdnkellytime').val(finalmsg);
    $('#txtMessage').val(finalmsg);

    $('#popupdiv').dialog("close");
}

 function SendMessage(event) {
    event.preventDefault();
 

    var isvalid = SMSClientValidation();

    //$disabledFields.prop('disabled', true);

    /*console.log(isvalid);*/

    if (isvalid) {
        var SMSMessage = '';
        var teamId = $('#ddrTeam').val();

        //Pranav Fix if (['1','2','3','4','5','6','7','8','9','10','11','12','13'].includes(teamId)) {
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
            phnumber: $('#txtPhoneNumber').val(),
            team: teamId,
            messagetitle: $('#ddrMsgTitle').val(),
            template: $('#drdptemplate').val(),
            originator: $('#TxtOriginator').val(),
            radcommunication: $('#radcommunication').val(),
        }

         $.ajax({
            url: "../SendSmsText/SendSms",
            type: "POST",
            data: param,
            //async: false,
             success: function (data) {
                 $('#msg-response').text(data.response);
                 $('#msg-response').show();
                 if (data.success == true) {
                     $('#txtnopopup').val('');
                     $('#txtMessage').val('');
                     $('#ddrMsgTitle').val(0);
                     $('#ddrTeam').val(0);

                     $('#txtPhoneNumber').val('');
                 }

                
            }
        });

        
        
    }
    


    
}

function SMSClientValidation() {
    //var $disabledFields = $('#SMSForm').find(':disabled');
    //$disabledFields.prop('disabled', false);

    $.validator.addMethod('phnumber_pattern', function (value, element, param) {
        var pattern = new RegExp(param);
        return this.optional(element) || pattern.test(value);
    });

    $.validator.addMethod('Default_value', function (value, element, param) {
        return value !== param;
    });

    $('#SMSForm').validate({
        errorClass: 'error-Label',
        //ignore:[],
        rules: {
            TxtOriginator: {
                required: true
            },
            txtPhoneNumber: {
                required: true,
                phnumber_pattern: /^[0-9,\s]*$/i
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
            txtPhoneNumber: {
                required: 'Please enter at least one phone number',
                phnumber_pattern: 'Only numbers are allowed'
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

    return $('#SMSForm').valid();

}


//async function SMSClientValidation() {
//    return new Promise((resolve, reject) => {
//        $.validator.addMethod('phnumber_pattern', function (value, element, param) {
//            var pattern = new RegExp(param);
//            return this.optional(element) || pattern.test(value);
//        });

//        $.validator.addMethod('Default_value', function (value, element, param) {
//            return value !== param;
//        });

//        $('#SMSForm').validate({
//            errorClass: 'error-Label',
//            rules: {
//                TxtOriginator: {
//                    required: true
//                },
//                txtPhoneNumber: {
//                    required: true,
//                    phnumber_pattern: /^[0-9,\s]*$/i
//                },
//                ddrTeam: {
//                    required: true,
//                    Default_value: '0'
//                },
//                ddrMsgTitle: {
//                    required: true,
//                    Default_value: '0'
//                },
//                txtMessage: {
//                    required: true
//                },
//                txtnopopup: {
//                    required: true
//                }
//            },
//            messages: {
//                TxtOriginator: {
//                    required: 'Please enter Originator name'
//                },
//                txtPhoneNumber: {
//                    required: 'Please enter at least one phone number',
//                    phnumber_pattern: 'Only numbers are allowed'
//                },
//                ddrTeam: {
//                    required: 'Select a Team',
//                    Default_value: 'Select a Team'
//                },
//                ddrMsgTitle: {
//                    required: 'Select a Message Ttile',
//                    Default_value: 'Select a Message Ttile'
//                },
//                txtMessage: {
//                    required: 'No message provided'
//                },
//                txtnopopup: {
//                    required: 'Please enter the message'
//                }
//            },
//            submitHandler: function (form) {
//                resolve($(form).valid());
//            },
//            invalidHandler: function (form) {
//                reject($(form).valid());
//            }
//        });

//        $('#SMSForm').submit();
//    });
//}
