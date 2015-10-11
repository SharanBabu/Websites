function toggleButton(id) {
    if ($("#" + id).val() == "0") {
        if ($("#" + id + "_btn").hasClass("notAvailBtn")) {
            $("#" + id + "_btn").addClass("AvailBtn");
            $("#" + id + "_btn").removeClass("notAvailBtn");
            $("#" + id).val("1");
        }
    }
    else if ($("#" + id).val() == "1") {
        if ($("#" + id + "_btn").hasClass("AvailBtn")) {
            $("#" + id + "_btn").addClass("notAvailBtn");
            $("#" + id + "_btn").removeClass("AvailBtn");
            $("#" + id).val("0");
        }
    }
}

