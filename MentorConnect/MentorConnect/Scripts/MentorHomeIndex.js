$(function () {
    $("#mtabs").tabs();
});

function DisplayFullScheduleRequest(mshipID) {
    $.ajax({
        url: "/Mentor/DisplayRequestDetail",
        type: "GET",
        dataType: "html",
        data: {mshipID:mshipID},
        success: function (data) {
            var $select = $('#SchReq');
            $select.html(data);
            $select.dialog({
                height: 637,
                width: 977,
                modal: false,
                draggable: true,
                resizable: false              
            });
        }
    });
}

function DisplayAllMentorShips() {
    $.ajax({
        url: "/Mentor/GetAllMentorships",
        type: "GET",
        dataType: "html",        
        success: function (data) {           
            var $select = $('#mshipListDiv');
            $select.html(data);
            $select.show();
        }
    });
}

function DisplayManageAccount() {
    $.ajax({
        url: "/Account/ManageAccount",
        type: "GET",
        dataType: "html",
        success: function (data) {
            var $select = $('#ManageAccount');
            $select.html(data);
            $select.show();
        }
    });
}

function DisplayEditProfile() {
    $.ajax({
        url: "/Mentor/EditProfile",
        type: "GET",
        dataType: "html",
        success: function (data) {
            var $select = $('#EditProfile');
            $select.html(data);
            $select.show();
        }
    });
}


