function submitReview() {
    var mshipid = $("#mshipID").val();
    var rating = $("#rating").val();
    var comments = $("#comments").val();
    $.ajax({
        url: "/Student/SubmitReview",
        type: "POST",
        dataType: "html",
        data: {mshipID: mshipid, rating:rating, comments:comments},
        success: function (data) {
            alert("Your review has been recorded successfully!");
            $('#writeReview').dialog("close");
        }
    });
}