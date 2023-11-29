$(function () {
    /*$('[data-toggle="tooltip"]').tooltip();*/

    GetDate();
    window.setInterval(function () {
        GetDate();
    }, 1000);

    const role = getUser().Role;

    if (role == 1) {
        $('#DvReports').hide();
        $('#DvAdmin').hide();
        $('#Li1').hide();
        $('#DvExport1').hide();
    }
    if (role == 2) {
        $('#DvAdmin').hide();
        $('#Li1').hide();
        $('#DvExport1').hide();
    }
   
});


function GetDate() {
    var dayarray = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")
    var montharray = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December")

    var mydate = new Date()
    var year = mydate.getYear()
    if (year < 1000)
        year += 1900
    var day = mydate.getDay()
    var month = mydate.getMonth()
    var daym = mydate.getDate()
    if (daym < 10)
        daym = "0" + daym
    var hours = mydate.getHours()
    var minutes = mydate.getMinutes()
    var seconds = mydate.getSeconds()
    var dn = "AM"
    if (hours >= 12)
        dn = "PM"
    if (hours > 12) {
        hours = hours - 12
    }
    if (hours == 0)
        hours = 12
    if (minutes <= 9)
        minutes = "0" + minutes
    if (seconds <= 9)
        seconds = "0" + seconds
    //change font size here
    var cdate = "<large><font color='Red' face='Arial'>" + dayarray[day] + ", " + montharray[month] + " " + daym + ", " + year + " " + hours + ":" + minutes + ":" + seconds + " " + dn
        + "</font></large>"
    if (document.all)
        document.all.clock.innerHTML = cdate
    else if (document.getElementById)
        document.getElementById("clock").innerHTML = cdate
    else
        document.write(cdate)
}