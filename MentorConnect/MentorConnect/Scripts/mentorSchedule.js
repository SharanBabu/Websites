function toggleButton(id) {
    if ($("#" + id).val() == "0") {        
        if ($("#"+id+"_btn").hasClass("AvailBtn")) {           
            $("#"+id+"_btn").addClass("SelectedBtn");
            $("#" + id + "_btn").removeClass("AvailBtn");
            $("#" + id).val("1");
        }
    }
    else if ($("#" + id).val() == "1") {
        if ($("#"+id+"_btn").hasClass("SelectedBtn")) {            
            $("#" + id + "_btn").addClass("AvailBtn");
            $("#" + id + "_btn").removeClass("SelectedBtn");
            $("#" + id).val("0");
        }
    }
}
function AcceptScheduleResponse(mentorID, stuID, subjectID, mshipID) {
    $.ajax({
        url: "/Mentor/AcceptScheduleRespose",
        type: "GET",
        dataType: "html",
        data: { mid: mentorID, stuid: stuID, subid: subjectID, mshipid:mshipID },
        success: function (data) {
            alert("Successfully recorded your response");            
            $('#SchReq').dialog("close");
            location.reload();
        }
    });
}

function RejectScheduleResponse(mentorID, stuID, subjectID, mshipID) {
    $.ajax({
        url: "/Mentor/RejectScheduleRespose",
        type: "GET",
        dataType: "html",
        data: { mid: mentorID, stuid: stuID, subid: subjectID, mshipid: mshipID },
        success: function (data) {
            alert("Successfully recorded your response");            
            $('#SchReq').dialog("close");
            location.reload();
        }
    });
}