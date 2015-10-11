function DisplayMShipSchedule(mshipID) {
    $.ajax({
        url: "/Mentor/DisplayMentorshipDetail",
        type: "GET",
        dataType: "html",
        data: { mshipID: mshipID},
        success: function (data) {
            var $select = $('#mshipSch');
            $select.html(data);
            $select.dialog({
                height: 437,
                width: 977,
                modal: false,
                draggable: true,
                resizable: false
            });
        }
    });
}

function DisplayWriteReview(mshipID) {
    $.ajax({
        url: "/Student/WriteReview",
        type: "GET",
        dataType: "html",
        data: { mshipID: mshipID },
        success: function (data) {
            var $select = $('#writeReview');
            $select.html(data);
            $select.dialog({
                height: 420,
                width: 977,
                modal: false,
                draggable: true,
                resizable: false,
                open: function (event, ui) {
                    loadRatingData();
                }
            });
        }
    });
}