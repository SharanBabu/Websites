(function ($) {
    $.widget("custom.combobox", {
        _create: function () {
            this.wrapper = $("<span>")
                    .addClass("custom-combobox")
                    .insertAfter(this.element);

            this.element.hide();
            this._createAutocomplete();
            this._createShowAllButton();
        },
        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
                    value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
                    .appendTo(this.wrapper)
                    .val(value)
                    .attr("title", "")
                    .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                    .autocomplete({
                        delay: 0,
                        minLength: 0,
                        source: $.proxy(this, "_source")
                    })
                    .tooltip({
                        tooltipClass: "ui-state-highlight"
                    });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },
                autocompletechange: "_removeIfInvalid"
            });
        },
        _createShowAllButton: function () {
            var input = this.input,
                    wasOpen = false;

            $("<a>")
                    .attr("tabIndex", -1)
                    .attr("title", "Show All Items")
                    .tooltip()
                    .appendTo(this.wrapper)
                    .button({
                        icons: {
                            primary: "ui-icon-triangle-1-s"
                        },
                        text: false
                    })
                    .removeClass("ui-corner-all")
                    .addClass("custom-combobox-button ui-corner-right")
                    .mousedown(function () {
                        wasOpen = input.autocomplete("widget").is(":visible");
                    })
                    .click(function () {
                        input.focus();

                        // Close if already visible
                        if (wasOpen) {
                            return;
                        }

                        // Pass empty string as value to search for, displaying all results
                        input.autocomplete("search", "");
                    });
        },
        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },
        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
                    valueLowerCase = value.toLowerCase(),
                    valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
                    .val("")
                    .attr("title", value + " didn't match any item")
                    .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },
        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });
})(jQuery);

$(function () {
    $("#searchButton").button();
    $("#searchButton2").button();
    $("#searchButton3").button();
    $("#searchButton4").button();
    $("#searchButton5").button();
    $("#dtseat").datepicker();
    $("#custdate").datepicker();
    makeAjaxToFillCombos();
});
function displayMain(id) {
    $('#tabDiv').children().each(function () {
        if (this.id == id)
            this.style.display = 'block';
        else
            this.style.display = 'none';
    });
}

function makeAjaxToFillCombos() {
    $.ajax({
        url: "SearchAirports",
        type: "GET",
        dataType: "json",
        success: function (data) {
            var $select = $('#fromAirports');
            $.each(data, function (id, value) {
                $('<option>').val(value.airport_code).text(value.name+' ('+value.airport_code+')').appendTo($select);
            });
            var $select = $('#toAirports');
            $.each(data, function (id, value) {
                $('<option>').val(value.airport_code).text(value.name+' ('+value.airport_code+')').appendTo($select);
            });
            $("#fromAirports").combobox();
            $("#toAirports").combobox();
        }
    });
}

function fetchFlights() {
    var from = $("#fromAirports").val();
    var to = $("#toAirports").val();
    if (from == '') {
        alert("Please select departure");
        return;
    }
    if (to == '') {
        alert("Please select arrival");
        return;
    }
    if (from == to) {
        alert("Please choose a different destination!");
        return;
    }
    var maxStops = $("#maxStops").val();
    $.ajax({
        url: "SearchFlights",
        type: "GET",
        data: {from: from, to: to, maxStops: maxStops},
        dataType: "json",
        success: function (data) {
            var count = Object.keys(data).length;
            var re = true;
            if(count > 100){
                re = confirm(count + " flights found. Do you want to display them all?");
            }
            if(re == true){
            $("#resCnt").html("<p>Number of Results: "+count+"</p>");
            $("#flightResults").css('display', "block");
            $("#flightResults").html("");
            $("#flightTableTemplate").tmpl(data).appendTo("#flightResults");
           }
        }
    });
}

function fetchSeatAvailability() {
    var flightNo = $("#flno").val();
    var date = $("#dtseat").val();
    if (flightNo == '') {
        alert("Please enter flight number");
        return;
    }
    if (date == '') {
        alert("Please select date");
        return;
    }
    $.ajax({
        url: "CheckSeats",
        type: "GET",
        data: {flight: flightNo, date: date},
        dataType: "json",
        success: function (data) {
            $("#seatResults").css('display', "block");
            $("#seatResults").html("");
            $("#seatResTemplate").tmpl(data).appendTo("#seatResults");
        }
    });
}
function fetchFareDetails() {
    var flightNo = $("#flnum").val();
    if (flightNo == '') {
        alert("Please enter flight number");
        return;
    }
    $.ajax({
        url: "fetchFareDetails",
        type: "GET",
        data: {flight: flightNo},
        dataType: "json",
        success: function (data) {
            $("#fareResults").css('display', "block");
            $("#fareResults").html("");
            $("#fareResTemplate").tmpl(data).appendTo("#fareResults");
        }
    });
}

function fetchAllCustomers() {
    var flightNo = $("#flnumber").val();
    var date = $("#custdate").val();
    if (flightNo == '') {
        alert("Please enter flight number");
        return;
    }
    if (date == '') {
        alert("Please select date");
        return;
    }
    $.ajax({
        url: "CheckManifest",
        type: "GET",
        data: {flight: flightNo, date: date},
        dataType: "json",
        success: function (data) {
            $("#custTable").css('display', "table");
            $("#custResultsData").html("");
            $("#custResTemplate").tmpl(data).appendTo("#custResultsData");
        }
    });
}

function fetchAllFlightInst(){
    var name = $("#pname").val();   
    if (name == '') {
        alert("Please enter name of the passenger");
        return;
    }    
        $.ajax({
            url: "FetchAllFlightInstances",
            type: "GET",
            data: {name: name},
            dataType: "json",
            success: function (data) {
                $("#flInstTable").css('display', "table");
                $("#flInstData").html("");
                $("#flInstDataTemplate").tmpl(data).appendTo("#flInstData");
            }
        });
}

function ajaxindicatorstart(text)
{
	if(jQuery('body').find('#resultLoading').attr('id') != 'resultLoading'){
	jQuery('body').append('<div id="resultLoading" style="display:none"><div><img src="css/images/ajax-loader.gif"><div>'+text+'</div></div><div class="bg"></div></div>');
	}

	jQuery('#resultLoading').css({
		'width':'100%',
		'height':'100%',
		'position':'fixed',
		'z-index':'10000000',
		'top':'0',
		'left':'0',
		'right':'0',
		'bottom':'0',
		'margin':'auto'
	});

	jQuery('#resultLoading .bg').css({
		'background':'#000000',
		'opacity':'0.7',
		'width':'100%',
		'height':'100%',
		'position':'absolute',
		'top':'0'
	});

	jQuery('#resultLoading>div:first').css({
		'width': '250px',
		'height':'75px',
		'text-align': 'center',
		'position': 'fixed',
		'top':'0',
		'left':'0',
		'right':'0',
		'bottom':'0',
		'margin':'auto',
		'font-size':'16px',
		'z-index':'10',
		'color':'#ffffff'

	});

    jQuery('#resultLoading .bg').height('100%');
       jQuery('#resultLoading').fadeIn(300);
    jQuery('body').css('cursor', 'wait');
}

function ajaxindicatorstop()
{
    jQuery('#resultLoading .bg').height('100%');
       jQuery('#resultLoading').fadeOut(300);
    jQuery('body').css('cursor', 'default');
}

jQuery(document).ajaxStart(function () {
 		//show ajax indicator
ajaxindicatorstart('loading data..');
}).ajaxStop(function () {
//hide ajax indicator
ajaxindicatorstop();
});